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

        [OneTimeSetUp]
        public void Init()
        {
            petrovich = new Petrovich();
        }

        [Test]
        public void Test01()
        {
            Assert.AreEqual("Иванов", petrovich.Inflect("Иванов", NamePart.LastName, Case.Nominative, Gender.Male));
            Assert.AreEqual("Иванова", petrovich.Inflect("Иванов", NamePart.LastName, Case.Genitive, Gender.Male));
            Assert.AreEqual("Иванову", petrovich.Inflect("Иванов", NamePart.LastName, Case.Dative, Gender.Male));
            Assert.AreEqual("Иванова", petrovich.Inflect("Иванов", NamePart.LastName, Case.Accusative, Gender.Male));
            Assert.AreEqual("Ивановым", petrovich.Inflect("Иванов", NamePart.LastName, Case.Instrumental, Gender.Male));
            Assert.AreEqual("Иванове", petrovich.Inflect("Иванов", NamePart.LastName, Case.Prepositional, Gender.Male));
        }

        [Test]
        [TestCaseSource(typeof(TestDataFactory), nameof(TestDataFactory.LastNamesData))]
        [TestCaseSource(typeof(TestDataFactory), nameof(TestDataFactory.FirstNamesData))]
        [TestCaseSource(typeof(TestDataFactory), nameof(TestDataFactory.MidNamesData))]
        public void TestSinglePartInflection(string value, NamePart part, Gender gender, Case targetCase, string expected)
        {
            string result = petrovich.Inflect(value, part, targetCase, gender);
            Assert.AreEqual(expected, result, string.Format("Part: {0}, Gender: {1}, Case: {2}", part, gender, targetCase));
        }
    }
}
