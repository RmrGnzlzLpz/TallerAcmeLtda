using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Models;
using Application.Requests;
using Application.Services;
using Domain.Contracts;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadoController : ControllerBase
    {
        readonly IUnitOfWork _unitOfWork;
        readonly EmpleadoService _service;

        public EmpleadoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _service = new EmpleadoService(_unitOfWork);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Empleado>> GetAll()
        {
            return Ok(_service.Buscar(include: "Creditos"));
        }

        [HttpGet("{id}")]
        public ActionResult<IEnumerable<Empleado>> Get(int id)
        {
            return Ok(_service.Buscar(x => x.Id == id, include: "Creditos"));
        }

        [HttpGet("{id}/credito/{anio}/{mes}")]
        public ActionResult<IEnumerable<Credito>> GetCredito(int id, int anio, int mes)
        {
            Empleado empleado = _service.Buscar(x => x.Id == id, include: "Creditos").FirstOrDefault();
            return Ok(empleado.Creditos.Where(x => x.FechaDeCreacion.Year == anio && x.FechaDeCreacion.Month == mes));
        }

        [HttpPost]
        public ActionResult<Response<Empleado>> Post(EmpleadoRequest empleado)
        {
            return _service.Crear(empleado);
        }
    }
}
