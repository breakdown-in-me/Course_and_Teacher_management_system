using System;
using System.Collections.Generic;

// Базовые классы
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    
    public Student(int id, string name, string email)
    {
        Id = id;
        Name = name;
        Email = email;
    }
    
    public override string ToString()
    {
        return $"{Name} ({Email})";
    }
}

public class Teacher
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Department { get; set; }
    
    public Teacher(int id, string name, string department)
    {
        Id = id;
        Name = name;
        Department = department;
    }
    
    public override string ToString()
    {
        return $"{Name} - {Department}";
    }
}

// Интерфейс курса
public interface ICourse
{
    int CourseId { get; }
    string CourseName { get; }
    Teacher Teacher { get; set; }
    List<Student> Students { get; }
    
    void AddStudent(Student student);
    void RemoveStudent(Student student);
    string GetCourseInfo();
}

// Базовый класс курса
public abstract class Course : ICourse
{
    public int CourseId { get; protected set; }
    public string CourseName { get; protected set; }
    public Teacher Teacher { get; set; }
    public List<Student> Students { get; protected set; }
    
    protected Course(int courseId, string courseName)
    {
        CourseId = courseId;
        CourseName = courseName;
        Students = new List<Student>();
    }
    
    public virtual void AddStudent(Student student)
    {
        // Проверяем, нет ли уже такого студента
        bool exists = false;
        foreach (var s in Students)
        {
            if (s.Id == student.Id)
            {
                exists = true;
                break;
            }
        }
        
        if (!exists)
        {
            Students.Add(student);
        }
    }
    
    public virtual void RemoveStudent(Student student)
    {
        // Ищем студента для удаления
        for (int i = 0; i < Students.Count; i++)
        {
            if (Students[i].Id == student.Id)
            {
                Students.RemoveAt(i);
                break;
            }
        }
    }
    
    public abstract string GetCourseInfo();
    
    public override string ToString()
    {
        return $"{CourseId}: {CourseName}";
    }
}

// Онлайн курс
public class OnlineCourse : Course
{
    public string Platform { get; set; }
    public string MeetingLink { get; set; }
    
    public OnlineCourse(int courseId, string courseName, string platform, string meetingLink) 
        : base(courseId, courseName)
    {
        Platform = platform;
        MeetingLink = meetingLink;
    }
    
    public override string GetCourseInfo()
    {
        return $"Онлайн курс: {CourseName}\n" +
               $"Платформа: {Platform}\n" +
               $"Ссылка: {MeetingLink}\n" +
               $"Преподаватель: {Teacher?.Name ?? "Не назначен"}\n" +
               $"Студентов: {Students.Count}";
    }
}

// Офлайн курс
public class OfflineCourse : Course
{
    public string Classroom { get; set; }
    public string Schedule { get; set; }
    
    public OfflineCourse(int courseId, string courseName, string classroom, string schedule) 
        : base(courseId, courseName)
    {
        Classroom = classroom;
        Schedule = schedule;
    }
    
    public override string GetCourseInfo()
    {
        return $"Офлайн курс: {CourseName}\n" +
               $"Аудитория: {Classroom}\n" +
               $"Расписание: {Schedule}\n" +
               $"Преподаватель: {Teacher?.Name ?? "Не назначен"}\n" +
               $"Студентов: {Students.Count}";
    }
}

// Система управления
public class CourseManagementSystem
{
    private List<ICourse> _courses;
    private List<Teacher> _teachers;
    private List<Student> _students;
    
    public CourseManagementSystem()
    {
        _courses = new List<ICourse>();
        _teachers = new List<Teacher>();
        _students = new List<Student>();
    }
    
    // Курсы
    public void AddCourse(ICourse course)
    {
        _courses.Add(course);
    }
    
    public void RemoveCourse(int courseId)
    {
        for (int i = 0; i < _courses.Count; i++)
        {
            if (_courses[i].CourseId == courseId)
            {
                _courses.RemoveAt(i);
                break;
            }
        }
    }
    
    public ICourse GetCourse(int courseId)
    {
        foreach (var course in _courses)
        {
            if (course.CourseId == courseId)
            {
                return course;
            }
        }
        return null;
    }
    
    public List<ICourse> GetAllCourses()
    {
        return new List<ICourse>(_courses);
    }
    
    // Преподаватели
    public void AddTeacher(Teacher teacher)
    {
        _teachers.Add(teacher);
    }
    
    public Teacher GetTeacher(int teacherId)
    {
        foreach (var teacher in _teachers)
        {
            if (teacher.Id == teacherId)
            {
                return teacher;
            }
        }
        return null;
    }
    
    public List<Teacher> GetAllTeachers()
    {
        return new List<Teacher>(_teachers);
    }
    
    // Студенты
    public void AddStudent(Student student)
    {
        _students.Add(student);
    }
    
