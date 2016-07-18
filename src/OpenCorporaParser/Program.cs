using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OpenCorporaParser
{
    class Program
    {
        static void Main(string[] args)
        {
            string filename = args[0];
            Console.WriteLine("reading dom " + filename);
            XmlDocument doc = new XmlDocument();
            using (StreamReader reader = new StreamReader(filename, Encoding.UTF8))
            {
                doc.Load(reader);
            }

            Console.WriteLine("listing surnames");
            XmlNodeList nodes = doc.SelectNodes("/dictionary/lemmata/lemma[l/g[@v=\"Surn\"] and f/g[@v=\"sing\"]]");

            OrderedDictionary lemmaToGender = new OrderedDictionary();
            Console.WriteLine("writing to file");
            using (StreamWriter writerFull = new StreamWriter("surnames.oc.tsv", false, Encoding.UTF8))
            {
                writerFull.WriteLine("lemma\tword\tgrammemes");
                foreach (XmlNode lemmaNode in nodes)
                {
                    XmlNode lemmaSubNode = lemmaNode["l"];
                    string lemma = lemmaSubNode.Attributes["t"].Value.ToUpper();
                    List<string> grammemes = lemmaSubNode.OfType<XmlNode>().Select(g => g.Attributes["v"].Value).ToList();
                    string gender = null;
                    bool isFixed = false;
                    foreach (string gr in grammemes)
                    {
                        switch (gr)
                        {
                        case "NOUN":    //существительное
                        case "anim":    //одушевленное
                        case "Surn":    //фамилия
                        case "Sgtm":    //единственное число (singularia tantum)
                                        //not interesting, or already known, nothing to do
                            break;
                        case "Pltm":    //множественное число (pluralia tantum)
                                        //skipping
                            continue;
                        case "masc":
                            if (gender != null)
                            {
                                throw new ApplicationException($"Two genders for lemma {lemma}");
                            }
                            gender = "мр";
                            break;
                        case "femn":
                            if (gender != null)
                            {
                                throw new ApplicationException($"Two genders for lemma {lemma}");
                            }
                            gender = "жр";
                            break;
                        case "Ms-f":    //общий род
                        case "GNdr":    //род / род не выражен
                            if (gender != null && gender != "мр-жр")
                            {
                                throw new ApplicationException($"Two genders for lemma {lemma}");
                            }
                            gender = "мр-жр";
                            break;
                        case "Fixd":
                            isFixed = true;
                            break;
                        default:
                            Console.WriteLine(gr);
                            break;
                        }
                    }
                    if (gender == null)
                    {
                        throw new ApplicationException($"No gender detected for '{lemma}'");
                    }
                    if (lemmaToGender.Contains(lemma))
                    {
                        //already had an item with same lemma, so it must be another gender
                        if ((string)lemmaToGender[lemma] == gender)
                        {
                            throw new ApplicationException($"Duplicate lemma {lemma} with same gender {gender}");
                        }
                        lemmaToGender[lemma] = "мр-жр";
                    }
                    else
                    {
                        lemmaToGender[lemma] = gender;
                    }

                    if (isFixed)
                    {
                        writerFull.WriteLine($"{lemma}\t{lemma}\t{gender},ед,0");
                    }
                    else
                    {
                        foreach (XmlNode childNode in lemmaNode.SelectNodes("f[g[@v=\"sing\"]]"))
                        {
                            string form = childNode.Attributes["t"].Value.ToUpper();
                            string @case = ConvertCase(childNode, lemma);
                            if (@case == null)
                            {
                                continue;   //unsupported weird case
                            }
                            writerFull.WriteLine($"{lemma}\t{form}\t{gender},ед,{@case}");
                        }
                    }
                }
            }

            using (StreamWriter writerGender = new StreamWriter("surnames.oc.gender.tsv", false, Encoding.UTF8))
            {
                writerGender.WriteLine("lemma\tgender");
                foreach (DictionaryEntry entry in lemmaToGender)
                {
                    writerGender.WriteLine($"{entry.Key}\t{entry.Value}");
                }
            }
        }

private static string ConvertCase(XmlNode childNode, string lemma)
        {
            string @case = null;
            string source = childNode.SelectSingleNode("g[@v!=\"sing\"]").Attributes["v"].Value;
            switch (source)
            {
            case "nomn":
                @case = "им";
                break;
            case "gent":
                @case = "рд";
                break;
            case "datv":
                @case = "дт";
                break;
            case "accs":
                @case = "вн";
                break;
            case "ablt":
                @case = "тв";
                break;
            case "loct":
                @case = "пр";
                break;
            case "voct":    //звательный падеж
            case "gen1":    //первый родительный падеж
            case "gen2":    //второй родительный (частичный) падеж
            case "acc2":    //второй винительный падеж
            case "loc1":    //первый предложный падеж
            case "loc2":    //второй предложный (местный) падеж
                return null; //no support for now
            default:
                throw new ApplicationException($"Unknown case {source} for lemma {lemma}");
            }
            if (@case == null)
            {
                throw new ApplicationException($"No case at all for lemma {lemma}");
            }
            return @case;
        }
    }
}
