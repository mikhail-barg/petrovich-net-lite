﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPetrovichLite.Tests.App
{
    class Program
    {
        static void Main(string[] args)
        {
            //Petrovich petrovich = new Petrovich(@"c:\Dev\_Projects\petrovich-net\src\rules\rules.json");
            Petrovich petrovich = new Petrovich();
            string result = petrovich.InflectNamePart("Маша", NamePart.FirstName, Gender.Female, Case.Instrumental);
            Console.WriteLine(result);
        }
    }
}
