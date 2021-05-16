using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGR2
{
    class MatrixByWord1x2 : IMatrixService
    {
        public string Word { get; }
        public MatrixByWord1x2(string word)
        {
            Word = word;
        }
        public List<int[,]> GetMatrixes()
        {
            List<int[,]> wordsList = new List<int[,]>();

            for (int i = 0; i < Word.Length; i += 2)
                wordsList.Add(new int[1, 2] { 
                    { Word[i] - 'a', Word[i + 1] - 'a' } 
                });

            return wordsList;
        }
    }
}
