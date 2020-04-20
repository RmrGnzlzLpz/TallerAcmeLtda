using Domain.Contracts;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class CuotaRepository : GenericRepository<Cuota>, ICuotaRepository
    {
        public CuotaRepository(IDbContext context) : base(context)
        {
        }
    }
}
