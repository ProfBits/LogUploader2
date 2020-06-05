using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogUploader.JSONHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.JSONHelper.Tests
{
    [TestClass()]
    public class JSONHelperTests
    {
        [TestMethod()]
        public void DesirealizeTest()
        {
            var a = new JSONHelper();
            var result = a.Desirealize("{\n\"root\":\"this is the root\",\n\"i'mNumber\" : -2.56e 2,\n\"FALSE\" : tRue,\n\"ReallyTrue\" : FALSE,\n\"notHere\": null,\n\"Listings here\": [\"A\", 1, true, null],\n\"IFeelEmpty\":[],\n\"Inceptione\": {\n	\"child\":\"this is the child\",\n	\"i'mNumber\" : +128e - 2,\n	\"FALSE\" : true,\n	\"ReallyTrue\" : fAlSE,\n	\"notHere\": NuLl,\n	\"Listings here\": [\"B\", 2, falsE, nuLl],\n	\"IFeelEmptyToo\":[]\n	}}");
            
        }

        [TestMethod()]
        public void FindStringTest()
        {
            var testString = "  \"test hallo\" :x";
            var a = new JSONHelper();
            var p = new PrivateObject(a);
            p.SetFieldOrProperty("str", testString);
            var res = a.FindString(2);
            Assert.AreEqual("test hallo", res.Item1);
            Assert.AreEqual(':', testString[2 + res.Item2]);


            testString = "  \"test hallo\" \n\t \r[x";
            a = new JSONHelper();
            p = new PrivateObject(a);
            p.SetFieldOrProperty("str", testString);
            res = a.FindString(2);
            Assert.AreEqual("test hallo", res.Item1);
            Assert.AreEqual('[', testString[2 + res.Item2]);
        }

        [TestMethod()]
        public void FindNumberTest()
        {
            var testString = "  \t-12 .34E\n+ 1 :x";
            var a = new JSONHelper();
            var p = new PrivateObject(a);
            p.SetFieldOrProperty("str", testString);
            var res = a.FindNumber(2);
            Assert.AreEqual("-123.4", res.Item1);
            Assert.AreEqual(':', testString[2 + res.Item2]);
        }

        [TestMethod()]
        public void IgnoreWhiteSpaceTest()
        {
            var testString = " 1  4 \n 8 \"11";
            var a = new JSONHelper();
            var p = new PrivateObject(a);
            p.SetFieldOrProperty("str", testString);
            Assert.AreEqual(1, a.IgnoreWhiteSpace(0));
            Assert.AreEqual(2, a.IgnoreWhiteSpace(2));
            Assert.AreEqual(3, a.IgnoreWhiteSpace(5));
            Assert.AreEqual(1, a.IgnoreWhiteSpace(9));
        }
    }
}