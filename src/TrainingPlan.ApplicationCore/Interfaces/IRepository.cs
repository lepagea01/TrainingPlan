using System.Collections.Generic;
using TrainingPlan.ApplicationCore.Entities;

namespace TrainingPlan.ApplicationCore.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        T Create(T entity);
        IEnumerable<T> ReadAll();
        T ReadOne(int id);
        T Update(T entity);
        T Delete(int id);
    }
}