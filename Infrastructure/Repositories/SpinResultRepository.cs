using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class SpinResultRepository : BaseRepository<SpinResult>, ISpinResultRepository
    {
        public SpinResultRepository(DataContext context) : base(context)
        {
        }
    }
}
