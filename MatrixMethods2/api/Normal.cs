using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixMethods.api
{
    public class Normal : MatrixMethod
    {
        public int[,] Fulldata;
        public Normal(int size, bool pointers)
        {
            this.size = size;
            Fulldata = new int[size, size];
            this.UsePointers = pointers;
        }
        public override long GetDataSize()
        {
            return GetObjectSize(Fulldata);
        }
        public override int GetDataCount()
        {
            return this.Fulldata.Length;
        }
        public override int ReadFromMatrix(int j, int i)
        {
            if (i >= size || j >= size)
                return 0;
            sw.Start();
            Debug.WriteLine("reading {0} {1}",i,j);
            int ret = Fulldata[i, j];
            sw.Stop();
            return ret;
            
        }

        public override void StopWatchReset()
        {
            this.sw = new System.Diagnostics.Stopwatch();
        }

        public override void WriteToMatrix(int i, int j, int k)
        {

            sw.Start();            
            Debug.WriteLine("writing to {0} value {1} with i={2} j={3}", 0, k, i, j);
            Fulldata[i, j] = k;
            sw.Stop();
        }
    }
}
