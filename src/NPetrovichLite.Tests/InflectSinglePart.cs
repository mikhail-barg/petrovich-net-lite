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
            Assert.That("Иванов".Equals(petrovich.Inflect("Иванов", NamePart.LastName, Case.Nominative, Gender.Male)));
            Assert.That("Иванова".Equals(petrovich.Inflect("Иванов", NamePart.LastName, Case.Genitive, Gender.Male)));
            Assert.That("Иванову".Equals(petrovich.Inflect("Иванов", NamePart.LastName, Case.Dative, Gender.Male)));
            Assert.That("Иванова".Equals(petrovich.Inflect("Иванов", NamePart.LastName, Case.Accusative, Gender.Male)));
            Assert.That("Ивановым".Equals(petrovich.Inflect("Иванов", NamePart.LastName, Case.Instrumental, Gender.Male)));
            Assert.That("Иванове".Equals(petrovich.Inflect("Иванов", NamePart.LastName, Case.Prepositional, Gender.Male)));
        }

        [Test]
        public void LoveTest()
        {
            Assert.That(Gender.Male.Equals(petrovich.GetGender("Рауль", NamePart.FirstName)));
            Assert.That(Gender.Female.Equals(petrovich.GetGender("Руфь", NamePart.FirstName)));
            Assert.That(Gender.Female.Equals(petrovich.GetGender("Любовь", NamePart.FirstName)));
            Assert.That("Любови".Equals(petrovich.Inflect("Любовь", NamePart.FirstName, Case.Dative)));
        }

        [Test]
        public void Test2()
        {
            Assert.That("Дарвином".Equals(petrovich.Inflect("Дарвин", NamePart.LastName, Case.Instrumental, Gender.Male)));
        }

        [Test]
        [TestCaseSource(typeof(TestDataFactory), nameof(TestDataFactory.OpencorporaInflectionData), new object[] { "surnames.misc.tsv", NamePart.LastName })]
        [TestCaseSource(typeof(TestDataFactory), nameof(TestDataFactory.OpencorporaInflectionData), new object[] { "firstnames.misc.tsv", NamePart.FirstName })]
        [TestCaseSource(typeof(TestDataFactory), nameof(TestDataFactory.OpencorporaInflectionData), new object[] { "midnames.misc.tsv", NamePart.MiddleName })]
        public void TestSinglePartInflection(string value, NamePart part, Gender? gender, Case targetCase, string expected)
        {
            string result = petrovich.Inflect(value, part, targetCase, gender);
            Assert.That(expected.Equals(result), $"Value: {value}, Part: {part}, Gender: {gender}");
        }
    }
}
