using NLPtest.WorldObj;

namespace NLPtest
{
    internal class gufObject :WorldObject
    {
        private  amountType amount;
        private  gufType guf;
        private  timeType time;
        private genderType gender;
        private string word;

        internal amountType Amount
        {
            get
            {
                return amount;
            }

            set
            {
                amount = value;
            }
        }

        internal gufType Guf
        {
            get
            {
                return guf;
            }

            set
            {
                guf = value;
            }
        }

        internal timeType Time
        {
            get
            {
                return time;
            }

            set
            {
                time = value;
            }
        }

        internal genderType Gender
        {
            get
            {
                return gender;
            }

            set
            {
                gender = value;
            }
        }

        public string Word
        {
            get
            {
                return word;
            }

            set
            {
                word = value;
            }
        }

        public gufObject(string word)
        {
            this.Word = word;
        }

        public gufObject(amountType amount, gufType guf, timeType time, genderType gender, string word)
        {
            this.Amount = amount;
            this.Guf = guf;
            this.Time = time;
            this.Gender = gender;
            this.Word = word;
        }



        public enum amountType
        {
            singular,
            plural,
            unspecified
        }

        public enum gufType
        {
            unspecified,
            First,
            Second,
            Third
        }

        public enum genderType
        {
            masculine,
            feminine,
            unspecified
        }

        public enum timeType
        {
            past,
            present,
            future,
            none,
            unspecified
        }

    }
}