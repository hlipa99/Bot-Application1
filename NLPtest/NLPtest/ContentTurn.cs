using NLPtest.view;
using NLPtest.WorldObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLPtest
{
    //fifo stack
    public class ContentTurn
    {
        List<WorldObject> list = new List<WorldObject>();
        ConversationContext conversationContext;


        public void Add(WorldObject obj)
        {
            list.Add(obj);
        }


        public bool empty()
        {
            return list.Count <= 0;
        }


        public WorldObject pop()
        {
            var next = list.FirstOrDefault();
            if (!empty()) {
                list.RemoveAt(0);
            }
            return next;
        }

        public IEnumerator<WorldObject> GetEnumerator()
        {
            //return copy allow change as enumarating 
            return list.ToList<WorldObject>().GetEnumerator();
        }



        internal void Add(ContentTurn contentTurn)
        {
            list.AddRange(contentTurn.getList());
        }

        internal WorldObject Get(int idx)
        {
            try
            {
                return list.ElementAt(idx);
            }catch(Exception ex)
            {
                return null;
            }
        }


        private IEnumerable<WorldObject> getList()
        {
            return list;
        }


        public override string ToString()
        {
            var res = "";
            foreach(var o in list)
            {
                    res += o + " | ";
            }
            return res;
        }

        internal void replace(WorldObject o, WorldObject g)
        {
            var i = list.IndexOf(o);
            list.RemoveAt(i);
            list.Insert(i,g);
        }

        internal int Count()
        {
            return list.Count;
        }
    }
}
