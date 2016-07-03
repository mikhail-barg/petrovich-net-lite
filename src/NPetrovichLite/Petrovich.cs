using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPetrovichLite
{
    public static class Petrovich
    {
        private static readonly char[] NAME_CHUNK_SPLIT = { '-' };

        private static string InflectNamePart(string namePartValue, NamePart namePart, Gender gender, Case targetCase, RulesContainer rules)
        {
            if (namePartValue == null)
            {
                throw new ArgumentNullException(nameof(namePartValue));
            }
            /*
            if (string.IsNullOrWhiteSpace(namePartValue))
            {
                throw new ArgumentException("name part is whitespace or empty", nameof(namePartValue));
            }
            */
            PartRules partRules = rules[namePart];
            string[] chunks = namePartValue.ToLower().Split(NAME_CHUNK_SPLIT);
            for (int i = 0; i < chunks.Length; ++i)
            {
                Tags tags = i == 0 ? Tags.FirstWord : Tags.None;
                chunks[i] = InflectChunk(chunks[i], partRules, gender, tags, targetCase);
            }
            return String.Join("-", chunks);
        }

        private static string InflectChunk(string chunk, PartRules partRules, Gender gender, Tags tags, Case targetCase)
        {
            foreach (IRule rule in partRules)
            {
                if (rule.Matches(chunk, gender, tags))
                {
                    return rule.Inflect(chunk, targetCase);
                }
            }
            //TODO: log unhandled cases
            return chunk;
        }
    }
}
