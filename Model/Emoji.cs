using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public static class Emoji
    {
        static Dictionary<string, string> d = new Dictionary<string, string>();

        static Emoji()
        {
            d.Add("grinning", "\U0001F600");
            d.Add("smiling", "\U0001F601");
            d.Add("confused", "\U0001F615");
            d.Add("frowning ", "\u2639");
            d.Add("crying", "\U0001F62D");
            d.Add("fearful", "\U0001F628");
            d.Add("flushed", "\U0001F633");
            d.Add("halo", "\U0001F607");
            d.Add("horns", "\U0001F608");
            d.Add("poo", "\U0001F4A9");
            d.Add("unicorn", "\U0001F984");
            d.Add("penguin", "\U0001F427");
            d.Add("meat", "\U0001F356");
            d.Add("pizza", "\U0001F355");
            d.Add("cofee", "\u2615");
            d.Add("island", "\U0001F3DD");
            d.Add("medal1", "\U0001F947");
            d.Add("medal2", "\U0001F948");
            d.Add("medal3", "\U0001F949");
            d.Add("trophy", "\U0001F396");
            d.Add("musical ", "\U0001F3B6");
            d.Add("guitar", "\U0001F3B8");
            d.Add("israel", "\U0001F1EE\U0001F1F1");
            d.Add("thinking", "\U0001F914");
            d.Add("monkey", "\U0001F648");
            d.Add("robot", "\U0001F916");
            d.Add("facepalming", "\U0001F929");
            d.Add("student", "\U0001F468\u200D\U0001F393");
            d.Add("dizzy", "\U0001F635");
            d.Add("sunglasses", "\U0001F60E");
            d.Add("tear", "\U0001F622");
            d.Add("nerd", "\U0001F60E");
            d.Add("sad", "\U0001F61E");
            d.Add("exhausted", "\U0001F605");
            d.Add("tongue", "\U0001F61B");
            d.Add("hammer", "\U0001F528");
            d.Add("sun", "\u2600");
            d.Add("bird", "\U0001F426");
            d.Add("tumbup", "\U0001F44D");
            d.Add("blush", "\U0001F60A");
            d.Add("teeth", "\U0001F62C");
            d.Add("clock", "\u23F0");
            d.Add("winking", "\U0001F609");
            d.Add("loving", "\U0001F60D");
            d.Add("rollingEye", "\U0001F644");
            d.Add(" ", "\U0001F634");
            d.Add("astonished", "\U0001F632");
            d.Add("angry", "\U0001F620");
            d.Add("alienMonster", "\U0001F47E");
            d.Add("poutingCat", "\U0001F63E");
            d.Add("baby", "\U0001F476");
            d.Add("fire", "\U0001F525");
        }


        public static string get(string key)
        {
            string val = "";
            d.TryGetValue(key,out val);
            if(val != null)
            {
                return val;
            }else
            {
                return "";
            }
        }


    }
}
