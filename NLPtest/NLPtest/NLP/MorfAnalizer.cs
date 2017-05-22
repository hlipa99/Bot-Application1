
using static NLP.HebWords.WordObject.WordType;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLP.WorldObj;
using hebrewNER;
//using java.io;
//using static vohmm.corpus.Sentence;
using System.Text.RegularExpressions;
using NLP.view;
using NLP.Controllers;
//using java.awt;
using System.Xml.Serialization;
using NLP.MorfObjects;
using Newtonsoft.Json;
using Model.dataBase;
using static NLP.HebWords.WordObject;
using NLP.HebWords;
using Model.Models;
using System.Data.Entity;
using static NLP.WorldObj.personObject;

namespace NLP.NLP
{
    public class MorfAnalizer
    {

        private HebDictionary hebDictionary;
        OuterAPIController httpCtrl = new OuterAPIController();
        DataBaseController DBctrl = new DataBaseController();

        public OuterAPIController HttpCtrl
        {
            get
            {
                return httpCtrl;
            }

            set
            {
                httpCtrl = value;
            }
        }

        internal void updateEntities(IEnumerable<IentityBase> enumerable)
        {
            foreach(var e in enumerable)
            {
                DBctrl.addUpdateEntity(e);
            }
        }

        public DataBaseController DBctrl1
        {
            get
            {
                return DBctrl;
            }

            set
            {
                DBctrl = value;
            }
        }



        public MorfAnalizer()
        {
            hebDictionary = new HebDictionary();
        }


        public List<WordObject> getWordsObjectFromParserServer(String str)
        {
            List<WordObject> sentenceFromServer = new List<WordObject>();
            var res = new List<WordObject>();
            try
            {


                string JsonRes = HttpCtrl.sendToHebrewMorphAnalizer(str);

                if (JsonRes != null && JsonRes != "")
                {
                    sentenceFromServer = JsonConvert.DeserializeObject<List<WordObject>>(JsonRes);
                }

                //may be mispelling for the first time


                var firstWord = true;
                // print tagged sentence by using AnalysisInterface, as follows:
                foreach (WordObject w in sentenceFromServer)
                {


                    WordObject word = w;

                    //two NRI in a row
                    //join word if ist part of a name
                    if (res.Count > 0)
                    {
                        var last = res.LastOrDefault();
                        if (((last.Ner == word.Ner && last.Ner != "O") || (last.isA(properNameWord) && word.isA(properNameWord))) &&
                            !word.Prefixes.Contains("ו") && !firstWord)
                        {
                            res.LastOrDefault().Text = res.LastOrDefault().Text.Remove(0, res.LastOrDefault().Prefixes.Count());
                            res.LastOrDefault().Text += " " + word.Text;
                            res.LastOrDefault().Lemma = res.LastOrDefault().Text;
                            continue;
                        }
                    }
                    firstWord = false;
                    res.Add(w);
                }
            }
            catch (Exception ex) //if parser server is down
            {
                var words = str.Split(' ');
                foreach (var w in words)
                {
                    var word = new WordObject(w, nounWord);
                    sentenceFromServer.Add(word);
                }
            }

            return res;
        }





        public virtual List<List<WordObject>> meniAnalize(String str, bool isUserInput)
        {
            List<List<WordObject>> allRes = new List<List<WordObject>>();
            if (str != null)
            {
                str = str.Trim();
                if (str != null && str.Length > 0)
                {

                    if (isUserInput)
                    {
                        var correctSpelling = HttpCtrl.correctSpelling(str);
                        if (correctSpelling != null)
                        {
                            str = correctSpelling.Replace("\\", "");
                        }
                    }


                    var strRes = removeParentheses(str, '(', ')');
                    strRes = removeParentheses(strRes, '[', ']');
                    var sentenceFromServer = getWordsObjectFromParserServer(strRes);
                    

                    if (sentenceFromServer != null && sentenceFromServer.Count >= 0)
                    {

                        var allText = splitByLine(sentenceFromServer);
                        List<WordObject> res = null;
                        foreach (var sentence in allText)
                        {
                            res = new List<WordObject>();
                            if (sentence != null && sentence.Count >= 0)
                            {
                                //remove nikod etc.s
                                var firstWord = true;
                                // print tagged sentence by using AnalysisInterface, as follows:
                                foreach (WordObject w in sentence)
                                {


                                    WordObject word = w;
                                    //         if (hebDictionary.contains(w.Text))
                                    //       {
                                    //              word = hebDictionary.get(word.Text);
                                    //         }
                    
                                    //joinwords
                                    //if (res.LastOrDefault() != null && word.isA(nounWord) && res.LastOrDefault().isA(nounWord))
                                    //{
                                    //    var last = res.LastOrDefault();
                                    //    res.RemoveAt(res.Count - 1);
                                    //    last.Text = last.Text + " " + word.Text;
                                    //    last.WordT = last.WordT | word.WordT; //combin flages
                                    //    word = last;
                                    //}

                                    res.Add(word);

                                }
                                res.RemoveAll(x => (x.Text.Length <= 1) && (x.Pos == "punctuation") && (x.Text != "|"));
                                res = tryMatchEntities(res, isUserInput);

                            }

                            // res = checkPhrases(res);

                            allRes.Add(res);
                        }
                    }



                }
            }
            return allRes;
        }

