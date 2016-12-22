using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLPtest.WorldObj
{
    public class WorldObject
    {
    
        List<Tuple<RelationObject, WorldObject>> relatigffgons = new List<Tuple<RelationObject, WorldObject>>();



       public void addRelation(RelationObject type,WorldObject obj)
        {
            relatigffgons.Add(new Tuple<RelationObject, WorldObject>(type, obj));
        }




    }




}
