using Application.Base;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Requests
{
    public class CreditoRequest : IRequest<Credito>
    {
        public string CedulaEmpleado { get; set; }
        public int Plazo { get; set; }
        public double TasaDeInteres { get; set; }
        public double Valor { get; set; }

        public CreditoRequest()
        {
            TasaDeInteres = 0.005;
        }

        public Credito ToEntity()
        {
            return new Credito
            {
                Plazo = Plazo,
                TasaDeInteres = TasaDeInteres,
                Valor = Valor
            };
        }
    }
}
