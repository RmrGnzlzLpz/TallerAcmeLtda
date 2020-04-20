using Application.Models;
using Domain.Base;
using Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Application.Base
{
    public abstract class Service<T> : IService<T> where T : BaseEntity
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly IGenericRepository<T> _repository;
        public Service(IUnitOfWork unitOfWork, IGenericRepository<T> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public virtual Response<T> Agregar(T entity)
        {
            _repository.Add(entity);
            _unitOfWork.Commit();
            return new Response<T> {
                Mensaje = "Registro agregado con éxito",
                Entity = entity
            };
        }

        public virtual IEnumerable<T> Buscar(Expression<Func<T, bool>> predicate, string include = "")
        {
            return _repository.FindBy(predicate, includeProperties: include);
        }

        public virtual IEnumerable<T> Buscar()
        {
            return _repository.GetAll();
        }
    }
}
