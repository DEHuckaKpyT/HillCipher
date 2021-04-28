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
        static ManualResetEvent[] events;
        static void Main(string[] args)
        {
            CommitNowTime();
            Matrix.dicWords = Matrix.GetDicWords("sources\\dic1k.txt");
            SortWords(Matrix.dicWords, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);//здесь пишется длина всех встречающихся слов (это вся)
            Matrix.allMatrixes = GetEncryptMatrixes();

            DecryptHaHa(7, Matrix.GetStartWords("startWords.txt"));//здесь число - количество потоков

            StrucFiles();

            Console.WriteLine("End");
            CommitNowTime();
            ConsoleKeyInfo key = Console.ReadKey();
            while(key.Key!=ConsoleKey.Escape)
                key = Console.ReadKey();
        }
        static void DecryptHaHa(int streamsCount, string[] startWords)
        {
            streamsCount = streamsCount < startWords.Length ? streamsCount : startWords.Length;
            events = new ManualResetEvent[streamsCount];

            for (int i = 0; i < streamsCount; i++)
            {
                events[i] = new ManualResetEvent(false);
                Matrix matrix = new Matrix(events[i], i, startWords[i]);
                new Thread(new ThreadStart(matrix.CheckWord)).Start();
            }

            for (int i = streamsCount; i < startWords.Length; i++)
            {
                int numb = WaitHandle.WaitAny(events);
                events[numb].Reset();
                Matrix matrix = new Matrix(events[numb], i, startWords[i]);
                new Thread(new ThreadStart(matrix.CheckWord)).Start();
            }

            WaitHandle.WaitAll(events);
        }
        static List<int[,]> GetEncryptMatrixes()
        {
            List<int[,]> matrixes = new List<int[,]>();
            string[] tempStrings;
            int[,] tempMatrix;

            using (StreamReader reader = new StreamReader("sources\\EncryptMatrixes.txt"))
                while (!reader.EndOfStream)
                {
                    tempStrings = reader.ReadLine().Split(' ');

                    tempMatrix = new int[2, 2]
                    {
                        { int.Parse(tempStrings[0]), int.Parse(tempStrings[1]) },
                        { int.Parse(tempStrings[2]), int.Parse(tempStrings[3]) }
                    };
                    matrixes.Add(tempMatrix);
                }

            return matrixes;
        }
        static void SortWords(string[] wordsList, params int[] lengths)
        {
            Dictionary<int, List<string>> lenWords = new Dictionary<int, List<string>>();
            int maxLenght = lengths.Max();

            for (int i = 1; i <= maxLenght; i++)
                lenWords.Add(i, new List<string>());

            foreach (string word in wordsList)
                if (word.Length <= maxLenght) lenWords[word.Length].Add(word);

            foreach (var i in lengths)
            {
                lenWords[i].Sort();
                WriteWords(lenWords[i], i);
            }
        }
        static void WriteWords(List<string> words, int len)
        {
            Directory.CreateDirectory("sources\\temp");
            using (StreamWriter streamWriter = new StreamWriter($"sources\\temp\\Lenght{len}.txt", false, Encoding.ASCII))
            {
                foreach (string word in words)
                    streamWriter.WriteLine(word);
            }
        }
        static void CommitNowTime()
        {
            Console.WriteLine(DateTime.Now);
            using (StreamWriter stream = new StreamWriter("Time.txt", true))
                stream.WriteLine(DateTime.Now);
        }
        static void StrucFiles()
        {
            DirectoryInfo dir = new DirectoryInfo("EncryptedWords");
            var files = dir.GetFiles().OrderBy(x => int.Parse(x.Name.Substring(0,x.Name.IndexOf('.'))));

            Directory.CreateDirectory("Total");
            using (StreamWriter stream = new StreamWriter("Total\\EnWords.txt", false))
                foreach (var file in files)
                    RewriteFiles(stream, file.FullName, file.Name);
        }
        static void RewriteFiles(StreamWriter writer, string path, string name)
        {
            List<string> strings = new List<string>();
            using (StreamReader stream = new StreamReader(path))
                while (!stream.EndOfStream)
                    strings.Add(stream.ReadLine());

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            foreach (var str in strings)
            {
                string[] strs = str.Split(' ');
                if (!dic.ContainsKey(strs[0]))
                    dic.Add(strs[0], "{" + $"{strs[1]} {strs[2]} {strs[3]} {strs[4]}" + "}");
                else
                    dic[strs[0]] = dic[strs[0]].Replace("}", "") + ", " + $"{strs[1]} {strs[2]} {strs[3]} {strs[4]}" + "}";
            }

            Directory.CreateDirectory("EncryptedStructuredWords");
            writer.WriteLine(name.Replace(".txt", ""));
            using (StreamWriter stream = new StreamWriter($"EncryptedStructuredWords\\{name}", false))
                foreach (var i in dic)
                {
                    stream.WriteLine(i.Key + " " + i.Value);
                    writer.WriteLine(i.Key);
                }
        }
    }
}
