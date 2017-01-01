
using static NLPtest.Word.WordType;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLPtest.WorldObj;
using vohmm.application;
using hebrewNER;
using yg.sentence;
using yg.chunker;
using java.io;
using static vohmm.corpus.Sentence;
using vohmm.corpus;
using System.Text.RegularExpressions;
using NLPtest.view;

namespace NLPtest
{
    static public class MorfAnalizer
    {

        static SimpleTagger3 tagger;


        static NERTagger nerTagger;
        static MeniTaggeedSentenceFactory sentenceFactory;
        static TaggerBasedHebrewChunker chunker;
        private static HebDictionary hebDictionary;

        static public void initialize()
        {
            //    hspellPath = Directory.GetCurrentDirectory() + "..\\..\\..\\hspell-data-files";
            //   radix = Loader.LoadDictionaryFromHSpellFolder(hspellPath, true);
            //if (m_lemmatizer == null || !m_lemmatizer.IsInitialized || !(m_lemmatizer is StreamLemmatizer))
            //{

            //    if (hspellPath == null) throw new Exception("hspellPath");


            //    radix.AllowValueOverride = true;
            //    var md = new MorphData();
            //    md.Lemmas = new string[] { "היי" };
            //    md.Prefixes = 0;
            //    md.DescFlags = new DMask[] { DMask.D_CUSTOM };
            //    radix.AddNode("היי", md);

            //    m_lemmatizer = new HebMorph.StreamLemmatizer(radix, false);
            //}

            var path = "C:/Program Files (x86)/IIS Express/botServer/hebdata/";
            tagger = new SimpleTagger3(path);
            nerTagger = new NERTagger(path, tagger);
           //  create the noun-phrase chunker
            sentenceFactory = new MeniTaggeedSentenceFactory(null, MeniTokenExpander.expander);
            String chunkModelPrefix = path + vohmm.util.Dir.CHUNK_MODEL_PREF;
            chunker = new TaggerBasedHebrewChunker(sentenceFactory, chunkModelPrefix);
            hebDictionary = new HebDictionary();
        }

