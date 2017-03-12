

namespace NLP.WorldObj
{
    public class personObject :WorldObject
    {
        private  amountType amount;
        private  personType person;
        private  timeType time;
        private genderType gender;

        public amountType Amount
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

        public personType Guf
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

        public timeType Time
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

        public genderType Gender
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
            unspecified,
            dual
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
            masculineandfeminine,
            unspecified,
        }


        public enum timeType
        {
            past,
            present,
            future,
            none,
            unspecified,
             imperative
        }

        public override IWorldObject Clone()
        {
            personObject res = new personObject(amount,person,time,gender,Word);
            res.Copy(this);
            return res;
        }

    }
}