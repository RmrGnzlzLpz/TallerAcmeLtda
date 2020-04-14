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
                if (Saldo == 0) return EstadoDeCuota.Pagada;
                if (FechaDePago < DateTime.UtcNow) return EstadoDeCuota.Vencida;
                if (Pagado == 0) return EstadoDeCuota.Pendiente;
                return EstadoDeCuota.Parcial;
            }
        }
        public string Abonar(double monto)
        {
            if (monto > Saldo) throw new Exception("No se puede abonar este valor");
            Pagado += monto;
            return ToString();
        }
        public void Saldar()
        {
            Pagado += Saldo;
        }

        private string EstadoToString()
        {
            switch (Estado)
            {
                case EstadoDeCuota.Pendiente: return "Pendiente";
                case EstadoDeCuota.Pagada: return "Pagada";
                case EstadoDeCuota.Vencida: return "Vencida";
                case EstadoDeCuota.Parcial: return "Parcial";
                default: return "Vencida";
            }
        }

        override
        public string ToString()
        {
            return $"Numero = {Orden}, Estado = {EstadoToString()}, Valor = {Valor}, Saldo = {Saldo}, Fecha = {FechaDePago}";
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
