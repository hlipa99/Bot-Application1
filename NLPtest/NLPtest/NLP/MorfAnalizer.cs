﻿
using static NLPtest.HebWords.WordObject.WordType;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLPtest.WorldObj;
using hebrewNER;
//using java.io;
//using static vohmm.corpus.Sentence;
using System.Text.RegularExpressions;
using NLPtest.view;
using NLPtest.Controllers;
//using java.awt;
using System.Xml.Serialization;
using NLPtest.MorfObjects;
using Newtonsoft.Json;
using Model.dataBase;
using static NLPtest.HebWords.WordObject;
using NLPtest.HebWords;


namespace NLPtest.NLP
{
      public class MorfAnalizer
    {
     //   SimpleTagger3 tagger;
      //  NERTagger nerTagger;
      //  MeniTaggeedSentenceFactory sentenceFactory;
      //  TaggerBasedHebrewChunker chunker;
        private   HebDictionary hebDictionary;
        OuterAPIController httpCtrl = new OuterAPIController();
       // private IEnumerable<Word> wordList;
        DataBaseController DBctrl = new DataBaseController();
        public MorfAnalizer()
        {
         //   var path = "C:/Program Files (x86)/IIS Express/botServer/hebdata/";
         //   tagger = new SimpleTagger3(path);
         //   nerTagger = new NERTagger(path, tagger);
            //   create the noun-phrase chunker
       //     sentenceFactory = new MeniTaggeedSentenceFactory(null, MeniTokenExpander.expander);
        //    String chunkModelPrefix = path + vohmm.util.Dir.CHUNK_MODEL_PREF;
     //       chunker = new TaggerBasedHebrewChunker(sentenceFactory, chunkModelPrefix);
            hebDictionary = new HebDictionary();
        }


        public List<WordObject> getWordsObjectFromParserServer(String str,bool isSpellCorrected)
        {
            List<WordObject> sentenceFromServer = null;
            str = str.Trim();
            if (str == "") return new List<WordObject>();
            try
            {

                //may be mispelling for the first time
                //   if (!isSpellCorrected && sentenceFromServer.Count(x => x.isA(WordType.unknownWord) && x.Text.Length > 1) > 0)
                try
                {
                    if (!isSpellCorrected)
                    {
                        var correctSpelling = httpCtrl.correctSpelling(str);
                        if (correctSpelling != null && correctSpelling.Length > str.Length - 10)
                        {
                            return getWordsObjectFromParserServer(correctSpelling, true);
                        }else
                        {
                            throw new ServerException("spelling server");
                        }
                    }
                }catch(Exception ex)
                {
                    return getWordsObjectFromParserServer(str, true);
                }


                string JsonRes = httpCtrl.sendToHebrewMorphAnalizer(str);

                sentenceFromServer = new List<WordObject>();
                if (JsonRes != null)
                {
                    sentenceFromServer = JsonConvert.DeserializeObject<List<WordObject>>(JsonRes);
                }

            
            }
            catch (Exception ex) //if parser server is down
            {
                var words = str.Split(' '); 
                foreach(var w in words)
                {
                    var word = new WordObject(w, nounWord); 
                }
            }
            return sentenceFromServer;
        }
        




        public List<List<WordObject>> meniAnalize(String str,Boolean userInput)
        {

            // The follwoing object constructions are heavy - SHOULD BE APPLIED ONLY ONCE!
            // create the morphological analyzer and disambiguator 

            var sentenses = str.Split('.',',');
            List<List<WordObject>> allRes = new List<List<WordObject>>();

            foreach (var s in sentenses)
            {

                var strRes = s;

                List<WordObject> res = new List<WordObject>();
                strRes = removeParentheses(strRes, '(', ')');
                strRes = removeParentheses(strRes, '[', ']');

                if (strRes.Length > 0)
                {
                    //   var taggedSentences = tagger.getTaggedSentences(strRes);


                    var sentenceFromServer = getWordsObjectFromParserServer(strRes, !userInput);
                    //remove nikod etc.
                    sentenceFromServer.RemoveAll(x => (x.Text.Length <= 1) && (x.Pos == "punctuation"));

                    if (sentenceFromServer.Count >= 0)
                    {


                        sentenceFromServer = tryMatchEntities(sentenceFromServer);

                        // print tagged sentence by using AnalysisInterface, as follows:
                        foreach (WordObject w in sentenceFromServer)
                        {
                            WordObject word = w;



                           
                            //two NRI in a row
                            //join word if ist part of a name

                            if (res.Count > 0)
                            {
                                if (res.LastOrDefault().Ner == w.Ner && res.LastOrDefault().Ner != "O")
                                {
                                    res.LastOrDefault().Text += " " + word.Text;
                                    continue;
                                }
                            }

                            if (hebDictionary.contains(word.Text))
                            {
                                word = hebDictionary.get(word.Text);
                            }
                            //joinwords
                            if (res.LastOrDefault() != null && word.isA(nounWord) && res.LastOrDefault().isA(nounWord)&& res.LastOrDefault().Text.Split(' ').Count() < 3) //TODO
                            {
                                var last = res.LastOrDefault();
                                res.RemoveAt(res.Count - 1);
                                last.Text = last.Text + " " + word.Text;
                                last.WordT = last.WordT | word.WordT; //combin flages
                                word = last;
                            }

                                res.Add(word);


                            }
                          
                        }
                        
                       // res = checkPhrases(res);
                        allRes.Add(res);
                    }
                }
            
            return allRes;
        }