        internal void updateEntities()
        {
            var db = DBctrl1.updateDBmanual();
            var list = new List<IMultyEntity>();
            foreach (var e in db.entity)
            {
                if (e.entityValue.Split(' ').Count() > 1)
                {
                    var val = e.entityValue.Trim().Replace(' ', '#');
                    var newmulty = new multyEntity();
                    newmulty.entityValue = e.entityValue;
                    newmulty.parts = ";" + val + ";";
                    newmulty.entityType = e.entityType;
                    list.Add(newmulty);

                    foreach (var w in e.entityValue.Split(' '))
                    {
                        if (db.entity.Where(x => x.entitySynonimus.Contains(";" + w + ";")).Count() == 0)
                        {
                            var ent = new entity();
                            ent.entityValue = w;
                            ent.entityType = e.entityType;
                            var word = getWordsObjectFromParserServer(w).Single();
                            if (word.Lemma != w)
                            {
                                ent.entitySynonimus = (";" + w + ";" + word.Lemma + ";");
                            }
                            else
                            {
                                ent.entitySynonimus = (";" + w + ";");
                            }
                            db.entity.Add(ent);

                        }
                    }
                }

                foreach (var s in e.entitySynonimus.Split(';'))
                {
                    if (s != "")
                    {
                        if (s.Split(' ').Count() > 1)
                        {
                            var val = e.entityValue.Trim().Replace(' ', '#');
                            var newmulty = new multyEntity();
                            newmulty.entityValue = s;
                            newmulty.parts = ";" + val + ";";
                            newmulty.singleValue = e.entityValue;
                            newmulty.entityType = e.entityType;
                            list.Add(newmulty);

                            foreach (var w in e.entityValue.Split(' '))
                            {
                                if (db.entity.Where(x => x.entitySynonimus.Contains(";" + w + ";")).Count() == 0)
                                {
                                    var ent = new entity();
                                    ent.entityValue = w;
                                    ent.entityType = e.entityType;
                                    var word = getWordsObjectFromParserServer(w).Single();
                                    if (word.Lemma != w)
                                    {
                                        ent.entitySynonimus = (";" + w + ";" + word.Lemma + ";");
                                    }
                                    else
                                    {
                                        ent.entitySynonimus = (";" + w + ";");
                                    }
                                    db.entity.Add(ent);

                                }
                            }
                        }
                    }
                }



            }

            foreach (var m in list)
            {
                var curr = db.multyEntity.Where(x => x.entityValue == m.entityValue);
                if (curr.Count() != 0)
                {
                    var c = curr.Single();
                    c.parts = (c.parts + m.parts).Replace(";;", ";");
                }
                else
                {
                    db.multyEntity.Add(m as multyEntity);
                }

            }
            db.SaveChanges();



        }

        private List<List<WordObject>> splitByLine(List<WordObject> wordObjectList)
        {
            List<List<WordObject>> result = new List<List<WordObject>>();
            var currentSentence = new List<WordObject>();
            foreach (var w in wordObjectList)
            {
                if (w.Text == "|")
                {
                    if (currentSentence.Any())
                    {
                        result.Add(currentSentence);
                    }
                    currentSentence = new List<WordObject>();
                }
                else
                {
                    currentSentence.Add(w);
                }
            }
            result.Add(currentSentence);
            return result;
        }

        //private List<WordObject> fixFunctuation(List<WordObject> sentenceFromServer)
        //{
        //    List<WordObject> newList = new List<WordObject>();

        //    for (int i = 0; i < sentenceFromServer.Count; i++)
        //    {
        //        var word = sentenceFromServer[i];
        //        if (word.Text == "\\")
        //        {
        //            if (i + 2 < sentenceFromServer.Count && newList.Any())
        //            {
        //                if (sentenceFromServer[i + 1].Text == "'")
        //                {
        //                    newList.LastOrDefault().Lemma += "'" + sentenceFromServer[i + 1];
        //                    newList.LastOrDefault().Text = newList.LastOrDefault().Lemma;
        //                    i = i + 2;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            newList.Add(sentenceFromServer[i]);
        //        }