    public Student GetStudent(int studentId)
    {
        foreach (var student in _students)
        {
            if (student.Id == studentId)
            {
                return student;
            }
        }
        return null;
    }
    
    public List<Student> GetAllStudents()
    {
        return new List<Student>(_students);
    }
    
    // Бизнес-логика
    public void AssignTeacherToCourse(int teacherId, int courseId)
    {
        var teacher = GetTeacher(teacherId);
        var course = GetCourse(courseId);
        
        if (teacher != null && course != null)
        {
            course.Teacher = teacher;
        }
    }
    
    public List<ICourse> GetCoursesByTeacher(int teacherId)
    {
        var result = new List<ICourse>();
        foreach (var course in _courses)
        {
            if (course.Teacher != null && course.Teacher.Id == teacherId)
            {
                result.Add(course);
            }
        }
        return result;
    }
    
    public void EnrollStudentInCourse(int studentId, int courseId)
    {
        var student = GetStudent(studentId);
        var course = GetCourse(courseId);
        
        if (student != null && course != null)
        {
            course.AddStudent(student);
        }
    }
    
    // Новые методы для работы со студентами на курсах
    public void RemoveStudentFromCourse(int studentId, int courseId)
    {
        var student = GetStudent(studentId);
        var course = GetCourse(courseId);
        
        if (student != null && course != null)
        {
            course.RemoveStudent(student);
        }
    }
    
    public List<Student> GetStudentsOnCourse(int courseId)
    {
        var course = GetCourse(courseId);
        if (course != null)
        {
            return new List<Student>(course.Students);
        }
        return new List<Student>();
    }
    
    public string GetCourseStudentsInfo(int courseId)
    {
        var course = GetCourse(courseId);
        if (course == null)
        {
            return "Курс не найден!";
        }
        
        var info = $"Студенты на курсе '{course.CourseName}':\n";
        if (course.Students.Count == 0)
        {
            info += "Нет записанных студентов\n";
        }
        else
        {
            foreach (var student in course.Students)
            {
                info += $"- {student.Name} (ID: {student.Id}, Email: {student.Email})\n";
            }
        }
        return info;
    }
}

// Главная программа
class Program
{
    private static CourseManagementSystem system = new CourseManagementSystem();
    
