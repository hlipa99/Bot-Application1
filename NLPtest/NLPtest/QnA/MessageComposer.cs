using Model;
using Model.dataBase;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLPtest.QnA
{
    public class MessageComposer
    {

        DataBaseController db = new DataBaseController();




        public string[] CreateUserStatMessage(UserStatistics stat)
        {
            if (stat != null)
            {
                var verbalStat = getPhrase(Pkey.userStatistics);
                verbalStat = mergeText(verbalStat, getPhrase(Pkey.numOfQuestions));
                verbalStat = mergeText(verbalStat, stat.scorByTime.Length.ToString());
                verbalStat = mergeText(verbalStat, "|");
                var categoreis = db.getAllCategory();

                var missingSubjects = categoreis.Where(s => !stat.perSubjectScore.ContainsKey(s));

                var scored = categoreis.Except(missingSubjects);

                var goodSubjects = scored.Where(s => stat.perSubjectScore[s] > 70);
                var badSubjects = scored.Where(s => stat.perSubjectScore[s] <= 30);
                var mediumSubjects = scored.Where(s => stat.perSubjectScore[s] > 30 && stat.perSubjectScore[s] <= 70);


                if (goodSubjects.Any())
                {
                    verbalStat = mergeText(verbalStat, getPhrase(Pkey.goodCategoryStat));
                    verbalStat = mergeText(verbalStat, getPhrase(Pkey.inSubjectOf));
                    verbalStat = mergeText(verbalStat, mergeTextWithOr(goodSubjects.ToArray()));
                    verbalStat = mergeText(verbalStat, "|");
                }
                if (badSubjects.Any())
                {
                    verbalStat = mergeText(verbalStat, getPhrase(Pkey.badCategoryStat));
                    verbalStat = mergeText(verbalStat, getPhrase(Pkey.inSubjectOf));
                    verbalStat = mergeText(verbalStat, mergeTextWithOr(badSubjects.ToArray()));
                    verbalStat = mergeText(verbalStat, "|");
                }


                foreach (var s in mediumSubjects)
                {
                    var subCategories = db.getAllSubCategory(s);
                    var missingSubCategory = subCategories.Where(c => !stat.perSubjectScore.ContainsKey(s));

                    verbalStat = mergeText(verbalStat, getPhrase(Pkey.inSubjectOf));
                    verbalStat = mergeText(verbalStat, s + ":");


                    var good = stat.perSubjectScore.Where(d => subCategories.Contains(d.Key) && d.Value > 70);
                    var medium = stat.perSubjectScore.Where(d => subCategories.Contains(d.Key) && d.Value <= 70 && d.Value > 33);
                    var bad = stat.perSubjectScore.Where(d => subCategories.Contains(d.Key) && d.Value <= 66);
                    if (good.Any())
                    {
                        verbalStat = mergeText(verbalStat, "|");
                        verbalStat = mergeText(verbalStat, getPhrase(Pkey.goodSubCategorysStat));
                        verbalStat = mergeText(verbalStat, mergeText(mergeTextWithOr(good.Select(x => x.Key).ToArray())));
                    }
                    if (medium.Any())
                    {
                        verbalStat = mergeText(verbalStat, "|");
                        verbalStat = mergeText(verbalStat, getPhrase(Pkey.midiumSubCategorysStat));
                        verbalStat = mergeText(verbalStat, mergeText(mergeTextWithOr(medium.Select(x => x.Key).ToArray())));

                    }
                    if (bad.Any())
                    {
                        verbalStat = mergeText(verbalStat, "|");
                        verbalStat = mergeText(verbalStat, getPhrase(Pkey.badSubCategorysStat));
                        verbalStat = mergeText(verbalStat, mergeText(mergeTextWithOr(bad.Select(x => x.Key).ToArray())));
                    }

                    if (missingSubCategory.Count() > 0)
                    {
                        verbalStat = mergeText(verbalStat, getPhrase(Pkey.missingSubCategory));
                        verbalStat = mergeText(verbalStat, mergeText(mergeTextWithOr(missingSubCategory.ToArray())));
                    }

                }
                verbalStat = mergeText(verbalStat, "|");




                if (missingSubjects.Count() > 0)
                {
                    verbalStat = mergeText(verbalStat, getPhrase(Pkey.misingSubjectsStat));
                    verbalStat = mergeText(verbalStat, mergeText(missingSubjects.ToArray()));
                }

                verbalStat = mergeText(verbalStat, "|");
                verbalStat = mergeText(verbalStat, getPhrase(Pkey.keepTraning));
                return verbalStat;
            }
            else
            {
                return getPhrase(Pkey.notEnoughAnswersForStat);
            }
            
        }

        private string[] mergeTextWithOr(string[] list)
        {

            var verbalFeedback = new string[] { };
            for(int i=0;i< list.Length;i++)
            {
                if (i!=0)
                {
                    if (i == list.Length - 1)
                    {
                        verbalFeedback = mergeText(verbalFeedback, getPhrase(Pkey.and));
                    }
                    else
                    {
                        verbalFeedback = mergeText(verbalFeedback, ",");
                    }
                }

                verbalFeedback = mergeText(verbalFeedback, list[i]);
            }
            verbalFeedback = mergeText(verbalFeedback, ".");
            return verbalFeedback;
        }

        private bool isSpecialEntity(string entityType)
        {
            return entityType == "personWord" ||
               entityType == "organizationWord" ||
                entityType == "locationWord" ||
                entityType == "conceptWord" ||
                 entityType == "eventWord";
        }

        public string[] createFeedBack(AnswerFeedback answerFeedback)
        {
            string[] verbalFeedback = new string[] { };
            //check sub question
            //  studySession.CurrentSubQuestion.AnswerScore = answerFeedback.score;

            var neededFeedback = answerFeedback.answersFeedbacks.OrderByDescending(x => x.score).Take(answerFeedback.Need);
            if (neededFeedback.Where(x => x.score >= 80).Count() >= answerFeedback.Need)
            {
                verbalFeedback = getPhrase(Pkey.goodAnswer);
            }
            else if (neededFeedback.Where(x => x.score <= 80 && x.score >= 20).Count() > 0)
            {
                var entities = neededFeedback.Where(x => x.score > 60).SelectMany(x => x.missingEntitis.Where(z => isSpecialEntity(z.entityType))).Distinct();
                if (neededFeedback.Where(x => x.score >= 60).Count() >= 0)
                {
                    if (entities.Count() > 0)
                    {
                        verbalFeedback = getPhrase(Pkey.goodPartialAnswer);
                        verbalFeedback = mergeText(verbalFeedback, getPhrase(Pkey.missingAnswerEntity));
                        var EntityIter = entities.Distinct().OrderBy(x => x.entityType);
                        verbalFeedback = mergeText(verbalFeedback, mergeTextWithOr(EntityIter.Select(x => x.entityValue).ToArray()));
                    }
                    else
                    {
                        verbalFeedback = mergeText(verbalFeedback, getPhrase(Pkey.goodButNotAllAnswerParts));
                    }
                }
                else
                {
                    verbalFeedback = mergeText(verbalFeedback, getPhrase(Pkey.partialAnswer));
                }

                var partial = neededFeedback.Where(x => x.score <= 60 && x.score >= 20);
                if (partial.Count() > 0)
                {
                    verbalFeedback = mergeText(verbalFeedback, getPhrase(Pkey.MyAnswerToQuestion));
                    verbalFeedback = mergeText(verbalFeedback, mergeTextWithOr(partial.Select(x=>x.answer.Trim()).ToArray()));
                }

                var empty = neededFeedback.Where(x => x.score < 20);
                if (empty.Count() > 0)
                {
                    verbalFeedback = mergeText(verbalFeedback, getPhrase(Pkey.shouldWrite));
                    verbalFeedback =  mergeText(verbalFeedback, mergeTextWithOr(empty.Select(x => x.answer.Trim()).ToArray()));
                }

            }

            else
            {
                if (neededFeedback.Count() > 0)
                {
                        if (answerFeedback.answer != null && answerFeedback.answer.Split(' ').Length > 2)
                    {
                        verbalFeedback = getPhrase(Pkey.wrongAnswer);
                    }
                    else
                    {
                        verbalFeedback = getPhrase(Pkey.notAnAnswer);
                    }
                    verbalFeedback = mergeText(verbalFeedback, getPhrase(Pkey.MyAnswerToQuestion));
                     verbalFeedback = mergeText(verbalFeedback, mergeTextWithOr(neededFeedback.Select(x => x.answer.Trim()).ToArray()));
                }

            }

            var optionalAnswers = answerFeedback.answersFeedbacks.Except(neededFeedback);
            if (optionalAnswers.Any())
            {
                verbalFeedback = mergeText(verbalFeedback, "|");
                verbalFeedback = mergeText(verbalFeedback, getPhrase(Pkey.possibleAnswer));
                verbalFeedback = mergeText(verbalFeedback, mergeTextWithOr(optionalAnswers.Select(x => x.answer.Trim()).ToArray()));
            }

            return verbalFeedback;

        }






        private string[] getPhrase(Pkey goodAnswer)
        {
            return new string[] { "<p:" + Enum.GetName(typeof(Pkey), goodAnswer) + ">" };
        }

        internal string[] mergeText(string[] v1, string v2)
        {
            var space = v2.Length > 1 ? " " : "";
            return new string[] { mergeText(v1).Trim() + space + v2.Trim() };
        }

        internal string[] mergeText(string[] v1, string[] v2)
        {
            return new string[] { mergeText(v1) + " " + mergeText(v2) };
        }

        private string mergeText(string[] phraseValue)
        {
            if (phraseValue.Count() > 0)
            {
                var res = phraseValue[0];
                var left = phraseValue.ToList();
                left.RemoveAt(0);
                foreach (var s in left)
                {
                    if (s.Length > 1)
                    {
                        res += " " + s;
                    }
                    else
                    {
                        res += s;

                    }
                }
                return res;
            }
            else
            {
                return "";
            }

        }
    }
}
