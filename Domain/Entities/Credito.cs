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
        public List<Abono> Abonos { get; set; }
        public List<Cuota> Cuotas { get; set; }
        public DateTime FechaDeCreacion { get; private set; } = DateTime.UtcNow;
        [NotMapped]
        public IList<Cuota> CuotasPorPagar
        {
            get => Cuotas.Where(x => x.Estado != EstadoDeCuota.Pagada).OrderBy(x => x.FechaDePago).ToList();
        }

        public Credito(double valor, int plazo, double tasaDeInteres = 0.005)
        {
            ValidarEntidad(valor, plazo, tasaDeInteres);
            Valor = valor;
            Plazo = plazo;
            TasaDeInteres = tasaDeInteres;
            Cuotas = GenerarCuotas(plazo);
            Abonos = new List<Abono>();
        }
        public string Abonar(double monto)
        {
            PuedeAbonar(monto);
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
        public void PuedeAbonar(double monto)
        {
            if (monto <= 0 || monto > Saldo) throw new Exception("El valor del abono es incorrecto.");
            if (CuotasPorPagar.Count == 0) throw new Exception("No hay cuotas pendientes");
            if (monto < CuotasPorPagar.FirstOrDefault().Saldo) throw new Exception($"El valor del abono debe ser mínimo de ${CuotasPorPagar.FirstOrDefault().Saldo}.");
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

        /* Validations */
        private void ValidarEntidad(double valor, int numeroDeCuotas, double tasaDeInteres)
        {
            ValidarValor(valor);
            ValidarCuotas(numeroDeCuotas);
            ValidarTasa(tasaDeInteres);
        }
        private void ValidarValor(double valor)
        {
            if (valor < 5000000 || valor > 10000000) throw new Exception("El valor del crédito debe estar entre 5 y 10 millones.");
        }
        private void ValidarCuotas(int numeroDeCuotas)
        {
            if (numeroDeCuotas < 0 || numeroDeCuotas > 10) throw new Exception("El plazo para el pago del crédito debe ser de máximo 10 meses.");
        }
        private void ValidarTasa(double tasaDeInteres)
        {
            if (tasaDeInteres < 0 || tasaDeInteres > 1) throw new Exception("Tasa Incorrecta.");
        }
    }
}
