using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace task2
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("Enter command: ");
                string command = Console.ReadLine();

                if (command == "gen_db")
                {
                    Console.Write("File name: ");
                    string f = Console.ReadLine();

                    Console.Write("Directory path: ");
                    string d = Console.ReadLine();

                    int n, m;
                    Console.Write("n = ");
                    if (!int.TryParse(Console.ReadLine(), out n))
                    {
                        Console.WriteLine("ERROR");
                        continue;
                    }
                    Console.Write("m = ");
                    if (!int.TryParse(Console.ReadLine(), out m))
                    {
                        Console.WriteLine("ERROR");
                        continue;
                    }

                    ProcessGenDb(f, d, n, m);
                }
                else if (command == "get_award")
                {
                    Console.Write("Directory: ");
                    string d = Console.ReadLine();

                    Console.Write("Award: ");
                    string award = Console.ReadLine();

                    List<Oscar> awards = ProcessGetAward(d, award);
                    foreach (Oscar item in awards)
                    {
                        Console.WriteLine(item.ToString());
                    }
                }
                else if (command == "merge_xml")
                {
                    Console.Write("Directory: ");
                    string d = Console.ReadLine();

                    Console.Write("File for writing: ");
                    string fout = Console.ReadLine();

                    ProcessMergeXml(d, fout);
                }
            }
        }

        static void ProcessGenDb(string f, string d, int n, int m)
        {
            
        }

        static List<Oscar> ProcessGetAward(string d, string award)
        {
            List<Oscar> awards = new Repository(d).GetAward(award);
            return awards;
        }

        static void ProcessMergeXml(string d, string fout)
        {
            HashSet<Oscar> oscarsList = new Repository(d).GetAll();
            XmlSerializer ser = new XmlSerializer(typeof(List<Oscar>));
            System.IO.StreamWriter writer = new System.IO.StreamWriter(fout);
            ser.Serialize(writer, oscarsList);
            writer.Close();
        }
    }
}
