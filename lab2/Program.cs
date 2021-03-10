using System;
using Microsoft.Data.Sqlite;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace lab2
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

        public int GetCount()
        {
            return _size;
        }
    }

    class CourseRepository 
    {
        private SqliteConnection connection;

        public CourseRepository(SqliteConnection connection)
        {
            this.connection = connection;
        }
    
        public Course GetById(int id)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM courses WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);

            Course course = new Course();
            SqliteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                course.id = int.Parse(reader.GetString(0));
                course.name = reader.GetString(1);
                course.group = reader.GetString(2);
                course.semester = int.Parse(reader.GetString(3));
            }
            else
            {
                course = null;
            }
            reader.Close();

            return course;
        }

        public int DeleteById(int id)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM courses WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);

            int nChanged = command.ExecuteNonQuery();
            if (nChanged == 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public long Insert(Course course)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO courses (name, 'group', semester)
                VALUES ($name, $group, $semester);

                SELECT last_insert_rowid();";

            command.Parameters.AddWithValue("$name", course.name);
            command.Parameters.AddWithValue("$group", course.group);
            command.Parameters.AddWithValue("$semester", course.semester);

            long newID = (long)command.ExecuteScalar();
            if (newID == 0)
            {
                return 0;
            }
            else
            {
                return newID;
            }
        }

        private long GetCount()
        {
            connection.Open();
        
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT COUNT(*) FROM courses";
            
            long count = (long)command.ExecuteScalar();
            return count;
        }

        public int GetTotalPages()
        {
            const int pageSize = 10;
            return (int)Math.Ceiling(this.GetCount() / (double)pageSize);
        }

        public ListCourse GetPage(int pageNumber)
        {
            if (pageNumber < 1)
            {
                throw new Exception("Page number must be greater than 0!");
            }
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM courses LIMIT $pageSize OFFSET $pageSize * ($pageNumber - 1)";
            command.Parameters.AddWithValue("$pageSize", 10);
            command.Parameters.AddWithValue("$pageNumber", pageNumber);

            SqliteDataReader reader = command.ExecuteReader();
            ListCourse courses = new ListCourse();

            while (reader.Read())
            {
                Course course = new Course();
                course.id = int.Parse(reader.GetString(0));
                course.name = reader.GetString(1);
                course.group = reader.GetString(2);
                course.semester = int.Parse(reader.GetString(3));

                courses.AddItem(course);
            }
            reader.Close();
            return courses;
        }
    
        public ListCourse GetExport(string valueX)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM courses WHERE name LIKE $valueX";
            command.Parameters.AddWithValue("$valueX", valueX);

            SqliteDataReader reader = command.ExecuteReader();
            ListCourse courses = new ListCourse();

            while (reader.Read())
            {
                Course course = new Course();
                course.id = int.Parse(reader.GetString(0));
                course.name = reader.GetString(1);
                course.group = reader.GetString(2);
                course.semester = int.Parse(reader.GetString(3));

                courses.AddItem(course);
            }
            reader.Close();
            return courses;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            string dbFilePath = "./courses";
            SqliteConnection connection = new SqliteConnection($"Data Source = {dbFilePath}");
            CourseRepository courseRepo = new CourseRepository(connection);

            Course c = new Course(){name = "Modular programming", group = "KP-92", semester = 4};

            connection.Open();
            
            Console.WriteLine("getById {idInteger} - отримання курсу за ідентифікатором");
            Console.WriteLine("deleteById {idInteger} - видалення курсу за ідентифікатором");
            Console.WriteLine("insert {courseName},{groupNumber},{semesterNumber} - додавання нового курсу");
            Console.WriteLine("getTotalPages - отримання кількості сторінок з курсами");
            Console.WriteLine("getPage {pageNumberInteger} - отримання сторінки з курсами за номером сторінки");
            Console.WriteLine("export {valueString} - експорт обраних курсів у CSV файл");
            Console.WriteLine("exit - вихід з програми");
            Console.WriteLine();

            while (true)
            {
                Console.Write("Введіть одну із запропонованих вище команд: ");
                string command = Console.ReadLine();

                if (command.StartsWith("getById"))
                {
                    ProcessGetById(command, courseRepo);
                }
                else if (command.StartsWith("deleteById"))
                {
                    ProcessDeleteById(command, courseRepo);
                }
                else if (command.StartsWith("insert"))
                {
                    ProcessInsert(command, courseRepo);
                }
                else if (command == "getTotalPages")
                {
                    ProcessGetTotalPages(courseRepo);
                }
                else if (command.StartsWith("getPage"))
                {
                    ProcessGetPage(command, courseRepo);
                }
                else if (command.StartsWith("export"))
                {
                    ProcessExport(command, courseRepo);
                }
                else if (command == "exit")
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("Bye!");
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Такої команди не існує! Перевірте правильність написання!");
                    Console.ResetColor();
                    Console.WriteLine();
                }
            }

            connection.Close();
        }

        static void ProcessGetById(string command, CourseRepository courseRepo)
        {
            string[] subcommands = command.Split(" ");
            if (subcommands[0] != "getById")
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"Команди `{subcommands[0]}` не існує! Спробуйте `getById + ціле число`!");
                Console.ResetColor();
                Console.WriteLine();
                return;
            }

            if (subcommands.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Помилка! Дана команда має відповідати формату `getById + ціле число`!");
                Console.ResetColor();
                Console.WriteLine();
                return;
            }

            int id;
            bool parsedId = int.TryParse(subcommands[1], out id);
            if (!parsedId)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Помилка! Дана команда має відповідати формату `getById + ціле число`!");
                Console.ResetColor();
                Console.WriteLine();
                return;
            }

            Course course = courseRepo.GetById(id);
            if (course == null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"Курсу з ідентифікатором `{id}` у базі даних не існує! Спробуйте ввести інший!");
                Console.ResetColor();
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine(course);
                Console.WriteLine();
            }
        }

        static void ProcessDeleteById(string command, CourseRepository courseRepo)
        {
            string[] subcommands = command.Split(" ");
            if (subcommands[0] != "deleteById")
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"Команди `{subcommands[0]}` не існує! Спробуйте `deleteById + ціле число`!");
                Console.ResetColor();
                Console.WriteLine();
                return;
            }

            if (subcommands.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Помилка! Дана команда має відповідати формату `deleteById + ціле число`");
                Console.ResetColor();
                Console.WriteLine();
                return;
            }

            int id;
            bool parsedId = int.TryParse(subcommands[1], out id);
            if (!parsedId)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Помилка! Дана команда має відповідати формату `deleteById + ціле число`");
                Console.ResetColor();
                Console.WriteLine();
                return;
            }

            if (courseRepo.DeleteById(id) == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"Курсу з ідентифікатором `{id}` у базі даних не існує! Спробуйте ввести інший!");
                Console.ResetColor();
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine($"Курс з ідентифікатором `{id}` був успішно видалений з бази даних!");
                Console.WriteLine();
            }
        }

        static void ProcessInsert(string command, CourseRepository courseRepo)
        {
            string[] subcommands = command.Split(" ");
            if (subcommands[0] != "insert")
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"Команди `{subcommands[0]}` не існує! Спробуйте `insert + назва курсу,номер групи,номер семестру`!");
                Console.ResetColor();
                Console.WriteLine();
                return;
            }

            command = DeleteSubstr(command, "insert ");
            string[] courseData = command.Split(",");
            if (courseData.Length != 3)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Помилка! Дана команда має відповідати формату `insert + назва курсу,номер групи,номер семестру`!");
                Console.ResetColor();
                Console.WriteLine();
                return;
            }
            if (courseData[0] == "" || courseData[1] == "")
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Помилка! Поля \"назва курсу\" або \"номер групи\" не мають бути порожніми!");
                Console.ResetColor();
                Console.WriteLine();
                return;
            }

            Course course = new Course();
            course.name = courseData[0];
            course.group = courseData[1];

            bool parsedSemester;
            parsedSemester = int.TryParse(courseData[2], out course.semester);
            if (!parsedSemester)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Помилка! Номеру семестра має відповідати ціле число!");
                Console.ResetColor();
                Console.WriteLine();
                return;
            }

            Console.WriteLine($"Курс успішно додано в базу даних! ID доданого курсу: {courseRepo.Insert(course)}.");
            Console.WriteLine();
        }

        static void ProcessGetTotalPages(CourseRepository courseRepo)
        {
            Console.WriteLine($"Загальна кількість сторінок: {courseRepo.GetTotalPages()}");
            Console.WriteLine();
        }

        static void ProcessGetPage(string command, CourseRepository courseRepo)
        {
            string[] subcommands = command.Split(" ");
            if (subcommands[0] != "getPage")
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"Команди `{subcommands[0]}` не існує! Спробуйте `getPage + ціле число`!");
                Console.ResetColor();
                Console.WriteLine();
                return;
            }

            if (subcommands.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Помилка! Дана команда має відповідати формату `getPage + ціле число`");
                Console.ResetColor();
                Console.WriteLine();
                return;
            }

            int pageNumber;
            bool parsedNum = int.TryParse(subcommands[1], out pageNumber);
            if (!parsedNum)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Помилка! Дана команда має відповідати формату `getPage + ціле число`");
                Console.ResetColor();
                Console.WriteLine();
                return;
            }

            if (pageNumber < 1 || pageNumber > courseRepo.GetTotalPages())
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"Помилка! Номер сторінки має бути в межах від 1 до {courseRepo.GetTotalPages()} включно!");
                Console.ResetColor();
                Console.WriteLine();
                return;
            }

            ListCourse courses = courseRepo.GetPage(pageNumber);
            Console.WriteLine($"Сторінкка # {pageNumber}:");
            for (int i = 0; i < courses.GetCount(); i++)
            {
                Console.WriteLine(courses.GetCourse(i));
            }
            Console.WriteLine();
        }

        static void ProcessExport(string command, CourseRepository courseRepo)
        {
            string[] subcommands = command.Split(" ");
            if (subcommands[0] != "export")
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"Команди `{subcommands[0]}` не існує! Спробуйте `export + назва курсу`!");
                Console.ResetColor();
                Console.WriteLine();
                return;
            }

            string courseName = DeleteSubstr(command, "export ");
            ListCourse exportedCourses = courseRepo.GetExport(courseName);
            if (exportedCourses.GetCount() == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"Помилка! Курсів з назвою `{courseName}` у базі даних не знайдено!");
                Console.ResetColor();
                Console.WriteLine();
            }
            else
            {
                string filePath = "./export.csv";
                Console.WriteLine($"Курси з назвою `{courseName}` успішно експортовані у файл `{filePath}`!\n" +
                    $"Кількість експортованих курсів: {WriteCoursesToCsv($"{filePath}", exportedCourses)}.");
                Console.WriteLine();
            }
        }

        static int WriteCoursesToCsv(string filePath, ListCourse lc)
        {
            StreamWriter sw = new StreamWriter(filePath);
            StringReader sr = new StringReader(ListCourseToCsv(lc).ToString());
            string s = "";
            int numOfCourses = 0;
            while (true)
            {
                s = sr.ReadLine();
                if (s == null)
                {
                    break;
                }
                numOfCourses++;
                sw.WriteLine(s);
            }
            sw.Close();
            sr.Close();

            return numOfCourses-1;
        }

        static StringBuilder ListCourseToCsv(ListCourse lc)
        {
            StringBuilder sb = new StringBuilder("id,name,group,semester\r");
            for (int i = 0; i < lc.GetCount(); i++)
            {
                sb.Append(CourseToCsv(lc.GetCourse(i)));
            }
            return sb;
        }

        static string CourseToCsv(Course c)
        {
            return $"{c.id}, {c.name}, {c.group}, {c.semester}\r";
        }

        static string DeleteSubstr(string str, string substr)
        {
            int n = str.IndexOf(substr);
            str = str.Remove(n, substr.Length);
            return str;
        }
    }
}
