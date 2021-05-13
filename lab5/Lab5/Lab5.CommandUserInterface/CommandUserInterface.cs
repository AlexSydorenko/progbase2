using System;
using System.IO;

namespace lab5
{
    public class CommandUserInterface
    {
        public static void ProcessUsersCommands()
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("COMMAND LIST:");
            Console.ResetColor();
            
            Console.WriteLine("load {filename} - десеріалізувати курси XML із заданого файлу у об’єкти в процесі.");
            Console.WriteLine("print {pageNum} - вивести загальну кількість сторінок і дані сторінки (за номером)" +
                "десеріалізованих даних з об’єктів у консоль. 1 сторінка - 10 курсів.");
            Console.WriteLine("save {filename} - серіалізувати всі курси у заданий XML файл (з відступами).");
            Console.WriteLine("export {N} {filename} - зберегти у XML перші N курсів з найвищою кількістю зарахованих.");
            Console.WriteLine("prefixes - список назв всіх унікальних предметів");
            Console.WriteLine("titles {bldg} - список назв (title) всіх курсів, що проводяться у заданому будинку (за назвою, place.bldg).");
            Console.WriteLine("instructors - список всіх унікальних викладачів (instructor).");
            Console.WriteLine("image {filename} - створити і зберегти зображення з графіком у файл.");
            Console.WriteLine("exit - закрити програму.");
            Console.WriteLine();

            Courses courses = new Courses();