    static void Main(string[] args)
    {
        // Добавляем начальные данные
        InitializeData();
        
        bool running = true;
        while (running)
        {
            ShowMenu();
            var input = Console.ReadLine();
            
            switch (input)
            {
                case "1":
                    ShowAllCourses();
                    break;
                case "2":
                    AddNewCourse();
                    break;
                case "3":
                    RemoveCourse();
                    break;
                case "4":
                    ShowAllTeachers();
                    break;
                case "5":
                    AssignTeacherToCourse();
                    break;
                case "6":
                    ShowAllStudents();
                    break;
                case "7":
                    ShowStudentsOnCourse();
                    break;
                case "8":
                    EnrollStudentToCourse();
                    break;
                case "9":
                    RemoveStudentFromCourse();
                    break;
                case "10":
                    ShowCourseDetails();
                    break;
                case "0":
                    running = false;
                    Console.WriteLine("Выход из программы...");
                    break;
                default:
                    Console.WriteLine("Неверный выбор! Попробуйте снова.");
                    break;
            }
            
            if (running)
            {
                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }
    }
    
    static void InitializeData()
    {
        // Добавляем преподавателей
        system.AddTeacher(new Teacher(1, "Иван Петров", "Информатика"));
        system.AddTeacher(new Teacher(2, "Мария Сидорова", "Математика"));
        system.AddTeacher(new Teacher(3, "Алексей Козлов", "Физика"));
        
        // Добавляем студентов
        system.AddStudent(new Student(1, "Алексей Иванов", "alex@email.com"));
        system.AddStudent(new Student(2, "Елена Козлова", "elena@email.com"));
        system.AddStudent(new Student(3, "Петр Сидоров", "petr@email.com"));
        system.AddStudent(new Student(4, "Ольга Новикова", "olga@email.com"));
        system.AddStudent(new Student(5, "Дмитрий Волков", "dmitry@email.com"));
        
        // Добавляем курсы
        system.AddCourse(new OnlineCourse(1, "Программирование на C#", "Microsoft Teams", "teams.com/csharp"));
        system.AddCourse(new OfflineCourse(2, "Высшая математика", "Аудитория 301", "Пн/Ср 10:00-11:30"));
        system.AddCourse(new OnlineCourse(3, "Физика для начинающих", "Zoom", "zoom.com/physics"));
        system.AddCourse(new OfflineCourse(4, "Алгоритмы и структуры данных", "Аудитория 205", "Вт/Чт 14:00-15:30"));
        
        // Назначаем преподавателей
        system.AssignTeacherToCourse(1, 1);
        system.AssignTeacherToCourse(2, 2);
        system.AssignTeacherToCourse(3, 3);
        system.AssignTeacherToCourse(1, 4);
        
        // Записываем студентов на курсы
        system.EnrollStudentInCourse(1, 1);
        system.EnrollStudentInCourse(2, 1);
        system.EnrollStudentInCourse(3, 2);
        system.EnrollStudentInCourse(4, 2);
        system.EnrollStudentInCourse(5, 3);
        system.EnrollStudentInCourse(1, 4);
        system.EnrollStudentInCourse(5, 4);
    }
    
    static void ShowMenu()
    {
        Console.Clear();
        Console.WriteLine("=== СИСТЕМА УПРАВЛЕНИЯ КУРСАМИ ===");
        Console.WriteLine("1. Показать все курсы");
        Console.WriteLine("2. Добавить новый курс");
        Console.WriteLine("3. Удалить курс");
        Console.WriteLine("4. Показать всех преподавателей");
        Console.WriteLine("5. Назначить преподавателя на курс");
        Console.WriteLine("6. Показать всех студентов");
        Console.WriteLine("7. Показать студентов на курсе");
        Console.WriteLine("8. Записать студента на курс");
        Console.WriteLine("9. Отчислить студента с курса");
        Console.WriteLine("10. Подробная информация о курсе");
        Console.WriteLine("0. Выход");
        Console.Write("Выберите действие: ");
    }
    
    static void ShowAllCourses()
    {
        Console.WriteLine("\n=== ВСЕ КУРСЫ ===");
        var courses = system.GetAllCourses();
        if (courses.Count == 0)
        {
            Console.WriteLine("Курсы не найдены.");
            return;
        }
        
        foreach (var course in courses)
        {
            Console.WriteLine($"ID: {course.CourseId}");
            Console.WriteLine($"Название: {course.CourseName}");
            Console.WriteLine($"Преподаватель: {course.Teacher?.Name ?? "Не назначен"}");
            Console.WriteLine($"Студентов: {course.Students.Count}");
            Console.WriteLine("---");
        }
    }
    
    static void AddNewCourse()
    {
        Console.WriteLine("\n=== ДОБАВЛЕНИЕ НОВОГО КУРСА ===");
        
        Console.Write("Введите ID курса: ");
        if (!int.TryParse(Console.ReadLine(), out int courseId))
        {
            Console.WriteLine("Ошибка: ID должен быть числом!");
            return;
        }
        
        if (system.GetCourse(courseId) != null)
        {
            Console.WriteLine("Ошибка: Курс с таким ID уже существует!");
            return;
        }
        
        Console.Write("Введите название курса: ");
        var courseName = Console.ReadLine();
        
        Console.WriteLine("Выберите тип курса:");
        Console.WriteLine("1. Онлайн курс");
        Console.WriteLine("2. Офлайн курс");
        Console.Write("Ваш выбор: ");
        var typeChoice = Console.ReadLine();
        
        ICourse newCourse;
        
        if (typeChoice == "1")
        {
            Console.Write("Введите платформу: ");
            var platform = Console.ReadLine();
            Console.Write("Введите ссылку на встречу: ");
            var meetingLink = Console.ReadLine();
            
            newCourse = new OnlineCourse(courseId, courseName, platform, meetingLink);
        }
        else if (typeChoice == "2")
        {
            Console.Write("Введите аудиторию: ");
            var classroom = Console.ReadLine();
            Console.Write("Введите расписание: ");
            var schedule = Console.ReadLine();
            
            newCourse = new OfflineCourse(courseId, courseName, classroom, schedule);
        }
        else
        {
            Console.WriteLine("Неверный выбор типа курса!");
            return;
        }
        
        system.AddCourse(newCourse);
        Console.WriteLine($"Курс '{courseName}' успешно добавлен!");
    }
    
    static void RemoveCourse()
    {
        Console.WriteLine("\n=== УДАЛЕНИЕ КУРСА ===");
        ShowAllCourses();
        
        Console.Write("Введите ID курса для удаления: ");
        if (!int.TryParse(Console.ReadLine(), out int courseId))
        {
            Console.WriteLine("Ошибка: ID должен быть числом!");
            return;
        }
        
        var course = system.GetCourse(courseId);
        if (course == null)
        {
            Console.WriteLine("Курс с таким ID не найден!");
            return;
        }
        
        system.RemoveCourse(courseId);
        Console.WriteLine($"Курс '{course.CourseName}' успешно удален!");
    }
    
    static void ShowAllTeachers()
    {
        Console.WriteLine("\n=== ВСЕ ПРЕПОДАВАТЕЛИ ===");
        var teachers = system.GetAllTeachers();
        if (teachers.Count == 0)
        {
            Console.WriteLine("Преподаватели не найдены.");
            return;
        }
        
        foreach (var teacher in teachers)
        {
            Console.WriteLine($"ID: {teacher.Id}");
            Console.WriteLine($"Имя: {teacher.Name}");
            Console.WriteLine($"Кафедра: {teacher.Department}");
            
            var courses = system.GetCoursesByTeacher(teacher.Id);
            Console.WriteLine($"Курсов: {courses.Count}");
            Console.WriteLine("---");
        }
    }
    
    static void AssignTeacherToCourse()
    {
        Console.WriteLine("\n=== НАЗНАЧЕНИЕ ПРЕПОДАВАТЕЛЯ НА КУРС ===");
        
        ShowAllTeachers();
        Console.Write("Введите ID преподавателя: ");
        if (!int.TryParse(Console.ReadLine(), out int teacherId))
        {
            Console.WriteLine("Ошибка: ID должен быть числом!");
            return;
        }
        
        ShowAllCourses();
        Console.Write("Введите ID курса: ");
        if (!int.TryParse(Console.ReadLine(), out int courseId))
        {
            Console.WriteLine("Ошибка: ID должен быть числом!");
            return;
        }
        
        system.AssignTeacherToCourse(teacherId, courseId);
        Console.WriteLine("Преподаватель успешно назначен на курс!");
    }
    
    static void ShowAllStudents()
    {
        Console.WriteLine("\n=== ВСЕ СТУДЕНТЫ ===");
        var students = system.GetAllStudents();
        if (students.Count == 0)
        {
            Console.WriteLine("Студенты не найдены.");
            return;
        }
        
        foreach (var student in students)
        {
            Console.WriteLine($"ID: {student.Id}");
            Console.WriteLine($"Имя: {student.Name}");
            Console.WriteLine($"Email: {student.Email}");
            Console.WriteLine("---");
        }
    }
    
    static void ShowStudentsOnCourse()
    {
        Console.WriteLine("\n=== СТУДЕНТЫ НА КУРСЕ ===");
        ShowAllCourses();
        
        Console.Write("Введите ID курса: ");
        if (!int.TryParse(Console.ReadLine(), out int courseId))
        {
            Console.WriteLine("Ошибка: ID должен быть числом!");
            return;
        }
        
        var studentsInfo = system.GetCourseStudentsInfo(courseId);
        Console.WriteLine(studentsInfo);
    }
    
    static void EnrollStudentToCourse()
    {
        Console.WriteLine("\n=== ЗАПИСЬ СТУДЕНТА НА КУРС ===");
        
        ShowAllStudents();
        Console.Write("Введите ID студента: ");
        if (!int.TryParse(Console.ReadLine(), out int studentId))
        {
            Console.WriteLine("Ошибка: ID должен быть числом!");
            return;
        }
        
        ShowAllCourses();
        Console.Write("Введите ID курса: ");
        if (!int.TryParse(Console.ReadLine(), out int courseId))
        {
            Console.WriteLine("Ошибка: ID должен быть числом!");
            return;
        }
        
        system.EnrollStudentInCourse(studentId, courseId);
        Console.WriteLine("Студент успешно записан на курс!");
    }
    
    static void RemoveStudentFromCourse()
    {
        Console.WriteLine("\n=== ОТЧИСЛЕНИЕ СТУДЕНТА С КУРСА ===");
        
        ShowAllCourses();
        Console.Write("Введите ID курса: ");
        if (!int.TryParse(Console.ReadLine(), out int courseId))
        {
            Console.WriteLine("Ошибка: ID должен быть числом!");
            return;
        }
        
        var studentsInfo = system.GetCourseStudentsInfo(courseId);
        Console.WriteLine(studentsInfo);
        
        Console.Write("Введите ID студента для отчисления: ");
        if (!int.TryParse(Console.ReadLine(), out int studentId))
        {
            Console.WriteLine("Ошибка: ID должен быть числом!");
            return;
        }
        
        system.RemoveStudentFromCourse(studentId, courseId);
        Console.WriteLine("Студент успешно отчислен с курса!");
    }
    
    static void ShowCourseDetails()
    {
        Console.WriteLine("\n=== ПОДРОБНАЯ ИНФОРМАЦИЯ О КУРСЕ ===");
        ShowAllCourses();
        
        Console.Write("Введите ID курса: ");
        if (!int.TryParse(Console.ReadLine(), out int courseId))
        {
            Console.WriteLine("Ошибка: ID должен быть числом!");
            return;
        }
        
        var course = system.GetCourse(courseId);
        if (course == null)
        {
            Console.WriteLine("Курс не найден!");
            return;
        }
        
        Console.WriteLine("\n" + course.GetCourseInfo());
        Console.WriteLine("\n" + system.GetCourseStudentsInfo(courseId));
    }
}