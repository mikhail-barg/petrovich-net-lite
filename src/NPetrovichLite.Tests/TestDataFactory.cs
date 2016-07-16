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
                        //actually, '0' in 'case' column means that the test valid for all cases
                        // but for now there's only a single such line, and it also contains unsupported 'мр-жр' so we just skip it
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
                return Case.Accusative;
            case "тв":
                return Case.Instrumental;
            case "пр":
                return Case.Prepositional;
            default:
                throw new ApplicationException("Bad value: '" + value + "'");
            }
        }

        public static IEnumerable ReadGendersSingleData()
        {
            using (StreamReader reader = new StreamReader(Path.Combine(NUnit.Framework.TestContext.CurrentContext.TestDirectory, "Data", "GendersSingle.csv")))
            {
                string line;
                line = reader.ReadLine();  //skip header
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    string[] chunks = line.Split(',').Select(s => s.Trim()).ToArray();
                    Gender gender = (Gender)Enum.Parse(typeof(Gender), chunks[0]);
                    NamePart namePart = (NamePart)Enum.Parse(typeof(NamePart), chunks[1]);

                    yield return new object[] { namePart, chunks[2], gender };
                }
            }
        }

        public static IEnumerable ReadGendersMultipleData()
        {
            using (StreamReader reader = new StreamReader(Path.Combine(NUnit.Framework.TestContext.CurrentContext.TestDirectory, "Data", "GendersMultiple.csv")))
            {
                string line;
                line = reader.ReadLine();  //skip header
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    string[] chunks = line.Split(',')
                        .Select(s => String.IsNullOrWhiteSpace(s)? null : s.Trim())
                        .ToArray();
                    Gender gender = (Gender)Enum.Parse(typeof(Gender), chunks[0]);

                    yield return new object[] { chunks[1], chunks[2], chunks[3], gender };
                }
            }
        }

        public static IEnumerable ReadCombinedData()
        {
            using (StreamReader reader = new StreamReader(Path.Combine(NUnit.Framework.TestContext.CurrentContext.TestDirectory, "Data", "Combined.csv")))
            {
                string line;
                line = reader.ReadLine();  //skip header
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    string[] chunks = line.Split(',')
                        .Select(s => String.IsNullOrWhiteSpace(s) ? null : s.Trim())
                        .ToArray();
                    Gender? gender = chunks[0] == null? null : (Gender?)Enum.Parse(typeof(Gender), chunks[0]);
                    Case @case = (Case)Enum.Parse(typeof(Case), chunks[4]);

                    yield return new object[] { chunks[1], chunks[2], chunks[3], gender, @case, chunks[5], chunks[6], chunks[7] };
                }
            }
        }
    }
}
