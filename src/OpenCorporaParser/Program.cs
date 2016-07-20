using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO.Compression;

namespace OpenCorporaParser
{
    class Program
    {
        private const string dictUrl = @"http://opencorpora.org/files/export/dict/dict.opcorpora.xml.zip";
        private const string dictArcFile = @"dict.opcorpora.xml.zip";
        private const string dictFile = @"dict.opcorpora.xml";

        static void Main(string[] args)
        {

            Console.WriteLine($"Downloading {dictArcFile} from OpenCorpora");
            if (!File.Exists(dictArcFile))
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(dictUrl, dictArcFile);
                }
            }
            else
            {
                Console.WriteLine("Already exists");
            }

            Console.WriteLine($"Unpacking to {dictFile}");
            if (!File.Exists(dictFile))
            {
                using (ZipArchive archive = ZipFile.OpenRead(dictArcFile))
                {
                    archive.Entries.Where(item => item.FullName == dictFile).First().ExtractToFile(dictFile);
                }
            }
            else
            {
                Console.WriteLine("Already exists");
            }

            Console.WriteLine($"reading dom from {dictFile}");
            XmlDocument doc = new XmlDocument();
            using (StreamReader reader = new StreamReader(dictFile, Encoding.UTF8))
            {
                doc.Load(reader);
            }
            ProcessNamePart(doc, "surnames", "Surn");
            ProcessNamePart(doc, "firstnames", "Name");
            ProcessNamePart(doc, "midnames", "Patr");

        }

        private static void ProcessNamePart(XmlDocument doc, string namePart, string namePartGrammeme)
        {
            Console.WriteLine($"listing {namePart}");
            XmlNodeList nodes = doc.SelectNodes(
                $@"/dictionary/lemmata/lemma[
                        l/g[@v=""{namePartGrammeme}""] 
                        and f/g[@v=""sing""] 
                        and not(l/g[@v=""Erro""]) 
                        and not(l/g[@v=""Abbr""]) 
                        and not(l/g[@v=""Infr""])
                ]"
            );

            OrderedDictionary lemmaToGender = new OrderedDictionary();
            Console.WriteLine("writing to file");
            using (StreamWriter writerFull = new StreamWriter($"{namePart}.tsv", false, new UTF8Encoding(false)))
            {
                writerFull.NewLine = "\n";
                writerFull.WriteLine("lemma\tword\tgrammemes");
                foreach (XmlNode lemmaNode in nodes)
                {
                    XmlNode lemmaSubNode = lemmaNode["l"];
                    string lemma = lemmaSubNode.Attributes["t"].Value.ToUpper();
                    List<string> grammemes = lemmaSubNode.OfType<XmlNode>().Select(g => g.Attributes["v"].Value).ToList();
                    string gender = null;
                    //bool isFixed = false;
                    foreach (string gr in grammemes)
                    {
                        if (gr == namePartGrammeme)
                        {
                            continue;   //already know it;
                        }

                        switch (gr)
                        {
                        case "NOUN":    //существительное
                        case "anim":    //одушевленное
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
                        case "GNdr":    //род / род не выражен
                            if (gender != null && gender != "мр-жр")
                            {
                                throw new ApplicationException($"Two genders for lemma {lemma}");
                            }
                            gender = "мр-жр";
                            break;
                        case "Ms-f":    //общий род
                            if (gender == null)
                            {
                                throw new ApplicationException($"Two genders for lemma {lemma}");
                            }
                            //gender = "мр-жр";
                            break;
                        case "Fixd":
                            //isFixed = true; //for fixed 
                            break;
                        case "inan":    //strangely happens sometimes, eg. "РОЖЕ"
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
                            //for some reason happens with "АБДАЛОВИЧ"
                            //throw new ApplicationException($"Duplicate lemma {lemma} with same gender {gender}");
                        }
                        else
                        {
                            lemmaToGender[lemma] = "мр-жр";
                        }
                    }
                    else
                    {
                        lemmaToGender[lemma] = gender;
                    }

                    /*
                    if (isFixed)
                    {
                        writerFull.WriteLine($"{lemma}\t{lemma}\t{gender},ед,0");
                    }
                    else
                    {
                    */
                    foreach (XmlNode childNode in lemmaNode.SelectNodes(
                        @"f[
                                g[@v=""sing""] 
                                and not(g[@v=""V-ey""]) and not(g[@v=""V-oy""])
                                and not(g[@v=""Dist""]) and not(g[@v=""Erro""])
                                and not(g[@v=""voct""]) and not(g[@v=""gen1""]) and not(g[@v=""gen2""]) and not(g[@v=""acc2""]) and not(g[@v=""loc1""]) and not(g[@v=""loc2""])
                        ]"
                        ))
                    {
                        string form = childNode.Attributes["t"].Value.ToUpper();
                        switch (form)   //fix mistakes i dict.opencorpora (see https://github.com/petrovich/petrovich-eval/pull/2/commits/4c33042ae0aa46bfbe4e5e46a289e3137aaa9549)
                        {
                        case "ЖУРАВЛВЁОЙ":
                            form = "ЖУРАВЛЁВОЙ";
                            break;
                        case "КОВАЁВУ":
                            form = "КОВАЛЁВУ";
                            break;
                        }
                        string @case = ConvertCase(childNode, lemma);
                        if (@case == null)
                        {
                            continue;   //unsupported weird case
                        }
                        if (childNode.ChildNodes.Count != 2)
                        {
                            throw new ApplicationException($"some strange info for lemma {lemma} in form {form}");
                        }
                        writerFull.WriteLine($"{lemma}\t{form}\t{gender},ед,{@case}");
                    }
                    //}
                }
            }

            using (StreamWriter writerGender = new StreamWriter($"{namePart}.gender.tsv", false, new UTF8Encoding(false)))
            {
                writerGender.NewLine = "\n";
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
