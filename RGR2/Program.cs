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

        static void Main(string[] args)
        {
            CommitNowTime();

            Decrypter decrypter = new DecrypterWithStreams(StreamsCount,
                new TextFileReader(StartWords).ReadStrings().ToArray(),
                new TextFileReader(Dictionary).ReadStrings(),
                new MatrixByFile2x2(new TextFileReader(Matrixes)).GetMatrixes(),
                "EncryptedWords",
                new FileProcessing("EncryptedWords", "EncryptedStructedWords", "Total"));

            decrypter.Decrypt();

            Console.WriteLine("End");
            CommitNowTime();
            ConsoleKeyInfo key = Console.ReadKey();
            while (key.Key != ConsoleKey.Escape)
                key = Console.ReadKey();
        }
        static void CommitNowTime()
        {
            Console.WriteLine(DateTime.Now);
            using (StreamWriter stream = new StreamWriter("Time.txt", true))
                stream.WriteLine(DateTime.Now);
        }
    }
}