        public static List<Sentence> meniAnalize(String str)
        {

            // The follwoing object constructions are heavy - SHOULD BE APPLIED ONLY ONCE!
            // create the morphological analyzer and disambiguator 

            var sentenses = str.Split('.');
            List<Sentence> allRes = new List<Sentence>();

            foreach (var s in sentenses)
            {

                var strRes = s;
                vohmm.corpus.Sentence sentence = null;
                Sentence res = null;
                strRes = removeParentheses(strRes, '(', ')');
                strRes = removeParentheses(strRes, '[', ']');

                if (strRes.Length > 0)
                {
                    var taggedSentences = tagger.getTaggedSentences(strRes);
                    if (taggedSentences.size() > 0)
                    {
                        sentence = (vohmm.corpus.Sentence)taggedSentences.get(0);
                        res = new Sentence(strRes);
                    }else
                    {
                        return new List<Sentence>();
                    }




                    // Named-entiry recognition for the given tagged sentence
                    nerTagger.addNerLabels(sentence);

                    //Noun-phrase chunking for the given tagged sentence (will be available soon in Java)
                    //      chunker.addBIOLabels(sentence);

                    // print tagged sentence by using AnalysisInterface, as follows:
                    foreach (TokenExt tokenExt in sentence.getTokens().toArray())
                    {
                        vohmm.corpus.Token token = tokenExt._token;

                        Anal anal = token.getSelectedAnal();


                        //    res += Environment.NewLine + "Lemma: " + anal.getLemma();

                        //    // NOTE: In our tagger we consider participle of a 'verb' type as a present verb.
                        //    // In order to adapt it to MILA's schema the last parameter of BitmaskResolver constructor should be 'false' (no present verb)
                        AnalysisInterface bitmaskResolver = new BitmaskResolver(anal.getTag().getBitmask(), token.getOrigStr(), false);



                        Word word = null;
                        var pos = bitmaskResolver.getPOS();
                        var postype = bitmaskResolver.getPOSType();
                        var origString = token.getOrigStr();
                        var gender = bitmaskResolver.getGender();
                        var gender2 = bitmaskResolver.getSuffixGender();
                        var ner = tokenExt.getNER();
                        if (hebDictionary.contains(token.getOrigStr()))
                        {
                            word = hebDictionary.get(token.getOrigStr());

                        }
                        else if (tokenExt.getNER() != "O")
                        {
                            if (tokenExt.getNER().Contains("ORG"))
                            {
                                word = new Word(token.getOrigStr(), orginazationWord | nounWord);
                            }
                            else if (tokenExt.getNER().Contains("MISC__AFF"))
                            {
                                word = new Word(token.getOrigStr(), identityWord | nounWord);
                            }
                            else if (tokenExt.getNER().Contains("PERS"))
                            {
                                word = new Word(token.getOrigStr(), personWord | nounWord);
                            }
                            else if (tokenExt.getNER().Contains("MISC_EVENT"))
                            {
                                word = new Word(token.getOrigStr(), eventWord);
                            }
                            else if (tokenExt.getNER().Contains("LOC"))
                            {
                                word = new Word(token.getOrigStr(), locationWord | nounWord);
                            }
                            else if (tokenExt.getNER().Contains("DATE"))
                            {
                                word = new Word(token.getOrigStr(), dateWord);
                            }
                            else if (tokenExt.getNER().Contains("Time"))
                            {
                                word = new Word(token.getOrigStr(), timeWord);
                            }
                            else if (tokenExt.getNER().Contains("MONEY"))
                            {
                                word = new Word(token.getOrigStr(), timeWord);
                            }
                            else if (tokenExt.getNER().Contains("PERCENT"))
                            {
                                word = new Word(token.getOrigStr(), timeWord);
                            }
                            else
                            {
                                throw new Exception("unknown NRI");
                            }

                            var f = tokenExt.getNER();
                            //two NRI in a row
                            //join word if ist part of a name
                            if (res.Words.Count > 0)
                            {
                                if (res.Words.LastOrDefault().WordT.HasFlag(word.WordT))
                                {
                                    res.Words.LastOrDefault().word += " " + word.word;
                                    continue;
                                }
                            }
                        }


                        else if (bitmaskResolver.getPOS() == "interrogative" || bitmaskResolver.getPOS() == "quantifier")
                        {
                            word = new Word(token.getOrigStr(), questionWord);

                            //if(bitmaskResolver.getPOSType == "PRONOUN" "PROADVERB" "PRODET" "PRODET" "YESNO")

                        }
                        else if (bitmaskResolver.getPOS() == "interjection")
                        {
                            word = hebDictionary.get(anal.getLemma().ToString());
                        }
                        else if (bitmaskResolver.getPOS() == "verb" || bitmaskResolver.getPOS() == "participle")
                        {
                            word = new Word(token.getOrigStr(), verbWord);
                            word.setGuf(bitmaskResolver.getSuffixPerson());
                            word.setTime(bitmaskResolver.getTense());
                            word.setAmount(bitmaskResolver.getNumber());
                            word.setGender(bitmaskResolver.getGender());
                        }
                        else if (bitmaskResolver.getPOS() == "adverb")
                        {
                            word = new Word(token.getOrigStr(), adverbWord);
                            word.setTime(bitmaskResolver.getTense());
                        }
                        else if (bitmaskResolver.getPOS() == "noun")
                        {

                                word = new Word(token.getOrigStr(), nounWord);
                            word.setGuf(bitmaskResolver.getSuffixPerson());
                            word.setAmount(bitmaskResolver.getNumber());
                            word.setGender(bitmaskResolver.getGender());
                        }
                        else if (bitmaskResolver.getPOS() == "negation")
                        {

                            word = new Word(token.getOrigStr(), negationWord);
                            word.setGuf(bitmaskResolver.getSuffixPerson());
                            word.setAmount(bitmaskResolver.getNumber());
                            word.setGender(bitmaskResolver.getGender());
                        }


                        
                        else if (bitmaskResolver.getPOS() == "pronoun" && bitmaskResolver.getPOSType() == "personal")
                        {

                            word = new Word(token.getOrigStr(), gufWord | nounWord);

                            word.setGuf(bitmaskResolver.getPerson());
                            word.setAmount(bitmaskResolver.getNumber());
                            word.setGender(bitmaskResolver.getGender());

                        }
                        else if (bitmaskResolver.getPOS() == "preposition")
                        {
                            word = hebDictionary.get(token.getOrigStr());
                            if (word == null)
                            {
                                word = new Word(token.getOrigStr(), prepWord, new PrepRelObject(null));
                            }
                        }
                        else if (bitmaskResolver.getPOS() == "punctuation")
                        {
                            if (bitmaskResolver.getPOSType() == "question-mark")
                            {
                                word = new Word(token.getOrigStr(), questionWord);
                            }
                            else if (bitmaskResolver.getPOSType() == "mark")
                            {
                                word = new Word(token.getOrigStr(), markWord);
                            }
                            else if (token.getOrigStr() == "-" || token.getOrigStr() == "–")
                            {
                                word = new Word(token.getOrigStr(), hyphenWord);
                            }
                            else
                            {
                                word = new Word(token.getOrigStr(), unknownWord);
                            }
                        }
                        else if (bitmaskResolver.getPOS() == "copula")
                        {

                            word = new Word(token.getOrigStr(), copulaWord);
                            word.setGuf(bitmaskResolver.getPerson());
                            word.setTime(bitmaskResolver.getTense());
                            word.setAmount(bitmaskResolver.getNumber());
                            word.setGender(bitmaskResolver.getGender());
                        }
                        else if (bitmaskResolver.getPOS() == "conjunction")
                        {

                            word = new Word(token.getOrigStr(), conjunctionWord);

                        }
                        else if (bitmaskResolver.getPOS() == "adjective")
                        {

                            word = new Word(token.getOrigStr(), adjectiveWord);

                        }
                        else if (bitmaskResolver.getPOS() == "numeral")
                        {

                            word = new Word(token.getOrigStr(), numeralWord);

                        }
                        else if (bitmaskResolver.getPOS() == "propername")
                        {

                            word = new Word(token.getOrigStr(), properNameWord);
                        }
                        else if (bitmaskResolver.getPOS() == "unknown")
                        {

                            word = tryGetUnknown(token.getOrigStr());
                            if(word == null)
                            {
                                word = new Word(token.getOrigStr(), unknownWord);
                            }
                        }
                        else
                        {

                            //ignor unknown char, maybe nikod
                            if (token.getOrigStr().Length > 1)
                            {
                                word = new Word(token.getOrigStr(), unknownWord);
                            }
                        }



                        //joinwords
                        if (res.Words.LastOrDefault() != null && word.isA(nounWord) && res.Words.LastOrDefault().isA(nounWord))
                        {
                            var last = res.Words.LastOrDefault();
                            res.Words.RemoveAt(res.Words.Count - 1);
                            last.word = last.word + " " + token.getOrigStr();
                            last.WordT = last.WordT | word.WordT; //combin flages
                            word = last;
                        }
                     





                            //prefixes
                            if (word != null)
                        {
                            var a = bitmaskResolver.getPrefixes();
                            if (bitmaskResolver.getPrefixes() != null)
                            {
                                foreach (Affix p in bitmaskResolver.getPrefixes().toArray())
                                {
                                    word.prefix = p.getStr();


                                    if (p.getStr() == "ל")
                                    {
                                        word.le = true;
                                    }
                                    else if (p.getStr() == "מ")
                                    {
                                        word.me = true;
                                    }
                                    else if (p.getStr() == "כ")
                                    {
                                        word.ce = true;
                                    }
                                    else if (p.getStr() == "ב")
                                    {
                                        word.be = true;
                                    }
                                    else if (p.getStr() == "ו")
                                    {
                                        word.ve = true;
                                    }
                                    else if (p.getStr() == "כש")
                                    {
                                        word.kshe = true;
                                    }

                                    if (p.getStr() == "ה")
                                    {
                                        word.ha = true;
                                    }

                                    if (p.getStr() == "ש")
                                    {
                                        word.sh = true;
                                    }




                                    //PREFIX ={
                                    //'CONJ':0x0000000000000002, 
                                    //'DEF':0x0000000000000004,  # used as a feature..
                                    //'INTERROGATIVE':0x0000000000000010,
                                    //'PREPOSITION':0x0000000000000040,
                                    //'REL-SUBCONJ':0x00000000000000100,
                                    //'TEMP-SUBCONJ':0x00000000000000200,
                                    //'TENSEINV':0x0000000000000020,
                                    //'ADVERB':0x00000000000000400,
                                    //'PREPOSITION2':0x0000000000000080, #??



                                }




                            }


                            var b = bitmaskResolver.hasPrefix();
                            var e = bitmaskResolver.hasSuffix();
                            var d = bitmaskResolver.isDefinite();

                            //ha
                            if (bitmaskResolver.isDefinite())
                            {
                                word.ha = true;
                            }


                            res.Add(word);


                        }
                        //          'ADJECTIVE':         0x0000000000010000,
                        //'ADVERB':            0x0000000000020000,
                        //'CONJUNCTION':       0x0000000000030000,
                        //'AT_PREP':           0x0000000000040000, # NOT IN MILA
                        //'NEGATION':          0x0000000000050000,
                        //'NOUN':              0x0000000000060000,
                        //'NUMERAL':           0x0000000000070000,
                        //'PREPOSITION':       0x00000000000080000,
                        //'PRONOUN':           0x0000000000090000,
                        //'PROPERNAME':        0x00000000000a0000,
                        //'PARTICLE':          0x00000000000b0000, # NOT USED
                        //#'AUXVERB':           0x00000000000c0000, # NOT USED
                        //'VERB':              0x00000000000d0000,
                        //'PUNCTUATION':       0x00000000000e0000,
                        //'INTERROGATIVE':     0x00000000000f0000,
                        //'INTERJECTION':      0x0000000000100000,
                        //'UNKNOWN':           0x0000000000110000,
                        //'QUANTIFIER':        0x0000000000120000,
                        //'EXISTENTIAL':       0x0000000000130000,
                        //'MODAL':             0x0000000000140000,
                        //'PREFIX':            0x0000000000150000,
                        //'URL':               0x0000000000160000,
                        //'FOREIGN':           0x0000000000170000,
                        //'JUNK':              0x0000000000180000,
                        //#'IMPERSONAL':        0x0000000000190000, # NOT USED
                        //'PARTICIPLE':        0x00000000001a0000,
                        //'COPULA':            0x00000000001b0000,
                        //'NUMEXP':            0x00000000001c0000,
                        //'TITULA':            0x00000000001d0000,




                        // word.root 


                        //res.Words.Add(word);
                    }


                    res = checkPhrases(res);
                    allRes.Add(res);
                }
            }

            return allRes;
        }

