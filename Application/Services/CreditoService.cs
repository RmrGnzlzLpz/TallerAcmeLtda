using Application.Base;
using Application.Models;
using Application.Requests;
using Domain.Builders;
using Domain.Contracts;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Application.Services
{
    public class CreditoService : Service<Credito>
    {
        public CreditoService(IUnitOfWork unitOfWork) : base(unitOfWork, unitOfWork.CreditoRepository)
        {
        }

        public Response<Credito> Crear(CreditoRequest request)
        {
            Empleado empleado = _unitOfWork.EmpleadoRepository.FindFirstOrDefault(x => x.Cedula == request.CedulaEmpleado);
            if (empleado == null)
            {
                return new Response<Credito>
                {
                    Mensaje = $"El número de cédula {request.CedulaEmpleado} no existe.",
                    Entity = request.ToEntity()
                };
            }

            var _errores = CreditoBuilder.PuedeCrearCredito(request.Valor, request.Plazo, request.TasaDeInteres);
            if (_errores.Any()) return new Response<Credito> { Mensaje = string.Join(",", _errores), Entity = request.ToEntity() };

            Credito credito = CreditoBuilder.CrearCredito(request.Valor, request.Plazo, request.TasaDeInteres);
            empleado.Creditos.Add(credito);
            _unitOfWork.EmpleadoRepository.Edit(empleado);
            _unitOfWork.Commit();
            return new Response<Credito>
            {
                Mensaje = $"Crédito registrado. Valor a pagar: ${credito.ValorAPagar}.",
                Entity = credito
            };
        }

        public Response<Credito> Abonar(AbonoRequest request)
        {
            Credito credito = Buscar(x => x.Id == request.CreditoId).FirstOrDefault();
            if (credito == null)
            {
                return new Response<Credito>
                {
                    Mensaje = "Crédito no encontrado."
                };
            }
            List<string> errores = credito.PuedeAbonar(request.Monto);
            if (errores.Any())
            {
                return new Response<Credito>
                {
                    Mensaje = string.Join(",", errores)
                };
            }
            string mensaje = credito.Abonar(request.Monto);
            _unitOfWork.CreditoRepository.Edit(credito);
            _unitOfWork.Commit();
            credito.Abonos = null; // Eliminar las listas internas por problemas de ciclos en Json
            credito.Cuotas = null; // Cuotas contiene AbonoCuotas y AbonoCuotas contiene Cuota
            return new Response<Credito>
            {
                Mensaje = mensaje, 
                Entity = credito
            };
        }

        public override IEnumerable<Credito> Buscar(Expression<Func<Credito, bool>> predicate = null, string include = "Cuotas,Abonos")
        {
            return base.Buscar(predicate: predicate, include: include);
        }
    }
}
