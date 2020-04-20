using Application.Base;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Requests
{
    public class EmpleadoRequest : IRequest<Empleado>
    {
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public double Salario { get; set; }

        public Empleado ToEntity()
        {
            return new Empleado
            {
                Cedula = Cedula,
                Nombre = Nombre,
                Salario = Salario
            };
        }
    }
}
