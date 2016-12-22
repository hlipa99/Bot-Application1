using System;
using System.Collections.Generic;
using NLPtest.WorldObj;

namespace NLPtest
{
    internal class Sentence
    {

        internal enum SentenceType {Question ,Regular ,Exclamation};


        private string text;
        List<Word> words = new List<Word>();
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

        internal List<Word> Words
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

        internal void Add(Word word)
        {
            Words.Add(word);
        }


        public override string ToString()
        {
            var str = "";
            foreach(Word w in Words)
            {
                str += w + " ";
            }
            return str;
        }
    }
}