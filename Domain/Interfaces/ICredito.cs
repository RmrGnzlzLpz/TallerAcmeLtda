using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface ICredito
    {
        double Valor { get; set; }
        double Pagado { get; set; }
        double Saldo { get; }
        string Abonar(double monto);
    }
}
