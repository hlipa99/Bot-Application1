
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
            List<WordObject> sentenceFromServer = new List<WordObject>(); ;
            try
            {


                string JsonRes = HttpCtrl.sendToHebrewMorphAnalizer(str);

                if (JsonRes != null && JsonRes != "")
                {
                    sentenceFromServer = JsonConvert.DeserializeObject<List<WordObject>>(JsonRes);
                }

                //may be mispelling for the first time

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
            return sentenceFromServer;
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

                                    //two NRI in a row
                                    //join word if ist part of a name
                                    if (hebDictionary.contains(w.Text))
                                    {
                                        word = hebDictionary.get(word.Text);
                                    }

                                    if (res.Count > 0)
                                    {
                                        var last = res.LastOrDefault();
                                        if (((last.Ner == word.Ner && last.Ner != "O") || (last.isA(properNameWord) && word.isA(properNameWord)))  &&
                                            !word.Prefixes.Contains("ו") && !firstWord)
                                        {
                                            res.LastOrDefault().Text += " " + word.Text;
                                            res.LastOrDefault().Lemma = res.LastOrDefault().Text;
                                            continue;
                                        }
                                    }

                             
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
                                    firstWord = false;
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
      



        private List<WordObject> tryMatchEntities(List<WordObject> sentence, bool isUserInput)
        {
            //increase the match found to implement maximal munch
            if (entities == null) { entities = DBctrl1.getEntitys().ToList(); }
            if (multyEntities == null) { multyEntities = DBctrl1.getMultyEntitys().ToList(); }
            var newSentence = new List<WordObject>();

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

            for (int i = 0; i < matchedEntity.Count; i++)
            {
                var ent = matchedEntity[i];
                var resEntityPart = new List<IentityBase>();
                var res = findMultyMatch(multyEntities,matchedEntity, i, resEntityPart).Distinct();
                var max = -1;
                if (res.Any()) max = res.Max(x => x.entityID);



                if (max > 0)
                {
                    //original index of the first word entity
                    var idx = matchedEntity[i].First().entityID;
                      matchedEntity.RemoveRange(i, max);
                    var multyRes = new List<IentityBase>();
                    foreach (IMultyEntity me  in res)
                    {
                        if((me.singleValue != null  && me.singleValue != "")){
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

        

        private void WordObjectFromEntity(List<WordObject> sentence, List<WordObject> newSentence, IentityBase ent)
        {

            var newWord = sentence[ent.entityID].clone();
            newWord.WordT = WordObject.typeFromString(ent.entityType);

            if (ent.entityType == "organizationWord")
            {
                newWord.Amount = amountType.plural;
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


        //finds multyEntity match with maximal munch using recursive calls
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
            quarible = quarible.Where(x => x.entitySynonimus.Contains(";" + text + ";"));
            return quarible;
        }


        public void searchAllAnswerForentities()
        {
            List<entity> entList = new List<entity>();
            var entities = DBctrl1.getEntitys().AsQueryable();
            foreach (var s in DBctrl1.getAllSubQuestions())
            {
                var sentenses = meniAnalize(s.answerText, false);
                foreach (var sen in sentenses)
                {
                    var relevant = sen.Where(x => x.isEntity());
                    foreach (var w in relevant)
                    {
                        var wText = w.Lemma == null || w.Lemma.Length == 1 ? w.Text : w.Lemma;
                        if (!findMatch(entities, wText).Any())
                        {
                            var ent = new entity();
                            ent.entitySynonimus = ";" + wText + ";";
                            ent.entityType = Enum.GetName(typeof(WordType), w.WordT);
                            ent.entityValue = wText;
                            entList.Add(ent);
                        }
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
    }
    }
