using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using RGR2;

namespace RGR2TEST2
{
    [TestClass]
    class MatrixTest
    {
        [TestMethod]
        public void MatrixMultiplication1x2and2x2Test1()
        {
            int[,] matrixA = new int[1, 2] { { 5, 6 } };
            int[,] matrixB = new int[2, 2] { { 1, 2 }, { 3, 4 } };

            int[,] result = Matrix.MatrixMultiplication1x2and2x2(matrixA, matrixB);

            Assert.AreEqual(result, new int[1, 2] { { 23, 34 } });
        }
    }
}
