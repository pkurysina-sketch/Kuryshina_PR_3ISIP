using System;
using System.Collections.Generic;
using System.Linq;

namespace _3ИСИП_424
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nОнлайн маркетплейс GMWOG||GG.MOW||WONGG");
                if (Core.CurrentUser == null)
                {
                    Console.WriteLine("1. Войти;");
                    Console.WriteLine("2. Зарегистрироваться;");
                    Console.WriteLine("3. Каталог товаров;");
                    Console.WriteLine("0. Выход");
                    Console.Write("Выбор: ");
                    string choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1": LoginMenu(); break;
                        case "2": RegisterMenu(); break;
                        case "3": ProductsMenu(); break;
                        case "0": exit = true; break;
                        default: Console.WriteLine("Неверная команда."); break;
                    }
                }
                else
                {
                    Console.WriteLine($"Вы вошли как: {Core.CurrentUser.Nickname}");
                    Console.WriteLine("1. Товары / В корзину;");
                    Console.WriteLine("2. Корзина;");
                    Console.WriteLine("3. История заказов;");
                    Console.WriteLine("4. Выйти");
                    Console.Write("Выбор: ");
                    string choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1": ProductsMenu(); break;
                        case "2": CartMenu(); break;
                        case "3": HistoryMenu(); break;
                        case "4": Core.Logout(); break;
                        default: Console.WriteLine("Неверная команда."); break;
                    }
                }
            }
        }

        static void RegisterMenu()
        {
            Console.WriteLine("Регистрация");
            Console.Write("Никнейм: ");
            string nick = Console.ReadLine();
            Console.Write("Пароль: ");
            string pass1 = Console.ReadLine();
            Console.Write("Повторите пароль: ");
            string pass2 = Console.ReadLine();
            string result = Core.RegisterUser(nick, pass1, pass2);
            Console.WriteLine(result == "Успешно" ? "Вы успешно зарегистрированы!" : $"Ошибка: {result}");
        }

        static void LoginMenu()
        {
            Console.WriteLine("Вход");
            Console.Write("Никнейм: ");
            string nick = Console.ReadLine();
            Console.Write("Пароль: ");
            string pass = Console.ReadLine();
            if (Core.Login(nick, pass)) Console.WriteLine("Успешный вход!");
            else Console.WriteLine("Неверный логин или пароль!");
        }

        static void ProductsMenu()
        {
            Console.WriteLine("Список товаров");
            var products = Core.GetProducts();
            if (!products.Any())
            {
                Console.WriteLine("Товаров нет.");
                return;
            }

            foreach (var p in products)
            {
                string status = p.IsAvailable ? "В наличии" : "Нет в наличии";
                Console.WriteLine($"ID: {p.Id} {p.Name} {p.Price} руб. ({status})");
            }

            if (Core.CurrentUser == null)
            {
                Console.WriteLine("Для покупки нужно авторизоваться.");
                return;
            }

            Console.Write("Введите ID товара для добавления (0 - назад): ");
            if (int.TryParse(Console.ReadLine(), out int id) && id != 0)
            {
                Console.WriteLine(Core.AddToCart(id));
            }
        }

        static void CartMenu()
        {
            Console.WriteLine("Ваша корзина");
            var cart = Core.GetCartItems();
            if (!cart.Any())
            {
                Console.WriteLine("Корзина пуста.");
                return;
            }

            decimal total = 0;
            foreach (var item in cart)
            {
                var product = Core.Context.Product.Find(item.ProductId);
                if (product != null)
                {
                    Console.WriteLine($"ID: {product.Id} {product.Name} {product.Price} руб.");
                    total += product.Price;
                }
            }
            Console.WriteLine($"Итого: {total} руб.");

            Console.WriteLine("1. Оформить заказ;");
            Console.WriteLine("2. Удалить товар;");
            Console.WriteLine("0. Назад");
            Console.Write("Выбор: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1": CheckoutMenu(cart); break;
                case "2":
                    Console.Write("Введите ID товара для удаления: ");
                    if (int.TryParse(Console.ReadLine(), out int id)) Core.RemoveFromCart(id);
                    break;
            }
        }
        static void CheckoutMenu(List<CartItem> cart)
        {
            Console.WriteLine("Оформление");
            var points = Core.GetPickupPoint();
            if (!points.Any())
            {
                Console.WriteLine("Нет ПВЗ.");
                return;
            }

            foreach (var p in points) Console.WriteLine($"ID: {p.Id} {p.Address}");

            Console.Write("Введите ID ПВЗ: ");
            if (!int.TryParse(Console.ReadLine(), out int pointId) || !points.Any(p => p.Id == pointId))
            {
                Console.WriteLine("Неверный ID ПВЗ.");
                return;
            }

            var ids = cart.Select(c => c.ProductId).ToList();
            Console.WriteLine(Core.CheckoutSelected(ids, pointId));
        }

        static void HistoryMenu()
        {
            Console.WriteLine("История заказов");
            var orders = Core.GetOrderHistory();
            if (!orders.Any())
            {
                Console.WriteLine("Заказов нет.");
                return;
            }

            foreach (var order in orders)
            {
                string addr = Core.GetPickupPointAddress(order.PickupPointId);
                string status = order.IsCompleted ? "Доставлен" : "В обработке";
                Console.WriteLine($"Заказ №{order.Id} от {order.CreatedAt:dd.MM.yyyy} {status} {addr}");

                var products = Core.GetProductInOrder(order.Id);
                foreach (var name in products) Console.WriteLine($" {name}");
            }
        }
    }
}

