using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paraliso
{
    public class ImageProcessModel
    {
        public string FileName { get; set; }
        public int ProcessingThreadId { get; set; }
        public int SizeInPixels { get; set; }
    }
}
