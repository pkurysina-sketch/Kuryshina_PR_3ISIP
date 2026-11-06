using System;
using System.Collections.Generic;
using System.Linq;

namespace _3ИСИП_424
{
    public static class Core
    {
        public static MyBDMagazinEntities Context = new MyBDMagazinEntities();
        
        public static User CurrentUser { get; set; } = null;

        //ТОВАРЫ
        public static List<Product> GetProducts()
        {
            return Context.Product.ToList();
        }

        public static List<PickupPoint> GetPickupPoint()
        {
            return Context.PickupPoint.ToList();
        }

        //АВТОРИЗАЦИЯ
        public static string RegisterUser(string nick, string pass1, string pass2)
        {
            if (string.IsNullOrWhiteSpace(nick) || string.IsNullOrWhiteSpace(pass1))
                return "Поля не могут быть пустыми!";

            if (pass1 != pass2)
                return "Пароли не совпадают!";

            if (Context.User.Any(p => p.Nickname == nick))
                return "Пользователь с таким ником уже существует!";

            try
            {
                Context.User.Add(new User { Nickname = nick, PasswordHash = pass1 });
                Context.SaveChanges();
                return "Успешно";
            }
            catch (Exception ex)
            {
                return $"Ошибка БД: {ex.Message}";
            }
        }

        public static bool Login(string nick, string pass)
        {
            var user = Context.User.FirstOrDefault(p => p.Nickname == nick && p.PasswordHash == pass);
            if (user != null)
            {
                CurrentUser = user;
                return true;
            }
            return false;
        }

        public static void Logout()
        {
            CurrentUser = null;
        }

        //КОРЗИНА
        public static string AddToCart(int productID)
        {
            if (CurrentUser == null) return "Вы не авторизованы!";

            var product = Context.Product.Find(productID);
            if (product == null) return "Товар не найден!";
            if (!product.IsAvailable) return "Товара нет в наличии!";

            try
            {
                // Проверяем, есть ли уже этот товар в корзине, чтобы не дублировать
                var existingItem = Context.CartItem.FirstOrDefault(c => c.UserId == CurrentUser.Id && c.ProductId == productID);
                if (existingItem != null) return "Этот товар уже есть в корзине!";

                Context.CartItem.Add(new CartItem { UserId = CurrentUser.Id, ProductId = productID });
                Context.SaveChanges();
                return "Товар добавлен в корзину!";
            }
            catch (Exception ex)
            {
                return $"Ошибка добавления: {ex.Message}";
            }
        }

        public static List<CartItem> GetCartItems()
        {
            if (CurrentUser == null) return new List<CartItem>();
            return Context.CartItem.Where(p => p.UserId == CurrentUser.Id).ToList();
        }

        public static void RemoveFromCart(int productID)
        {
            if (CurrentUser == null) return;

            var item = Context.CartItem.FirstOrDefault(p => p.UserId == CurrentUser.Id && p.ProductId == productID);
            if (item != null)
            {
                Context.CartItem.Remove(item);
                Context.SaveChanges();
                Console.WriteLine("Товар удален из корзины.");
            }
            else
            {
                Console.WriteLine($"Товара с ID {productID} нет в вашей корзине.");
            }
        }

        //ЗАКАЗЫ
        public static string CheckoutSelected(List<int> selectedProductIDs, int pickupPointID)
        {
            if (CurrentUser == null) return "Вы не авторизованы!";
            if (selectedProductIDs == null || selectedProductIDs.Count == 0) return "Корзина пуста!";

            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var newOrder = new Order
                    {
                        UserId = CurrentUser.Id,
                        PickupPointId = pickupPointID,
                        CreatedAt = DateTime.Now,
                        IsCompleted = false
                    };
                    Context.Order.Add(newOrder);
                    Context.SaveChanges();

                    foreach (var prodID in selectedProductIDs)
                    {
                        Context.OrderProduct.Add(new OrderProduct { OrderId = newOrder.Id, ProductId = prodID });

                        // Удаляем из корзины
                        var cartItem = Context.CartItem.FirstOrDefault(p => p.UserId == CurrentUser.Id && p.ProductId == prodID);
                        if (cartItem != null) Context.CartItem.Remove(cartItem);
                    }

                    Context.SaveChanges();
                    transaction.Commit();
                    return "Заказ успешно оформлен!";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return $"Ошибка оформления: {ex.Message}";
                }
            }
        }

        public static List<Order> GetOrderHistory()
        {
            if (CurrentUser == null) return new List<Order>();
            return Context.Order.Where(p => p.UserId == CurrentUser.Id).OrderByDescending(p => p.CreatedAt).ToList();
        }

        public static string GetPickupPointAddress(int pointID)
        {
            return Context.PickupPoint.Find(pointID)?.Address ?? "Неизвестный адрес";
        }

        public static List<string> GetProductInOrder(int orderID)
        {
            return (from op in Context.OrderProduct
                    join p in Context.Product on op.ProductId equals p.Id
                    where op.OrderId == orderID
                    select p.Name).ToList();
        }
    }
}

