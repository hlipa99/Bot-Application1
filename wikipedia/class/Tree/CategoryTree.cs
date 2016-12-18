using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wikipedia
{
    class CategoryTree
    {
        private String catgoryName;
        private List<categorymembers> pagesList;

        public CategoryTree()
        {
            PagesList = new List<categorymembers>();
        }

        public string CatgoryName
        {
            get
            {
                return catgoryName;
            }

            set
            {
                catgoryName = value;
            }
        }

        internal List<categorymembers> PagesList
        {
            get
            {
                return pagesList;
            }

            set
            {
                pagesList = value;
            }
        }
    }
}

