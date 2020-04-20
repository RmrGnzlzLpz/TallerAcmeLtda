using Domain.Entities;
using Infrastructure.Base;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;

namespace Infrastructure.Test
{
    public class CreditoTests
    {
        UnitOfWork UnitOfWork;
        CreditoContext Context;
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CreditoContext>().UseSqlServer("Server=localhost;Database=AcmeLtda;Trusted_Connection=True;MultipleActiveResultSets=true").Options;
            Context = new CreditoContext(options);
            UnitOfWork = new UnitOfWork(Context);
        }

        [Test]
        public void ConsultarCredito()
        {
            Credito credito = UnitOfWork.CreditoRepository.FindBy(x => x.Id == 1, includeProperties: "Cuotas").FirstOrDefault();
            var cuotas = UnitOfWork.CuotaRepository.FindBy(x => x.CreditoId == credito.Id).ToList();
            Assert.AreEqual(credito.Cuotas, cuotas);
        }
    }
}