        //    }

        //    return newList;

        //}

        IEnumerable<Ientity> entities;
        IEnumerable<IMultyEntity> multyEntities;
      


        //find entities and multyEntities in the words list
        private List<WordObject> tryMatchEntities(List<WordObject> sentence, bool isUserInput)
        {
            //increase the match found to implement maximal munch
            if (entities == null) { entities = DBctrl1.getEntitys().ToList(); }
            if (multyEntities == null) { multyEntities = DBctrl1.getMultyEntitys().ToList(); }
            var newSentence = new List<WordObject>();

            List<IEnumerable<IentityBase>> matchedEntity = findMathedEntities(sentence);

            for (int i = 0; i < matchedEntity.Count; i++)
            {
                var ent = matchedEntity[i];
                var resEntityPart = new List<IentityBase>();
                var res = findMultyMatch(multyEntities, matchedEntity, i, resEntityPart).Distinct();
                var max = -1;
                if (res.Any()) max = res.Max(x => x.entityID);



                if (max > 0)
                {
                    //original index of the first word entity
                    var idx = matchedEntity[i].First().entityID;
                    matchedEntity.RemoveRange(i, max);
                    var multyRes = new List<IentityBase>();
                    foreach (IMultyEntity me in res)
                    {
                        if ((me.singleValue != null && me.singleValue != ""))
                        {
                            var singleValue = entities.Where(x => x.entityValue == me.singleValue);
                            if (singleValue != null && singleValue.Any())
                            {
                                singleValue.Single().entityID = idx;
                                multyRes.Add(singleValue.Single().clone());

                            }
                        }
                        else
                        {
                            me.entityID = idx;
                            multyRes.Add(me.clone());
                        }

                    }
                    matchedEntity.Insert(i, multyRes);


                }

            }


            //turn entitys for words objects
            foreach (var e in matchedEntity)
            {
                if (!isUserInput)
                {
                    var list = e.ToList();
                    list.Sort((x, y) => entitySelector(x) - entitySelector(y));
                    var ent = list.FirstOrDefault();
                    WordObjectFromEntity(sentence, newSentence, ent);

                }
                else //in user input, we save all the possibileties
                {

                    foreach (var w in e)
                    {
                        WordObjectFromEntity(sentence, newSentence, w);
                    }
                }
            }


            return newSentence;
        }

        private List<IEnumerable<IentityBase>> findMathedEntities(List<WordObject> sentence)
        {
            var matchedEntity = new List<IEnumerable<IentityBase>>();
            for (int i = 0; i < sentence.Count; i++)
            {
                var tryMatch = findMatch(entities, sentence[i].Lemma.Trim());
                if (tryMatch != null && tryMatch.Any())
                {
                    tryMatch.ToList().ForEach(x => x.entityID = i);
                    matchedEntity.Add(new List<IentityBase>(tryMatch.Select(x => x.clone())));
                    //   addMatch(sentence[i], isUserInput, newSentence, match);
                }
            }

            return matchedEntity;
        }


        private void WordObjectFromEntity(List<WordObject> sentence, List<WordObject> newSentence, IentityBase ent)
        {

            var newWord = sentence[ent.entityID].clone();
            newWord.WordT = WordObject.typeFromString(ent.entityType);

            if (ent.entityType == "organizationWord")
            {
                newWord.Amount = amountType.plural;
                newWord.Gender = genderType.masculine;
            }else if (ent.entityType == "locationWord")
                {
                    newWord.Amount = amountType.singular;
                    newWord.Gender = genderType.feminine;
                }
            else if (ent.entityType == "eventWord")
            {
                newWord.Amount = amountType.singular;
                newWord.Gender = genderType.masculine;
            }


            IMultyEntity mEnt;
            if (ent.entityValue.Split(' ').Count() > 0) {
                newWord.Text = ent.entityValue;
                if ((mEnt = ent as IMultyEntity) != null){
                    newWord.Lemma = mEnt.parts.Split(';')[0];
                }else{
                    newWord.Lemma = newWord.Text;
                }
              
            }
            else
            {
                newWord.Lemma = ent.entityValue;
            }

            newWord.WordT = WordObject.typeFromString(ent.entityType);
            newSentence.Add(newWord);
        }


