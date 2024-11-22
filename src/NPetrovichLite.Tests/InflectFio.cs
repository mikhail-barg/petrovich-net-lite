using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NPetrovichLite.Tests
{
    [TestFixture]
    public sealed class InflectFio
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
            Petrovich.FIO fio = new Petrovich.FIO() {
                lastName = "Иванов",
                firstName = "Иван",
                midName = "Иванович"
            };

            Petrovich.FIO result = petrovich.Inflect(fio, Case.Nominative);
            Assert.That(fio.Equals(result));

            result = petrovich.Inflect(fio, Case.Genitive);
            Assert.That("Иванова".Equals(result.lastName));
            Assert.That("Ивана".Equals(result.firstName));
            Assert.That("Ивановича".Equals(result.midName));
        }

        
#pragma warning disable S125 // Sections of code should not be "commented out"
/*
        [Test]
        [TestCaseSource(typeof(TestDataFactory), nameof(TestDataFactory.LastNamesData))]
        [TestCaseSource(typeof(TestDataFactory), nameof(TestDataFactory.FirstNamesData))]
        [TestCaseSource(typeof(TestDataFactory), nameof(TestDataFactory.MidNamesData))]
        public void TestMultipleBySinglePartInflection(string value, NamePart part, Gender gender, Case targetCase, string expected)
        {
            switch (part)
            {
            case NamePart.LastName:
                {
                    string result = petrovich.Inflect(new Petrovich.FIO() { lastName = value }, targetCase, gender).lastName;
                    Assert.AreEqual(expected, result, string.Format("Part: {0}, Gender: {1}, Case: {2}", part, gender, targetCase));
                }
                break;
            case NamePart.FirstName:
                {
                    string result = petrovich.Inflect(new Petrovich.FIO() { firstName = value }, targetCase, gender).firstName;
                    Assert.AreEqual(expected, result, string.Format("Part: {0}, Gender: {1}, Case: {2}", part, gender, targetCase));
                }
                break;
            case NamePart.MiddleName:
                {
                    string result = petrovich.Inflect(new Petrovich.FIO() { midName = value }, targetCase, gender).midName;
                    Assert.AreEqual(expected, result, string.Format("Part: {0}, Gender: {1}, Case: {2}", part, gender, targetCase));
                }
                break;
            }
            
        }
        */

        [Test]
#pragma warning restore S125 // Sections of code should not be "commented out"
        [TestCaseSource(typeof(TestDataFactory), nameof(TestDataFactory.ReadPeopleCombinedInflectionData))]
        public void TestMultipleInflection(string lastName, string firstName, string midName, Gender? gender, Case @case, string expectedLast, string expectedFirst, string expectedMid)
        {
            Petrovich.FIO source = new Petrovich.FIO() {
                lastName = lastName,
                firstName = firstName,
                midName = midName
            };
            Petrovich.FIO result = petrovich.Inflect(source, @case, gender);
            Assert.That(result.lastName, Is.EqualTo(expectedLast), string.Format("Part: {0}, Gender: {1}, Case: {2}", NamePart.LastName, gender, @case));
            Assert.That(result.firstName, Is.EqualTo(expectedFirst), string.Format("Part: {0}, Gender: {1}, Case: {2}", NamePart.FirstName, gender, @case));
            Assert.That(result.midName, Is.EqualTo(expectedMid), string.Format("Part: {0}, Gender: {1}, Case: {2}", NamePart.MiddleName, gender, @case));
        }
    }
}
