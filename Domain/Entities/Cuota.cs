using Domain.Base;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Cuota : Entity<int>, ICredito
    {
        public double Valor { get; set; }
        public double Pagado { get; set; }
        public double Saldo { get => Valor - Pagado; }
        public int Orden { get; set; }
        public DateTime FechaDePago { get; set; }
        public EstadoDeCuota Estado {
            get
            {
                if (Pagado == Valor) return EstadoDeCuota.Pagada;
                if (FechaDePago < DateTime.UtcNow) return EstadoDeCuota.Vencida;
                if (Pagado > 0) return EstadoDeCuota.Parcial;
                return EstadoDeCuota.Pendiente;
            }
        }
        public void Abonar(double monto)
        {
            if (monto > Saldo) throw new Exception("No se puede abonar este valor");
            Pagado += monto;
        }
        public void Saldar()
        {
            Pagado += Saldo;
        }
    }

    public enum EstadoDeCuota
    {
        Pendiente = 0,
        Pagada = 1,
        Vencida = 2,
        Parcial = 3
    }
}
