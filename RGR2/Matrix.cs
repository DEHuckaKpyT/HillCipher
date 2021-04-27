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
        public Matrix(ManualResetEvent eve, int num, string wrd)
        {
            ev = eve;
            number = num;
            word = wrd;
        }
        public void CheckWord()
        {
            int[,] a = new int[2, 2] { { 0, 0 }, { 0, 0 } };//по условию создаём матрицу 2х2

            Console.WriteLine($"{number + 1}.{word}");
            Directory.CreateDirectory("EncryptedWords");
            using (StreamWriter streamWriter = new StreamWriter($"EncryptedWords\\{number + 1}.{word}.txt", false, Encoding.ASCII))//для записи найденных слов
            {
                foreach (var enWord in GetLenghtWords(word.Length))//перебираем по одному расшифрованному слову
                {
                    List<int[,]> matrixes = GetMatrixes(enWord);//получаем матрицы 1х2
                    Console.WriteLine(enWord);
                    for (int i1 = 0; i1 < 26; i1++)//перебор всех матриц 2х2 с числами от 0 до 25
                        for (int i2 = 0; i2 < 26; i2++)
                            for (int i3 = 0; i3 < 26; i3++)
                                for (int i4 = 0; i4 < 26; i4++)
                                {
                                    a[0, 0] = i1;
                                    a[0, 1] = i2;
                                    a[1, 0] = i3;
                                    a[1, 1] = i4;
                                    string testWord = "";//для записи в него текущего варианта зашифрованного слова

                                    foreach (var i in matrixes)//идём по каждой матрице 1х2
                                    {
                                        int[,] c = MatrixMultiplication(i, a);//перемножаем матрицы 2х2 и 1х2
                                        testWord += (char)((c[0, 0] % 26) + 97);//получаем два зашифрованных символа
                                        testWord += (char)((c[0, 1] % 26) + 97);//записываем их в текущее зашифрованное слово
                                        if (!word.Substring(0, testWord.Length).Equals(testWord)) break;//если начало шифрограммы не совпадает с началом текущего зашифрованного слова, то берём следущую матрицу
                                    }
                                    if (word.Equals(testWord))//если слова одинаковвые, то записываем
                                    {
                                        Console.WriteLine($"{enWord} {i1} {i2} {i3} {i4}");
                                        streamWriter.WriteLine($"{enWord} {i1} {i2} {i3} {i4}");
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
        public static string[] GetDicWords(string path)//метод для считывания всех слов словаря
        {
            List<string> words = new List<string>();
            using (StreamReader streamReader = new StreamReader(path))
                while (!streamReader.EndOfStream)
                    words.Add(streamReader.ReadLine().ToLower());
            return words.ToArray();
        }
        public static string[] GetStartWords(string path)//метод для считывания с файла зашифрованных слов
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
