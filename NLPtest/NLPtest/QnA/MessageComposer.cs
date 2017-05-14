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
    class MessageComposer
    {

        DataBaseController db = new DataBaseController();




        public string[] CreateUserStatMessage(UserStatistics stat)
        {
            var verbalStat = getPhrase(Pkey.userStatistics);
            verbalStat = mergeText(verbalStat, getPhrase(Pkey.numOfQuestions));
            verbalStat = mergeText(verbalStat, stat.scorByTime.Length.ToString());
            verbalStat = mergeText(verbalStat, "|");


            var missingSubjects = db.getAllCategory().Where(s => !stat.perSubjectScore.ContainsKey(s));

            foreach (var s in db.getAllCategory().Except(missingSubjects))
            {
                verbalStat = mergeText(verbalStat, getPhrase(Pkey.inSubjectOf));
                verbalStat = mergeText(verbalStat, s + "|");


                var subCategories = db.getAllSubCategory(s);
                var missingSubCategory = subCategories.Where(c => !stat.perSubjectScore.ContainsKey(s));

                if (stat.perSubjectScore[s] > 70)
                {
                    verbalStat = mergeText(verbalStat, getPhrase(Pkey.goodCategoryStat));

                }
                else if (stat.perSubjectScore[s] < 33)
                {
                    verbalStat = mergeText(verbalStat, getPhrase(Pkey.badCategoryStat));
                }
                else
                {
                    var good = stat.perSubjectScore.Where(d => subCategories.Contains(d.Key) && d.Value > 70);
                    var medium = stat.perSubjectScore.Where(d => subCategories.Contains(d.Key) && d.Value <= 70 && d.Value > 33);
                    var bad = stat.perSubjectScore.Where(d => subCategories.Contains(d.Key) && d.Value <= 66);
                    verbalStat = mergeText(verbalStat, getPhrase(Pkey.goodSubCategorysStat));
                    verbalStat = mergeText(verbalStat, mergeText(good.Select(x => x.Key).ToArray()));
                    verbalStat = mergeText(verbalStat, "|");
                    verbalStat = mergeText(verbalStat, getPhrase(Pkey.midiumSubCategorysStat));
                    verbalStat = mergeText(verbalStat, mergeText(good.Select(x => x.Key).ToArray()));
                    verbalStat = mergeText(verbalStat, "|");
                    verbalStat = mergeText(verbalStat, getPhrase(Pkey.badSubCategorysStat));
                    verbalStat = mergeText(verbalStat, mergeText(good.Select(x => x.Key).ToArray()));
                }
                verbalStat = mergeText(verbalStat, "|");


                if (missingSubCategory.Count() > 0)
                {
                    verbalStat = mergeText(verbalStat, getPhrase(Pkey.missingSubCategory));
                    verbalStat = mergeText(verbalStat, mergeText(missingSubCategory.ToArray()));
                }

            }

            if (missingSubjects.Count() > 0)
            {
                verbalStat = mergeText(verbalStat, getPhrase(Pkey.misingSubjectsStat));
                verbalStat = mergeText(verbalStat, mergeText(missingSubjects.ToArray()));
            }

            verbalStat = mergeText(verbalStat, "|");
            verbalStat = mergeText(verbalStat, getPhrase(Pkey.keepTraning));
            return verbalStat;
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
                        var EntityIter = entities.Distinct().OrderBy(x => x.entityType).GetEnumerator();
                        EntityIter.MoveNext();
                        verbalFeedback = mergeText(verbalFeedback, EntityIter.Current.entityValue);

                        while (EntityIter.MoveNext())
                        {

                            verbalFeedback = mergeText(verbalFeedback, ", " + EntityIter.Current.entityValue);
                        }
                        verbalFeedback = mergeText(verbalFeedback, ".");
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
                    var first = true;
                    foreach (var f in partial)
                    {
                        if (!first)
                        {
                            verbalFeedback = mergeText(verbalFeedback, getPhrase(Pkey.and));
                        }
                        first = false;
                        verbalFeedback = mergeText(verbalFeedback, f.answer.Trim());
                    }
                    verbalFeedback = mergeText(verbalFeedback, ".");
                }

                var empty = neededFeedback.Where(x => x.score < 20);
                if (empty.Count() > 0)
                {
                    verbalFeedback = mergeText(verbalFeedback, getPhrase(Pkey.shouldWrite));
                    var first = true;
                    foreach (var f in empty)
                    {
                        if (!first)
                        {
                            verbalFeedback = mergeText(verbalFeedback, getPhrase(Pkey.and));
                        }
                        first = false;
                        verbalFeedback = mergeText(verbalFeedback, f.answer.Trim());
                    }
                    verbalFeedback = mergeText(verbalFeedback, ".");
                }

            }

            else
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


                if (answerFeedback.answersFeedbacks.Count > 0)
                {
                    var feedbackEnumerator = answerFeedback.answersFeedbacks.GetEnumerator();
                    feedbackEnumerator.MoveNext();
                    verbalFeedback = mergeText(verbalFeedback, feedbackEnumerator.Current.answer);
                    while (feedbackEnumerator.MoveNext())
                    {
                        verbalFeedback = mergeText(verbalFeedback, getPhrase(Pkey.and));
                        verbalFeedback = mergeText(verbalFeedback, feedbackEnumerator.Current.answer.Trim());
                    }
                    verbalFeedback = mergeText(verbalFeedback, ".");
                }

            }

            var optionalAnswers = answerFeedback.answersFeedbacks.Except(neededFeedback);
            if (optionalAnswers.Any())
            {
                verbalFeedback = mergeText(verbalFeedback, "|");
                verbalFeedback = mergeText(verbalFeedback, getPhrase(Pkey.possibleAnswer));
                var first = true;
                foreach (var f in optionalAnswers)
                {
                    if (!first)
                    {
                        verbalFeedback = mergeText(verbalFeedback, getPhrase(Pkey.and));
                    }
                    verbalFeedback = mergeText(verbalFeedback, f.answer.Trim());
                    first = false;
                }
                verbalFeedback = mergeText(verbalFeedback, ".");
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
