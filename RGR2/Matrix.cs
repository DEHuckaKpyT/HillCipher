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
            StringBuilder tempStringBuilder = new StringBuilder();
            string tempString;
            string letters = "abcdefghijklmnopqrstuvwxyz";
            using (StreamWriter streamWriter = new StreamWriter($"EncryptedWords\\{number + 1}.{word}.txt", false, Encoding.ASCII))
            {
                foreach (var enWordFromDictionary in GetLenghtWords(word.Length))
                {
                    List<int[,]> matrixes = GetMatrixes(enWordFromDictionary);
                    Console.WriteLine(enWordFromDictionary);
                    foreach (int[,] matrix in allMatrixes)
                    {
                        tempStringBuilder.Clear();

                        foreach (var i in matrixes)
                        {
                            int[,] c = MatrixMultiplication(matrix, i);
                            tempStringBuilder.Append(Convert.ToChar((c[0, 0] % 26) + 'a'));
                            tempStringBuilder.Append(Convert.ToChar((c[1, 0] % 26) + 'a'));
                            if (!word.Substring(0, tempStringBuilder.Length).Equals(tempStringBuilder.ToString())) break;
                        }

                        tempString = tempStringBuilder.ToString();
                        if (word.Equals(tempString))
                        {
                            Console.WriteLine($"{enWordFromDictionary} {matrix[0, 0]} {matrix[0, 1]} {matrix[1, 0]} {matrix[1, 1]}");
                            streamWriter.WriteLine($"{enWordFromDictionary} {matrix[0, 0]} {matrix[0, 1]} {matrix[1, 0]} {matrix[1, 1]}");
                        }
                    }
                }
                foreach (var enWordFromDictionary in GetLenghtWords(word.Length - 1))
                {
                    foreach (var letter in letters)
                    {
                        string curEnWordFromDictionary = enWordFromDictionary + letter;
                        List<int[,]> matrixes = GetMatrixes(curEnWordFromDictionary);
                        Console.WriteLine(curEnWordFromDictionary);
                        foreach (int[,] matrix in allMatrixes)
                        {
                            tempStringBuilder.Clear();

                            foreach (var i in matrixes)
                            {
                                int[,] c = MatrixMultiplication(matrix, i);
                                tempStringBuilder.Append(Convert.ToChar((c[0, 0] % 26) + 'a'));
                                tempStringBuilder.Append(Convert.ToChar((c[1, 0] % 26) + 'a'));
                                if (!word.Substring(0, tempStringBuilder.Length).Equals(tempStringBuilder.ToString())) break;
                            }

                            tempString = tempStringBuilder.ToString();
                            if (word.Equals(tempString))
                            {
                                Console.WriteLine($"{enWordFromDictionary} {matrix[0, 0]} {matrix[0, 1]} {matrix[1, 0]} {matrix[1, 1]}");
                                streamWriter.WriteLine($"{enWordFromDictionary} {matrix[0, 0]} {matrix[0, 1]} {matrix[1, 0]} {matrix[1, 1]}");
                            }
                        }
                    }
                }
            }
            ev.Set();
        }
        public static List<string> GetLenghtWords(int lenght)
        {
            List<string> words = new List<string>();
            using (StreamReader streamReader = new StreamReader($"sources\\temp\\Lenght{lenght}.txt"))
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
                wordsList.Add(new int[2, 1] { { word[i] - 97 },{ word[i + 1] - 97 } });//записываем коды букв

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
    static class MatrixExt
    {
        // метод расширения для получения количества строк матрицы
        public static int RowsCount(this int[,] matrix)
        {
            return matrix.GetUpperBound(0) + 1;
        }

        // метод расширения для получения количества столбцов матрицы
        public static int ColumnsCount(this int[,] matrix)
        {
            return matrix.GetUpperBound(1) + 1;
        }
    }
}
