using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Application1
{
    public static class RandomNum
    {
        static Random r;
        static RandomNum()
        {
            r = new Random();
        }

        public static int getNumber(int max)
        {
            return r.Next(max);
        }

    }
}