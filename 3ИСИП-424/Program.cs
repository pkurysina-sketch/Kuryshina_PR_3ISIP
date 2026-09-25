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
            string line = "";
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
                        line = Console.ReadLine();
                        if (line.Length < 101)
                        {
                            Console.WriteLine("Ошибка ваша страка слишком маленькая!(меньше 100 символов) Введите строку заново!");
                            line = " ";
                        }
                        break;
                    case 2:
                        char[] separators = { ' ', '.', ',', '!', '?', ';', ':', '-', '\n', '\r', '\t' };
                        string[] words = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        Console.WriteLine($"Количество слов: {words.Length}");
                        break;
                    case 3:
                        char[] sep3 = { ' ', '.', ',', '!', '?', ';', ':', '-', '\n', '\r', '\t' };
                        string[] words3 = line.Split(sep3, StringSplitOptions.RemoveEmptyEntries);
                        if (words3.Length > 0)
                        {
                            string shortest = words3[0];
                            foreach (string w in words3)
                                if (w.Length < shortest.Length) shortest = w;
                            Console.WriteLine($"Самое короткое слово: '{shortest}' ({shortest.Length} симв.)");
                        }
                        break;
                    case 4:
                        char[] sep4 = { '.', '!', '?'};
                        string[] sentence = line.Split(sep4, StringSplitOptions.RemoveEmptyEntries);
                        Console.WriteLine($"Количество предложений: {sentence.Length}");
                        break;
                    case 5:
                        break;
                    case 6:
                        char[] sep6 = { ' ', '.', ',', '!', '?', ';', ':', '-', '\n', '\r', '\t' };
                        string[] words6 = line.Split(sep6, StringSplitOptions.RemoveEmptyEntries);
                        if (words6.Length > 0)
                        {
                            string longest = words6[0];
                            foreach (string w in words6)
                                if (w.Length > longest.Length) longest = w;
                            Console.WriteLine($"Самое длинное слово: '{longest}' ({longest.Length} симв.)");
                        }
                        break;
                }
            } while (e != 0);
        }
    }
}
