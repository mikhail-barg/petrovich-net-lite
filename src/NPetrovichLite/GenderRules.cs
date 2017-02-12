using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPetrovichLite
{
    internal sealed class GenderRulesContainer
    {
        private readonly PartGenderRules[] m_rulesByNamePart;

        internal GenderRulesContainer()
        {
            m_rulesByNamePart = new PartGenderRules[Enum.GetValues(typeof(NamePart)).Length];
            for (int i = 0; i < m_rulesByNamePart.Length; ++i)
            {
                m_rulesByNamePart[i] = new PartGenderRules();
            }
        }

        internal PartGenderRules this[NamePart part]
        {
            get { return m_rulesByNamePart[(int)part]; }
        }
    }

    internal sealed class PartGenderRules
    {
        private readonly Dictionary<string, Gender> m_explicitMatchRules = new Dictionary<string, Gender>(StringComparer.InvariantCultureIgnoreCase);
        private readonly Dictionary<string, Gender> m_suffixMatchRules = new Dictionary<string, Gender>();

        internal bool TryMatch(string namePartValue, out Gender gender)
        {
            if (m_explicitMatchRules.TryGetValue(namePartValue, out gender))
            {
                return true;
            }
            foreach (KeyValuePair<string, Gender> pair in m_suffixMatchRules)
            {
                if (namePartValue.EndsWith(pair.Key, StringComparison.InvariantCultureIgnoreCase))
                {
                    gender = pair.Value;
                    return true;
                }
            }
            gender = Gender.Androgynous;
            return false;
        }

        internal void AddSuffixRule(string suffix, Gender gender)
        {
            if (m_suffixMatchRules.ContainsKey(suffix))
            {
                throw new ParseException(String.Format("Duplicate suffix '{0}' for gender {1}", suffix, gender));
            }
            m_suffixMatchRules.Add(suffix, gender);
        }

        internal void AddExplicitMatchRule(string word, Gender gender)
        {
            if (m_explicitMatchRules.ContainsKey(word))
            {
                throw new ParseException(String.Format("Duplicate explicit (exception) word '{0}' for gender {1}", word, gender));
            }
            m_explicitMatchRules.Add(word, gender);
        }
    }


}