        private static Word tryGetUnknown(string token)
        {
            return null;
        }

        public  static string getClass(string text)
        {
            //    var a = ma.createSentence(inputText);
            var context = new Context();
            var sen = meniAnalize(text);
            string res = null;
            ContentTurn input = new ContentTurn();
            foreach (var s in sen)
            {
                foreach (var w in s.Words)
                {
                    
                    switch (w.word)
                    {
                        case "א":
                        case "'א":
                        case "אלף":

                            res = "א";
                            break;
                        case "ב":
                        case "'ב":
                        case "בית":

                            res = "ב";
                            break;
                        case "ג":
                        case "'ג":
                        case "גימל":
                            res = "ג";

                            break;
                        case "ד":
                        case "'ד":
                        case "דלת":
                            res = "ד";

                            break;
                        case "ה":
                        case "'ה":
                        case "הי":
                            res = "ה";

                            break;
                        case "ו":
                        case "'ו":
                        case "וו":

                            res = "ו";
                            break;

                        case "ז":
                        case "'ז":
                        case "זוד":
                        case "שזשזת":
                            res = "ז";
                            break;

                        case "ח":
                        case "'ח":
                        case "חית":
                            res = "ח";
                            break;

                        case "ט":
                        case "'ט":
                        case "טוד":
                        case "חמישית":
                        case "חמשוש":
                        case "חמשושית":
                            res = "ט";
                            break;




                        case "י":
                        case "'י":
                        case "יוד":
                        case "שישית":
                        case "שישיסט":
                        case "שישיסטית":
                            res = "י";
                            break;

                        case "יא":
                        case "י\"א":
                        case "יא'":
                        case "'יא":
                        case "שביעית":
                        case "שביעיסט":
                        case "שביעיסטית":
                            res = "יא";
                            break;

                        case "יב":
                        case "י\"ב":
                        case "יב'":
                        case "'יב":
                        case "שמינית":
                            res = "יב";
                            break;


                    }
                }
            }

            return res;
        }