        public List<WordObject> tryMatchEntities(List<WordObject> sentence)
        {
            //increase the match found to implement maximal munch
            for(int i = 0; i < sentence.Count; i++)
            {
                WordObject word = sentence[i];


                var entities = DBctrl.getEntitys();
                var searchText = "";
                IQueryable<entity> match = null;
                int j = i;
                for (; j < sentence.Count; j++)
                {
                    searchText += " " + sentence[j].Text;
                    var tryMatch = findMatch(entities, searchText);
                    if(tryMatch.Count() != 0)
                    {
                        match = tryMatch;
                    }else
                    {
                        //try lemma in the first word
                        if (j == i)
                        {
                            searchText = sentence[i].Lemma;
                             tryMatch = findMatch(entities, searchText);
                            if (tryMatch.Count() != 0)
                            {
                                match = tryMatch;
                            }
                            else
                            {
                               
                                break;
                            }
                        }else
                        {
                            break;
                        }
                    }
                    
                }

                if (match != null && match.Count() != 0)
                {
                         //TODO implament selector or create multiple answer
                        var entity = match.FirstOrDefault();
                        for(int k = i; k <= j-1; k++)
                        {
                            sentence.RemoveAt(i);
                        }
                    //foreach(var w in match)
                    //  {
                        var w = match.FirstOrDefault();
                         if(w != null) { 
                                sentence.Insert(i, new WordObject(entity.entityValue, (WordType)Enum.Parse(typeof(WordType), entity.entityType)));
                          }
                    //i = i + match.Count() - 1;
                }
 
            }
            return sentence;
        }

        public IQueryable<entity> findMatch(IQueryable<entity> quarible, string text)
        {
            quarible = quarible.Where(x => x.entitySynonimus.Contains(";" +text +" ") || x.entitySynonimus.Contains(";" + text + ";"));
            return quarible;
        }


        public void searchAllAnswerForentities()
        {
            List<entity> entList = new List<entity>(); 
            foreach (var s in DBctrl.getAllSubQuestions())
            {
                var sentenses = meniAnalize(s.answerText,false);
                foreach(var sen in sentenses)
                {
                    var relevant = sen.Where(x => x.isEntity());
                    foreach(var w in relevant)
                    {
                      
                        var wText = w.Lemma == null || w.Lemma.Length == 1 || w.Text.Split(' ').Length > 1 ? w.Text : w.Lemma;
                        if (wText == null)
                        {

                        }

                        var ent = new entity();
                        ent.entityValue = wText;
                        ent.entityType = Enum.GetName(typeof(WordType),w.WordT);
                        ent.entitySynonimus = ";" + wText + ";";
                        entList.Add(ent);
                    }
                }
            }
            DBctrl.saveEntitiesFromQuestions(entList);
        }



        //public    string getClass(string text)
        //{
        //    //    var a = ma.createSentence(inputText);
        //    var context = new TextContext();
        //    var sen = meniAnalize(text);
        //    string res = null;
        //    ContentList input = new ContentList();
        //    foreach (var s in sen)
        //    {
        //        foreach (var w in s.Words)
        //        {
                    
        //            switch (w.Text)
        //            {
        //                case "א":
        //                case "'א":
        //                case "אלף":

        //                    res = "א";
        //                    break;
        //                case "ב":
        //                case "'ב":
        //                case "בית":

        //                    res = "ב";
        //                    break;
        //                case "ג":
        //                case "'ג":
        //                case "גימל":
        //                    res = "ג";

        //                    break;
        //                case "ד":
        //                case "'ד":
        //                case "דלת":
        //                    res = "ד";

        //                    break;
        //                case "ה":
        //                case "'ה":
        //                case "הי":
        //                    res = "ה";

        //                    break;
        //                case "ו":
        //                case "'ו":
        //                case "וו":

