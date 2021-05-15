using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGR2
{
    class MatrixByFile2x2 : IMatrixService
    {
        public IReaderService Reader { get; }
        public MatrixByFile2x2(IReaderService reader)
        {
            Reader = reader;
        }
        public List<int[,]> GetMatrixes()
        {
            List<int[,]> matrixes = new List<int[,]>();
            string[] tempStrings;
            int[,] tempMatrix;

            foreach (string str in Reader.ReadStrings())
            {
                tempStrings = str.Split(' ');
                tempMatrix = new int[2, 2]
                {
                        { int.Parse(tempStrings[0]), int.Parse(tempStrings[1]) },
                        { int.Parse(tempStrings[2]), int.Parse(tempStrings[3]) }
                };
                matrixes.Add(tempMatrix);
            }

            return matrixes;
        }
    }
}