        public static string removeParentheses(string input, char start, char end)
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
        private static Sentence checkPhrases(Sentence sentence)
        {

            var words = sentence.Words;


            //length
            for (int i = words.Count; i > 1; i--)
            {

                //start
                for (int k = 0; k + i <= words.Count; k++)
                {
                    var str = "";
                    //accemulate
                    for (int j = 0; j < i && k + j < words.Count; j++)
                    {
                        str += words[k + j].word + " ";
                    }


                    if (hebDictionary.contains(str.Trim()))
                    {
                        sentence.Words.RemoveRange(k, i);
                        sentence.Words.Insert(k, hebDictionary.get(str.Trim()));
                    }


                }

            }


            return sentence;
        }



        public static String getName(string inputText)
        {

            //    var a = MorfAnalizer.createSentence(inputText);
            var context = new Context();
            var sen = meniAnalize(inputText);
            var sa = new SemanticAnalizer();

            if (sen.Count == 1 && sen[0].Words.Count == 1 && sen[0].Words[0].isA(nounWord))
            {
                return sen[0].Words[0].word;
            }

            ContentTurn input = new ContentTurn();
            foreach (var s in sen)
            {
                foreach (var w in s.Words)
                {
                    if (sa.isAName(w))
                    {
                        return w.word;
                    }
                }
            }

            return null;

        }

