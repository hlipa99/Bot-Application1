using Model.dataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLPtest.QnA
{
    class StatisticAgent
    {
        DataBaseController db = new DataBaseController();
        public string createGeneralUserStatistics(User user)
        {





            return null;
        }



        public UserStatistics createUserLerningStatistics(User user)
        {
            var stat = new UserStatistics();
            var userScore = db.getUserAnswersStat(user.UserID);
            if (userScore.Count < 4)
            {
                return null;
            }
            else
            {
                List<int> total = new List<int>();
                Dictionary<string,int> perSubject = new Dictionary<string, int>();
                Dictionary<string, int> perSubjectScore = new Dictionary<string, int>();
                userScore.OrderByDescending(x => x.dateTime);
                userScore.Distinct(new questionIDscoreDistinct());

                foreach (var s in userScore)
                {
                    total.Add(s.Score.Value);
                    int val;
                    if(perSubject.TryGetValue(s.subCategory,out val))
                    {
                        perSubjectScore[s.subCategory] += 1;
                        perSubject[s.subCategory] = (val * perSubject[s.subCategory] + s.Score.Value)/(val+1);
                    }
                    else
                    {
                        perSubjectScore[s.subCategory] = 1;
                        perSubject[s.subCategory] = s.Score.Value;
                    }

                    if (perSubject.TryGetValue(s.category, out val))
                    {
                        perSubjectScore[s.category] += 1;
                        perSubject[s.category] = (val * perSubject[s.category] + s.Score.Value) / (val + 1);
                    }
                    else
                    {
                        perSubjectScore[s.category] = 1;
                        perSubject[s.category] = s.Score.Value;
                    }
                }


                stat.perSubjectScore = perSubjectScore;
                stat.scorByTime = userScore.OrderBy(x => x.dateTime).Select(x => x.Score).ToArray();
                //   stat.scorByTime[0] = userScore.OrderBy(x => x.dateTime).Select(x => x.Score).ToArray();


                return stat;
            }
        }


        class questionIDscoreDistinct : IEqualityComparer<userScore>
        {
            public bool Equals(userScore x, userScore y)
            {
                return (x.QuestionID == y.QuestionID && x.SubquestionID == y.SubquestionID);
       
            }

            public int GetHashCode(userScore obj)
            {
                return obj.GetHashCode();
            }
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
