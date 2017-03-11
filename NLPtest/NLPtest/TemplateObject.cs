using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP
{
    public interface ITemplate
    {
        int ObjectType();
        bool haveTypeOf(ITemplate template);

        bool Equals(object obj);
    }
}
