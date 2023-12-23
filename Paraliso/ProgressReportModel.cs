using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paraliso
{
    public class ProgressReportModel
    {
        public int PercentageComplete { get; set; }

        public List<ImageProcessModel> ProcessedImages { get; set; }
    }
}
