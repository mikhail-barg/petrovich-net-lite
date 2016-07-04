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

        private static IEnumerable ReadSinglePartData(string fileName, NamePart part)
        {
            using (StreamReader reader = new StreamReader(Path.Combine("Data", fileName)))
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
    }
}
