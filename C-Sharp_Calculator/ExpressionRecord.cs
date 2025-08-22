using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Claculator
{
    internal class ExpressionRecord
    {
        public int ID { get; set; }
        public string Expression { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Preorder { get; set; }
        public string Postorder { get; set; }
        public double DecimalResult { get; set; }
        public string BinaryResult { get; set; }
    }
}
