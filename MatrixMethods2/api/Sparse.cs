using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixMethods.api
{
    class Sparse : MatrixMethod
    {
        private MatrixItem[] FullData;
        public void PushMatrix(int[,] m)
        {
            this.size = Convert.ToInt32(Math.Sqrt(m.Length));
            List<MatrixItem> toAdd = new List<MatrixItem>();
            for (int i = 0;i<size;i++)
            {
                for(int j = 0;j<size;j++)
                {
                    if (m[i, j] != 0)
                    {
                        MatrixItem matrixItem = new MatrixItem()
                        {
                            i = i,
                            j = j,
                            k = m[i, j]
                        };
                        toAdd.Add(matrixItem);
                    }
                }
            }
            FullData = toAdd.ToArray();
            if(UsePointers)
            {
                for (int i = 0; i < FullData.Length; i++)
                {
                    MatrixItem matrixItem1 = FullData[i];
                    for (int k = i+1;k< FullData.Length;k++)
                    {
                        MatrixItem matrixItem2 = FullData[k];
                        if (matrixItem1.j == matrixItem2.j)
                        {
                            unsafe {
                                fixed (MatrixItem* ptr = FullData)
                                {
                                    FullData[i] = new MatrixItem()
                                    {
                                        i = matrixItem1.i,
                                        j = matrixItem1.j,
                                        k = matrixItem1.k,
                                        p = ptr + k
                                    };
                                }
                            }
                        }
                    }
                }
            }
            Debug.WriteLine(FullData.Length);


        }
        public override int GetDataCount()
        {
            return this.FullData.Length;
        }
        public override long GetDataSize()
        {
            return FullData.Length*4;
        }
        public Sparse(bool pointers)
        {
            this.UsePointers = pointers;
        }
        public override int ReadFromMatrix(int j, int i)
        {

            if (i >= FullData.Length || j>= FullData.Length)
                return 0;
            sw.Start();
            int d = 2 * j + i - 2;
            Debug.WriteLine("reading " + d);
            if (UsePointers)
            {
                unsafe
                {
                    fixed (MatrixItem* ptr = FullData)
                    {
                        for (int n = 0; n < FullData.Length; n++)
                        {
                            MatrixItem mi = *(ptr + n);
                            if (mi.i == i && mi.j == j)
                            {
                                sw.Stop();
                                return mi.k;
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (MatrixItem mi in FullData)
                {
                    if (mi.i == i && mi.j == j)
                    {
                        sw.Stop();
                        return mi.k;
                    }
                }
            }
            sw.Stop();
            return 0;
        }

        public override void StopWatchReset()
        {
            this.sw = new System.Diagnostics.Stopwatch();
        }

        public override void WriteToMatrix(int i, int j, int k)
        {

            sw.Start();
            if (!(i == j || j == i + 1 || j == i - 1) || 2 * j + i - 2 >= data.Length - 2)
                return;
            int d = 2 * j + i - 2;
            Debug.WriteLine("writing to {0} value {1} with i={2} j={3}", d, k, i, j);
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
            data[d + 2] = k;
            sw.Stop();
        }
    }
}
