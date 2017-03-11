using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vohmm.corpus;

namespace NLP.MorfObjects
{
    class ExtTokenWithBuilder
    {

            public string _chunk;
            public bool _hasPN;
            public string _ner;
            public string _origStr;
            public vohmm.corpus.Token _token;

        public ExtTokenWithBuilder(string _chunk, bool _hasPN, string _ner, string _origStr, Token _token)
        {
            this._chunk = _chunk;
            this._hasPN = _hasPN;
            this._ner = _ner;
            this._origStr = _origStr;
            this._token = _token;
        }

        public virtual string getNER() {
            return _ner;
        }

            public virtual void setNER(string ner)
        {

            _ner = ner;

        }
      
        
    }
}
