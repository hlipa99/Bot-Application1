using NLPtest.WorldObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLPtest
{
    //fifo stack
    class ContentTurn //: ICollection<WorldObject>
    {
        List<WorldObject> list = new List<WorldObject>();
        


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

        internal void Add(ContentTurn contentTurn)
        {
            list.AddRange(contentTurn.getList());
        }

        private IEnumerable<WorldObject> getList()
        {
            return list;
        }
    }
}
