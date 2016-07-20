using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NPetrovichLite.Tests
{
    [TestFixture(Category = "OpenCorpora")]
    public sealed class OpencorporaTests
    {
        private Petrovich petrovich;

        [OneTimeSetUp]
        public void Init()
        {
            petrovich = new Petrovich();
        }


        /*
        [Test]
        //[TestCaseSource(typeof(TestDataFactory), nameof(TestDataFactory.OpencorporaInflectionData), new object[] { "surnames.tsv", NamePart.LastName })]
        [TestCaseSource(typeof(TestDataFactory), nameof(TestDataFactory.OpencorporaInflectionData), new object[] { "firstnames.tsv", NamePart.FirstName })]
        public void TestSinglePartInflection(string value, NamePart part, Gender? gender, Case targetCase, string expected)
        {
            string result = petrovich.Inflect(value, part, targetCase, gender);
            Assert.AreEqual(expected, result, $"Value: {value}, Part: {part}, Gender: {gender}");
        }
        */

        /*
        [Test]
        //[TestCaseSource(typeof(TestDataFactory), nameof(TestDataFactory.OpencorporaGenderDetectionData), new object[] { "surnames.gender.tsv", NamePart.LastName})]
        [TestCaseSource(typeof(TestDataFactory), nameof(TestDataFactory.OpencorporaGenderDetectionData), new object[] { "firstnames.gender.tsv", NamePart.FirstName })]
        public void TestGenderDetection(string value, NamePart part, Gender expected)
        {
            Gender gender = petrovich.GetGender(value, part);
            Assert.AreEqual(expected, gender, $"Value: {value} Part: {part}");
        }
        */
    }
}