            string command = "";
            while (command != "exit")
            {
                Console.Write("Введіть одну із запропонованих команд: ");
                command = Console.ReadLine();

                if (command.StartsWith("load"))
                {
                    ProcessLoad(command, ref courses);
                }
                else if (command.StartsWith("print"))
                {
                    ProcessPrint(command, courses);
                }
                else if (command.StartsWith("save"))
                {
                    ProcessSave(command, courses);
                }
                else if (command.StartsWith("export"))
                {
                    ProcessExport(command, courses);
                }
                else if (command == "prefixes")
                {
                    ProcessPrefixes(courses);
                }
                else if (command.StartsWith("titles"))
                {
                    ProcessTitles(command, courses);
                }
                else if (command == "instructors")
                {
                    ProcessInstructors(courses);
                }
                else if (command.StartsWith("image"))
                {
                    ProcessImage(command, courses);
                }
                else if (command == "exit")
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("Bye!");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"Неправильна команда: `{command}`! Сапробуйте ще раз!");
                    Console.ResetColor();
                }
            }
        }

        static void ProcessLoad(string command, ref Courses courses)
        {
            string[] subcommands = command.Split(" ");
            if (subcommands[0] != "load")
            {
                Console.WriteLine($"Неправильна команда: `{subcommands[0]}`! Сапробуйте ще раз!");
                return;
            }
            if (subcommands.Length != 2)
            {
                Console.WriteLine("Команда `load` має відповідати формату `load {fileName}`!");
                return;
            }
            if (!File.Exists(subcommands[1]))
            {
                Console.WriteLine($"Такого файлу не існує: `{subcommands[1]}`! Перевірте правильність шляху!");
                return;
            }
            
            XMLSerializer serializer = new XMLSerializer(subcommands[1]);
            try
            {
                courses = serializer.Deserialize();
                Console.WriteLine("Усі курси успішно десеріалізовані!");
            }
            catch
            {
                Console.WriteLine("Файл містить помилки або дані не відповідають формату курсів!");
            }
        }

        static void ProcessPrint(string command, Courses courses)
        {
            string[] subcommands = command.Split(" ");
            if (subcommands[0] != "print")
            {
                Console.WriteLine($"Неправильна команда: `{subcommands[0]}`! Сапробуйте ще раз!");
                return;
            }
            if (subcommands.Length != 2)
            {
                Console.WriteLine("Команда `print` має відповідати формату `print {pageNum}`!");
                return;
            }
            if (courses.listCourses == null)
            {
                Console.WriteLine("Спочатку десеріалізуйте курси!");
                return;
            }
            DataProcessor dp = new DataProcessor(courses.listCourses);
            int totalPages = dp.GetTotalPages();
            Console.WriteLine("Загальна кількість сторінок: " + totalPages);
            int pageNum = 0;
            if (!int.TryParse(subcommands[1], out pageNum) || pageNum < 1 || pageNum > totalPages)
            {
                Console.WriteLine($"Номер сторінки - ціле число від 1 до {totalPages}! Спробуйте ще раз!");
                return;
            }

            Console.WriteLine($"Сторінка {pageNum}:");
            foreach (Course course in dp.GetPage(pageNum))
            {
                Console.WriteLine(course.ToString());
            }
        }

        static void ProcessSave(string command, Courses courses)
        {
            string[] subcommands = command.Split(" ");
            if (subcommands[0] != "save")
            {
                Console.WriteLine($"Неправильна команда: `{subcommands[0]}`! Сапробуйте ще раз!");
                return;
            }
            if (subcommands.Length != 2)
            {
                Console.WriteLine("Команда `save` має відповідати формату `save {fileName}`!");
                return;
            }
            if (courses.listCourses == null)
            {
                Console.WriteLine("Спочатку десеріалізуйте курси!");
                return;
            }
            if (!File.Exists(subcommands[1]))
            {
                File.Create(subcommands[1]).Close();
            }

            XMLSerializer serializer = new XMLSerializer(subcommands[1]);
            serializer.Serialize(courses);
            Console.WriteLine($"Усі курси успішно серіалізовані у файл `{subcommands[1]}`!");
        }

        static void ProcessExport(string command, Courses courses)
        {
            string[] subcommands = command.Split(" ");
            if (subcommands[0] != "export")
            {
                Console.WriteLine($"Неправильна команда: `{subcommands[0]}`! Сапробуйте ще раз!");
                return;
            }
            if (subcommands.Length != 3)
            {
                Console.WriteLine("Команда `export` має відповідати формату `export {N} {fileName}`!");
                return;
            }
            if (courses.listCourses == null)
            {
                Console.WriteLine("Спочатку десеріалізуйте курси!");
                return;
            }
            int numOfCourses = 0;
            if (!int.TryParse(subcommands[1], out numOfCourses) || numOfCourses < 1 || numOfCourses > courses.listCourses.Count)
            {
                Console.WriteLine($"Кількість курсів - ціле число від 1 до `{courses.listCourses.Count}`!");
                return;
            }
            if (!File.Exists(subcommands[2]))
            {
                File.Create(subcommands[2]).Close();
            }

            XMLSerializer serializer = new XMLSerializer(subcommands[2]);
            DataProcessor dp = new DataProcessor(courses.listCourses);
            Courses coursesToExport = new Courses(){
                listCourses = courses.listCourses
            };
            coursesToExport.listCourses = dp.SortByEnrolled();
            coursesToExport.listCourses = coursesToExport.listCourses.GetRange(0, numOfCourses - 1);
            serializer.Serialize(coursesToExport);

            Console.WriteLine($"Перші `{numOfCourses}` курсів з найвищою кількістю зарахованих успішно експортовані у файл `{subcommands[2]}`!");
        }

        static void ProcessPrefixes(Courses courses)
        {
            if (courses.listCourses == null)
            {
                Console.WriteLine("Спочатку десеріалізуйте курси!");
                return;
            }

            DataProcessor dp = new DataProcessor(courses.listCourses);

            Console.WriteLine("Оберіть, як хочете отримати інформацію:");
            Console.WriteLine("f - зберегти у файл");
            Console.WriteLine("c - вивести в консоль");
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            while (keyInfo.Key != ConsoleKey.F && keyInfo.Key != ConsoleKey.C)
            {
                keyInfo = Console.ReadKey(true);
            }

            if (keyInfo.Key == ConsoleKey.F)
            {
                Console.Write("Введіть назву файлу: ");
                string fileName = Console.ReadLine();
                if (fileName == "")
                {
                    Console.WriteLine("Назва файлу не має бути порожньою!");
                    return;
                }
                StreamWriter writer = new StreamWriter(fileName);
                foreach (string prefix in dp.GetUniquePrefixes())
                {
                    writer.WriteLine(prefix);
                }
                writer.Close();
                Console.WriteLine($"Cписок назв всіх унікальних предметів записано у файл `{fileName}`!");
            }
            else
            {
                foreach (string prefix in dp.GetUniquePrefixes())
                {
                    Console.WriteLine(prefix);
                }
                Console.WriteLine();
            }
        }

        static void ProcessTitles(string command, Courses courses)
        {
            string[] subcommands = command.Split(" ");
            if (subcommands[0] != "titles")
            {
                Console.WriteLine($"Неправильна команда: `{subcommands[0]}`! Сапробуйте ще раз!");
                return;
            }
            if (subcommands.Length != 2)
            {
                Console.WriteLine("Команда `titles` має відповідати формату `titles {bldg}`!");
                return;
            }
            if (courses.listCourses == null)
            {
                Console.WriteLine("Спочатку десеріалізуйте курси!");
                return;
            }

            DataProcessor dp = new DataProcessor(courses.listCourses);
            if (dp.GetCourseTitlesFromTheBuilding(subcommands[1]).Count == 0)
            {
                Console.WriteLine($"Помилка в назві будинка: `{subcommands[1]}`! Спробуйте ще раз!");
                return;
            }

            Console.WriteLine("Оберіть, як хочете отримати інформацію:");
            Console.WriteLine("f - зберегти у файл");
            Console.WriteLine("c - вивести в консоль");
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            while (keyInfo.Key != ConsoleKey.F && keyInfo.Key != ConsoleKey.C)
            {
                keyInfo = Console.ReadKey(true);
            }

            if (keyInfo.Key == ConsoleKey.F)
            {
                Console.Write("Введіть назву файлу: ");
                string fileName = Console.ReadLine();
                if (fileName == "")
                {
                    Console.WriteLine("Назва файлу не має бути порожньою!");
                    return;
                }
                StreamWriter writer = new StreamWriter(fileName);
                foreach (string title in dp.GetCourseTitlesFromTheBuilding(subcommands[1]))
                {
                    writer.WriteLine(title);
                }
                writer.Close();
                Console.WriteLine($"Cписок назв всіх курсів, що проводяться в будинку `{subcommands[1]}` записано у файл `{fileName}`!");
            }
            else
            {
                foreach (string title in dp.GetCourseTitlesFromTheBuilding(subcommands[1]))
                {
                    Console.WriteLine(title);
                }
                Console.WriteLine();
            }
        }

        static void ProcessInstructors(Courses courses)
        {
            if (courses.listCourses == null)
            {
                Console.WriteLine("Спочатку десеріалізуйте курси!");
                return;
            }

            DataProcessor dp = new DataProcessor(courses.listCourses);

            Console.WriteLine("Оберіть, як хочете отримати інформацію:");
            Console.WriteLine("f - зберегти у файл");
            Console.WriteLine("c - вивести в консоль");
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            while (keyInfo.Key != ConsoleKey.F && keyInfo.Key != ConsoleKey.C)
            {
                keyInfo = Console.ReadKey(true);
            }

            if (keyInfo.Key == ConsoleKey.F)
            {
                Console.Write("Введіть назву файлу: ");
                string fileName = Console.ReadLine();
                if (fileName == "")
                {
                    Console.WriteLine("Назва файлу не має бути порожньою!");
                    return;
                }
                StreamWriter writer = new StreamWriter(fileName);
                foreach (string instructor in dp.GetInstructors())
                {
                    writer.WriteLine(instructor);
                }
                writer.Close();
                Console.WriteLine($"Cписок усіх унікальних викладачів записано у файл `{fileName}`!");
            }
            else
            {
                foreach (string instructor in dp.GetInstructors())
                {
                    Console.WriteLine(instructor);
                }
                Console.WriteLine();
            }
        }

        static void ProcessImage(string command, Courses courses)
        {
            string[] subcommands = command.Split(" ");
            if (subcommands[0] != "image")
            {
                Console.WriteLine($"Неправильна команда: `{subcommands[0]}`! Сапробуйте ще раз!");
                return;
            }
            if (subcommands.Length != 2)
            {
                Console.WriteLine("Команда `image` має відповідати формату `image {fileName}`!");
                return;
            }
            if (courses.listCourses == null)
            {
                Console.WriteLine("Спочатку десеріалізуйте курси!");
                return;
            }
            if (!subcommands[1].EndsWith(".png"))
            {
                Console.WriteLine("Файл, у який хочете зберегти графік, повинен мати розширення `.png`!");
                return;
            }
            Console.WriteLine("Введіть діапазон курсів, дані яких будуть враховуватися при генерації графіку " +
                "(усі курси відсортовано за кількістю зарахованих за спаданням):");
            int startCourse = 0;
            Console.Write("Номер початкового курсу: ");
            if (!int.TryParse(Console.ReadLine(), out startCourse) || startCourse < 1 || startCourse > courses.listCourses.Count)
            {
                Console.WriteLine($"Номер початкового курсу - ціле число від 1 до {courses.listCourses.Count}!");
                return;
            }
            int endCourse = 0;
            Console.Write("Номер кінцевого курсу: ");
            if (!int.TryParse(Console.ReadLine(), out endCourse) || endCourse < startCourse || endCourse > courses.listCourses.Count)
            {
                Console.WriteLine($"Номер кінцевого курсу має бути не меншим за номер початкового та не більшим за {courses.listCourses.Count}!");
                return;
            }
            if (!File.Exists(subcommands[1]))
            {
                File.Create(subcommands[1]).Close();
            }

            GraphGenerator gg = new GraphGenerator(subcommands[1]);
            DataProcessor dp = new DataProcessor(courses.listCourses);
            Courses coursesForImage = new Courses(){
                listCourses = dp.SortByEnrolled().GetRange(startCourse - 1, endCourse - startCourse + 1)
            };
            gg.GenerateGraph(coursesForImage);
            Console.WriteLine($"Графік збережено у файл `{subcommands[1]}!`");
        }
    }
}
