using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class SpinResultResponse
    {
        public string Result { get; set; }
        public decimal WinAmount { get; set; }
        public decimal CurrentBalance { get; set; }
    }
}
