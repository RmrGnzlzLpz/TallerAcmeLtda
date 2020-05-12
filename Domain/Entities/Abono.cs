using Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Abono : Entity<int>
    {
        public double Monto { get; set; }
        public DateTime FechaDeCreacion { get; set; } = DateTime.UtcNow;
        public ICollection<AbonoCuota> AbonoCuotas { get; set; } = new List<AbonoCuota>();

        override
        public string ToString()
        {
            return $"Valor = {Monto}, Fecha = {FechaDeCreacion}";
        }
    }
}
