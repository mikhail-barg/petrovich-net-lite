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

        public Petrovich()
        {
            m_rules = JsonRulesLoader.LoadEmbeddedResource();
        }

        public Petrovich(string rulesFileName)
        {
            m_rules = JsonRulesLoader.LoadFromFile(rulesFileName);
        }

        public string InflectNamePart(string namePartValue, NamePart namePart, Gender gender, Case targetCase)
        {
            if (namePartValue == null)
            {
                throw new ArgumentNullException(nameof(namePartValue));
            }
            PartRules partRules = m_rules[namePart];
            string[] chunks = namePartValue.Split(NAME_CHUNK_SPLIT);
            for (int i = 0; i < chunks.Length; ++i)
            {
                Tags tags = i == 0 ? Tags.FirstWord : Tags.None;
                chunks[i] = partRules.InflectChunk(chunks[i], gender, tags, targetCase);
            }
            return String.Join("-", chunks);
        }

        public Gender? TryGetGender(string namePartValue, NamePart namePart)
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
            return null;
        }


    }
}
