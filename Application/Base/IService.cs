using Application.Models;
using Domain.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Base
{
    public interface IService<T> where T : BaseEntity
    {
        Response<T> Agregar(T request);
    }
}
