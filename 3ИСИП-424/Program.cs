using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ИСИП_424
{

    internal class Program
    {
        static void Main(string[] args)
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nМаркетплейс");
                if (Core.CurrentUser == null)
                {
                    Console.WriteLine("1. Войти");
                    Console.WriteLine("2. Зарегистрироваться");
                    Console.WriteLine("3. Посмотреть товары (без входа)");
                    Console.WriteLine("0. Выход");
                    Console.Write("Ваш выбор: ");
                    string choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1": LoginMenu(); break;
                        case "2": RegisterMenu(); break;
                        case "3": ProductsMenu(); break;
                        case "0": exit = true; break;
                        default: Console.WriteLine("Неверная команда. Попробуйте снова."); break;
                    }
                }
                else
                {
                    Console.WriteLine($"Вы вошли как: {Core.CurrentUser.Nickname}");
                    Console.WriteLine("1. Посмотреть товары и добавить в корзину");
                    Console.WriteLine("2. Моя корзина");
                    Console.WriteLine("3. История заказов");
                    Console.WriteLine("4. Выйти из аккаунта");
                    Console.Write("Ваш выбор: ");
                    string choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1": ProductsMenu(); break;
                        case "2": CartMenu(); break;
                        case "3": Core.Logout(); break;
                        default: Console.WriteLine("Неверная команда. Попробуйте снова."); break;
                    }
                }
            }
        }

        static void RegisterMenu()
        {
            Console.WriteLine("\n2. Регистрация пользователя");
            Console.Write("Придумайте никнейм: ");
            string nick = Console.ReadLine();
            Console.Write("Придумайте пароль: ");
            string password1 = Console.ReadLine();
            Console.Write("Повторите пароль: ");
            string password2 = Console.ReadLine();

            string result = Core.RegisterUser(nick, password1, password2);
            if (result == "Успешно")
            {
                Console.WriteLine("Регистрация успешна! Теперь можете войти");
            }
            else
            {
                Console.WriteLine(result);
            }
        }
        static void LoginMenu()
        {
            Console.WriteLine("\n1. Вход пользователя");
            Console.Write("Никнейм: ");
            string nick = Console.ReadLine();
            Console.Write("Пароль: ");
            string password = Console.ReadLine();

            if (Core.Login(nick, password))
            {
                Console.WriteLine("Успешный вход!");
            }
            else
            {
                Console.WriteLine("Неверный логин или пароль!");
            }
        }
        static void ProductsMenu()
        {
            Console.WriteLine("\nСписок товаров");
            var product = Core.GetProducts();
            if (product.Count == 0)
            {
                Console.WriteLine("Товаров нет");
                return;
            }
            foreach (var item in product)
            {
                string status;
                if (item.IsAvailable)
                {
                    status = "В наличии";
                }
                else
                {
                    status = "Нет в наличии";
                }
                Console.WriteLine($"ID: {item.ProductID}\n" + $"Название: {item.Name}\n" + $"Цена: {item.Price} руб.\n" + $"{status}\n");
            }
            if (Core.CurrentUser == null)
            {
                Console.WriteLine("Для добавления в корзину необходимо войти в аккаунт");
                return;
            }
            Console.Write("\nВведите ID товара, чтобы добавить в корзину (или 0 для выхода): ");
            string input = Console.ReadLine();
            if (int.TryParse(input, out int productID))
            {
                if (productID == 0)
                {
                    return;
                }
                string result = Core.AddToCart(Core.CurrentUser.UserID, productID);
                Console.WriteLine(result);
                if (result == "Товар добавлен в корзину!")
                {
                    Console.WriteLine("\nВаша корзина: ");
                    CartMenu();
                }
            }
        }
        static void CartMenu()
        {
            var cartItems = Core.GetCartItems();
            if (cartItems.Count == 0)
            {
                Console.WriteLine("Корзина пуста");
                return;
            }
            decimal total = 0;
            foreach (var item in cartItems)
            {
                var product = Core.Context.Product.Find(item.ProductID);
                if (product != null)
                {
                    Console.WriteLine($"ID: {product.ProductID}\n" + $"Название: {product.Name}\n" + $"Цена: {product.Price} руб.\n");
                    total += product.Price;
                }
            }
            Console.WriteLine($"Итого: {total} руб.");
            Console.WriteLine("\n1. Оформить заказ");
            Console.WriteLine("2. Удалить товар");
            Console.WriteLine("0. Назад");
            Console.Write("Ваш выбор: ");
            string choice = Console.ReadLine();
        }
    }
}