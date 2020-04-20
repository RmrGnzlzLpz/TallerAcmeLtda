using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class AbonoCuotaRepository : GenericRepository<AbonoCuota>, IAbonoCuotaRepository
    {
        public AbonoCuotaRepository(IDbContext context) : base(context)
        {
        }
    }
}
