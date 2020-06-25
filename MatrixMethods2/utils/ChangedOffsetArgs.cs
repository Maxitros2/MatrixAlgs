using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixMethods.utils
{
    public class ChangedOffsetArgs
    {
        public int offsetX, offsetY, size;

        public ChangedOffsetArgs(int offsetX, int offsetY, int size)
        {
            this.offsetX = offsetX;
            this.offsetY = offsetY;
            this.size = size;
        }
    }
}
