using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPetrovichLite
{
    public class Petrovich
    {
        private static readonly char[] NAME_CHUNK_SPLIT = { '-' };

        private readonly RulesContainer m_rules;

        public struct FIO
        {
            public string lastName;
            public string firstName;
            public string midName;
        }

        public Petrovich()
        {
            m_rules = JsonRulesLoader.LoadEmbeddedResource();
        }

        public Petrovich(string rulesFileName)
        {
            m_rules = JsonRulesLoader.LoadFromFile(rulesFileName);
        }

        public string Inflect(string namePartValue, NamePart namePart, Case targetCase, Gender? gender = null)
        {
            if (namePartValue == null)
            {
                throw new ArgumentNullException(nameof(namePartValue));
            }
            if (gender == null)
            {
                gender = GetGender(namePartValue, namePart);
            }

            PartRules partRules = m_rules[namePart];
            string[] chunks = namePartValue.Split(NAME_CHUNK_SPLIT);
            for (int i = 0; i < chunks.Length; ++i)
            {
                Tags tags = i == 0 ? Tags.FirstWord : Tags.None;
                chunks[i] = partRules.InflectChunk(chunks[i], gender.Value, tags, targetCase);
            }
            return String.Join("-", chunks);
        }

        public FIO Inflect(FIO fio, Case targetCase, Gender? gender = null)
        {
            if (gender == null)
            {
                gender = GetGender(fio);
            }
            if (fio.lastName != null)
            {
                fio.lastName = Inflect(fio.lastName, NamePart.LastName, targetCase, gender.Value);
            }
            if (fio.firstName != null)
            {
                fio.firstName = Inflect(fio.firstName, NamePart.FirstName, targetCase, gender.Value);
            }
            if (fio.midName != null)
            {
                fio.midName = Inflect(fio.midName, NamePart.MiddleName, targetCase, gender.Value);
            }
            return fio;
        }

        public Gender GetGender(string namePartValue, NamePart namePart)
        {
            if (namePartValue == null)
            {
                throw new ArgumentNullException(nameof(namePartValue));
            }
            GenderRules rules = m_rules.genderRules[namePart];
            foreach (KeyValuePair<string, Gender> pair in rules)
            {
                if (namePartValue.EndsWith(pair.Key, StringComparison.InvariantCultureIgnoreCase))
                {
                    return pair.Value;
                }
            }
            return Gender.Androgynous;
        }

        public Gender GetGender(string lastName = null, string firstName = null, string midName = null)
        {
            if (lastName == null && firstName == null && midName == null)
            {
                throw new ArgumentNullException("All arguments are null");
            }

            if (midName != null)
            {
                Gender gender = GetGender(midName, NamePart.MiddleName);
                if (gender != Gender.Androgynous)
                {
                    //Return gender if middlename is specified and gender is determined.
                    return gender;
                }
                //otherwise no interest in midName at all
            }

            Gender firstNameGender = firstName != null? GetGender(firstName, NamePart.FirstName) : Gender.Androgynous;
            Gender lastNameGender = lastName != null? GetGender(lastName, NamePart.LastName) : Gender.Androgynous;

            if (firstNameGender == lastNameGender)
            {
                return firstNameGender;
            }
            if (firstNameGender == Gender.Androgynous)
            {
                return lastNameGender;
            }
            if (lastNameGender == Gender.Androgynous)
            {
                return firstNameGender;
            }
            if (firstNameGender != lastNameGender)
            {
                //weird case should not happen probably
            }
            return Gender.Androgynous;
        }

        public Gender GetGender(FIO fio)
        {
            return GetGender(fio.lastName, fio.firstName, fio.midName);
        }


    }
}
