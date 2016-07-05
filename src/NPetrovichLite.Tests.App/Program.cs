using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NPetrovichLite.Tests.App
{
    class Program
    {
        static void Main(string[] args)
        {
            //NPetrovichLite.Tests.TestDataFactory.ReadSurnamesData().OfType<object[]>().ToList();

            /*
            //Petrovich petrovich = new Petrovich(@"c:\Dev\_Projects\petrovich-net\src\rules\rules.json");
            Petrovich petrovich = new Petrovich();
            string result = petrovich.InflectNamePart("Маша", NamePart.FirstName, Gender.Female, Case.Instrumental);
            Console.WriteLine(result);
            */

            InflectSinglePart testFixture = new InflectSinglePart();
            testFixture.Init();
            MethodInfo mi = testFixture.GetType().GetMethod(nameof(InflectSinglePart.TestSinglePartInflection));
            foreach (object[] par in TestDataFactory.ReadSurnamesData())
            {
                mi.Invoke(testFixture, par);
            }
        }
    }
}
