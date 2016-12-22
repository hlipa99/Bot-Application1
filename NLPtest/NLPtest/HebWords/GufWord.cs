using HebMorph;
using HebMorph.HSpell;

namespace NLPtest
{
     class GufWord// : Word
    {
        private HebDictionary.Min both;
        private HebrewToken ht;
        private HebDictionary.Guf rishon;
        private string v2;
        private HebDictionary.Kamot yahid;

        public object WorldObject { get; internal set; }

        public GufWord(string word, string v2, HebDictionary.Guf rishon, HebDictionary.Kamot yahid, HebDictionary.Min both) : base(word)
        {
            this.v2 = v2;
            this.rishon = rishon;
            this.yahid = yahid;
            this.both = both;
        }

        public GufWord(HebrewToken ht) : base(ht.Text)
        {
              if (ht.Mask.HasFlag(DMask.D_FIRST | DMask.D_NUMBASE))
            {
                word = "אנחנו";
            }
            else if (ht.Mask.HasFlag(DMask.D_FIRST))
            {
                word = "אני";
            }else if (ht.Mask.HasFlag(DMask.D_SECOND | DMask.D_NUMBASE | DMask.D_MASCULINE))
                {
                    word = "אתם";
                }
            else if (ht.Mask.HasFlag(DMask.D_SECOND | DMask.D_MASCULINE))
            {
                word = "אתה";
            }
            else if (ht.Mask.HasFlag(DMask.D_SECOND | DMask.D_NUMBASE | DMask.D_FEMININE))
            {
                word = "אתן";
            }
            else if (ht.Mask.HasFlag(DMask.D_SECOND | DMask.D_MASCULINE))
            {
                word = "את";
            }
            else if (ht.Mask.HasFlag(DMask.D_THIRD | DMask.D_NUMBASE | DMask.D_MASCULINE))
            {
                word = "הם";
            }
            else if (ht.Mask.HasFlag(DMask.D_THIRD | DMask.D_MASCULINE))
            {
                word = "הוא";
            }
            else if (ht.Mask.HasFlag(DMask.D_THIRD | DMask.D_NUMBASE | DMask.D_FEMININE))
            {
                word = "הן";
            }
            else if (ht.Mask.HasFlag(DMask.D_THIRD | DMask.D_MASCULINE))
            {
                word = "היא";
            }
            else if (ht.Mask.HasFlag(DMask.D_OFIRST | DMask.D_NUMBASE))
            {
                word = "שלנו";
            }
            else if (ht.Mask.HasFlag(DMask.D_OFIRST))
            {
                word = "שלי";
            }
            else if (ht.Mask.HasFlag(DMask.D_OSECOND | DMask.D_NUMBASE | DMask.D_MASCULINE))
            {
                word = "שלכם";
            }
            else if (ht.Mask.HasFlag(DMask.D_OSECOND | DMask.D_MASCULINE))
            {
                word = "שלך";
            }
            else if (ht.Mask.HasFlag(DMask.D_OSECOND | DMask.D_NUMBASE | DMask.D_FEMININE))
            {
                word = "שלכן";
            }
            else if (ht.Mask.HasFlag(DMask.D_OSECOND | DMask.D_MASCULINE))
            {
                word = "שלך";
            }
            else if (ht.Mask.HasFlag(DMask.D_OTHIRD | DMask.D_NUMBASE | DMask.D_MASCULINE))
            {
                word = "שלהם";
            }
            else if (ht.Mask.HasFlag(DMask.D_OTHIRD | DMask.D_MASCULINE))
            {
                word = "שלו";
            }
            else if (ht.Mask.HasFlag(DMask.D_OTHIRD | DMask.D_NUMBASE | DMask.D_FEMININE))
            {
                word = "שלהן";
            }
            else if (ht.Mask.HasFlag(DMask.D_OTHIRD | DMask.D_MASCULINE))
            {
                word = "שלה";
            }
            else
            {
                word = ht.Mask.ToString();
            }
















        }

        public GufWord(string s) : base (s)
        {
        }
    }
}