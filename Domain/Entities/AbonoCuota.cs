using Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class AbonoCuota : BaseEntity
    {
        public int AbonoId { get; set; }
        public Abono Abono { get; set; }
        public int CuotaId {get; set;}
        public Cuota Cuota {get; set; }
    }
}