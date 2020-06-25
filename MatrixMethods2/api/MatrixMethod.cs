using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MatrixMethods.api
{
    public unsafe abstract class MatrixMethod
    {
        public abstract void WriteToMatrix(int x, int y, int k);
        public abstract int ReadFromMatrix(int x, int y);
        public bool UsePointers = false;
        public int[] data;
        public int size;
        public String name;
        public Stopwatch sw;
        public abstract void StopWatchReset();
        public abstract long GetDataSize();
        public abstract int GetDataCount();
        public int GetObjectSize(object TestObject)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            byte[] Array;
            bf.Serialize(ms, TestObject);
            Array = ms.ToArray();
            return Array.Length;
        }
        public MatrixMethod MatrixMultiplication(MatrixMethod  matrixB)
        {
            if (size!=matrixB.size)
            {
                MessageBox.Show("Неверное кол-во сток в матрицах!");
                return null;
            }
            
            
            Normal matrixC = new Normal(size,false);
            matrixC.StopWatchReset();
            matrixC.sw.Start();
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    matrixC.Fulldata[i, j] = 0;

                    for (var k = 0; k < size; k++)
                    {
                        matrixC.Fulldata[i, j] += this.ReadFromMatrix(i, k) * matrixB.ReadFromMatrix(k, j);
                    }
                }
            }
            matrixC.sw.Stop();
            return matrixC;
        }
        public virtual MatrixMethod Add(MatrixMethod matrixB)
        {
            if (size != matrixB.size)
            {
                MessageBox.Show("Неверное кол-во сток в матрицах!");
                return null;
            }     
            
            Normal matrixC = new Normal(size, false);
            matrixC.StopWatchReset();
            matrixC.sw.Start();
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    matrixC.Fulldata[i, j] = this.ReadFromMatrix(i, j) + matrixB.ReadFromMatrix(i, j);
                }
            }
            matrixC.sw.Stop();
            return matrixC;
        }
        public virtual MatrixMethod Subtract(MatrixMethod matrixB)
        {
            if (size != matrixB.size)
            {
                MessageBox.Show("Неверное кол-во сток в матрицах!");
                return null;
            }
           
            sw.Start();
            Normal matrixC = new Normal(size, false);
            matrixC.StopWatchReset();
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    matrixC.Fulldata[i, j] = this.ReadFromMatrix(i, j) - matrixB.ReadFromMatrix(i, j);
                }
            }
            sw.Stop();
            return matrixC;
        }

    }
}
