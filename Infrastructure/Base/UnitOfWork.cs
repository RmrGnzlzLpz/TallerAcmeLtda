using Domain.Contracts;
using Domain.Repositories;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Infrastructure.Base
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbContext _dbContext;

        private IEmpleadoRepository _empleadoRepository;
        private ICreditoRepository _creditoRepository;
        private ICuotaRepository _cuotaRepository;
        private IAbonoCuotaRepository _abonoCuotaRepository;
        public IEmpleadoRepository EmpleadoRepository { get { return _empleadoRepository ?? (_empleadoRepository = new EmpleadoRepository(_dbContext)); } }
        public ICreditoRepository CreditoRepository { get { return _creditoRepository ?? (_creditoRepository = new CreditoRepository(_dbContext)); } }
        public ICuotaRepository CuotaRepository { get { return _cuotaRepository ?? (_cuotaRepository = new CuotaRepository(_dbContext)); } }
        public IAbonoCuotaRepository AbonoCuotaRepository { get { return _abonoCuotaRepository ?? (_abonoCuotaRepository = new AbonoCuotaRepository(_dbContext)); } }

        public UnitOfWork(IDbContext context)
        {
            _dbContext = context;
        }

        public int Commit()
        {
            return _dbContext.SaveChanges();
        }
        public void Dispose()
        {
            Dispose(true);
        }
        /// <summary>
        /// Disposes all external resources.
        /// </summary>
        /// <param name="disposing">The dispose indicator.</param>
        private void Dispose(bool disposing)
        {
            if (disposing && _dbContext != null)
            {
                ((DbContext)_dbContext).Dispose();
                _dbContext = null;
            }
        }
    }
}
