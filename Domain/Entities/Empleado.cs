using System;
using System.Collections.Generic;
using System.Text;
using Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Empleado : Entity<int>
    {
        [Required] public string Cedula { get; set; }
        [Required] public string Nombre { get; set; }
        [Required] public double Salario { get; set; }
    }
}