        public static string GetGender(string text)
        {
            //    var a = MorfAnalizer.createSentence(inputText);
            var context = new Context();
            var sen = MorfAnalizer.meniAnalize(text);

            ContentTurn input = new ContentTurn();
            foreach (var s in sen)
            {
                foreach (var w in s.Words)
                {
                    if (w.word == "בן")
                    {
                        return "masculine";
                    }
                    if (w.gender != gufObject.genderType.unspecified)
                    {
                        return w.gender.ToString();
                    }
                }
            }

            return null;
        }

        public static string GetGeneralFeeling(string text)
        {
            //    var a = MorfAnalizer.createSentence(inputText);
            if (text.Contains("לא טוב") || text.Contains("רע") || text.Contains("גרוע") || text.Contains("על הפנים"))
            {
                return "good";
            }
            else if (text.Contains("טוב") || text.Contains("סבבה") || text.Contains("מצויין") || text.Contains("אחלה"))
            {
                return "bad";
            }
            else
            {
                return "netural";
            }
        }

        // Map bitmasks returned by Meni Adler's tagger into readable tags.
        // Yoav Goldberg 2008
        //public class bitmapsTags
        //{
        //    public enum MASKS : long
        //    {
        //        PREFIX = 0x756,// # ^ 0x4,           # 0x756 ^ DEF
        //        POS = 0x1F0000,           // bits 17-21
        //        GENDER = 0x0000000000600000, // bits 22-23
        //        NUMBER = 0x7000000,          // bits 25-27
        //        PERSON = 0x38000000,         // bits 28-30
        //        STATUS = 0x00000000c0000000, // bits 31-32
        //        TENSE = 0xE00000000,        // bits 34-36
        //        SUFFIX = 0x0000fdb000000000, // bits 37-38, 40-41, 43-48
        //        SUFF_FUNC = 0x0000003000000000, // bits 37-38
        //        SUFF_GEN = 0x0000018000000000, // bits 40-41

        //        SUFF_NUM = 0x1c0000000000,     // bits 43-45
        //        SUFF_PERS = 0xE00000000000,     // bits 46-48
        //        CONT = 0x1000000000000,    // bits 49
        //        POLARITY = 0x6000000000000,    // bits 50-51
        //        BINYAN = 0x0038000000000000, // bits 52-54 base form BINYAN
        //        CONJ_TYPE = 0x00c0000000000000, // bits 55-56 base form conjunction type
        //        PRON_TYPE = 0x0700000000000000, // bits 57-59 base form pronoun type
        //        NUM_TYPE = 0x0000000000007000, // bits 13-15 @NEW

        //        INTERROGATIVE_TYPE = 0x0001004000000020, // bits 6,39,49 @NEW
        //        QUANTIFIER_TYPE = 0x0000000100800000, // bits 24,33 @NEW
        //    }

