using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace SimpleLibrary
{
    // Простой класс для книги
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }

        public override string ToString()
        {
            return $"ID: {Id} {Title} {Author} {Genre} {Year} г. {Price} руб.";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Book> books = new List<Book>();
            int nextId = 1;

            // 5 тестовых книг
            books.Add(new Book { Id = nextId++, Title = "Война и мир", Author = "Толстой", Genre = "История", Year = 1869, Price = 1500 });
            books.Add(new Book { Id = nextId++, Title = "1984", Author = "Оруэлл", Genre = "Фантастика", Year = 1949, Price = 800 });
            books.Add(new Book { Id = nextId++, Title = "Гарри Поттер", Author = "Роулинг", Genre = "Фэнтези", Year = 1997, Price = 1200 });
            books.Add(new Book { Id = nextId++, Title = "Шерлок Холмс", Author = "Дойл", Genre = "Детектив", Year = 1892, Price = 600 });
            books.Add(new Book { Id = nextId++, Title = "Космос", Author = "Саган", Genre = "Наука", Year = 1980, Price = 2000 });

            while (true)
            {
                Console.WriteLine("1. Добавить;");
                Console.WriteLine("2. Удалить;");
                Console.WriteLine("3. Поиск;");
                Console.WriteLine("4. Сортировка;");
                Console.WriteLine("5. Цены;");
                Console.WriteLine("6. Группы;");
                Console.WriteLine("7. Все;");
                Console.WriteLine("0. Выход.");

                Console.Write("Выбор: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        // Добавление
                        string title;
                        do
                        {
                            Console.Write("Название: ");
                            title = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(title)) Console.WriteLine("Ошибка: поле не может быть пустым!");
                        } while (string.IsNullOrWhiteSpace(title));

                        string author;
                        do
                        {
                            Console.Write("Автор: ");
                            author = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(author)) Console.WriteLine("Ошибка: поле не может быть пустым!");
                        } while (string.IsNullOrWhiteSpace(author));

                        string genre;
                        do
                        {
                            Console.Write("Жанр: ");
                            genre = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(genre)) Console.WriteLine("Ошибка: поле не может быть пустым!");
                        } while (string.IsNullOrWhiteSpace(genre));

                        int year;
                        do
                        {
                            Console.Write("Год: ");
                        } while (!int.TryParse(Console.ReadLine(), out year) || year <= 0);

                        decimal price;
                        do
                        {
                            Console.Write("Цена: ");
                        } while (!decimal.TryParse(Console.ReadLine(), out price) || price <= 0);

                        books.Add(new Book { Id = nextId++, Title = title, Author = author, Genre = genre, Year = year, Price = price });
                        Console.WriteLine("Книга добавлена!");
                        break;

                    case "2":
                        // Удаление
                        int idToRemove;
                        do
                        {
                            Console.Write("Введите ID для удаления: ");
                        } while (!int.TryParse(Console.ReadLine(), out idToRemove));

                        Book bookToRemove = null;
                        foreach (var b in books)
                        {
                            if (b.Id == idToRemove)
                            {
                                bookToRemove = b;
                                break;
                            }
                        }

                        if (bookToRemove != null)
                        {
                            books.Remove(bookToRemove);
                            Console.WriteLine("Удалено!");
                        }
                        else Console.WriteLine("Не найдено!");
                        break;

                    case "3":
                        // Поиск
                        Console.Write("Введите текст для поиска: ");
                        string quere = (Console.ReadLine() ?? "").ToLower();
                        //Book firstLetter = books.FirstOrDefault(b => b.Title == query || b.Author == query || b.Genre == query);
                        List<Book> found = new List<Book>();
                        foreach (var b in books)
                        {
                            if (b.Title.ToLower().Contains(quere) ||
                                b.Author.ToLower().Contains(quere) ||
                                b.Genre.ToLower().Contains(quere))
                            {
                                found.Add(b);
                            }
                        }

                        if (found.Count == 0) Console.WriteLine("Ничего не найдено.");
                        else foreach (var b in found) Console.WriteLine(b);
                        break;
                    case "4":
                        // Сортировка
                        string sortChoice;
                        do
                        {
                            Console.Write("1-По названию, 2-По году: ");
                            sortChoice = Console.ReadLine();
                        } while (sortChoice != "1" && sortChoice != "2");

                        // Копируем список, чтобы не менять исходный
                        List<Book> sorted = new List<Book>(books);

                        // Пузырьковая сортировка
                        for (int i = 0; i < sorted.Count - 1; i++)
                        {
                            for (int j = 0; j < sorted.Count - 1 - i; j++)
                            {
                                bool needSwap;
                                if (sortChoice == "1")
                                    needSwap = string.Compare(sorted[j].Title, sorted[j + 1].Title) > 0;
                                else
                                    needSwap = sorted[j].Year > sorted[j + 1].Year;

                                if (needSwap)
                                {
                                    Book temp = sorted[j];
                                    sorted[j] = sorted[j + 1];
                                    sorted[j + 1] = temp;
                                }
                            }
                        }

                        foreach (var b in sorted) Console.WriteLine(b);
                        break;

                    case "5":
                        // Самая дорогая и дешевая
                        if (books.Count == 0) { Console.WriteLine("Список пуст."); break; }

                        Book expensive = books[0];
                        Book cheap = books[0];

                        foreach (var b in books)
                        {
                            if (b.Price > expensive.Price) expensive = b;
                            if (b.Price < cheap.Price) cheap = b;
                        }

                        Console.WriteLine("Дорогая: " + expensive);
                        Console.WriteLine("Дешевая: " + cheap);
                        break;

                    case "6":
                        // Группировка по авторам
                        List<string> authors = new List<string>();
                        foreach (var b in books)
                        {
                            if (!authors.Contains(b.Author))
                                authors.Add(b.Author);
                        }

                        foreach (var authorName in authors)
                        {
                            int count = 0;
                            foreach (var b in books)
                            {
                                if (b.Author == authorName) count++;
                            }
                            Console.WriteLine($"Автор: {authorName} (Книг: {count})");
                        }
                        break;
                    case "7":
                        // Вывод всех
                        foreach (var b in books) Console.WriteLine(b);
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
            }
        }
    }
}

