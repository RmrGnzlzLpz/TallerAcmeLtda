using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public IEnumerable<Credito> Get()
        {
            return _service.Buscar();
        }
    }
}
