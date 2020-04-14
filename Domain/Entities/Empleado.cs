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
        public List<Credito> Creditos { get; set; }

        public Empleado()
        {
            Creditos = new List<Credito>();
        }
        public string SolicitarCredito(double valor, int plazo, double tasaDeInteres = 0.005)
        {
            Credito credito = new Credito(valor, plazo, tasaDeInteres);
            Creditos.Add(credito);
            return $"Crédito registrado. Valor a pagar: ${credito.ValorAPagar}.";
        }
    }
}
