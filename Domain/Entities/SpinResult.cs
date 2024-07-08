using Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SpinResult : BaseEntity
    {
        public string UserId { get; set; }
        public string Result { get; set; }
        public decimal WinAmount { get; set; }
    }
}
