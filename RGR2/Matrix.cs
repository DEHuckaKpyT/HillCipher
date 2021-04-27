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
            Directory.CreateDirectory("EncryptedWords");
            using (StreamWriter streamWriter = new StreamWriter($"EncryptedWords\\{number + 1}.{word}.txt", false, Encoding.ASCII))
            {
                foreach (var enWord in GetLenghtWords(word.Length))
                {
                    List<int[,]> matrixes = GetMatrixes(enWord);
                    Console.WriteLine(enWord);
                    foreach (int[,] matrix in allMatrixes)
                    {
                        string testWord = "";

                        foreach (var i in matrixes)
                        {
                            int[,] c = MatrixMultiplication(i, matrix);
                            testWord += (char)((c[0, 0] % 26) + 97);
                            testWord += (char)((c[0, 1] % 26) + 97);
                            if (!word.Substring(0, testWord.Length).Equals(testWord)) break;
                        }
                        if (word.Equals(testWord))
                        {
                            Console.WriteLine($"{enWord} {matrix[0,0]} {matrix[0, 1]} {matrix[1, 0]} {matrix[1, 1]}");
                            streamWriter.WriteLine($"{enWord} {matrix[0, 0]} {matrix[0, 1]} {matrix[1, 0]} {matrix[1, 1]}");
                        }
                    }
                }
            }
            ev.Set();
        }
        public static List<string> GetLenghtWords(int lenght)
        {
            List<string> words = new List<string>();
            using (StreamReader streamReader = new StreamReader($"temp\\Lenght{lenght}.txt"))
                while (!streamReader.EndOfStream)
                    words.Add(streamReader.ReadLine().ToLower());
            return words;
        }
        public static string[] GetDicWords(string path)
        {
            List<string> words = new List<string>();
            using (StreamReader streamReader = new StreamReader(path))
                while (!streamReader.EndOfStream)
                    words.Add(streamReader.ReadLine().ToLower());
            return words.ToArray();
        }
        public static string[] GetStartWords(string path)
        {
            List<string> startWords = new List<string>();
            using (StreamReader stream = new StreamReader(path))
                while (!stream.EndOfStream)
                    startWords.Add(stream.ReadLine());
            return startWords.ToArray();
        }
        public static List<int[,]> GetMatrixes(string word)
        {
            List<int[,]> wordsList = new List<int[,]>();//список матриц 1х2

            for (int i = 0; i < word.Length; i += 2)//идём по каждой паре букв в слове
                wordsList.Add(new int[1, 2] { { word[i] - 97, word[i + 1] - 97 } });//записываем коды букв

            return wordsList;
        }
        public static int[,] MatrixMultiplication(int[,] matrixA, int[,] matrixB)
        {
            if (matrixA.ColumnsCount() != matrixB.RowsCount())
            {
                throw new Exception("Умножение не возможно! Количество столбцов первой матрицы не равно количеству строк второй матрицы.");
            }

            var matrixC = new int[matrixA.RowsCount(), matrixB.ColumnsCount()];

            for (var i = 0; i < matrixA.RowsCount(); i++)
            {
                for (var j = 0; j < matrixB.ColumnsCount(); j++)
                {
                    matrixC[i, j] = 0;

                    for (var k = 0; k < matrixA.ColumnsCount(); k++)
                    {
                        matrixC[i, j] += matrixA[i, k] * matrixB[k, j];
                    }
                }
            }
            return matrixC;
        }
    }
}
