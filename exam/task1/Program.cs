using System;
using System.IO;
using System.Collections.Generic;

namespace exam
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("Enter command: ");
                string command = Console.ReadLine();

                // 1st part
                if (command == "gen_num")
                {
                    Console.Write("File path: ");
                    string f = Console.ReadLine();

                    int a, b, n;
                    Console.Write("a = ");
                    if (!int.TryParse(Console.ReadLine(), out a))
                    {
                        Console.WriteLine("ERROR");
                        continue;
                    }
                    Console.Write("b = ");
                    if (!int.TryParse(Console.ReadLine(), out b))
                    {
                        Console.WriteLine("ERROR");
                        continue;
                    }
                    Console.Write("n = ");
                    if (!int.TryParse(Console.ReadLine(), out n))
                    {
                        Console.WriteLine("ERROR");
                        continue;
                    }

                    ProcessGenNum(f, a, b, n);
                }
                else if (command == "num")
                {
                    Console.Write("File path: ");
                    string f = Console.ReadLine();

                    int maxDifference = ProcessNum(f);
                    Console.WriteLine(maxDifference);
                }
                else if (command == "num_uni")
                {
                    Console.Write("File with numbers: ");
                    string f = Console.ReadLine();

                    Console.Write("File for writing: ");
                    string fout = Console.ReadLine();
                    ProcessNumUni(f, fout);
                }

                // 2nd part
                else if (command == "gen_vec")
                {
                    Console.Write("File path: ");
                    string f = Console.ReadLine();

                    int a, b, n;
                    Console.Write("a = ");
                    if (!int.TryParse(Console.ReadLine(), out a))
                    {
                        Console.WriteLine("ERROR");
                        continue;
                    }
                    Console.Write("b = ");
                    if (!int.TryParse(Console.ReadLine(), out b))
                    {
                        Console.WriteLine("ERROR");
                        continue;
                    }
                    Console.Write("n = ");
                    if (!int.TryParse(Console.ReadLine(), out n))
                    {
                        Console.WriteLine("ERROR");
                        continue;
                    }

                    ProcessGenVec(f, a, b, n);
                }
                else if (command == "vec")
                {
                    Console.Write("File path: ");
                    string f = Console.ReadLine();

                    int numOfVecs = ProcessVec(f);
                    Console.WriteLine(numOfVecs);
                }
                else if (command == "exit")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Unknown command!");
                }
            }
        }

        // 1st part funcs
        static void ProcessGenNum(string f, int a, int b, int n)
        {
            StreamWriter sw = new StreamWriter(f);
            Random random = new Random();
            for (int i = 0; i < n; i++)
            {
                sw.WriteLine(random.Next(a, b));
            }
            sw.Close();
        }

        static int ProcessNum(string f)
        {
            StringReader sr = new StringReader(f);
            List<int> numbers = new List<int>();

            while (true)
            {
                string s = sr.ReadLine();
                if (s == null)
                {
                    sr.Close();
                    break;
                }
                numbers.Add(int.Parse(s));
            }

            int maxDifference = 0;
            for (int i = 0; i < numbers.Count - 1; i++)
            {
                if (maxDifference == 0)
                {
                    if (numbers[i] % 2 == 1 && numbers[i+1] % 2 == 1)
                    {
                        maxDifference = Math.Abs(numbers[i] - numbers[i+1]);
                    }
                }
                else
                {
                    if (numbers[i] % 2 == 1 && numbers[i+1] % 2 == 1 && Math.Abs(numbers[i] - numbers[i+1]) > maxDifference)
                    {
                        maxDifference = Math.Abs(numbers[i] - numbers[i+1]);
                    }
                }
            }

            return maxDifference;
        }

        static void ProcessNumUni(string f, string fout)
        {
            StreamReader sr = new StreamReader(f);
            HashSet<int> uniqueNums = new HashSet<int>();
            while (true)
            {
                string s = sr.ReadLine();
                if (s == null)
                {
                    break;
                }

                uniqueNums.Add(int.Parse(s));
            }

            StreamWriter sw = new StreamWriter(fout);
            foreach (int num in uniqueNums)
            {
                sw.WriteLine(num);
            }
        }

        // 2nd part funcs
        static void ProcessGenVec(string f, int a, int b, int n)
        {
            StreamWriter sw = new StreamWriter(f);
            Random random = new Random();
            sw.WriteLine("x,y");

            for (int i = 0; i < n; i++)
            {
                int x = random.Next(a, b);
                int y = random.Next(a, b);
                sw.WriteLine($"{x},{y}");
            }
        }

        static int ProcessVec(string f)
        {
            StreamReader sr = new StreamReader(f);
            List<Vector> vectors = new List<Vector>();
            while (true)
            {
                string s = sr.ReadLine();
                if (s == null)
                {
                    break;
                }
                if (s == "x,y")
                {
                    continue;
                }

                string[] coordinates = s.Split(',');
                vectors.Add(new Vector() {
                    x = int.Parse(coordinates[0]),
                    y = int.Parse(coordinates[1]),
                });
            }

            int count = 0;
            foreach (Vector vector in vectors)
            {
                if (vector.x > 0 && vector.y > 0)
                {
                    count++;
                }
            }

            return count;
        }
    }
}
