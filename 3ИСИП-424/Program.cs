using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ИСИП_424
{
        public abstract class Person
        {
            private static int idCounter = 1; // Для автоинкремента ID

            public int Id { get; private set; }
            public string Name { get; private set; }
            public int Age { get; private set; }
            public string Email { get; private set; }

            protected Person(string name, int age, string email)
            {
                // Валидация данных (Инкапсуляция)
                if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Имя не может быть пустым.");
                if (age < 16 || age > 100) throw new ArgumentException("Возраст должен быть в диапазоне от 16 до 100.");
                if (string.IsNullOrWhiteSpace(email) || !email.Contains("@")) throw new ArgumentException("Некорректный email.");

                Id = idCounter++;
                Name = name;
                Age = age;
                Email = email;
            }

            // Абстрактный метод для полиморфизма
            public abstract string GetInfo();
        }

        // Класс Студент
        public class Student : Person
        {
            public List<Course> EnrolledCourses { get; private set; } = new List<Course>();

            public Student(string name, int age, string email) : base(name, age, email) { }

            public void Enroll(Course course)
            {
                if (course == null) throw new ArgumentNullException(nameof(course));
                if (!EnrolledCourses.Contains(course))
                {
                    EnrolledCourses.Add(course);
                    course.AddStudent(this); // Связываем объекты двусторонне
                }
            }

            public override string GetInfo()
            {
                return $"Студент ID: {Id} {Name}, Возраст: {Age}, Email: {Email}. Курсов: {EnrolledCourses.Count}";
            }
        }

        // Класс Преподаватель
        public class Teacher : Person
        {
            public string Department { get; private set; }
            public List<Course> TeachingCourses { get; private set; } = new List<Course>();

            public Teacher(string name, int age, string email, string department) : base(name, age, email)
            {
                if (string.IsNullOrWhiteSpace(department)) throw new ArgumentException("Название кафедры не может быть пустым.");
                Department = department;
            }

            public void AssignToCourse(Course course)
            {
                if (course == null) throw new ArgumentNullException(nameof(course));
                if (!TeachingCourses.Contains(course))
                {
                    TeachingCourses.Add(course);
                    // Если у курса еще нет преподавателя, назначаем
                    if (course.Teacher == null)
                    {
                        course.SetTeacher(this);
                    }
                }
            }

            public override string GetInfo()
            {
                return $"Преподаватель ID: {Id} {Name}, Кафедра: {Department}, Email: {Email}. Ведет курсов: {TeachingCourses.Count}";
            }
        }

        // 2. КЛАСС КУРСА

        public class Course
        {
            private static int courseIdCounter = 100;

            public int Id { get; private set; }
            public string Title { get; private set; }
            public int Credits { get; private set; } //Учебная нагрузка

            // Инкапсуляция: списки доступны только для чтения извне
            public Teacher Teacher { get; private set; }
            private List<Student> students = new List<Student>();
            public IReadOnlyList<Student> Students => students;

            public Course(string title, int credits)
            {
                if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Название курса не может быть пустым.");
                if (credits <= 0) throw new ArgumentException("Количество кредитов должно быть положительным.");

                Id = courseIdCounter++;
                Title = title;
                Credits = credits;
            }

            public void SetTeacher(Teacher teacher)
            {
                if (teacher == null) throw new ArgumentNullException(nameof(teacher));
                // Если преподаватель уже назначен на этот курс, ничего не делаем
                if (Teacher == teacher) return;

                Teacher = teacher;
                teacher.AssignToCourse(this); // Обратная связь
            }

            public void AddStudent(Student student)
            {
                if (!students.Contains(student))
                {
                    students.Add(student);
                }
            }

            public string GetDetailedInfo()
            {
                string teacherName = Teacher != null ? Teacher.Name : "Не назначен";
                return $"Курс ID: {Id}  {Title}  Учебная нагрузка: {Credits}  Преподаватель: {teacherName}  Студентов: {students.Count}";
            }
        }

        // 3. УПРАВЛЕНИЕ СИСТЕМОЙ (LINQ)

        public class University
        {
            private List<Student> students = new List<Student>();
            private List<Teacher> teachers = new List<Teacher>();
            private List<Course> courses = new List<Course>();

            // Методы добавления
            public void AddStudent(Student s) => students.Add(s);
            public void AddTeacher(Teacher t) => teachers.Add(t);
            public void AddCourse(Course c) => courses.Add(c);

            // Методы получения списков (для меню)
            public IReadOnlyList<Student> GetAllStudents() => students.AsReadOnly();
            public IReadOnlyList<Teacher> GetAllTeachers() => teachers.AsReadOnly();
            public IReadOnlyList<Course> GetAllCourses() => courses.AsReadOnly();

            // Поиск по ID (LINQ)
            public Student FindStudentById(int id) => students.FirstOrDefault(s => s.Id == id);
            public Teacher FindTeacherById(int id) => teachers.FirstOrDefault(t => t.Id == id);
            public Course FindCourseById(int id) => courses.FirstOrDefault(c => c.Id == id);
        }

        // 4. ГЛАВНОЕ МЕНЮ И ВАЛИДАЦИЯ

        class Program
        {
            static University university = new University();

            static void Main(string[] args)
            {
                // Предзаполнение данными для теста
                SeedData();

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("СИСТЕМА УПРАВЛЕНИЯ УНИВЕРСИТЕТОМ");
                    Console.WriteLine("1. Добавить студента;");
                    Console.WriteLine("2. Добавить преподавателя;");
                    Console.WriteLine("3. Создать курс;");
                    Console.WriteLine("4. Записать студента на курс;");
                    Console.WriteLine("5. Назначить преподавателя на курс;");
                    Console.WriteLine("6. Просмотр всех студентов;");
                    Console.WriteLine("7. Просмотр всех преподавателей;");
                    Console.WriteLine("8. Просмотр всех курсов;");
                    Console.WriteLine("9. Детали курса (список студентов);");
                    Console.WriteLine("0. Выход.");
                    Console.Write("Выберите действие: ");

                    string choice = Console.ReadLine();

                    try
                    {
                        switch (choice)
                        {
                            case "1": AddStudentMenu(); break;
                            case "2": AddTeacherMenu(); break;
                            case "3": CreateCourseMenu(); break;
                            case "4": EnrollStudentMenu(); break;
                            case "5": AssignTeacherMenu(); break;
                            case "6": ShowAllStudents(); break;
                            case "7": ShowAllTeachers(); break;
                            case "8": ShowAllCourses(); break;
                            case "9": ShowCourseDetails(); break;
                            case "0": return;
                            default: Console.WriteLine("Неверный выбор. Нажмите любую клавишу..."); Console.ReadKey(); break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"ОШИБКА: {ex.Message}");
                        Console.WriteLine("Нажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                    }
                }
            }

            // Вспомогательные методы для ввода с валидацией

            static string GetValidString(string prompt)
            {
                while (true)
                {
                    Console.Write(prompt);
                    string input = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(input)) return input;
                    Console.WriteLine("Ошибка: Поле не может быть пустым.");
                }
            }

            static int GetValidInt(string prompt, int min, int max)
            {
                while (true)
                {
                    Console.Write(prompt);
                    if (int.TryParse(Console.ReadLine(), out int result) && result >= min && result <= max)
                    {
                        return result;
                    }
                    Console.WriteLine($"Ошибка: Введите число от {min} до {max}.");
                }
            }

            // Логика меню

            static void AddStudentMenu()
            {
                Console.WriteLine("Добавление студента");
                string name = GetValidString("Введите имя: ");
                int age = GetValidInt("Введите возраст (16-100): ", 16, 100);
                string email = GetValidString("Введите email: ");

                var student = new Student(name, age, email);
                university.AddStudent(student);
                Console.WriteLine($"Студент успешно добавлен! ID: {student.Id}");
                Pause();
            }

            static void AddTeacherMenu()
            {
                Console.WriteLine("Добавление преподавателя");
                string name = GetValidString("Введите имя: ");
                int age = GetValidInt("Введите возраст (21-100): ", 21, 100);
                string email = GetValidString("Введите email: ");
                string dept = GetValidString("Введите кафедру: ");

                var teacher = new Teacher(name, age, email, dept);
                university.AddTeacher(teacher);
                Console.WriteLine($"Преподаватель успешно добавлен! ID: {teacher.Id}");
                Pause();
            }

            static void CreateCourseMenu()
            {
                Console.WriteLine("Создание курса");
                string title = GetValidString("Введите название курса: ");
                int credits = GetValidInt("Введите количество кредитов (1-10): ", 1, 10);

                var course = new Course(title, credits);
                university.AddCourse(course);
                Console.WriteLine($"Курс успешно создан! ID: {course.Id}");
                Pause();
            }

            static void EnrollStudentMenu()
            {
                Console.WriteLine("Запись на курс");
                int studentId = GetValidInt("Введите ID студента: ", 1, int.MaxValue);
                var student = university.FindStudentById(studentId);
                if (student == null) { Console.WriteLine("Студент не найден."); Pause(); return; }

                int courseId = GetValidInt("Введите ID курса: ", 100, int.MaxValue);
                var course = university.FindCourseById(courseId);
                if (course == null) { Console.WriteLine("Курс не найден."); Pause(); return; }

                student.Enroll(course);
                Console.WriteLine($"Студент {student.Name} записан на курс {course.Title}.");
                Pause();
            }

            static void AssignTeacherMenu()
            {
                Console.WriteLine("Назначение преподавателя");
                int teacherId = GetValidInt("Введите ID преподавателя: ", 1, int.MaxValue);
                var teacher = university.FindTeacherById(teacherId);
                if (teacher == null) { Console.WriteLine("Преподаватель не найден."); Pause(); return; }

                int courseId = GetValidInt("Введите ID курса: ", 100, int.MaxValue);
                var course = university.FindCourseById(courseId);
                if (course == null) { Console.WriteLine("Курс не найден."); Pause(); return; }

                course.SetTeacher(teacher);
                Console.WriteLine($"Преподаватель {teacher.Name} назначен на курс {course.Title}.");
                Pause();
            }

            static void ShowAllStudents()
            {
                Console.WriteLine("Все студенты");
                var students = university.GetAllStudents();
                if (!students.Any()) Console.WriteLine("Список пуст.");
                foreach (var s in students)
                {
                    // Полиморфизм в действии: вызывается GetInfo() класса Student
                    Console.WriteLine(s.GetInfo());
                }
                Pause();
            }

            static void ShowAllTeachers()
            {
                Console.WriteLine("Все преподаватели");
                var teachers = university.GetAllTeachers();
                if (!teachers.Any()) Console.WriteLine("Список пуст.");
                foreach (var t in teachers)
                {
                    Console.WriteLine(t.GetInfo());
                }
                Pause();
            }

            static void ShowAllCourses()
            {
                Console.WriteLine("Все курсы");
                var courses = university.GetAllCourses();
                if (!courses.Any()) Console.WriteLine("Список пуст.");
                foreach (var c in courses)
                {
                    Console.WriteLine(c.GetDetailedInfo());
                }
                Pause();
            }

            static void ShowCourseDetails()
            {
                Console.WriteLine("Детали курса");
                int courseId = GetValidInt("Введите ID курса: ", 100, int.MaxValue);
                var course = university.FindCourseById(courseId);

                if (course == null) { Console.WriteLine("Курс не найден."); Pause(); return; }

                Console.WriteLine($"\nКурс: {course.Title}");
                Console.WriteLine($"Преподаватель: {(course.Teacher != null ? course.Teacher.Name : "Не назначен")}");
                Console.WriteLine("Список студентов:");

                if (course.Students.Count == 0)
                {
                    Console.WriteLine("  (Нет записанных студентов)");
                }
                else
                {
                    foreach (var s in course.Students)
                    {
                        Console.WriteLine($"  - ID: {s.Id}, Имя: {s.Name}");
                    }
                }
                Pause();
            }

            static void Pause()
            {
                Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
                Console.ReadKey();
            }

            // Метод для заполнения тестовыми данными
            static void SeedData()
            {
                var t1 = new Teacher("Иванов И.И.", 45, "ivanov@univ.ru", "Математика");
                var t2 = new Teacher("Петрова А.С.", 38, "petrova@univ.ru", "Программирование");
                university.AddTeacher(t1);
                university.AddTeacher(t2);

                var c1 = new Course("Линейная алгебра", 5);
                var c2 = new Course("Основы C#", 4);
                university.AddCourse(c1);
                university.AddCourse(c2);

                c1.SetTeacher(t1);
                c2.SetTeacher(t2);

                var s1 = new Student("Смирнов А.", 19, "smirnov@student.ru");
                var s2 = new Student("Кузнецова М.", 20, "kuznetsova@student.ru");
                university.AddStudent(s1);
                university.AddStudent(s2);

                s1.Enroll(c1);
                s1.Enroll(c2);
                s2.Enroll(c2);
            }
        }
    }