using System;
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
            Dictionary<int, string> oper = new Dictionary<int, string>();
            Console.WriteLine("Введите количество операций от 2 до 40: ");
            int sum = 0;
            int max = 0;
            int min = 1000;
            int s = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < s; i++)
            {
                if (s < 2 || s > 40)
                {
                    Console.WriteLine("Ошибка колоичество операций не может быть меньше 2 и больше 40!!!");
                    break;
                }
                Console.WriteLine("Введите название Операции: ");
                string text = Console.ReadLine();
                string[] words = text.Split(';');
                int number2;
                string word = (words[0]).ToString();
                bool result2 = int.TryParse(words[1], out number2);
                if (max < number2)
                {
                    max = number2;
                }
                if (min > number2)
                {
                    min = number2;
                }
                oper.Add(number2, word);
                sum += number2;
            }
            int w = -1;
            do
            {
                Console.WriteLine("Выберите задачу:");
                Console.WriteLine("1.Вывод данных");
                Console.WriteLine("2.Статистика (среднее, максимальное, минимальное, сумма)");
                Console.WriteLine("3.Сортировка по цене (пузырьковая сортировка)");
                Console.WriteLine("4.Конвертация валюты (пользователь вводит курс или выбирает из списка)");
                Console.WriteLine("5. Поиск по названию");
                Console.WriteLine("0. Выход");
                w = Convert.ToInt32(Console.ReadLine());
                switch (w)
                {
                    case 1:
                        foreach (var person in oper)
                        {
                            Console.WriteLine(person);
                        }
                        break;
                    case 2:
                        Console.WriteLine($"Сред. Знач.: {sum / s}");
                        Console.WriteLine($"Макс. Знач.: {max}");
                        Console.WriteLine($"Мин. Знач.: {min}");
                        Console.WriteLine($"Сум. Знач.: {sum}");
                        break;
                    case 3:
                        List<int> hz = new List<int>();
                        foreach (var gg in oper)
                        {
                            hz.Add(gg.Key);
                        }
                        for (int i = 0; i < hz.Count - 1; i++)
                        {
                            for (int j = 0; j < hz.Count - i - 1; j++)
                            {
                                if (hz[j] > hz[j + 1])
                                {
                                    int temp = hz[j];
                                    hz[j] = hz[j + 1];
                                    hz[j + 1] = temp;
                                }
                            }
                        }

                        Dictionary<int, string> oper1 = new Dictionary<int, string>();
                        for (int i = 0; i < hz.Count; i++)
                        {
                            oper1.Add(hz[i], oper[hz[i]]);
                        }
                        oper = oper1;
                        Console.WriteLine("Отсортировано.");
                        break;
                    case 4:
                        Console.WriteLine("Выберите валюту:");
                        Console.WriteLine("1. USD");
                        Console.WriteLine("2. EUR");
                        Console.WriteLine("3. CNY");
                        Console.WriteLine("4. Свой курс");
                        int cur = Convert.ToInt32(Console.ReadLine());
                        double rate = 0;
                        string curName = "";
                        if (cur == 1) { rate = 90; curName = "USD"; }
                        else if (cur == 2) { rate = 100; curName = "EUR"; }
                        else if (cur == 3) { rate = 12.5; curName = "CNY"; }
                        else if (cur == 4)
                        {
                            Console.Write("Введите курс: ");
                            rate = Convert.ToDouble(Console.ReadLine());
                            Console.Write("Введите название валюты: ");
                            curName = Console.ReadLine();
                        }
                        if (rate > 0)
                        {
                            foreach (var person in oper)
                            {
                                Console.WriteLine($"{person.Value} — {(person.Key / rate):F2} {curName}");
                            }
                        }
                        break;
                    case 5:
                        Console.Write("Введите название для поиска: ");
                        string search = Console.ReadLine().ToLower();

                        bool found = false;
                        foreach (var person in oper)
                        {
                            if (person.Value.ToLower().Contains(search))
                            {
                                Console.WriteLine($"{person.Value} — {person.Key} руб.");
                                found = true;
                            }
                        }
                        if (!found) Console.WriteLine("Ничего не найдено.");
                        break;
                    default:
                        Console.WriteLine("Такой команды нет.");
                        break;
                }

            } while (w != 0);
        }

    }
}
