using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGR2
{
    static class Matrix
    {
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
