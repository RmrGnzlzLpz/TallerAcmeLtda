using Application.Services;
using Domain.Contracts;
using Infrastructure;
using Infrastructure.Base;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Domain.Entities;
using Application.Requests;
using Application.Models;
using System.Linq;

namespace Application.Test
{
    public class CreditoTests
    {
        IUnitOfWork unitOfWork;
        CreditoContext context;
        CreditoService service;
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CreditoContext>().UseSqlServer("Server=localhost;Database=AcmeLtda;Trusted_Connection=True;MultipleActiveResultSets=true").Options;
            context = new CreditoContext(options);
            unitOfWork = new UnitOfWork(context);
            service = new CreditoService(unitOfWork);
        }

        [Test(Description = "Crear crédito con cédula errónea")]
        public void CrearCreditoCedulaIncorrecta()
        {
            CreditoRequest request = new CreditoRequest
            {
                CedulaEmpleado = "11111111",
                Plazo = 10,
                Valor = 6000000
            };
            var respuesta = service.Crear(request);
            Assert.AreEqual($"El número de cédula {request.CedulaEmpleado} no existe.", respuesta.Mensaje);
        }

        [Test(Description = "Crear crédito válido")]
        public void CrearCredito()
        {
            CreditoRequest request = new CreditoRequest
            {
                CedulaEmpleado = "1082480166",
                Plazo = 10,
                Valor = 6000000
            };
            var respuesta = service.Crear(request);
            Assert.AreEqual($"Crédito registrado. Valor a pagar: ${request.ToEntity().ValorAPagar}.", respuesta.Mensaje);
        }

        [Test(Description = "Abono a crédito por valor menor a cuota")]
        public void AbonoInferiorCuotaCredito()
        {
            Credito credito = service.Buscar(x => x.Id == 4).FirstOrDefault();
            double valorAbono = 100000;
            Assert.IsNotNull(credito);
            AbonoRequest request = new AbonoRequest
            {
                Monto = valorAbono,
                CreditoId = credito.Id
            };
            Response<Credito> response = service.Abonar(request);
            Assert.Contains($"El valor del abono debe ser mínimo de ${credito.ValorCuota}.", response.Mensaje.Split(','));
        }

        [Test(Description = "Abono a crédito por valor igual a cuota")]
        public void AbonoIgualCuotaCredito()
        {
            Credito credito = service.Buscar(x => x.Id == 4).FirstOrDefault();
            double valorAbono = 630000;
            double valorEsperado = credito.Saldo - valorAbono;
            Assert.IsNotNull(credito);
            AbonoRequest request = new AbonoRequest
            {
                Monto = valorAbono,
                CreditoId = credito.Id
            };
            Response<Credito> response = service.Abonar(request);
            Assert.AreEqual($"Abono registrado correctamente. Su nuevo saldo es: ${valorEsperado}.", response.Mensaje);
        }

        [Test(Description = "Abono a crédito por valor superior a cuota")]
        public void AbonoSuperiorCuotaCredito()
        {
            Credito credito = service.Buscar(x => x.Id == 4, include: "Cuotas").FirstOrDefault();
            double valorAbono = 631000;
            double valorEsperado = credito.Saldo - valorAbono;
            Assert.IsNotNull(credito);
            AbonoRequest request = new AbonoRequest
            {
                Monto = valorAbono,
                CreditoId = credito.Id
            };
            Response<Credito> response = service.Abonar(request);
            Assert.AreEqual($"Abono registrado correctamente. Su nuevo saldo es: ${valorEsperado}.", response.Mensaje);
        }

        [Test(Description = "Abono a crédito por valor superior a saldo")]
        public void AbonoSuperiorSaldoCredito()
        {
            Credito credito = service.Buscar(x => x.Id == 4, include: "Cuotas").FirstOrDefault();
            double valorAbono = credito.Saldo + 1;
            double valorEsperado = credito.Saldo - valorAbono;
            Assert.IsNotNull(credito);
            AbonoRequest request = new AbonoRequest
            {
                Monto = valorAbono,
                CreditoId = credito.Id
            };
            Response<Credito> response = service.Abonar(request);
            Assert.AreEqual($"Abono registrado correctamente. Su nuevo saldo es: ${valorEsperado}.", response.Mensaje);
        }
    }
}