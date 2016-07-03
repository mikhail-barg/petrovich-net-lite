using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NPetrovichLite
{
    internal class YamlRulesLoader
    {
        private const string RESOURCE_NAME = "NPetrovichLite.rules.yml";

        internal static RulesContainer LoadEmbeddedResource()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(RESOURCE_NAME))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    YamlRulesLoader loader = new YamlRulesLoader(reader);
                    return loader.Load();
                }
            }
        }
    }
}
