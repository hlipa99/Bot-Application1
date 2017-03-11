using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace NLP.WorldObj
{


    public interface IWorldObject : ITemplate
    {

        void CopyFromTemplate(ITemplate[] objects);
        IWorldObject Clone();
    }




}
