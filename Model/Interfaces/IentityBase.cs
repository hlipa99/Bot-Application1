using System;

namespace Model.Models
{
    public interface IentityBase
    {
        int entityID { get; set; }

        string entityValue { get; set; }
        string entityType { get; set; }

        IentityBase clone();

        bool Equals(Object obj);

    }
}