        //    public enum POS : long
        //    {
        //        ADJECTIVE = 0x0000000000010000,
        //        ADVERB = 0x0000000000020000,
        //        CONJUNCTION = 0x0000000000030000,
        //        AT_PREP = 0x0000000000040000, // NOT IN MILA
        //        NEGATION = 0x0000000000050000,
        //        NOUN = 0x0000000000060000,
        //        NUMERAL = 0x0000000000070000,
        //        PREPOSITION = 0x00000000000080000,
        //        PRONOUN = 0x0000000000090000,
        //        PROPERNAME = 0x00000000000a0000,
        //        PARTICLE = 0x00000000000b0000, // NOT USED
        //                                       // AUXVERB =           0x00000000000c0000, // NOT USED
        //        VERB = 0x00000000000d0000,
        //        PUNCTUATION = 0x00000000000e0000,
        //        INTERROGATIVE = 0x00000000000f0000,
        //        INTERJECTION = 0x0000000000100000,
        //        UNKNOWN = 0x0000000000110000,
        //        QUANTIFIER = 0x0000000000120000,
        //        EXISTENTIAL = 0x0000000000130000,
        //        MODAL = 0x0000000000140000,
        //        PREFIX = 0x0000000000150000,
        //        URL = 0x0000000000160000,
        //        FOREIGN = 0x0000000000170000,
        //        JUNK = 0x0000000000180000,
        //        // IMPERSONAL =        0x0000000000190000, // NOT USED
        //        PARTICIPLE = 0x00000000001a0000,
        //        COPULA = 0x00000000001b0000,
        //        NUMEXP = 0x00000000001c0000,
        //        TITULA = 0x00000000001d0000,
        //        SHEL_PREP = 0x00000000001e0000, // NOT IN MILA
        //    }


        //    public enum GENDER : long
        //    {
        //        M = 0x0000000000200000,
        //        F = 0x0000000000400000,
        //        MF = 0x0000000000600000,
        //    }

        //    public enum NUMBER : long
        //    {
        //        S = 0x0000000001000000,
        //        P = 0x0000000002000000,
        //        D = 0x0000000003000000,
        //        DP = 0x0000000004000000,
        //        SP = 0x0000000005000000,
        //    }
        //    public enum PERSON : long
        //    {
        //        FIRST = 0x0000000008000000,
        //        SECOND = 0x0000000010000000,
        //        THIRD = 0x0000000018000000,
        //        A = 0x0000000020000000,
        //    }

        //    public enum STATUS : long
        //    {

        //        ABS = 0x0000000040000000,
        //        CONST = 0x0000000080000000,
        //    }
        //    public enum TENSE : long
        //    {

        //        PAST = 0x0000000200000000,
        //        ALLTIME = 0x0000000400000000,  // @NEW
        //        BEINONI = 0x0000000600000000,
        //        FUTURE = 0x0000000800000000,
        //        IMPERATIVE = 0x0000000a00000000,
        //        TOINFINITIVE = 0x0000000c00000000,
        //        BAREINFINITIVE = 0x0000000e00000000,
        //    }

        //    public enum POLARITY : long
        //    {

        //        POSITIVE = 0x0002000000000000,
        //        NEGATIVE = 0x0004000000000000,
        //    }

        //    public enum BINYAN : long
        //    {

        //        PAAL = 0x0008000000000000,
        //        NIFAL = 0x0010000000000000,
        //        HIFIL = 0x0018000000000000,
        //        HUFAL = 0x0020000000000000,
        //        PIEL = 0x0028000000000000,
        //        PUAL = 0x0030000000000000,
        //        HITPAEL = 0x0038000000000000,
        //    }


        //    public enum CONJ_TYPE : long
        //    {

        //        COORD = 0x0040000000000000,
        //        SUB = 0x0080000000000000,
        //        REL = 0x00c0000000000000
        //    }

        //    public enum PRON_TYPE : long
        //    {

        //        PERS = 0x0100000000000000, // PERSONAL
        //        DEM = 0x0200000000000000, // DEMONSTRATIVE
        //        IMP = 0x0300000000000000, // IMPERSONAL
        //        REF = 0x0400000000000000, // REFLEXIVE @@@@
        //                                  // INT =  0x0300000000000000, // INTERROGATIVE
        //                                  // REL =  0x0400000000000000, // RELATIVIZER
        //    }

        //    public enum NUM_TYPE : long
        //    {

        //        ORDINAL = 0x1000,
        //        CARDINAL = 0x2000,
        //        FRACTIONAL = 0x3000,
        //        LITERAL = 0x4000,
        //        GIMATRIA = 0x5000,
        //    }

        //    public enum INTEROGATIVE_TYPE : long
        //    {

        //        PRONOUN = 0x20,
        //        PROADVERB = 0x0000004000000000,
        //        PRODET = 0x0000004000000020,
        //        YESNO = 0x0001000000000000,
        //    }

        //    public enum QUANTIFIER_TYPE : long
        //    {

