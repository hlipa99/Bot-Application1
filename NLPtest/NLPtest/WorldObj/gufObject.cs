using NLPtest.WorldObj;

namespace NLPtest
{
    internal class personObject :WorldObject
    {
        private  amountType amount;
        private  personType person;
        private  timeType time;
        private genderType gender;

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

        internal personType Guf
        {
            get
            {
                return person;
            }

            set
            {
                person = value;
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

       
        public personObject(string word)
        {
            this.Word = word;
        }

        public personObject(amountType amount, personType guf, timeType time, genderType gender, string word)
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

        public enum personType
        {
            unspecified,
            First,
            Second,
            Third,
            any
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

        public override IWorldObject Clone()
        {
            personObject res = new personObject(amount,person,time,gender,Word);
            cloneBase(res);
            return res;
        }

    }
}