        public List<IentityBase> findMatchingEntities(string text)
        {
            text = text.Trim();
            var res = new List<IentityBase>();
            if (entities == null) { entities = DBctrl1.getEntitys().ToList(); }
            if (multyEntities == null) { multyEntities = DBctrl1.getMultyEntitys().ToList(); }
            var words = getWordsObjectFromParserServer(text);
            var ent = findMathedEntities(words);
            var resEntityPart = new List<IentityBase>();
            IEnumerable<IentityBase> multyMatch = new List<IentityBase>();
            if (text.Split(' ').Count() > 1) {
                 multyMatch = findMultyMatch(multyEntities, ent, 0, resEntityPart).Distinct();
                if (multyMatch.Any())
                {


                    if (ent.Count() > 0 && ent[0].Where(e => multyMatch.Where(me => ((multyEntity)me).parts.Contains(e.entityValue)).Any()).Any())
                    {
                        res.AddRange(multyMatch);
                    }
                    else
                    {
                        foreach (var w in words)
                        {
                            DBctrl.addNewEntity(w.Lemma, w.getTypeString());
                        }
                    }

                } else if (words.TrueForAll(x => x.isA(WordType.properNameWord)))
                {

                    var newEnt = new entity();
                    newEnt.entityType = "personWord";
                    var name = "";
                    if (words.Count > 1)
                    {
                        name = words[0].Text.Remove(0, words[0].Prefixes.Where(x => x != "ה").Count());
                        for (int i = 1; i < words.Count(); i++)
                        {
                            name += words[i].Text;
                        }
                    }
                    else
                    {
                        name = words[0].Text;
                    }
                    newEnt.entityValue = name;
                    newEnt.entitySynonimus = ";" + name + ";";
                    res.Add(newEnt);
                }
                else
                {
                    var multyEntityVal = text.Remove(0, words[0].Prefixes.Where(x => x != "ה").Count());

                    var parts = ";";
                    foreach (var w in words)
                    {
                        parts += w.Lemma + "#";

                        var newEnt = new entity();
                        newEnt.entityValue = w.Text.Remove(0, w.Prefixes.Count());
                        newEnt.entitySynonimus = ";" + w.Lemma + ";";
                        newEnt.entityType = w.getTypeString();
                        res.Add(newEnt);
                    }

                    parts = parts.TrimEnd('#') + ";";
                    var mulEntity = new multyEntity();
                    mulEntity.entityType = "nounWord";
                    mulEntity.parts = parts;
                    mulEntity.entityValue = multyEntityVal;
                    res.Add(mulEntity);
                }
            }


            foreach(var e in ent)
            {
                res.AddRange(e);
            }

            if (res.Count() == 0) {
                foreach (var w in words)
                {
                    var newEnt = new entity();
                    newEnt.entityValue = w.Text.Remove(0, w.Prefixes.Count());
                    newEnt.entitySynonimus = ";" + w.Lemma + ";";
                    newEnt.entityType = w.getTypeString();
                    res.Add(newEnt);
                }
            }
     
            return res.Distinct().ToList();
        }

        public bool findMatchingEntities(IEnumerable<IentityBase> ent)
        {
            try
            {
                foreach(var e in ent)
                {
                    DBctrl.addUpdateEntity(e);
                }
            }catch(Exception ex)
            {
                return false;
            }



            return true;
        }



            //finds multyEntity match with maximal munch using recursive calls,matchedMultyEntity satrt with the full multy list
            private IEnumerable<IentityBase> findMultyMatch(IEnumerable<IMultyEntity> matchedMultyEntity, List<IEnumerable<IentityBase>> matchedEntity, int i, List<IentityBase> selected)
        {
            List<IentityBase> match = new List<IentityBase>();

            if (matchedMultyEntity != null && matchedMultyEntity.Any() && matchedEntity.Count > i)
            {
                IEnumerable<IMultyEntity> multyMatch = new List<IMultyEntity>();
                foreach (var e in matchedEntity[i])
                {
                    selected.Add(e);
                    if (((multyMatch = findMultyMatch(matchedMultyEntity, e.entityValue)).Count() > 0))
                    {

                         
                          var res = findMultyMatch(multyMatch, matchedEntity, i + 1, selected);
                            if (res.Count() > 0)
                            {
                                match.AddRange(res);
                        } else
                         {
                              match.AddRange(containAllEntities(multyMatch, selected).ToList());
                              
                         }
                    }
                    selected.Remove(selected.Last());
                }
            }

            
            if (match.Any()){ return match;}
            else {  return match; }
           
        }

