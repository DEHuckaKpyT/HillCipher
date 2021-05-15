using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGR2
{
    class MatrixBySearch2x2 : IMatrixService
    {
        public int MaxValue { get; }
        public MatrixBySearch2x2(int maxValue)
        {
            MaxValue = maxValue;
        }
        public List<int[,]> GetMatrixes()
        {
            List<int[,]> matrixes = new List<int[,]>();
            for (int i1 = 0; i1 <= MaxValue; i1++)
                for (int i2 = 0; i2 <= MaxValue; i2++)
                    for (int i3 = 0; i3 <= MaxValue; i3++)
                        for (int i4 = 0; i4 <= MaxValue; i4++)
                            matrixes.Add(new int[2, 2] { { i1, i2 }, { i3, i4 } });

            return matrixes;
        }
    }
}
