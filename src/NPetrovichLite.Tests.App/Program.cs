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

            //Petrovich petrovich = new Petrovich(@"c:\Dev\_Projects\petrovich-net\src\rules\rules.json");
            /*
            Petrovich petrovich = new Petrovich();
            Console.WriteLine(petrovich.Inflect("Маша", NamePart.FirstName, Case.Dative));
            Console.WriteLine(petrovich.Inflect("Паша", NamePart.FirstName, Case.Dative));
            Console.WriteLine(petrovich.Inflect("Саша", NamePart.FirstName, Case.Dative, Gender.Female));
            */

            /*
            Petrovich petrovich = new Petrovich();
            Petrovich.FIO a = petrovich.Inflect(new Petrovich.FIO() { lastName = "Пушкин", firstName = "Александр", midName = "Сергеевич" }, Case.Dative);
            Console.WriteLine(a);
            Petrovich.FIO b = petrovich.Inflect(new Petrovich.FIO() { lastName = "Воробей" }, Case.Dative, Gender.Female);
            Console.WriteLine(b);
            Petrovich.FIO c = petrovich.Inflect(new Petrovich.FIO() { lastName = "Воробей", firstName = "Александр" }, Case.Dative);
            Console.WriteLine(c);
            */

            /*
            Petrovich petrovich = new Petrovich();
            Console.WriteLine(petrovich.GetGender("Пушкин", NamePart.LastName));
            Console.WriteLine(petrovich.GetGender("Пушкин", null, "Сергеевич"));
            Console.WriteLine(petrovich.GetGender(new Petrovich.FIO() { lastName = "Воробей", firstName = "Александр" }));
            */

            /*
            InflectSinglePart testFixture = new InflectSinglePart();
            testFixture.Init();
            MethodInfo mi = testFixture.GetType().GetMethod(nameof(InflectSinglePart.TestSinglePartInflection));
            foreach (object[] par in TestDataFactory.ReadSurnamesData())
            {
                mi.Invoke(testFixture, par);
            }
            */
        }
    }
}
