using System;
using System.Collections.Generic;
using System.Text;

namespace TextAnalyzer
{
    // Класс для хранения статистики по одному тексту
    class TextStatistics
    {
        public string OriginalText { get; set; }
        public int WordCount { get; set; }
        public string ShortestWord { get; set; }
        public string LongestWord { get; set; }
        public int SentenceCount { get; set; }
        public int VowelCount { get; set; }
        public int ConsonantCount { get; set; }
        public Dictionary<char, int> LetterFrequency { get; set; }

        public TextStatistics()
        {
            LetterFrequency = new Dictionary<char, int>();
        }
    }

    class Program
    {
        // Список для хранения статистики по прошлым текстам
        static List<TextStatistics> history = new List<TextStatistics>();

        static void Main(string[] args)
        {
            string line = "";
            int choice = -1;

            do
            {
                Console.WriteLine("Выберите операцию:");
                Console.WriteLine("1. Ввести текст (минимум 100 символов)");
                Console.WriteLine("2. Подсчёт количества слов");
                Console.WriteLine("3. Поиск самого короткого слова");
                Console.WriteLine("4. Подсчёт количества предложений");
                Console.WriteLine("5. Подсчёт количества гласных и согласных букв");
                Console.WriteLine("6. Поиск самого длинного слова");
                Console.WriteLine("7. Создание статистики по частоте встречаемости каждой буквы");
                Console.WriteLine("8. Сохранить текущую статистику в список");
                Console.WriteLine("9. Вывести статистику по прошлым текстам");
                Console.WriteLine("0. Выход");
                Console.Write("Ваш выбор: ");

                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Ошибка ввода! Введите число.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        while (true)
                        {
                            Console.WriteLine("Введите строку (не меньше 100 символов):");
                            line = Console.ReadLine();
                            if (line != null && line.Length >= 100)
                            {
                                break;
                            }
                            Console.WriteLine("Ошибка: ваша строка слишком маленькая (меньше 100 символов). Введите строку заново!");
                        }
                        break;
                    case 2:
                        if (string.IsNullOrEmpty(line))
                        {
                            Console.WriteLine("Сначала введите текст (пункт 1).");
                            break;
                        }
                        char[] separators = { ' ', '.', ',', '!', '?', ';', ':', '-', '\n', '\r', '\t' };
                        string[] words = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        Console.WriteLine($"Количество слов: {words.Length}");
                        break;

                    case 3:
                        if (string.IsNullOrEmpty(line))
                        {
                            Console.WriteLine("Сначала введите текст (пункт 1).");
                            break;
                        }
                        char[] sep3 = { ' ', '.', ',', '!', '?', ';', ':', '-', '\n', '\r', '\t' };
                        string[] words3 = line.Split(sep3, StringSplitOptions.RemoveEmptyEntries);
                        if (words3.Length > 0)
                        {
                            string shortest = words3[0];
                            foreach (string w in words3)
                            {
                                if (w.Length < shortest.Length) shortest = w;
                            }
                            Console.WriteLine($"Самое короткое слово: '{shortest}' ({shortest.Length} симв.)");
                        }
                        break;
                    case 4:
                        if (string.IsNullOrEmpty(line))
                        {
                            Console.WriteLine("Сначала введите текст (пункт 1).");
                            break;
                        }
                        char[] sep4 = { '.', '!', '?' };
                        string[] sentence = line.Split(sep4, StringSplitOptions.RemoveEmptyEntries);
                        Console.WriteLine($"Количество предложений: {sentence.Length}");
                        break;
                    case 5:
                        if (string.IsNullOrEmpty(line))
                        {
                            Console.WriteLine("Сначала введите текст (пункт 1).");
                            break;
                        }
                        int vowels = 0;
                        int consonants = 0;
                        string vowelsStr = "аеёиоуыэюяaeiouy"; // русские и английские гласные
                        foreach (char c in line.ToLower())
                        {
                            if (char.IsLetter(c))
                            {
                                if (vowelsStr.IndexOf(c) >= 0)
                                    vowels++;
                                else
                                    consonants++;
                            }
                        }
                        Console.WriteLine($"Гласных букв: {vowels}");
                        Console.WriteLine($"Согласных букв: {consonants}");
                        break;
                    case 6:
                        if (string.IsNullOrEmpty(line))
                        {
                            Console.WriteLine("Сначала введите текст (пункт 1).");
                            break;
                        }
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
                    case 7:
                        if (string.IsNullOrEmpty(line))
                        {
                            Console.WriteLine("Сначала введите текст (пункт 1).");
                            break;
                        }
                        Dictionary<char, int> frequency = new Dictionary<char, int>();
                        foreach (char c in line.ToLower())
                        {
                            if (char.IsLetter(c))
                            {
                                if (frequency.ContainsKey(c))
                                    frequency[c]++;
                                else
                                    frequency[c] = 1;
                            }
                        }
                        Console.WriteLine("Частота встречаемости букв:");
                        foreach (var pair in frequency)
                        {
                            Console.WriteLine($"'{pair.Key}': {pair.Value}");
                        }
                        break;

                    case 8:
                        if (string.IsNullOrEmpty(line))
                        {
                            Console.WriteLine("Сначала введите текст (пункт 1).");
                            break;
                        }
                        // Создаём объект статистики и сохраняем в список
                        TextStatistics stats = new TextStatistics();
                        stats.OriginalText = line;

                        char[] sepAll = { ' ', '.', ',', '!', '?', ';', ':', '-', '\n', '\r', '\t' };
                        string[] allWords = line.Split(sepAll, StringSplitOptions.RemoveEmptyEntries);
                        stats.WordCount = allWords.Length;

                        if (allWords.Length > 0)
                        {
                            string shortestW = allWords[0];
                            string longestW = allWords[0];
                            foreach (string w in allWords)
                            {
                                if (w.Length < shortestW.Length) shortestW = w;
                                if (w.Length > longestW.Length) longestW = w;
                            }
                            stats.ShortestWord = shortestW;
                            stats.LongestWord = longestW;
                        }

                        char[] sepSent = { '.', '!', '?' };
                        string[] sentences = line.Split(sepSent, StringSplitOptions.RemoveEmptyEntries);
                        stats.SentenceCount = sentences.Length;

                        string vowelsStr2 = "аеёиоуыэюяaeiouy";
                        int vCount = 0, cCount = 0;
                        foreach (char c in line.ToLower())
                        {
                            if (char.IsLetter(c))
                            {
                                if (vowelsStr2.IndexOf(c) >= 0)
                                    vCount++;
                                else
                                    cCount++;
                            }
                        }
                        stats.VowelCount = vCount;
                        stats.ConsonantCount = cCount;

                        foreach (char c in line.ToLower())
                        {
                            if (char.IsLetter(c))
                            {
                                if (stats.LetterFrequency.ContainsKey(c))
                                    stats.LetterFrequency[c]++;
                                else
                                    stats.LetterFrequency[c] = 1;
                            }
                        }

                        history.Add(stats);
                        Console.WriteLine("Статистика сохранена в список.");
                        break;

                    case 9:
                        if (history.Count == 0)
                        {
                            Console.WriteLine("Список статистики пуст.");
                            break;
                        }
                        Console.WriteLine($"\nВсего сохранённых текстов: {history.Count}");
                        for (int i = 0; i < history.Count; i++)
                        {
                            TextStatistics s = history[i];
                            Console.WriteLine($"Текст №{i + 1}");
                            Console.WriteLine($"Исходный текст: {s.OriginalText}");
                            Console.WriteLine($"Количество слов: {s.WordCount}");
                            Console.WriteLine($"Самое короткое слово: '{s.ShortestWord}'");
                            Console.WriteLine($"Самое длинное слово: '{s.LongestWord}'");
                            Console.WriteLine($"Количество предложений: {s.SentenceCount}");
                            Console.WriteLine($"Гласных: {s.VowelCount}, Согласных: {s.ConsonantCount}");
                            Console.WriteLine("Частота букв:");
                            foreach (var pair in s.LetterFrequency)
                            {
                                Console.WriteLine($"{pair.Key}: {pair.Value}");
                            }
                        }
                        break;

                    case 0:
                        Console.WriteLine("Выход из программы.");
                        break;

                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            } while (choice != 0);
        }
    }
}
