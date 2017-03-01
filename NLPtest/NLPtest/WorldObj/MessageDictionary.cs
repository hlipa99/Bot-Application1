using NLPtest.Exceptions;
using NLPtest.MorfObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLPtest.WorldObj
{
    class MessageDictionary : IDictionary<string, HashSet<MessageObject>>
    {
        Dictionary<string, HashSet<MessageObject>> dict = new Dictionary<string, HashSet<MessageObject>>();

        public ICollection<string> Keys
        {
            get
            {
                return ((IDictionary<string, HashSet<MessageObject>>)dict).Keys;
            }
        }

        public ICollection<HashSet<MessageObject>> Values
        {
            get
            {
                return ((IDictionary<string, HashSet<MessageObject>>)dict).Values;
            }
        }

        public int Count
        {
            get
            {
                return ((IDictionary<string, HashSet<MessageObject>>)dict).Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return ((IDictionary<string, HashSet<MessageObject>>)dict).IsReadOnly;
            }
        }

        public HashSet<MessageObject> this[string key]
        {
            get
            {
                return ((IDictionary<string, HashSet<MessageObject>>)dict)[key];
            }

            set
            {
                ((IDictionary<string, HashSet<MessageObject>>)dict)[key] = value;
            }
        }

        public IEnumerator<MessageObject> getRandomEnumerator(string key)
        {
          
            if (dict.ContainsKey(key))
            {
                var set = dict[key];
                Random a = new Random();
                return set.OrderBy(x => a.Next(100)).GetEnumerator();
            }else
            {
                throw new WorldObjectException(key);
            }
        }

        


        internal string getTimeOfDay()
        {
            var time = DateTime.Now;
            if(time.Hour < 5)
            {
                return "בוקר מוקדם";
            }
            else if (time.Hour < 11)
            {
                return "בוקר";
            }
            else if (time.Hour < 13)
            {
                return "צהריים";
            }
            else if (time.Hour < 17)
            {
                return "צהריים";
            }
            else if (time.Hour < 21)
            {
                return "ערב";
            }else
            {
                return "לילה";
            }
        }

        public void Add(string key, MessageObject value)
        {
            if (!dict.ContainsKey(key))
            {
                dict.Add(key, new HashSet<MessageObject>());
            }

            dict[key].Add(value);

        }

        public bool ContainsKey(string key)
        {
            return ((IDictionary<string, HashSet<MessageObject>>)dict).ContainsKey(key);
        }

        public void Add(string key, HashSet<MessageObject> value)
        {
            ((IDictionary<string, HashSet<MessageObject>>)dict).Add(key, value);
        }

        public bool Remove(string key)
        {
            return ((IDictionary<string, HashSet<MessageObject>>)dict).Remove(key);
        }

        public bool TryGetValue(string key, out HashSet<MessageObject> value)
        {
            return ((IDictionary<string, HashSet<MessageObject>>)dict).TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<string, HashSet<MessageObject>> item)
        {
            ((IDictionary<string, HashSet<MessageObject>>)dict).Add(item);
        }

        public void Clear()
        {
            ((IDictionary<string, HashSet<MessageObject>>)dict).Clear();
        }

        public bool Contains(KeyValuePair<string, HashSet<MessageObject>> item)
        {
            return ((IDictionary<string, HashSet<MessageObject>>)dict).Contains(item);
        }

        public void CopyTo(KeyValuePair<string, HashSet<MessageObject>>[] array, int arrayIndex)
        {
            ((IDictionary<string, HashSet<MessageObject>>)dict).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, HashSet<MessageObject>> item)
        {
            return ((IDictionary<string, HashSet<MessageObject>>)dict).Remove(item);
        }

        public IEnumerator<KeyValuePair<string, HashSet<MessageObject>>> GetEnumerator()
        {
            return ((IDictionary<string, HashSet<MessageObject>>)dict).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary<string, HashSet<MessageObject>>)dict).GetEnumerator();
        }
    }
}
