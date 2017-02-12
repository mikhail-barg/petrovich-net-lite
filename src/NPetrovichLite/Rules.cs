using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPetrovichLite
{
    internal sealed class RulesContainer
    {
        private readonly PartRules[] m_partRules = new PartRules[Enum.GetValues(typeof(NamePart)).Length];

        internal PartRules this[NamePart part]
        {
            get { return m_partRules[(int)part]; }
            set { m_partRules[(int)part] = value; }
        }

        internal readonly GenderRulesContainer genderRules = new GenderRulesContainer();
    }

    internal sealed class PartRules : List<IRule>
    {
        internal string InflectChunk(string chunk, Gender gender, Tags tags, Case targetCase)
        {
            foreach (IRule rule in this)
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

    internal interface IRule
    {
        bool Matches(string nameChunk, Gender gender, Tags tags);
        string Inflect(string nameChunk, Case targetCase);
    }

    internal abstract class BaseRule : IRule
    {
#if STORE_SOURCE_LINE_IN_RULES
        public int startLineIndex;
#endif

        private readonly Gender m_gender;
        private readonly Tags m_tags;
        private readonly IModifier[] m_modifiers;

        protected BaseRule(Gender gender, Tags tags, IModifier[] modifiers)
        {
            m_gender = gender;
            m_tags = tags;
            m_modifiers = modifiers;
        }

        public bool Matches(string nameChunk, Gender gender, Tags tags)
        {
            //return false if gender == :male && match_gender == :female
            if (m_gender == Gender.Male && gender == Gender.Female)
            {
                return false;
            }

            //return false if gender == :female && match_gender != :female
            if (m_gender == Gender.Female && gender != Gender.Female)
            {
                return false;
            }

            if ((tags | m_tags) != tags)
            {
                return false;
            }

            return MatchesInternal(nameChunk);
        }

        protected abstract bool MatchesInternal(string nameChunk);

        public string Inflect(string nameChunk, Case targetCase)
        {
            if (targetCase == Case.Nominative)
            {
                return nameChunk;
            }
            return m_modifiers[(int)targetCase - 1].Apply(nameChunk);
        }
    }

    internal sealed class ExceptionRule : BaseRule
    {
        private readonly string[] m_testWords;

        public ExceptionRule(Gender gender, Tags tags, string[] testWords, IModifier[] modifiers) 
            : base(gender, tags, modifiers)
        {
            m_testWords = testWords;
        }

        protected override bool MatchesInternal(string nameChunk)
        {
            for (int i = 0; i < m_testWords.Length; ++i)
            {
                if (String.Equals(m_testWords[i], nameChunk, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }

    internal sealed class SufixRule : BaseRule
    {
        private readonly string[] m_testSuffixes;

        public SufixRule(Gender gender, Tags tags, string[] testSuffixes, IModifier[] modifiers)
            : base(gender, tags, modifiers)
        {
            m_testSuffixes = testSuffixes;
        }

        protected override bool MatchesInternal(string nameChunk)
        {
            for (int i = 0; i < m_testSuffixes.Length; ++i)
            {
                if (nameChunk.EndsWith(m_testSuffixes[i], StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }

    internal interface IModifier
    {
        string Apply(string nameChunk);
    }

    internal sealed class IdentityModifier : IModifier
    {
        internal static readonly IdentityModifier Instance = new IdentityModifier();

        public string Apply(string nameChunk)
        {
            return nameChunk;
        }
    }

    internal sealed class SuffixModifier : IModifier
    {
        private readonly int m_trimEndChars;
        private readonly string m_addSuffix;

        internal SuffixModifier(string modiferDescription)
        {
            int trimChars = 0;
            while (trimChars < modiferDescription.Length && modiferDescription[trimChars] == '-')
            {
                ++trimChars;
            }
            m_trimEndChars = trimChars;
            m_addSuffix = modiferDescription.Substring(trimChars);
        }

        public string Apply(string nameChunk)
        {
            return nameChunk.Substring(0, nameChunk.Length - m_trimEndChars) + m_addSuffix;
        }
    }
}
