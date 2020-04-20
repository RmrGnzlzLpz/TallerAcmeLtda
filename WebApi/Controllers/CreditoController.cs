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
    public class CreditoController : ControllerBase
    {
        readonly IUnitOfWork _unitOfWork;
        readonly CreditoService _service;

        public CreditoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _service = new CreditoService(_unitOfWork);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Credito>> GetAll()
        {
            return Ok(_service.Buscar());
        }

        [HttpGet("{id}")]
        public ActionResult<IEnumerable<Credito>> Get(int id)
        {
            var response = _service.Buscar(x => x.Id == id);
            return Ok(response);
        }

        [HttpPost]
        public ActionResult<Response<Credito>> Post(CreditoRequest credito)
        {
            var response = _service.Crear(credito);
            return Ok(response);
        }

        [HttpPost("abonar")]
        public ActionResult<Response<Credito>> Abonar(AbonoRequest abono)
        {
            var response = _service.Abonar(abono);
            return Ok(response);
        }
    }
}
