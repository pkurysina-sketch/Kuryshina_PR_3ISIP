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
                        int min1 = 1000;
                        List<int> hz = new List<int>();
                        foreach (var gg in oper)
                        {
                            hz.Add(gg.Key);
                        }
                        Dictionary<int, string> oper1 = new Dictionary<int, string>();
                        for (int i = 0; i < s - 1; i++)
                        {

                            for (int j = 0; j < s - i - 1; j++)
                            {
                                if (hz[j] > hz[j + 1])
                                {
                                    int temp = hz[j];
                                    hz[j] = hz[j + 1];
                                    hz[j + 1] = temp;
                                }
                            }
                        }
                        for (int i = 0; i < s - 1; i++)
                        {
                            string gf;
                            if (oper.TryGetValue(hz[i], out gf))
                                oper1.Add(hz[i], gf);
                        }
                            break;
                        case 4:

                                break;
                            case 5:
                                break;
                            default:
                                break;
                            }

                        } while (w != 0) ;
                }

            }
}
   