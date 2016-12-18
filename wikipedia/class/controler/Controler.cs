using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace wikipedia
{
    class Controler
    {
        
        private string urlWikipedia = "https://he.wikipedia.org/w/api.php?action=query&list=categorymembers&cmtitle=Category:";
        private string category = "";
        private List<CategoryTree> categoryList;
        private TreeNode treeNode;

        public void StartHistoryRendom(String title)
        {
            categoryList = new List<CategoryTree>();
            var webClient = new WebClient();
            string url = urlWikipedia + title + "&cmlimit=500&format=json&utf8=";

            var pageSourceCode = webClient.DownloadString(url);
            byte[] bytes = Encoding.Default.GetBytes(pageSourceCode);
            String myString = Encoding.UTF8.GetString(bytes);
            CategoriesList mainlistCategories = JsonConvert.DeserializeObject<CategoriesList>(myString);

            List<categorymembers> cm = mainlistCategories.query.getCategorymembersList();

            CategoryTree tr = new CategoryTree();
            tr.CatgoryName = title;
            tr.PagesList = cm;
            categoryList.Add(tr);


            do
            {
                String categoryName = getRendomeCatgoryTitle(cm);
                if (categoryName.Equals("notfind")) break;

                cm = getCategoryTree(categoryName);

                CategoryTree tree = new CategoryTree();
                tree.CatgoryName = categoryName;
                tree.PagesList = cm;
                categoryList.Add(tree);

            } while (true);




            Pagename page = getRendomPage();
            startHtml(page);

            
        }

        private void startHtml(Pagename page)
        {


            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(page.extract);
            IEnumerable<HtmlAgilityPack.HtmlNode> forms;

      //      forms = htmlDoc.DocumentNode.Descendants("h");
       //     List<HtmlAgilityPack.HtmlNode> h = forms.ToList();

     //       forms = htmlDoc.DocumentNode.Descendants("h1");
      //      List<HtmlAgilityPack.HtmlNode> h1 = forms.ToList();

//            forms = htmlDoc.DocumentNode.Descendants("h2");
  //          List<HtmlAgilityPack.HtmlNode> h2 = forms.ToList();

  //          forms = htmlDoc.DocumentNode.Descendants("h3");
   //         List<HtmlAgilityPack.HtmlNode> h3 = forms.ToList();

            forms = htmlDoc.DocumentNode.Descendants("p");
            List<HtmlAgilityPack.HtmlNode> p = forms.ToList();

//            forms = htmlDoc.DocumentNode.Descendants("p1");
  //          List<HtmlAgilityPack.HtmlNode> p1 = forms.ToList();
                                    
        }


        private String getRendomeCatgoryTitle(List<categorymembers> cm)
        {
            List<categorymembers> findSubCategory = cm.FindAll(x => x.ns == 14);
            if (findSubCategory.Count == 0) return "notfind";

            int num = generateNumber(findSubCategory.Count());

            String name = findSubCategory[num].title;
            name = name.Replace("קטגוריה:","");
            return name;
        }

        private List<categorymembers> getCategoryTree(String categoryName)
        {
            string url = urlWikipedia + categoryName + "&cmlimit=500&format=json&utf8=";
            var webClient = new WebClient();
            var pageSourceCode = webClient.DownloadString(url);
            byte[] bytes = Encoding.Default.GetBytes(pageSourceCode);
            String myString = Encoding.UTF8.GetString(bytes);
            CategoriesList SubCategoriesList = JsonConvert.DeserializeObject<CategoriesList>(myString);
            return SubCategoriesList.query.getCategorymembersList();
        }

        private CategoriesList getSubCatgories(CategoriesList mainlistCategories)
        {
            categorymembers subCategori = mainlistCategories.query.getRendomCategory();
            string url = urlWikipedia + subCategori.title+"&cmlimit=500&format=json&utf8=";
            var webClient = new WebClient();
            var pageSourceCode = webClient.DownloadString(url);
            byte[] bytes = Encoding.Default.GetBytes(pageSourceCode);
            String myString = Encoding.UTF8.GetString(bytes);
            CategoriesList SubCategoriesList = JsonConvert.DeserializeObject<CategoriesList>(myString);
            return SubCategoriesList;
        }




        public RootCategoryFromPage getCategoryFromPage(String pageName, int pageId)
        {
            String url = "https://he.wikipedia.org/w/api.php?format=json&action=query&prop=categories&titles="+pageName+"&utf8=";    //all categorys of page title
            var webClient = new WebClient();
            var pageSourceCode = webClient.DownloadString(url);
            byte[] bytes = Encoding.Default.GetBytes(pageSourceCode);
            String myString = Encoding.UTF8.GetString(bytes);

            int index = myString.IndexOf("categories");
            string piece = myString.Substring(index - 1);

            piece = piece.Insert(0, "{");
            piece = piece.Substring(0, piece.Length - 3);
            piece = piece.Replace("קטגוריה:", "");


            RootCategoryFromPage rootCategoryFromPage = JsonConvert.DeserializeObject<RootCategoryFromPage>(piece);
            return rootCategoryFromPage;


        }

        public Query getPage(String pageName, int pageId)
        {
            string url = "https://he.wikipedia.org/w/api.php?format=json&action=query&prop=extracts&titles="+pageName+"&redirects=true&utf8=";
            var webClient = new WebClient();
            var pageSourceCode = webClient.DownloadString(url);

            byte[] bytes = Encoding.Default.GetBytes(pageSourceCode);
            String myString = Encoding.UTF8.GetString(bytes);

            String piece = switchingNameInJsonPage(myString);

            Query page = JsonConvert.DeserializeObject<Query>(piece);

            return page;
        }


        public Pagename getRendomPage()
        {
            int size = categoryList.Count();
            int random = generateNumber(size);

            List < categorymembers > pageList = this.categoryList[random].PagesList;

            size = pageList.Count();
            

            categorymembers page;
            do
            {
                random = generateNumber(size);
                page = pageList[random];
            }
            while (page.ns == 14);
            

            Query query = getPage(page.title, page.pageid);
            return query.pages.pagename;

        }



        private String switchingNameInJsonPage(String st)
        {
            int index = st.IndexOf("pages");
            string piece = st.Substring(index-1);

             piece = piece.Insert(0, "{");
            piece = piece.Substring(0,piece.Length-1);


            String num = piece.Substring(11);
            string result = "";

            foreach (char c in num)
            {
                if (!Char.IsDigit(c))
                {
                    break;
                }
                result += c;
            }



            piece = ReplaceFirst(piece, result, "pagename");
            return piece;


            


        }

        string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }


        private int generateNumber(int size)
        {
            Random rnd = new Random();
            int card = rnd.Next(size - 1);     // creates a number between 0 and max-1
            return card;
        }

    }


   
}
