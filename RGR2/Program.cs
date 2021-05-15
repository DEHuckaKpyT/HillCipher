using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RGR2
{
    class Program
    {
        static string StartWords = "startWords.txt";//слова, которые нужно расшифровать
        static string Dictionary = "sources\\NotBadDic457k.txt";//словарь
        static string Matrixes = "sources\\DecryptMatrixes.txt";//матрицы
        static int StreamsCount = 3;//количество потоков

        static ManualResetEvent[] events;
        static void Main(string[] args)
        {
            Decrypter decrypter = new DecrypterWithStreams(StreamsCount,
                new TextFileReader(StartWords).ReadStrings().ToArray(),
                GetSortedDictionariesForLength(Dictionary),
                new MatrixByFile2x2(new TextFileReader(Matrixes)).GetMatrixes());
            decrypter.Decrypt();



            FileProcessing.RefactorResultFiles();

            Console.WriteLine("End");
            CommitNowTime();
            ConsoleKeyInfo key = Console.ReadKey();
            while (key.Key != ConsoleKey.Escape)
                key = Console.ReadKey();
        }
        static Dictionary<int, List<string>> GetSortedDictionariesForLength(string path)
        {
            Dictionary<int, List<string>> allLengthDictionaries = new Dictionary<int, List<string>>();
            IReaderService readerService = new TextFileReader(path);

            foreach (string str in readerService.ReadStrings())
            {
                if (!allLengthDictionaries.ContainsKey(str.Length))
                    allLengthDictionaries.Add(str.Length, new List<string>());
                allLengthDictionaries[str.Length].Add(str);
            }
            foreach (List<string> wordsList in allLengthDictionaries.Values)
                wordsList.Sort();
            return allLengthDictionaries;
        }
        static void CommitNowTime()
        {
            Console.WriteLine(DateTime.Now);
            using (StreamWriter stream = new StreamWriter("Time.txt", true))
                stream.WriteLine(DateTime.Now);
        }
        
    }
}
