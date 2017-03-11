using System;
using System.Collections.Generic;
using NLP.WorldObj;
using NLP.HebWords;

namespace NLP.MorfObjects
{
    public class Sentence
    {

        internal enum SentenceType {Question ,Regular ,Exclamation};


        private string text;
        List<WordObject> words = new List<WordObject>();
        SentenceType sType;
        internal WorldObject nose;
        internal QuestionObject question;
        internal TimeObject time;
        internal WorldObject Nose;
        internal WorldObject nasu;

        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                text = value;
            }
        }

        internal List<WordObject> Words
        {
            get
            {
                return words;
            }

            set
            {
                words = value;
            }
        }

        internal SentenceType SType
        {
            get
            {
                return sType;
            }

            set
            {
                sType = value;
            }
        }

        public Sentence(string inputText)
        {
           Text = inputText;
        }

        internal void Add(WordObject word)
        {
            Words.Add(word);
        }


        public override string ToString()
        {
            var str = "";
            foreach(WordObject w in Words)
            {
                str += w + " ";
            }
            return str;
        }
    }
}