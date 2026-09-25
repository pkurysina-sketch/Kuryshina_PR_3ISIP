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
            int e = -1;
            do
            {
                Console.WriteLine("Выберите операцию: ");
                Console.WriteLine("1. Ввод строки(Обязательно в первую очередь!);");
                Console.WriteLine("2. Подсчёт количества слов в тексте;");
                Console.WriteLine("3. Поиск самого короткого слова;");
                Console.WriteLine("4. Подсчёт количества предложений;");
                Console.WriteLine("5. Подсчёт количества гласных и согласных букв;");
                Console.WriteLine("6. Поиск самого длинного слова;");
                Console.WriteLine("7. Создание статистики по частоте встречаемости каждой буквы;");
                Console.WriteLine("8. Сохраненить статистику(7);");
                Console.WriteLine("9. Вывод статистики(7);");
                Console.WriteLine("0. Выход.");
                e = Convert.ToInt32(Console.ReadLine());
                switch (e)
                {
                    case 1:
                        Console.WriteLine("Введите строку (не меньше 100 символов)");
                        string line = Console.ReadLine();
                        if (line.Length < 101)
                        {
                            Console.WriteLine("Ошибка ваша страка слишком маленькая!(меньше 100 символов) Введите строку заново!");
                            line = " ";
                        }
                        break;
                }
            } while (e != 0);
        }
    }
}