        //helper function, filtering  multyMatch only where all the selected entities are present
        private IEnumerable<IentityBase> containAllEntities(IEnumerable<IMultyEntity> multyMatch, List<IentityBase> selected)
        {
            var res = new List<IMultyEntity>();
            var selectedSet = selected.Select(x => x.entityValue);
            foreach (var match in multyMatch)
            {
                foreach (var multyEntity in match.parts.Split(';').Where(x => x != ""))
                {
                    IEnumerable<string> entitySet = multyEntity.Split('#');
                    entitySet = entitySet.Select(x => x.Trim());
                    if ((entitySet.Count() == selectedSet.Count()) && !entitySet.Except(selectedSet).Any())
                    {
                        match.entityID = entitySet.Count();
                        res.Add(match);
                    }
                }
            }
            return res;
        }

        private IEnumerable<IMultyEntity> findMultyMatch(IEnumerable<IMultyEntity> matchedEntity,string ent)
        {
            return matchedEntity.Where(x => x.parts.Split('#', ';').Any(e => e.Trim() == ent));
        }

        private string removePrefix(string searchText, WordObject wordObject)
        {
            foreach (var c in wordObject.Prefixes)
            {
                //   if(c != "ה" && searchText.Length > 0)
                searchText = searchText.Remove(0, 1);
            }

            return searchText;
        }

        private int entitySelector(IentityBase x)
        {
            switch (x.entityType)
            {
                case ("eventWord"): return 0;
                case ("personWord"): return 1;
                case ("locationWord"): return 2;
                case ("organizationWord"): return 3;
                case ("conceptWord"): return 4;
                case ("nounWord"): return 5;
                case ("verbWord"): return 6;
                default: return 99;
            }
        }

        private IEnumerable<Ientity> findMatch(IEnumerable<Ientity> quarible, string text)
        {
            quarible = quarible.Where(x => x.entitySynonimus != null && x.entitySynonimus.Contains(";" + text + ";"));
            return quarible;
        }


        public void searchAllAnswerForentities()
        {
            List<entity> entList = new List<entity>();
            var entities = DBctrl1.getEntitys().AsQueryable();
            foreach (var s in DBctrl1.getAllSubQuestions())
            {
                var sentenses = getWordsObjectFromParserServer(s.answerText);

                    var relevant = sentenses.Where(x => x.isEntity());
                    foreach (var w in relevant)
                    {
                       
                        if (!findMatch(entities, w.Text).Any() && !findMatch(entities, w.Lemma).Any())
                        {
                            var ent = new entity();
                            ent.entitySynonimus = ";" + w.Lemma + ";";
                            ent.entityType = Enum.GetName(typeof(WordType), w.WordT);
                            ent.entityValue = w.Text;
                            entList.Add(ent);
                        }
                    }
                
            }


            DBctrl1.saveEntitiesFromQuestions(entList);
        }



        private string removeParentheses(string input, char start, char end)
        {
            string res = input;
            var counter = 0;
            while (res.Contains(start) && res.Contains(end) && counter < 5)
            {
                string s = input.Substring(0, input.IndexOf(start) - 1);
                string e = input.Substring(input.IndexOf(end) + 1);
                res = s + e;
                counter++;
            }

            return res;
        }

        


        private void addMatch(WordObject word, bool isUserInput, List<WordObject> newSentence, IEnumerable<Ientity> match)
        {
            Ientity entity = null;
            if (match.Count() > 1)
            {
                var list = match.ToList();
                list.Sort((x, y) => entitySelector(x) - entitySelector(y));
                entity = list.FirstOrDefault();
            }
            else
            {
                entity = match.FirstOrDefault();
            }

            if (!isUserInput)
            {
                var newWord = word.clone(); ;
                newWord.Text = entity.entityValue;
                newWord.Lemma = entity.entityValue;
                newWord.WordT = WordObject.typeFromString(entity.entityType);
                newSentence.Add(newWord);

            }
            else
            {
                foreach (var w in match)
                {
                    var newWord = word.clone(); ;
                    newWord.Text = w.entityValue;
                    newWord.Lemma = w.entityValue;
                    newWord.WordT = WordObject.typeFromString(w.entityType);
                    newSentence.Add(newWord);

                }
            }

        }



        public string getClass(string text)
        {
            //    var a = ma.createSentence(inputText);
            var context = new TextContext();
            text = text.Replace("\"", "");
            text = text.Replace("'", "");
            text = text.Replace("`", "");
            text = text.Replace("'", "");
            string res = null;

            foreach (var s in text.Split(' '))
            {

                    switch (s)
                    {
                    
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

            return res;
        }
    }
}
