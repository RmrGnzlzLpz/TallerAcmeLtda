using Application.Base;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Requests
{
    public class AbonoRequest : IRequest<Abono>
    {
        public int CreditoId { get; set; }
        public double Monto { get; set; }

        public Abono ToEntity()
        {
            return new Abono
            {
                Monto = Monto
            };
        }
    }
}
