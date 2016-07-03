using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPetrovichLite;

namespace NPetrovichLiteTest
{
    [TestClass]
    public class LastNames
    {
        private Petrovich petrovich;

        [TestInitialize]
        public void Init()
        {
            petrovich = new Petrovich();
        }

        [TestMethod]
        public void Test01()
        {
            Assert.AreEqual("Иванов", petrovich.InflectNamePart("Иванов", NamePart.LastName, Gender.Male, Case.Nominative));
        }
        [TestMethod]
        public void Test02()
        {
            Assert.AreEqual("Иванова", petrovich.InflectNamePart("Иванов", NamePart.LastName, Gender.Male, Case.Genitive));
        }
        [TestMethod]
        public void Test03()
        {
            Assert.AreEqual("Иванову", petrovich.InflectNamePart("Иванов", NamePart.LastName, Gender.Male, Case.Dative));
        }
        [TestMethod]
        public void Test04()
        {
            Assert.AreEqual("Иванова", petrovich.InflectNamePart("Иванов", NamePart.LastName, Gender.Male, Case.Accusative));
        }
        [TestMethod]
        public void Test05()
        {
            Assert.AreEqual("Ивановым", petrovich.InflectNamePart("Иванов", NamePart.LastName, Gender.Male, Case.Instrumental));
        }
        [TestMethod]
        public void Test06()
        {
            Assert.AreEqual("Иванове", petrovich.InflectNamePart("Иванов", NamePart.LastName, Gender.Male, Case.Prepositional));
        }
        
    }
}
