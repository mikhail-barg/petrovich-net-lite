using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NPetrovichLite.Tests
{
    [TestFixture]
    public sealed class GenderDetection
    {
        private Petrovich petrovich;

        [OneTimeSetUp]
        public void Init()
        {
            petrovich = new Petrovich();
        }

        [Test]
        [TestCaseSource(typeof(TestDataFactory), nameof(TestDataFactory.ReadGendersSingleData))]
        public void TestSinglePartGender(string value, NamePart part, Gender expected)
        {
            Gender? gender = petrovich.GetGender(value, part);
            Assert.AreEqual(expected, gender, string.Format("Part: {0}, Value: {1}", part, value));
        }


        [Test]
        [TestCaseSource(typeof(TestDataFactory), nameof(TestDataFactory.ReadGendersMultipleData))]
        public void TestGenderDetection(string lastName, string firstName, string midName, Gender expected)
        {
            Gender? gender = petrovich.GetGender(lastName, firstName, midName);
            Assert.AreEqual(expected, gender, string.Format("last: {0}, first: {1}, mid: {2}", lastName, firstName, midName));
        }
    }
}
