using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NPetrovichLite.Tests
{
    [TestFixture]
    public sealed class InflectSinglePart
    {
        private Petrovich petrovich;

        [SetUp]
        [OneTimeSetUp]
        public void Init()
        {
            petrovich = new Petrovich();
        }

        [Test]
        public void Test01()
        {
            Assert.AreEqual("Иванов", petrovich.InflectNamePart("Иванов", NamePart.LastName, Gender.Male, Case.Nominative));
            Assert.AreEqual("Иванова", petrovich.InflectNamePart("Иванов", NamePart.LastName, Gender.Male, Case.Genitive));
            Assert.AreEqual("Иванову", petrovich.InflectNamePart("Иванов", NamePart.LastName, Gender.Male, Case.Dative));
            Assert.AreEqual("Иванова", petrovich.InflectNamePart("Иванов", NamePart.LastName, Gender.Male, Case.Accusative));
            Assert.AreEqual("Ивановым", petrovich.InflectNamePart("Иванов", NamePart.LastName, Gender.Male, Case.Instrumental));
            Assert.AreEqual("Иванове", petrovich.InflectNamePart("Иванов", NamePart.LastName, Gender.Male, Case.Prepositional));
        }

        [Test]
        [TestCaseSource(typeof(TestDataFactory), nameof(TestDataFactory.LastNamesData))]
        [TestCaseSource(typeof(TestDataFactory), nameof(TestDataFactory.FirstNamesData))]
        [TestCaseSource(typeof(TestDataFactory), nameof(TestDataFactory.MidNamesData))]
        public void LastNames(string value, NamePart part, Gender gender, Case targetCase, string expected)
        {
            string result = petrovich.InflectNamePart(value, part, gender, targetCase);
            Assert.AreEqual(expected, result, string.Format("Part: {0}, Gender: {1}, Case: {2}", part, gender, targetCase));
        }

    }
}
