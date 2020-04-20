using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Models
{
    public class Response<T>
    {
        public string Mensaje { get; set; }
        public T Entity { get; set; }
    }
}
