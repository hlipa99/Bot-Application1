using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wikipedia
{
    class query
    {
        public List<categorymembers> categorymembers { get; set; }




        public void removePage()
        {
            for(var i =0; i < categorymembers.Count; i++)
            {
                if (categorymembers[i].ns == 0)
                    categorymembers.RemoveAt(i);
            }
        }

        public int getSize()
        {
            return categorymembers.Count;
        }

        public categorymembers getRendomCategory()
        {
            removePage();
            int numChosen = generateNumber();
            return categorymembers[numChosen];
            
        }

        private int generateNumber()
        {
            Random rnd = new Random();
            int card = rnd.Next(getSize() - 1);     // creates a number between 0 and max-1
            return card;
        }

        public List<categorymembers> getCategorymembersList()
        {
            return categorymembers;
        }
    }
}
