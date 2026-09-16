using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    enum Category
    {
        Электроника = 1,
        Продукты,
        Одежда,
        Бытовая_техника
    }

    class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public Category Category { get; set; }

        public bool IsInStock
        {
            get { return Quantity > 0; }
        }

        public Product(int id, string name, int price, int quantity, Category category)
        {
            Id = id;
            Name = name;
            Price = price;
            Quantity = quantity;
            Category = category;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, Product> products = new Dictionary<int, Product>();

            int nextId = 1;
            products.Add(nextId, new Product(nextId++, "Смартфон", 50000, 10, Category.Электроника));
            products.Add(nextId, new Product(nextId++, "Хлеб", 50, 100, Category.Продукты));
            products.Add(nextId, new Product(nextId++, "Футболка", 1500, 20, Category.Одежда));
            products.Add(nextId, new Product(nextId++, "Пылесос", 12000, 5, Category.Бытовая_техника));
            products.Add(nextId, new Product(nextId++, "Ноутбук", 80000, 0, Category.Электроника));

            Console.WriteLine("Учёт товаров в магазине");

            int w = -1;
            do
            {
                Console.WriteLine("Выберите задачу:");
                Console.WriteLine("1. Добавить товар;");
                Console.WriteLine("2. Удалить товар;");
                Console.WriteLine("3. Заказать поставку товара;");
                Console.WriteLine("4. Продать товар;");
                Console.WriteLine("5. Поиск товаров;");
                Console.WriteLine("6. Показать все товары;");
                Console.WriteLine("0. Выход");

                if (!int.TryParse(Console.ReadLine(), out w))
                {
                    Console.WriteLine("Ошибка: введите число!");
                    continue;
                }

                switch (w)
                {
                    case 1:
                        Console.WriteLine("Введите данные в формате: Название; Цена; Количество; Категория(1-4)");
                        Console.WriteLine("Пример: Телевизор; 30000; 5; 1");
                        Console.Write("Ввод: ");

                        string input = Console.ReadLine();
                        string[] parts = input.Split(';');

                        if (parts.Length != 4)
                        {
                            Console.WriteLine("Ошибка формата! Используйте: Название; Цена; Количество; Категория");
                            break;
                        }

                        string name = parts[0].Trim();
                        int price, quantity, catNum;

                        if (string.IsNullOrWhiteSpace(name))
                        {
                            Console.WriteLine("Ошибка: название не может быть пустым!");
                            break;
                        }

                        if (!int.TryParse(parts[1].Trim(), out price) || price < 0)
                        {
                            Console.WriteLine("Ошибка: цена должна быть неотрицательным числом!");
                            break;
                        }

                        if (!int.TryParse(parts[2].Trim(), out quantity) || quantity < 0)
                        {
                            Console.WriteLine("Ошибка: количество должно быть неотрицательным числом!");
                            break;
                        }

                        if (!int.TryParse(parts[3].Trim(), out catNum) || catNum < 1 || catNum > 4)
                        {
                            Console.WriteLine("Ошибка: категория должна быть от 1 до 4!");
                            break;
                        }

                        Category category = (Category)catNum;
                        Product newProduct = new Product(nextId, name, price, quantity, category);
                        products.Add(nextId, newProduct);
                        Console.WriteLine($"Товар успешно добавлен! Присвоен код: {nextId}");
                        nextId++;
                        break;

                    case 2:
                        Console.Write("Введите код товара для удаления: ");

                        int delId;
                        if (!int.TryParse(Console.ReadLine(), out delId))
                        {
                            Console.WriteLine("Ошибка: введите число!");
                            break;
                        }

                        if (products.ContainsKey(delId))
                        {
                            Console.WriteLine($"Товар '{products[delId].Name}' удалён.");
                            products.Remove(delId);
                        }
                        else
                        {
                            Console.WriteLine("Товар с таким кодом не найден.");
                        }
                        break;

                    case 3:
                        Console.Write("Введите код товара: ");

                        int supplyId;
                        if (!int.TryParse(Console.ReadLine(), out supplyId))
                        {
                            Console.WriteLine("Ошибка: введите число!");
                            break;
                        }

                        if (!products.ContainsKey(supplyId))
                        {
                            Console.WriteLine("Товар с таким кодом не найден.");
                            break;
                        }

                        Console.Write("Введите количество для поставки: ");
                        int addAmount;
                        if (!int.TryParse(Console.ReadLine(), out addAmount) || addAmount <= 0)
                        {
                            Console.WriteLine("Ошибка: количество должно быть положительным числом!");
                            break;
                        }

                        products[supplyId].Quantity += addAmount;
                        Console.WriteLine($"Поставка выполнена. Новое количество: {products[supplyId].Quantity}");
                        break;

                    case 4:
                        Console.Write("Введите код товара: ");

                        int sellId;
                        if (!int.TryParse(Console.ReadLine(), out sellId))
                        {
                            Console.WriteLine("Ошибка: введите число!");
                            break;
                        }

                        if (!products.ContainsKey(sellId))
                        {
                            Console.WriteLine("Товар с таким кодом не найден.");
                            break;
                        }

                        Product sellProduct = products[sellId];

                        if (!sellProduct.IsInStock)
                        {
                            Console.WriteLine("Ошибка: Товара нет в наличии!");
                            break;
                        }

                        Console.Write("Введите количество для продажи: ");
                        int sellAmount;
                        if (!int.TryParse(Console.ReadLine(), out sellAmount) || sellAmount <= 0)
                        {
                            Console.WriteLine("Ошибка: количество должно быть положительным числом!");
                            break;
                        }

                        if (sellAmount > sellProduct.Quantity)
                        {
                            Console.WriteLine($"Ошибка: Недостаточно товара. В наличии только {sellProduct.Quantity} шт.");
                            break;
                        }

                        sellProduct.Quantity -= sellAmount;
                        Console.WriteLine($"Продан успешно. Остаток: {sellProduct.Quantity}");
                        break;

                    case 5:
                        Console.WriteLine("1. По коду");
                        Console.WriteLine("2. По названию");
                        Console.WriteLine("3. По категории");
                        Console.Write("Выберите тип поиска: ");

                        int searchType;
                        if (!int.TryParse(Console.ReadLine(), out searchType))
                        {
                            Console.WriteLine("Ошибка: введите число!");
                            break;
                        }

                        bool found = false;

                        if (searchType == 1)
                        {
                            Console.Write("Введите код: ");
                            int searchId;
                            if (!int.TryParse(Console.ReadLine(), out searchId))
                            {
                                Console.WriteLine("Ошибка: введите число!");
                                break;
                            }

                            if (products.ContainsKey(searchId))
                            {
                                PrintProduct(products[searchId]);
                                found = true;
                            }
                        }
                        else if (searchType == 2)
                        {
                            Console.Write("Введите название (или часть): ");
                            string searchName = Console.ReadLine().ToLower();

                            foreach (var item in products)
                            {
                                if (item.Value.Name.ToLower().Contains(searchName))
                                {
                                    PrintProduct(item.Value);
                                    found = true;
                                }
                            }
                        }
                        else if (searchType == 3)
                        {
                            Console.WriteLine("Доступные категории:");
                            Console.WriteLine("1. Электроника");
                            Console.WriteLine("2. Продукты");
                            Console.WriteLine("3. Одежда");
                            Console.WriteLine("4. Бытовая техника");
                            Console.Write("Введите номер категории: ");

                            int catSearch;
                            if (!int.TryParse(Console.ReadLine(), out catSearch) || catSearch < 1 || catSearch > 4)
                            {
                                Console.WriteLine("Ошибка: неверная категория!");
                                break;
                            }

                            Category searchCat = (Category)catSearch;
                            foreach (var item in products)
                            {
                                if (item.Value.Category == searchCat)
                                {
                                    PrintProduct(item.Value);
                                    found = true;
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Неверный выбор.");
                            break;
                        }

                        if (!found)
                        {
                            Console.WriteLine("Ничего не найдено.");
                        }
                        break;

                    case 6:
                        Console.WriteLine("\nСписок всех товаров");
                        if (products.Count == 0)
                        {
                            Console.WriteLine("Список пуст.");
                        }
                        else
                        {
                            foreach (var item in products)
                            {
                                PrintProduct(item.Value);
                            }
                        }
                        break;

                    case 0:
                        Console.WriteLine("Выход из программы.");
                        break;

                    default:
                        Console.WriteLine("Неверный пункт меню.");
                        break;
                }

                Console.WriteLine();

            } while (w != 0);
        }
        static void PrintProduct(Product p)
        {
            string stockStatus = p.IsInStock ? "Есть на складе" : "Нет в наличии";
            Console.WriteLine($"Код: {p.Id} Название: {p.Name} Цена: {p.Price} руб. Кол-во: {p.Quantity} Категория: {p.Category} Статус: {stockStatus}");
        }
    }
}
