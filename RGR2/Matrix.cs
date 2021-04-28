using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RGR2
{
    class Matrix
    {
        ManualResetEvent ev;
        int number;
        string word;
        public static string[] dicWords;
        public static List<int[,]> allMatrixes;
        public Matrix(ManualResetEvent eve, int num, string wrd)
        {
            ev = eve;
            number = num;
            word = wrd;
        }
        public void CheckWord()
        {
            Console.WriteLine($"{number + 1}.{word}");
            string[] dictionaryEVEN = GetLenghtWordsAndSort(word.Length);
            Directory.CreateDirectory("EncryptedWords");
            using (StreamWriter streamWriter = new StreamWriter($"EncryptedWords\\{number + 1}.{word}.txt", false, Encoding.ASCII))
            {
                List<int[,]> wordMatrixes = GetMatrixesFromCryptWord(word);
                foreach (int[,] matrix in allMatrixes)
                {
                    string testWord = "";

                    foreach (var wordMatrix in wordMatrixes)
                    {
                        int[,] c = MatrixMultiplication(wordMatrix, matrix);
                        testWord += (char)((c[0, 0] % 26) + 97);
                        testWord += (char)((c[0, 1] % 26) + 97);
                    }

                    if (Array.BinarySearch(dictionaryEVEN, testWord) >=0 )
                    {
                        Console.WriteLine($"{testWord} {matrix[0, 0]} {matrix[0, 1]} {matrix[1, 0]} {matrix[1, 1]}");
                        streamWriter.WriteLine($"{testWord} {matrix[0, 0]} {matrix[0, 1]} {matrix[1, 0]} {matrix[1, 1]}");
                    }
                }
            }
            ev.Set();
        }
        public static string[] GetLenghtWordsAndSort(int lenght)
        {
            List<string> words = new List<string>();
            using (StreamReader streamReader = new StreamReader($"temp\\Lenght{lenght}.txt"))
                while (!streamReader.EndOfStream)
                    words.Add(streamReader.ReadLine().ToLower());
            string[] resultWords = words.ToArray();
            Array.Sort(resultWords);
            return resultWords;
        }
        public static string[] GetDicWordsAndSort(string path)
        {
            List<string> words = new List<string>();
            using (StreamReader streamReader = new StreamReader(path))
                while (!streamReader.EndOfStream)
                    words.Add(streamReader.ReadLine().ToLower());

            string[] resultWords = words.ToArray();
            Array.Sort(resultWords);
            return resultWords;
        }
        public static string[] GetStartWords(string path)
        {
            List<string> startWords = new List<string>();
            using (StreamReader stream = new StreamReader(path))
                while (!stream.EndOfStream)
                    startWords.Add(stream.ReadLine());
            return startWords.ToArray();
        }
        public static List<int[,]> GetMatrixesFromCryptWord(string word)
        {
            List<int[,]> wordsList = new List<int[,]>();//список матриц 1х2

            for (int i = 0; i < word.Length; i += 2)//идём по каждой паре букв в слове
                wordsList.Add(new int[1, 2] { { word[i] - 97, word[i + 1] - 97 } });//записываем коды букв

            return wordsList;
        }
        public static int[,] MatrixMultiplication(int[,] matrixA, int[,] matrixB)
        {
            int[,] matrixC = new int[1, 2];

            for (int i = 0; i < 1; i++)
                for (int j = 0; j < 2; j++)
                {
                    matrixC[i, j] = 0;
                    for (int k = 0; k < 2; k++)
                        matrixC[i, j] += matrixA[i, k] * matrixB[k, j];
                }

            return matrixC;
        }
    }
}
