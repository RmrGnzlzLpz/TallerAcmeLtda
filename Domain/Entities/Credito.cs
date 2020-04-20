using Domain.Base;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Domain.Entities
{
    public class Credito : Entity<int>, ICredito
    {
        public double TasaDeInteres { get; set; }
        public double Valor { get; set; }
        public double Pagado { get; set; }
        public double Saldo { get => ValorAPagar - Pagado; }
        public double ValorAPagar { get => Valor * (1 + TasaDeInteres * Plazo); }
        public double ValorCuota { get => ValorAPagar / Plazo; }
        public int Plazo { get; set; } /* El Plazo se mide en meses */
        public List<Abono> Abonos { get; set; } = new List<Abono>();
        public List<Cuota> Cuotas { get; set; } = new List<Cuota>();
        public DateTime FechaDeCreacion { get; private set; }
        [NotMapped]
        public IList<Cuota> CuotasPorPagar
        {
            get => Cuotas?.Where(x => x.Estado != EstadoDeCuota.Pagada).OrderBy(x => x.FechaDePago).ToList();
        }

        public Credito() { }

        public Credito(double valor, int plazo, double tasaDeInteres = 0.005)
        {
            FechaDeCreacion = DateTime.UtcNow;
            Valor = valor;
            Plazo = plazo;
            TasaDeInteres = tasaDeInteres;
            Cuotas = GenerarCuotas(plazo);
            Abonos = new List<Abono>();
        }
        public string Abonar(double monto)
        {
            if (PuedeAbonar(monto).Any())
            {
                throw new Exception(string.Join(",", PuedeAbonar(monto)));
            };
            Abono abono = new Abono { Monto = monto };
            Abonos.Add(abono);
            Pagado += abono.Monto;
            foreach (Cuota cuota in CuotasPorPagar)
            {
                if (monto > cuota.Saldo)
                {
                    monto -= cuota.Saldo;
                    cuota.Abonar(cuota.Saldo);
                    cuota.AbonoCuotas.Add(new AbonoCuota { Abono = abono, Cuota = cuota});
                }
                else
                {
                    cuota.Abonar(monto);
                    cuota.AbonoCuotas.Add(new AbonoCuota { Abono = abono, Cuota = cuota});
                    break;
                }
            }
            return $"Abono registrado correctamente. Su nuevo saldo es: ${Saldo}.";
        }
        public List<string> PuedeAbonar(double monto)
        {
            List<string> errores = new List<string>();
            if (monto <= 0 || monto > Saldo) errores.Add("El valor del abono es incorrecto.");
            if (CuotasPorPagar.Count == 0)
            {
                errores.Add("No hay cuotas pendientes");
            } else
            {
                if (monto < CuotasPorPagar.FirstOrDefault().Saldo) errores.Add($"El valor del abono debe ser mínimo de ${CuotasPorPagar.FirstOrDefault().Saldo}.");
            }
            return errores;
        }

        private List<Cuota> GenerarCuotas(int numeroDeCuotas)
        {
            var cuotas = new List<Cuota>();
            for (int i = 1; i <= numeroDeCuotas; i++)
            {
                cuotas.Add(new Cuota
                {
                    Orden = i,
                    Valor = ValorAPagar / numeroDeCuotas,
                    FechaDePago = FechaDeCreacion.AddMonths(i)
                });
            }
            return cuotas;
        }

        override
        public string ToString()
        {
            return $"Valor = {Valor}, Saldo = {ValorAPagar}, Cuotas = {Cuotas.Count}, Interes = {TasaDeInteres}";
        }
    }
}
