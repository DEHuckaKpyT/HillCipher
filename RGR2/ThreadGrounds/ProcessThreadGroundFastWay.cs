using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RGR2
{
    class ProcessThreadGroundFastWay : ProcessThreadGroundService
    {
        public ProcessThreadGroundFastWay(ManualResetEvent resetEvent, int num, string word, List<int[,]> allMatrixes,
            string[] evenDictionary, string[] oddDictionary, string pathDirectoryEncryptedWords)
        {
            ResetEvent = resetEvent;
            Number = num;
            Word = word;
            AllMatrixes = allMatrixes;
            DictionaryEvenLength = evenDictionary;
            DictionaryOddLength = oddDictionary;
            PathDirectoryEncryptedWords = pathDirectoryEncryptedWords;
        }

        public override void TryToDecryptWord()
        {
            Console.WriteLine($"{Number + 1}.{Word}");
            List<string> found = FindWords();

            Directory.CreateDirectory(PathDirectoryEncryptedWords);
            IWriterService writerService = new TextFileWriterToColumn(PathDirectoryEncryptedWords + $"\\{Number + 1}.{Word}.txt");
            writerService.WriteStrings(found);

            ResetEvent.Set();
        }

        void CheckTempWord(string[] dictionary, List<string> found, string tempString, int[,] matrix)
        {
            if (Array.BinarySearch(dictionary, tempString) >= 0)
            {
                Console.WriteLine($"{tempString} {matrix[0, 0]} {matrix[0, 1]} {matrix[1, 0]} {matrix[1, 1]}");
                found.Add($"{tempString} {matrix[0, 0]} {matrix[0, 1]} {matrix[1, 0]} {matrix[1, 1]}");
            }
        }

        List<string> FindWords()
        {
            List<string> found = new List<string>();
            StringBuilder tempStringBuilder = new StringBuilder();
            string tempString;

            foreach (int[,] matrix in AllMatrixes)
            {
                tempStringBuilder.Clear();

                foreach (var wordMatrix in new MatrixByWord1x2(Word).GetMatrixes())
                {
                    int[,] c = Matrix.MatrixMultiplication1x2and2x2(wordMatrix, matrix);
                    tempStringBuilder.Append(Convert.ToChar((c[0, 0] % 26) + 'a'));
                    tempStringBuilder.Append(Convert.ToChar((c[0, 1] % 26) + 'a'));
                }
                tempString = tempStringBuilder.ToString();

                CheckTempWord(DictionaryEvenLength, found, tempString, matrix);
                CheckTempWord(DictionaryOddLength, found, tempString.Substring(0, tempString.Length - 1), matrix);
            }

            return found;
        }
    }
}