        //        AMOUNT = 0x0000000000800000,
        //        PARTITIVE = 0x0000000100000000,
        //        DETERMINER = 0x0000000100800000,
        //    }


        //    //        public enum FEATURES : long
        //    //        {
        //    //            FEATURES = {}
        //    //for f in [GENDER, NUMBER, PERSON, STATUS, TENSE, POLARITY, BINYAN, CONJ_TYPE, PRON_TYPE]=
        //    //      FEATURES.update(f)


        //    public enum SUFFIX : long
        //    {

        //        POSSESSIVE = 0x0000001000000000,
        //        ACC_NOM = 0x0000002000000000,    // This is the nominative
        //        PRONOMINAL = 0x0000003000000000,  // for ADVERBS and PREPS
        //    }

        //    public enum SHORTSUFFIX : long
        //    {

        //        //POSSESSIVE = S_PP ,
        //        //PRONOMINAL =  S_PRN ,  // for ADVERBS and PREPS
        //        //ACC_NOM =  S_ANP ,    // This is the nominative
        //        // 0 = None // NO suffix
        //    }

        //    public enum SUFF_GEN : long
        //    {

        //        M = 0x0000008000000000,
        //        F = 0x0000010000000000,
        //        MF = 0x0000018000000000,
        //    }

        //    enum SUFF_NUM : long
        //    {

        //        S = 0x0000040000000000,
        //        P = 0x0000080000000000,
        //        D = 0x00000c0000000000,
        //        DP = 0x0000100000000000,
        //        SP = 0x0000140000000000,
        //    }

        //    enum SUFF_PERS : long
        //    {
        //        FIRST = 0x0000200000000000,
        //        SECOND = 0x0000400000000000,
        //        THIRD = 0x0000600000000000,
        //        A = 0x0000800000000000,
        //    }


        //    //        enum SFEATURES : long
        //    //        {
        //    //            SFEATURES = {}
        //    //for f in SUFF_PERS, SUFF_NUM, SUFF_GEN=
        //    //   SFEATURES.update(f)

        //    enum PREFIX : long
        //    {

        //        CONJ = 0x0000000000000002,
        //        DEF = 0x0000000000000004,  // used as a feature..
        //        INTERROGATIVE = 0x0000000000000010,
        //        PREPOSITION = 0x0000000000000040,
        //        REL_SUBCONJ = 0x00000000000000100,
        //        TEMP_SUBCONJ = 0x00000000000000200,
        //        TENSEINV = 0x0000000000000020,
        //        ADVERB = 0x00000000000000400,
        //        PREPOSITION2 = 0x0000000000000080, //??
        //    }

        //    //////// map long POS name to short encoding
        //    //  enum NUMBER : long
        //    //  {
        //    //      SHORTPOS = {
        //    // ADJECTIVE =          JJ ,
        //    // ADVERB =             RB ,
        //    // CONJUNCTION =        CC ,
        //    // AT_PREP =            AT ,
        //    // NEGATION =           NEG ,
        //    // NOUN =               NN ,
        //    // NUMERAL =            CD ,
        //    // PREPOSITION =        IN ,
        //    // PRONOUN =            PRP ,
        //    // PROPERNAME =         NNP ,
        //    // VERB =               VB ,
        //    // PUNCTUATION =        PUNC ,
        //    // INTERROGATIVE =      QW ,
        //    // INTERJECTION =       INTJ ,
        //    // UNKNOWN =            UNK ,
        //    // QUANTIFIER =         DT ,
        //    // EXISTENTIAL =        EX ,
        //    // MODAL =              MD ,
        //    // PREFIX =             P ,
        //    // URL =                URL ,
        //    // FOREIGN =            FW ,
        //    // JUNK =               JNK ,
        //    // PARTICIPLE =         BN ,
        //    // COPULA =             COP ,
        //    // NUMEXP =             NCD ,
        //    // TITULA =             TTL ,
        //    // SHEL_PREP =          POS ,
        //    // PARTICLE =           PRT ,
        //    //   =   ,
        //    //}

        //    enum __PREF_PRECEDENCE : long
        //    {

        //        CONJ = 0,
        //        REL_SUBCONJ = 1,
        //        TEMP_SUBCONJ = 1,
        //        PREPOSITION = 2,
        //        ADVERB = 3,
        //        TENSEINV = 5, //@VERIFY...
        //        DEF = 4,
        //    }
        //    //}}}}}}}
    }
    }
