using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MatrixMethods.api
{
    public unsafe struct MatrixItem 
    {
        public int i, j, k;
        public MatrixItem* p;
    }
}
