using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGR2
{
    //TODO доделать этот класс
    class FileProcessing : IAnswerProcessing
    {
        public string PathDirectoryEncryptedWords { get; private set; }
        public string PathDirectoryEncryptedStructuredWords { get; private set; }
        public string PathTotalDirectory { get; private set; }

        public FileProcessing(string pathDirectoryEncryptedWords, string pathDirectoryEncryptedStructuredWords, string totalDirectory)
        {
            PathDirectoryEncryptedWords = pathDirectoryEncryptedWords;
            PathDirectoryEncryptedStructuredWords = pathDirectoryEncryptedStructuredWords;
            PathTotalDirectory = totalDirectory;
        }
        public void StructResult()
        {
            var files = new DirectoryInfo(PathDirectoryEncryptedWords).GetFiles()
                .OrderBy(x => int.Parse(x.Name.Substring(0, x.Name.IndexOf('.'))));

            List<string> finalEncryptdeWordsToColumn = new List<string>();
            List<string> finalEncryptdeWordsToRow = new List<string>();

            foreach (var file in files)
                RefactorFiles(finalEncryptdeWordsToColumn, finalEncryptdeWordsToRow, file.FullName, file.Name);

            Directory.CreateDirectory(PathTotalDirectory);
            new TextFileWriterToColumn(PathTotalDirectory + "\\FinalEncryptdeWordsToColumn.txt").WriteStrings(finalEncryptdeWordsToColumn);
            new TextFileWriterToColumn(PathTotalDirectory + "\\finalEncryptdeWordsToRow.txt").WriteStrings(finalEncryptdeWordsToRow);
        }
        void RefactorFiles(List<string> finalEncryptdeWordsToColumn, List<string> finalEncryptdeWordsToRow, 
            string filePath, string fileName)
        {
            List<string> encryptedStructuredWords = new List<string>();
            SortedDictionary<string, string> dictionaryWihtStructedWords = 
                GetDictionaryWihtStructedWords(new TextFileReader(filePath).ReadStrings());

            finalEncryptdeWordsToColumn.Add(fileName.Replace(".txt", ""));
            finalEncryptdeWordsToRow.Add(fileName.Replace(".txt", " "));
            foreach (var pairStructedWord in dictionaryWihtStructedWords)
            {
                encryptedStructuredWords.Add(pairStructedWord.Key + " " + pairStructedWord.Value);
                finalEncryptdeWordsToColumn.Add(pairStructedWord.Key);
                finalEncryptdeWordsToRow.Add(pairStructedWord.Key + " ");
            }

            Directory.CreateDirectory(PathDirectoryEncryptedStructuredWords);
            new TextFileWriterToColumn(PathDirectoryEncryptedStructuredWords + $"\\{fileName}").WriteStrings(encryptedStructuredWords);
        }

        SortedDictionary<string, string> GetDictionaryWihtStructedWords(List<string> encryptedWordsWithMatrixes)
        {
            SortedDictionary<string, string> dictionaryWihtStructedWords = new SortedDictionary<string, string>();
            foreach (var encryptedWordWithMatrix in encryptedWordsWithMatrixes)
            {
                string[] encryptedWordWithMatrixSplited = encryptedWordWithMatrix.Split(' ');
                if (!dictionaryWihtStructedWords.ContainsKey(encryptedWordWithMatrixSplited[0]))
                    dictionaryWihtStructedWords.Add(encryptedWordWithMatrixSplited[0], 
                        "{" + $"{encryptedWordWithMatrixSplited[1]} {encryptedWordWithMatrixSplited[2]} {encryptedWordWithMatrixSplited[3]} {encryptedWordWithMatrixSplited[4]}" + "}");
                else
                    dictionaryWihtStructedWords[encryptedWordWithMatrixSplited[0]] = 
                        dictionaryWihtStructedWords[encryptedWordWithMatrixSplited[0]]
                        .Replace("}", "") + ", " + 
                        $"{encryptedWordWithMatrixSplited[1]} {encryptedWordWithMatrixSplited[2]} {encryptedWordWithMatrixSplited[3]} {encryptedWordWithMatrixSplited[4]}" + "}";
            }
            return dictionaryWihtStructedWords;
        }
    }
}
