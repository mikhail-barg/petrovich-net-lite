using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPetrovichLite.Tests
{
    public sealed class TestDataFactory
    {
        public static IEnumerable LastNamesData()
        {
            return ReadSinglePartData("LastNames.csv", NamePart.LastName);
        }

        public static IEnumerable FirstNamesData()
        {
            return ReadSinglePartData("FirstNames.csv", NamePart.FirstName);
        }

        public static IEnumerable MidNamesData()
        {
            return ReadSinglePartData("MiddleNames.csv", NamePart.MiddleName);
        }

        private static IEnumerable ReadSinglePartData(string fileName, NamePart part)
        {
            using (StreamReader reader = new StreamReader(Path.Combine(NUnit.Framework.TestContext.CurrentContext.TestDirectory, "Data", fileName)))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    string[] chunks = line.Split(',').Select(s => s.Trim()).ToArray();

                    Gender gender = (Gender)Enum.Parse(typeof(Gender), chunks[1]);
                    Case @case = (Case)Enum.Parse(typeof(Case), chunks[2]);

                    yield return new object[] { chunks[0], part, gender, @case, chunks[3] };
                }
            }
        }

        public static IEnumerable ReadSurnamesData()
        {
            using (StreamReader reader = new StreamReader(Path.Combine(NUnit.Framework.TestContext.CurrentContext.TestDirectory, "Data", "surnames.tsv")))
            {
                string line;
                line = reader.ReadLine();  //skip header
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    string[] chunks = line.ToLower().Split('\t').Select(s => s.Trim()).ToArray();
                    string[] chunks2 = chunks[2].Split(',');

                    if (chunks2[2] == "0")
                    {
                        //weird line "ВИНЧИ	ВИНЧИ	мр-жр,ед,0"
                        continue;
                    }
                    if (chunks2.Length > 3)
                    {
                        //weird lines "ЦЕЛИЙ	ЦЕЛИЙ	мр,имя,ед,им"
                        continue;
                    }

                    Gender gender = chunks2[0] == "жр" ? Gender.Female : Gender.Male;
                    Case @case = Parse2LetterCase(chunks2[2]);

                    yield return new object[] { chunks[0], NamePart.LastName, gender, @case, chunks[1] };
                }
            }
        }

        private static Case Parse2LetterCase(string value)
        {
            switch (value)
            {
            case "им":
                return Case.Nominative;
            case "рд":
                return Case.Genitive;
            case "дт":
                return Case.Dative;
            case "вн":
                return Case.Nominative;
            case "тв":
                return Case.Instrumental;
            case "пр":
                return Case.Prepositional;
            default:
                throw new ApplicationException("Bad value: '" + value + "'");
            }
        }
    }
}
