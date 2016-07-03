using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPetrovichLite;

namespace NPetrovichLiteTest
{
    [TestClass]
    public class UnitTest1
    {
        private Petrovich petrovich;

        [TestInitialize]
        public void Init()
        {
            petrovich = new Petrovich();
        }

        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual("Иванов", petrovich.InflectNamePart("Иванов", NamePart.LastName, Gender.Male, Case.Nominative));
        }
    }
}
