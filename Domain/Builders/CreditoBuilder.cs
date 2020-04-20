using System.Collections.Generic;
using System.Linq;
using Domain.Entities;

namespace Domain.Builders
{
    public class CreditoBuilder
    {
        private static List<string> _errores;
        public static List<string> PuedeCrearCredito(double valor, int plazo, double tasaDeInteres = 0.05)
        {
            _errores = new List<string>();
            ValidarEntidad(valor, plazo, tasaDeInteres);
            return _errores;
        }

        public static Credito CrearCredito(double valor, int plazo, double tasaDeInteres)
        {
            if (PuedeCrearCredito(valor, plazo, tasaDeInteres).Any())
            {
                throw new System.Exception(string.Join(",", _errores));
            }
            return new Credito(valor, plazo, tasaDeInteres);
        }

        /* Validations */
        private static void ValidarEntidad(double valor, int numeroDeCuotas, double tasaDeInteres)
        {
            ValidarValor(valor);
            ValidarCuotas(numeroDeCuotas);
            ValidarTasa(tasaDeInteres);
        }
        private static void ValidarValor(double valor)
        {
            if (valor < 5000000 || valor > 10000000) _errores.Add("El valor del crédito debe estar entre 5 y 10 millones.");
        }
        private static void ValidarCuotas(int numeroDeCuotas)
        {
            if (numeroDeCuotas < 0 || numeroDeCuotas > 10) _errores.Add("El plazo para el pago del crédito debe ser de máximo 10 meses.");
        }
        private static void ValidarTasa(double tasaDeInteres)
        {
            if (tasaDeInteres < 0 || tasaDeInteres > 1) _errores.Add("Tasa Incorrecta.");
        }
    }
}