using System;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace lab1__semester_2_
{
    class Course
    {
        public int id;
        public string name;
        public string group;
        public int semester;

        public Course()
        {
            this.id = 0;
            this.name = "";
            this.group = "";
            this.semester = 0;
        }
        public Course(int id, string name, string group, int semester)
        {
            this.id = id;
            this.name = name;
            this.group = group;
            this.semester = semester;
        }
        public override string ToString()
        {
            return string.Format($"ID: {this.id}; Name: {this.name}; Group: {this.group}; Semester: {this.semester}");
        }
    }

    class ListCourse
    {
        private Course[] _items;
        private int _size;

        public ListCourse()
        {
            _items = new Course[16];
            _size = 0;
        }

        public void AddItem(Course c)
        {
            _items[_size] = c;
            _size++;
            if (_items.Length == _size)
            {
                Array.Resize(ref _items, _items.Length * 2);
            }
        }

        public void DeleteItem(Course c, int itemPosition)
        {
            for (int i = itemPosition; i < _items.Length; i++)
            {
                if (i == _items.Length-1)
                {
                    _items[i] = null;
                    _size--;
                    return;
                }
                _items[i] = _items[i+1];
            }
        }

        public Course GetCourse(int index)
        {
            return this._items[index];
        }

        public int GetSize()
        {
            return _size;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // 1 part
            if((args.Length != 2) || (args.Length == 0))
            {
                throw new Exception($"ERROR! Should be 2 arguments but have {args.Length}");
            }
            if(!args[0].StartsWith("./"))
            {
                throw new Exception("ERROR! Incorrect file path! First argument must be started with `./`");
            }
            int numOfLines = 0;
            try
            {
                numOfLines = int.Parse(args[1]);
            }
            catch
            {
                throw new Exception("ERROR! Only integers in the 2nd argument!");
            }
            if(numOfLines < 0)
            {
                throw new Exception("ERROR! Only positive numbers!");
            }

            // int[] randomNums = GetArrayOfRandomUniqueNumbers(numOfLines, 1, 11);
            // StringBuilder sb = new StringBuilder("id,name,group,semester\r");
            // for(int i = 0; i < numOfLines; i++)
            // {
            //     sb.Append(AddCourse(GetRandomCourse(randomNums, i)));
            // }
            // File.WriteAllText($"{args[0]}", sb.ToString());

            GenerateItemsInFile(numOfLines, args[0], 1, 100);

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Перша частина успішно виконана! Сутності згенеровано та збережено у файл `{args[0]}`.");
            Console.ResetColor();
            Console.WriteLine();

            // 2 part
            ConsoleKeyInfo keyInfo;
            do
            {
                Console.Write("Натисніть [Enter], щоб виконати другу частину: ");
                keyInfo = Console.ReadKey();
                if (keyInfo.Key != ConsoleKey.Enter)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine();
                    Console.WriteLine("Помилка! Натисніть [Enter]!");
                    Console.ResetColor();
                }
            }
            while (keyInfo.Key != ConsoleKey.Enter);

            string inputFile1 = "./file1.csv";
            if (!IsExistingFile(inputFile1))
            {
                throw new FileNotFoundException($"ERROR! No such file in current directory: `{inputFile1}`");
            }
            // Генеруємо дані у файл 1
            GenerateItemsInFile(10, inputFile1, 1, 11);

            string inputFile2 = "./file2.csv";
            if (!IsExistingFile(inputFile2))
            {
                throw new FileNotFoundException($"ERROR! No such file in current directory: `{inputFile2}`");
            }
            // Генеруємо дані у файл 2
            GenerateItemsInFile(100000, inputFile2, 1, 150000);

            string outputFile = "./file3.csv";
            if (!IsExistingFile(outputFile))
            {
                throw new FileNotFoundException($"ERROR! No such file in current directory: `{outputFile}`");
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();
            ListCourse lc1 = ReadAllCourses(inputFile1);
            Console.WriteLine($"Кількість елементів списку #1: {lc1.GetSize()}");
            Console.WriteLine("Перші 10 елементів списку:");
            PrintFirst10Items(lc1);
            Console.WriteLine();

            ListCourse lc2 = ReadAllCourses(inputFile2);
            Console.WriteLine($"Кількість елементів списку #2: {lc2.GetSize()}");
            Console.WriteLine("Перші 10 елементів списку:");
            PrintFirst10Items(lc2);
            Console.WriteLine();

            ListCourse allCourses = ConnectLists(lc1, lc2);
            ListCourse listWithUniqueIDs = DeleteWithTheSameIDs(allCourses);
            DeleteIfLessThenAverageSemester(listWithUniqueIDs);
            WriteAllCourses(outputFile, listWithUniqueIDs);

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Друга частина виконана успішно! Дані з обох вхідних файлів оброблено та записано у файл `{outputFile}`");
            Console.ResetColor();
            Console.WriteLine();
            sw.Stop();
            Console.WriteLine($"Elapsed = {sw.Elapsed}");
        }

        static bool IsExistingFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return false;
            }
            return true;
        }

        static ListCourse ReadAllCourses(string filePath)
        {
            if (IsIncorrectInput(filePath))
            {
                throw new Exception("ERROR! Input file has mistake!");
            }
            ListCourse lc = new ListCourse();
            StreamReader sr = new StreamReader(filePath);
            string s = "";
            while (true)
            {
                s = sr.ReadLine();
                if (s == null)
                {
                    break;
                }
                if (s == "id,name,group,semester")
                {
                    continue;
                }
                Course c = new Course();
                string courseItem = "";
                int counter = -1;
                for (int i = 0; i < s.Length; i++)
                {
                    if(s[i] == ',')
                    {
                        counter++;
                        if(courseItem == "")
                        {
                            throw new Exception("ERROR! Input file has empty fields!");
                        }
                        if(counter == 0)
                        {
                            c.id = int.Parse(courseItem);
                            courseItem = "";
                            continue;
                        }
                        if(counter == 1)
                        {
                            c.name = courseItem;
                            courseItem = "";
                            continue;
                        }
                        if(counter == 2)
                        {
                            c.group = courseItem;
                            courseItem = "";
                            continue;
                        }
                    }
                    courseItem += s[i];
                    if(i == s.Length-1)
                    {
                        c.semester = int.Parse(courseItem);
                        continue;
                    }
                }
                lc.AddItem(c);
            }
            sr.Close();
            return lc;
        }

        static void WriteAllCourses(string filePath, ListCourse lc)
        {
            if (IsIncorrectInput(filePath))
            {
                throw new Exception("ERROR! Input file has mistake!");
            }
            StreamWriter sw = new StreamWriter(filePath);
            StringReader sr = new StringReader(ListCourseToCsv(lc).ToString());
            string s = "";
            while (true)
            {
                s = sr.ReadLine();
                if (s == null)
                {
                    break;
                }
                sw.WriteLine(s);
            }
            sw.Close();
            sr.Close();
        }

        static bool IsIncorrectInput(string filePath)
        {
            int counter = 0;
            StreamReader sr = new StreamReader(filePath);
            string s = "";
            while (true)
            {
                s = sr.ReadLine();
                if (s == null)
                {
                    break;
                }
                foreach (char item in s)
                {
                    if (item == ',')
                    {
                        counter++;
                    }
                }
                if (counter != 3)
                {
                    return true;
                }
                counter = 0;
            }
            return false;
        }

        static ListCourse ConnectLists(ListCourse lc1, ListCourse lc2)
        {
            for (int i = 0; i < lc2.GetSize(); i++)
            {
                if (lc2.GetCourse(i) == null)
                {
                    break;
                }
                lc1.AddItem(lc2.GetCourse(i));
            }
            return lc1;
        }

        static ListCourse DeleteWithTheSameIDs(ListCourse lc)
        {
            ListCourse listWithUniqueIDs = new ListCourse();
            listWithUniqueIDs.AddItem(lc.GetCourse(0));
            for (int i = 0; i < lc.GetSize(); i++)
            {
                if (lc.GetCourse(i) == null)
                {
                    break;
                }

                bool check = true;
                for (int j = 0; j < listWithUniqueIDs.GetSize(); j++)
                {
                    if (lc.GetCourse(i).id == listWithUniqueIDs.GetCourse(j).id)
                    {
                        check = false;
                        break;
                    }
                }
                if (check)
                {
                    listWithUniqueIDs.AddItem(lc.GetCourse(i));
                }
            }
            return listWithUniqueIDs;
        }

        static void PrintFirst10Items(ListCourse lc)
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(lc.GetCourse(i));
            }
        }

        static double GetAverageSemester(ListCourse lc)
        {
            int sum = 0;
            for (int i = 0; i < lc.GetSize(); i++)
            {
                sum += lc.GetCourse(i).semester;
            }
            return (double)sum / lc.GetSize();
        }

        static void GenerateItemsInFile(int numOfItems, string filePath, int low, int high)
        {
            if (numOfItems > (high - low))
            {
                throw new Exception($"ERROR! You want generate too much items! Maximum `{high-low}`");
            }
            int[] randomNums = GetArrayOfRandomUniqueNumbers(numOfItems, low, high);
            StringBuilder sb = new StringBuilder("id,name,group,semester\r");
            for(int i = 0; i < numOfItems; i++)
            {
                sb.Append(AddCourse(GetRandomCourse(randomNums, i)));
            }
            File.WriteAllText($"{filePath}", sb.ToString());
        }

        static void DeleteIfLessThenAverageSemester(ListCourse lc)
        {
            double avg = GetAverageSemester(lc);
            for (int i = 0; i < lc.GetSize(); i++)
            {
                if (lc.GetCourse(i).semester < avg)
                {
                    lc.DeleteItem(lc.GetCourse(i), i);
                    i--;
                }
            }
        }

        static Course GetRandomCourse(int[] randomNums, int orderOfNum)
        {
            string[] nameOfCourse = new string[]{"Основи програмування", "Мова програмування C#", "Веб-програмування",
                "Веб-дизайн", "Розробка додатків під IOS"};
            string[] group = new string[]{"KP-01", "KP-02", "KP-03", "KP-91", "KP-92", "KP-93"};
            // int[] randomNums = GetArrayOfRandomUniqueNumbers();
            Random random = new Random();
            Course c = new Course(){id = randomNums[orderOfNum], name = nameOfCourse[random.Next(0, nameOfCourse.Length-1)],
                group = group[random.Next(0, group.Length-1)], semester = random.Next(1, 6)};
            return c;
        }

        static int[] GetArrayOfRandomUniqueNumbers(int arrSize, int low, int high)
        {
            int[] arr = new int[arrSize];
            Random random = new Random();
            for (int i = 0; i < arr.Length; i++)
            {
                int randomNum = random.Next(low, high);
                bool check = true;
                for (int j = 0; j <= i; j++)
                {
                    if (arr[j] == randomNum)
                    {
                        check = false;
                        break;
                    }
                }
                if (check)
                {
                    arr[i] = randomNum;
                }
                else
                {
                    i--;
                    continue;
                }
            }
            return arr;
        }

        static string AddCourse(Course course)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(course.id + "," + course.name + "," + course.group + "," + course.semester + "\r");
            return sb.ToString();
        }

        static string CourseToCsv(Course c)
        {
            return $"{c.id}, {c.name}, {c.group}, {c.semester}\r";
        }

        static StringBuilder ListCourseToCsv(ListCourse lc)
        {
            StringBuilder sb = new StringBuilder("id,name,group,semester\r");
            for (int i = 0; i < lc.GetSize(); i++)
            {
                sb.Append(CourseToCsv(lc.GetCourse(i)));
            }
            return sb;
        }

        // static Exception CatchError(string[] args, out int numOfLines)
        // {
        //     numOfLines = 0;
        //     if(args.Length != 2)
        //     {
        //         return new Exception($"ERROR! Should be 2 arguments but have {args.Length}");
        //     }
        //     try
        //     {
        //         numOfLines = int.Parse(args[1]);
        //     }
        //     catch
        //     {
        //         Console.WriteLine("ERROR! Only numbers!");
        //     }
        //     return ;
        // }
    }
}

