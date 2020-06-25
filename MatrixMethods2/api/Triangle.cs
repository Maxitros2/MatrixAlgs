using System;

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MatrixMethods.api
{
    class Triangle : MatrixMethod
    {
      
        public Triangle(int size, bool pointers)
        {
            this.size = size;
            this.data = new int[Convert.ToInt32(size * ((double)(size - 1) / 2d))];
            Debug.WriteLine("data="+data.Length);
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
            
            if (i >= j || Convert.ToInt32((j * ((j - 1) / 2d)) + i) >= data.Length)
                return 0;
            sw.Start();
            int d = Convert.ToInt32((j* ((j - 1) / 2d)) + i);
            Debug.WriteLine("reading " + d);
            if (UsePointers)
            {
                unsafe
                {
                    fixed (int* ptr = data)
                    {
                        int retp = *(ptr + d);
                        sw.Stop();
                        return retp;
                    }
                }
            }
            int ret = data[d];
            sw.Stop();
            //MessageBox.Show(data[d].ToString());
            return ret;
        }

        public override void StopWatchReset()
        {
            this.sw = new System.Diagnostics.Stopwatch();
        }
        public override MatrixMethod Add(MatrixMethod matrix)
        {
            if (matrix.GetType() != typeof(Triangle))
                base.Add(matrix);
            Triangle triangle = new Triangle(size,UsePointers);
            triangle.StopWatchReset();
            triangle.sw.Start();
            for (int i = 0; i < data.Length; i++)
                triangle.data[i] = data[i] + matrix.data[i];
            triangle.sw.Stop();
            return triangle;

        }
        public override MatrixMethod Subtract(MatrixMethod matrix)
        {
            if (matrix.GetType() != typeof(Triangle))
                base.Add(matrix);
            Triangle triangle = new Triangle(size, UsePointers);
            triangle.StopWatchReset();
            triangle.sw.Start();
            for (int i = 0; i < data.Length; i++)
                triangle.data[i] = data[i] - matrix.data[i];
            triangle.sw.Stop();
            return triangle;

        }
        public override void WriteToMatrix(int i, int j, int k)
        {
            
            sw.Start();
            if (i>=j)
                return;
            int d = Convert.ToInt32(j*((double)(j - 1) / 2d) + i);
            Debug.WriteLine("writing to {0} value {1} with i={2} j={3}",d,k,i,j);
            if (UsePointers)
            {
                unsafe
                {
                    fixed (int* ptr = data)
                    {
                        *(ptr + d) = k;
                        sw.Stop();
                        return;
                    }
                }
            }
            data[d] = k;
            sw.Stop();
        }
    }
}
