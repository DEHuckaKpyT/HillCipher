﻿using System;
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
            CommitNowTime();

            DecryptHaHa(StreamsCount, StartWords, Matrixes, Dictionary);

            FileProcessing.RefactorResultFiles();

            Console.WriteLine("End");
            CommitNowTime();
            ConsoleKeyInfo key = Console.ReadKey();
            while (key.Key != ConsoleKey.Escape)
                key = Console.ReadKey();
        }
        static void DecryptHaHa(int streamsCount, string pathStartWords, string pathAllMatrixes, string pathDictionary)
        {
            IReaderService readerService = new TextFileReader(pathStartWords);
            string[] startWords = readerService.ReadStrings().ToArray();
            streamsCount = streamsCount < startWords.Length ? streamsCount : startWords.Length;
            events = new ManualResetEvent[streamsCount];
            IMatrixService matrixService = new MatrixByFile(new TextFileReader(pathAllMatrixes));
            Dictionary<int, List<string>> allLengthDictionaries = GetSortedDictionariesForLength(pathDictionary);

            for (int i = 0; i < streamsCount; i++)
            {
                events[i] = new ManualResetEvent(false);
                ProcessThreadGroundService treadGrond = new ProcessThreadGroundFastWay(events[i], i, startWords[i],
                    matrixService.GetMatrixes(), 
                    allLengthDictionaries[startWords[i].Length].ToArray(),
                    allLengthDictionaries[startWords[i].Length - 1].ToArray());
                new Thread(new ThreadStart(treadGrond.TryToDecryptWord)).Start();
            }

            for (int i = streamsCount; i < startWords.Length; i++)
            {
                int numb = WaitHandle.WaitAny(events);
                events[numb].Reset();
                ProcessThreadGroundService treadGrond = new ProcessThreadGroundFastWay(events[numb], i, startWords[i],
                    matrixService.GetMatrixes(),
                    allLengthDictionaries[startWords[i].Length].ToArray(),
                    allLengthDictionaries[startWords[i].Length - 1].ToArray());
                new Thread(new ThreadStart(treadGrond.TryToDecryptWord)).Start();
            }

            WaitHandle.WaitAll(events);
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
