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
                List<Tuple<string, int>> perSubject = new List<Tuple<string, int>>();
                userScore.OrderByDescending(x => x.dateTime);
                userScore.Distinct(new questionIDscoreDistinct());

                foreach (var s in userScore)
                {
                    total.Add(s.Score.Value);
                    perSubject.Add(new Tuple<string, int>(s.subCategory, s.Score.Value));
                }

                // stat.scorByTime[0] = userScore.OrderBy(x => x.dateTime).Select(x => x.Score).ToArray();
                //   stat.scorByTime[0] = userScore.OrderBy(x => x.dateTime).Select(x => x.Score).ToArray();


                return null;
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

    }
}