        //                    res = "ו";
        //                    break;

        //                case "ז":
        //                case "'ז":
        //                case "זוד":
        //                case "שזשזת":
        //                    res = "ז";
        //                    break;

        //                case "ח":
        //                case "'ח":
        //                case "חית":
        //                    res = "ח";
        //                    break;

        //                case "ט":
        //                case "'ט":
        //                case "טוד":
        //                case "חמישית":
        //                case "חמשוש":
        //                case "חמשושית":
        //                    res = "ט";
        //                    break;




        //                case "י":
        //                case "'י":
        //                case "יוד":
        //                case "שישית":
        //                case "שישיסט":
        //                case "שישיסטית":
        //                    res = "י";
        //                    break;

        //                case "יא":
        //                case "י\"א":
        //                case "יא'":
        //                case "'יא":
        //                case "שביעית":
        //                case "שביעיסט":
        //                case "שביעיסטית":
        //                    res = "יא";
        //                    break;

        //                case "יב":
        //                case "י\"ב":
        //                case "יב'":
        //                case "'יב":
        //                case "שמינית":
        //                    res = "יב";
        //                    break;


        //            }
        //        }
        //    }

        //    return res;
        //}





        public   string removeParentheses(string input, char start, char end)
        {
            string res = input;
            while (res.Contains(start) && res.Contains(end))
            {
                string s = input.Substring(0, input.IndexOf(start) - 1);
                string e = input.Substring(input.IndexOf(end) + 1);
                res = s + e;
            }

            return res;
        }



        //public Sentence createSentence(string inputText)
        //{

        //    var sentence = new Sentence(inputText);

        //    m_lemmatizer.SetStream(new System.IO.StringReader(inputText));

        //    string word = string.Empty;
        //    List<HebMorph.Token> tokens = new List<HebMorph.Token>();
        //    while (m_lemmatizer.LemmatizeNextToken(out word, tokens) > 0)
        //    {
        //        if (tokens.Count == 0)
        //        {
        //            throw new Exception("{0}: Unrecognized word" + word);
        //        }



        //        string curWord = string.Empty;
        //        //   foreach (Token r in tokens)
        //        //    {



        //        //  HebrewToken ht = tokens[0] as HebrewToken;



        //        HebrewToken ht = findBestToken(tokens);


        //        var a = radix.Lookup(ht.Text);

        //        if (ht == null)
        //            continue;

        //        var dictWord = HebDictionary.get(ht.Lemma);

        //        if (ht.Mask.HasFlag(DMask.D_VERB))
        //        {
        //            sentence.Add(new GufWord(ht));
        //            sentence.Add(new VerbWord(ht));
        //        }
        //        else if (ht.Mask.HasFlag(DMask.D_NOUN))
        //        {
        //            sentence.Add(new NounWord(ht));
        //            sentence.Add(new GufWord(ht));
        //        }
        //        else if (dictWord is PrepWord)
        //        {
        //            createPrep(ht);
        //            sentence.Add(new unknownWord(ht));
        //            sentence.Add(new GufWord(ht));
        //        }
        //        else if (dictWord != null)
        //        {
        //            sentence.Add(dictWord);
        //        }
        //        else
        //        {
        //            sentence.Add(new unknownWord(ht));
        //        }

        //        //    }



        //    }

        //    if (inputText.EndsWith("?"))
        //    {
        //        sentence.Add(new MarkWord("?"));
        //    }
        //    else if (inputText.EndsWith("!"))
        //    {
        //        sentence.Add(new MarkWord("!"));
        //    }
        //    return sentence;
        //}

        //private HebrewToken findBestToken(List<HebMorph.Token> tokens)
        //{

        //    //reduce verb score (strange verbs..)
        //    foreach (HebrewToken ht in tokens)
        //    {
        //        if (ht.Mask.HasFlag(DMask.D_VERB))
        //        {
        //            ht.Score *= 0.9f;
        //        }
        //    }





        //    var hebComp = new HebComp();
        //    tokens.Sort(hebComp);
        //    return tokens[0] as HebrewToken;
        //}



        //class HebComp : IComparer<HebMorph.Token>
        //{
        //    public int Compare(HebMorph.Token x, HebMorph.Token y)
        //    {

        //        var xt = x as HebrewToken;
        //        var yt = y as HebrewToken;

        //        var diff = (int)((yt.Score - xt.Score) * 100);
        //        return diff;
        //    }
        //}

        //PrepositionObject createPrep(HebrewToken ht)
        //{


