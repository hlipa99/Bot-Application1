using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bot_Application1.YAndex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Application1.YAndex.Tests
{
    [TestClass()]
    public class ControlerTranslateTests
    {
        [TestMethod()]
        public void TranslateTest()
        {
            ControlerTranslate ct = new ControlerTranslate();
            //good
            Assert.AreEqual(ControlerTranslate.Translate("yochai"), "יוחאי");

            //bad
            Assert.AreNotEqual(ControlerTranslate.Translate("יוחאי"), "yochai");

            //ugly
            Assert.AreEqual(ControlerTranslate.Translate(null), "");

        }
    }
}