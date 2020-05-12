using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Repositories
{
    public class CreditoRepository : GenericRepository<Credito>, ICreditoRepository
    {
        public CreditoRepository(IDbContext context) : base(context) { }

        public override IQueryable<Credito> FindBy(System.Linq.Expressions.Expression<Func<Credito, bool>> filter = null, Func<System.Linq.IQueryable<Credito>, System.Linq.IOrderedQueryable<Credito>> orderBy = null, string includeProperties = "")
        {
            return base.FindBy(filter, orderBy, includeProperties).Include(x => x.Cuotas).ThenInclude(x => x.AbonoCuotas);
        }
    }
}
