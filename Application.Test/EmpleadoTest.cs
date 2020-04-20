using Application.Services;
using Domain.Contracts;
using Infrastructure;
using Infrastructure.Base;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Domain.Entities;
using Application.Requests;
using System.Linq;

namespace Application.Test
{
    public class EmpleadoTests
    {
        IUnitOfWork unitOfWork;
        CreditoContext context;
        EmpleadoService service;
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CreditoContext>().UseSqlServer("Server=localhost;Database=AcmeLtda;Trusted_Connection=True;MultipleActiveResultSets=true").Options;
            context = new CreditoContext(options);
            unitOfWork = new UnitOfWork(context);
            service = new EmpleadoService(unitOfWork);
        }

        [Test]
        public void CrearEmpleado()
        {
            var respuesta = service.Crear(new EmpleadoRequest
            {
                Cedula = "1082410166",
                Nombre = "Ramiro González",
                Salario = 1200000
            });
            Assert.AreEqual("Registro agregado con éxito", respuesta.Mensaje);
            Empleado empleado = unitOfWork.EmpleadoRepository.Find(respuesta.Entity.Id);
            Assert.AreEqual(respuesta.Entity, empleado);
        }

        [Test(Description = "Crear empleado con cédula duplicada")]
        public void CrearEmpleadoDuplicado()
        {
            string cedula = service.Buscar(x => x.Id > 0).FirstOrDefault().Cedula;
            var respuesta = service.Crear(new EmpleadoRequest
            {
                Cedula = cedula,
                Nombre = "Ramiro González",
                Salario = 1200000
            });
            Assert.AreEqual($"El empleado con Cédula = {cedula}, ya está registrado.", respuesta.Mensaje);
        }
    }
}