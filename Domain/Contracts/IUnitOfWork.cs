using Domain.Repositories;
using System;

namespace Domain.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IEmpleadoRepository EmpleadoRepository { get; }
        ICreditoRepository CreditoRepository { get; }
        ICuotaRepository CuotaRepository { get; }
        IAbonoCuotaRepository AbonoCuotaRepository { get; }
        int Commit();
    }
}
