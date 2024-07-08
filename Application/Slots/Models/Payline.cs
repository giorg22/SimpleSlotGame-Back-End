using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Slots.Models
{
    public class Payline
    {
        public List<Symbol> Symbols { get; set; }
        public decimal Multiplier { get; set; }
    }
}
