using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotTests
{
    static class AssertNLP
    {
        public static void contains(List<string> listOptions, string str)
        {
            var flag = false;
            var res = "[";
            foreach (var o in listOptions)
            {
                res += o + ",";
                if (contains(o, str)) {
                    flag = true;
                    break;
                }
            }

            if (!flag)
            {
                res += "]";
                res = str + " Not in Array " + res;
                throw new AssertFailedException(res);
            }
        }


        static bool contains(string sorcse, string input)
        {
            var counter = sorcse.Length;
            foreach (var o in input)
            {
                if (!sorcse.Contains(o)){
                    counter--;
                }
            }

            if (counter < sorcse.Length * 0.5) return false;
            else return true;
        }


        public static void contains(List<string> listOptions, string[] str)
        {

            foreach (var o in str)
            {
                contains(listOptions, o);
            }

        }
    }
}
