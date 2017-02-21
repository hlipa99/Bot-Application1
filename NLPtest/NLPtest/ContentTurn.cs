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
    public class ContentList
    {
        List<WorldObject> list = new List<WorldObject>();

        ConversationContext conversationContext;

        public List<WorldObject> List
        {
            get
            {
                return list;
            }

            set
            {
                list = value;
            }
        }

        public void Add(WorldObject obj)
        {
            List.Add(obj);
        }
        public void Add(List<WorldObject> obj)
        {
            List.AddRange(obj);
        }


        public bool empty()
        {
            return List.Count <= 0;
        }


        public WorldObject pop()
        {
            var next = List.FirstOrDefault();
            if (!empty()) {
                List.RemoveAt(0);
            }
            return next;
        }

        public IEnumerator<WorldObject> GetEnumerator()
        {
            //return copy allow change as enumarating 
            return List.ToList<WorldObject>().GetEnumerator();
        }



        internal void Add(ContentList contentTurn)
        {
            List.AddRange(contentTurn.getList());
        }

        internal WorldObject Get(int idx)
        {
            try
            {
                return List.ElementAt(idx);
            }catch(Exception ex)
            {
                return null;
            }
        }


        private IEnumerable<WorldObject> getList()
        {
            return List;
        }


        public override string ToString()
        {
            var res = "";
            foreach(var o in List)
            {
                    res += o + " | ";
            }
            return res;
        }

        internal void replace(WorldObject o, WorldObject g)
        {
            var i = List.IndexOf(o);
            List.RemoveAt(i);
            List.Insert(i,g);
        }

        internal int Count()
        {
            return List.Count;
        }
    }
}
