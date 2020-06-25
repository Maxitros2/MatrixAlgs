using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixMethods.api
{
    class Block : MatrixMethod
    {
       
        public Block(int size, bool pointers)
        {
            this.size = size;
            this.data = new int[3*size-2];
            Debug.WriteLine("data=" + data.Length);
            this.UsePointers = pointers;
        }

        public override int GetDataCount()
        {
            return this.data.Length;
        }

        public override long GetDataSize()
        {
            return GetObjectSize(this.data);
        }

        public override int ReadFromMatrix(int j, int i)
        {

            if (!(i == j || j == i + 1 || j == i - 1) || 2 * j + i - 2 >= data.Length-2)
                return 0;
            sw.Start();
            int d = 2 * j + i - 2;
            Debug.WriteLine("reading " + d);
            if (UsePointers)
            {
                unsafe
                {
                    fixed (int* ptr = data)
                    {
                        int retp = *(ptr + d+2);
                        sw.Stop();
                        return retp;
                    }
                }
            }
            int ret = data[d + 2];
            sw.Stop();          
            return ret;
        }
        public override MatrixMethod Add(MatrixMethod matrix)
        {
            if (matrix.GetType() != typeof(Block))
                base.Add(matrix);
            Block triangle = new Block(size, UsePointers);
            triangle.StopWatchReset();
            triangle.sw.Start();
            for (int i = 0; i < data.Length; i++)
                triangle.data[i] = data[i] + matrix.data[i];
            triangle.sw.Stop();
            return triangle;

        }
        public override MatrixMethod Subtract(MatrixMethod matrix)
        {
            if (matrix.GetType() != typeof(Block))
                base.Add(matrix);
            Block triangle = new Block(size, UsePointers);
            triangle.StopWatchReset();
            triangle.sw.Start();
            for (int i = 0; i < data.Length; i++)
                triangle.data[i] = data[i] - matrix.data[i];
            triangle.sw.Stop();
            return triangle;

        }
        public override void StopWatchReset()
        {
            this.sw = new System.Diagnostics.Stopwatch();
        }

        public override void WriteToMatrix(int i, int j, int k)
        {

            sw.Start();
            if (!(i == j || j == i + 1 || j == i - 1) || 2 * j + i - 2 >= data.Length-2)
                return;
            int d = 2 * j + i - 2;
            Debug.WriteLine("writing to {0} value {1} with i={2} j={3}", d, k, i, j);
            if (UsePointers)
            {
                unsafe
                {
                    fixed (int* ptr = data)
                    {
                        *(ptr + d+2) = k;
                        sw.Stop();
                        return;
                    }
                }
            }
            data[d+2] = k;
            sw.Stop();
        }
    }
}
