using Domain.Entities;
using Infrastructure.Base;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Infrastructure.Test
{
    public class EmpleadoTests
    {
        UnitOfWork UnitOfWork;
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CreditoContext>().UseSqlServer("Server=localhost;Database=AcmeLtda;Trusted_Connection=True;MultipleActiveResultSets=true").Options;
            CreditoContext context = new CreditoContext(options);
            UnitOfWork = new UnitOfWork(context);
        }
    }
}