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
        private static Gender[] MALE_GENDER_LIST = new Gender[] { Gender.Male };
        private static Gender[] FEMALE_GENDER_LIST = new Gender[] { Gender.Female };
        private static Gender[] BOTH_GENDER_LIST = new Gender[] { Gender.Female, Gender.Male };
        public static IEnumerable OpencorporaInflectionData(string fileName, NamePart namePart)
        {
            using (StreamReader reader = new StreamReader(Path.Combine(NUnit.Framework.TestContext.CurrentContext.TestDirectory, "Data", fileName)))
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

                    foreach (Tuple<Gender, Case> inflection in ParseGrammemes(chunks[2]))
                    {
                        yield return new object[] { chunks[0], namePart, inflection.Item1, inflection.Item2, chunks[1] };
                    }
                }
            }
        }

        public static IEnumerable OpencorporaGenderDetectionData(string fileName, NamePart namePart)
        {
            using (StreamReader reader = new StreamReader(Path.Combine(NUnit.Framework.TestContext.CurrentContext.TestDirectory, "Data", fileName)))
            {
                string line;
                line = reader.ReadLine();  //skip header
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    string[] chunks = line.ToLower()
                        .Split('\t')
                        .Select(s => s.Trim())
                        .ToArray();
                    Gender gender = ParseGenderForGenderDetection(chunks[1]);

                    yield return new object[] { chunks[0], namePart, gender};
                }
            }
        }

        private static IEnumerable<Tuple<Gender, Case>> ParseGrammemes(string grammemes)
        {
            string[] chunks2 = grammemes.Split(',');

            if (chunks2.Length > 3)
            {
                //weird lines "ЦЕЛИЙ	ЦЕЛИЙ	мр,имя,ед,им"
                yield break;
            }

            Gender[] genders;
            switch (chunks2[0])
            {
            case "жр":
                genders = FEMALE_GENDER_LIST;
                break;
            case "мр":
                genders = MALE_GENDER_LIST;
                break;
            case "мр-жр":
                genders = BOTH_GENDER_LIST;
                break;
            default:
                throw new ApplicationException($"Unexpected gender string '{chunks2[0]}'");
            }

            if (chunks2[2] == "0")
            {
                //actually, '0' in 'case' column means that the test valid for all cases
                foreach (Gender gender in genders)
                {
                    foreach (Case @case in Enum.GetValues(typeof(Case)))
                    {
                        yield return Tuple.Create(gender, @case);
                    }
                }
            }
            else
            {
                Case @case = Parse2LetterCase(chunks2[2]);
                foreach (Gender gender in genders)
                {
                    yield return Tuple.Create(gender, @case);
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

        private static Gender ParseGenderForGenderDetection(string genderStr)
        {
            switch (genderStr)
            {
            case "жр":
                return Gender.Female;
            case "мр":
                return Gender.Male;
            case "мр-жр":
                return Gender.Androgynous;
            default:
                throw new ApplicationException($"Unexpected gender string '{genderStr}'");
            }
        }

        public static IEnumerable ReadPeopleCombinedGenderData()
        {
            using (StreamReader reader = new StreamReader(Path.Combine(NUnit.Framework.TestContext.CurrentContext.TestDirectory, "Data", "people.gender.tsv")))
            {
                string line;
                line = reader.ReadLine();
                //skip header
                //lastname	firstname	middlename	gender
                //0         1           2           3
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    string[] chunks = line.ToLower()
                        .Split('\t')
                        .Select(s => s.Trim())
                        .Select(s => String.IsNullOrWhiteSpace(s) ? null : s)
                        .ToArray();

                    Gender gender = ParseGenderForGenderDetection(chunks[3]);

                    yield return new object[] { chunks[0], chunks[1], chunks[2], gender };
                }
            }
        }

        public static IEnumerable ReadPeopleCombinedInflectionData()
        {
            using (StreamReader reader = new StreamReader(Path.Combine(NUnit.Framework.TestContext.CurrentContext.TestDirectory, "Data", "people.tsv")))
            {
                string line;
                line = reader.ReadLine();  //skip header
                //header is 
                //0         1           2       3                   4                   5                   6
                //lastname	firstname	midname	lastname_expected	firstname_expected	middlename_expected	grammemes
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    string[] chunks = line.ToLower()
                        .Split('\t')
                        .Select(s => s.Trim())
                        .Select(s => String.IsNullOrWhiteSpace(s) ? null : s)
                        .ToArray();

                    foreach (Tuple<Gender, Case> inflection in ParseGrammemes(chunks[6]))
                    {
                        yield return new object[] { chunks[0], chunks[1], chunks[2], inflection.Item1, inflection.Item2, chunks[3], chunks[4], chunks[5] };
                    }
                }
            }
        }
    }
}
