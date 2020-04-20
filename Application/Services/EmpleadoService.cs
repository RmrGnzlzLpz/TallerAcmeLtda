using Application.Base;
using Application.Models;
using Application.Requests;
using Domain.Contracts;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Application.Services
{
    public class EmpleadoService : Service<Empleado>
    {
        public EmpleadoService(IUnitOfWork unitOfWork) : base(unitOfWork, unitOfWork.EmpleadoRepository)
        {
        }

        public Response<Empleado> Crear(EmpleadoRequest request)
        {
            Empleado empleado = _unitOfWork.EmpleadoRepository.FindFirstOrDefault(x => x.Cedula == request.Cedula);
            if (empleado != null)
            {
                return new Response<Empleado> { Mensaje = $"El empleado con Cédula = {request.Cedula}, ya está registrado.", Entity = request.ToEntity() };
            }
            return base.Agregar(request.ToEntity());
        }
    }
}