        //    ht.Mask = DMask.D_ADJ;
        //    if (ht.Lemma != ht.Text)
        //    {
        //        var diff = ht.Text.Remove(0, ht.Lemma.Length);
        //        if (diff == "ך" | diff == "יך")
        //        {
        //            ht.Mask = ht.Mask | DMask.D_SECOND | DMask.D_SINGULAR | DMask.D_ADJ;
        //        }
        //        else if (diff == "י" | diff == "יי")
        //        {
        //            ht.Mask = ht.Mask | DMask.D_FIRST | DMask.D_SINGULAR | DMask.D_MASCULINE;
        //        }
        //        else if (diff == "ו" | diff == "יו")
        //        {
        //            ht.Mask = ht.Mask | DMask.D_SECOND | DMask.D_SINGULAR | DMask.D_MASCULINE;
        //        }
        //        else if (diff == "ה" | diff == "יה")
        //        {
        //            ht.Mask = ht.Mask | DMask.D_SECOND | DMask.D_SINGULAR | DMask.D_FEMININE;
        //        }
        //        else if (diff == "כם" | diff == "יכם")
        //        {
        //            ht.Mask = ht.Mask | DMask.D_SECOND | DMask.D_NUMMASK | DMask.D_MASCULINE;
        //        }
        //        else if (diff == "כן" | diff == "יכן")
        //        {
        //            ht.Mask = ht.Mask | DMask.D_SECOND | DMask.D_NUMMASK | DMask.D_FEMININE;
        //        }
        //        else if (diff == "נו" | diff == "ינו")
        //        {
        //            ht.Mask = ht.Mask | DMask.D_FIRST | DMask.D_NUMMASK;
        //        }
        //        else if (diff == "הם" | diff == "יהם")
        //        {
        //            ht.Mask = ht.Mask | DMask.D_THIRD | DMask.D_NUMMASK | DMask.D_MASCULINE; ;
        //        }
        //        else if (diff == "הן" | diff == "יהן")
        //        {
        //            ht.Mask = ht.Mask | DMask.D_THIRD | DMask.D_NUMMASK | DMask.D_FEMININE;
        //        }

        //    }



        //    return null;
        //}



        //ceack if part of the sentence is a prase
        //private List<WordObject> checkPhrases(List<WordObject> sentence)
        //{

        //    var words = sentence;


        //    //length
        //    for (int i = words.Count; i > 1; i--)
        //    {

        //        //start
        //        for (int k = 0; k + i <= words.Count; k++)
        //        {
        //            var str = "";
        //            //accemulate
        //            for (int j = 0; j < i && k + j < words.Count; j++)
        //            {
        //                str += words[k + j].Text + " ";
        //            }


        //            if (hebDictionary.contains(str.Trim()))
        //            {
        //                sentence.RemoveRange(k, i);
        //                sentence.Insert(k, hebDictionary.get(str.Trim()));
        //            }


        //        }

        //    }


        //    return sentence;
        //}



        //public   String getName(string inputText)
        //{

        //    //    var a = MorfAnalizer.createSentence(inputText);
        //    var context = new TextContext();
        //    var sen = meniAnalize(inputText);
        //    var sa = new SemanticAnalizer();

        //    if (sen.Count == 1 && sen[0].Count == 1 && sen[0][0].isA(nounWord))
        //    {
        //        return sen[0][0].Text;
        //    }

        //    ContentList input = new ContentList();
        //    foreach (var s in sen)
        //    {
        //        foreach (var w in s)
        //        {
        //            if (sa.isAName(w))
        //            {
        //                return w.Text;
        //            }
        //        }
        //    }

        //    return null;

        //}

        //public   string GetGender(string text)
        //{
        //    //    var a = MorfAnalizer.createSentence(inputText);
        //    var context = new TextContext();
        //    var sen = meniAnalize(text);

        //    ContentList input = new ContentList();
        //    foreach (var s in sen)
        //    {
        //        foreach (var w in s)
        //        {
        //            if (w.Text == "בן")
        //            {
        //                return "masculine";
        //            }
        //            if (w.Gender != personObject.genderType.unspecified)
        //            {
        //                return w.Gender.ToString();
        //            }
        //        }
        //    }

        //    return null;
        //}

        //public   string GetGeneralFeeling(string text)
        //{
        //    //    var a = MorfAnalizer.createSentence(inputText);
        //    if (text.Contains("לא טוב") || text.Contains("רע") || text.Contains("גרוע") || text.Contains("על הפנים"))
        //    {
        //        return "good";
        //    }
        //    else if (text.Contains("טוב") || text.Contains("סבבה") || text.Contains("מצויין") || text.Contains("אחלה"))
        //    {
        //        return "bad";
        //    }
        //    else
        //    {
        //        return "netural";
        //    }
        //}

    }
    }
