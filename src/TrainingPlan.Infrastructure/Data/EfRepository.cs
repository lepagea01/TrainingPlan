using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrainingPlan.ApplicationCore.Entities;
using TrainingPlan.ApplicationCore.Interfaces;

namespace TrainingPlan.Infrastructure.Data
{
    public class EfRepository<T> : IAsyncRepository<T>, IRepository<T> where T : BaseEntity
    {
        private readonly TrainingPlanContext _trainingPlanContext;

        protected EfRepository(TrainingPlanContext trainingPlanContext)
        {
            _trainingPlanContext = trainingPlanContext ?? throw new ArgumentNullException(nameof(trainingPlanContext));
        }

        public async Task<T> CreateAsync(T entity)
        {
            _trainingPlanContext.Set<T>().Add(entity);
            await _trainingPlanContext.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<T>> ReadAllAsync()
        {
            return await _trainingPlanContext.Set<T>().ToListAsync();
        }

        public async Task<T> ReadOneAsync(int id)
        {
            return await _trainingPlanContext.Set<T>().FindAsync(id);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            var entityToUpdate = await _trainingPlanContext.Set<T>().FindAsync(entity.Id);
            if (entityToUpdate == null) return null;
            _trainingPlanContext.Entry(entityToUpdate)
                .CurrentValues
                .SetValues(entity);
            await _trainingPlanContext.SaveChangesAsync();

            return entityToUpdate;
        }

        public async Task<T> DeleteAsync(int id)
        {
            var entityToDelete = await _trainingPlanContext.Set<T>().FindAsync(id);
            // ReSharper disable once InvertIf
            if (entityToDelete != null)
            {
                _trainingPlanContext.Set<T>().Remove(entityToDelete);
                await _trainingPlanContext.SaveChangesAsync();
            }

            return entityToDelete;
        }

        public T Create(T entity)
        {
            _trainingPlanContext.Set<T>().Add(entity);
            _trainingPlanContext.SaveChanges();

            return entity;
        }

        public IEnumerable<T> ReadAll()
        {
            return _trainingPlanContext.Set<T>().AsEnumerable();
        }

        public T ReadOne(int id)
        {
            return _trainingPlanContext.Set<T>().Find(id);
        }

        public T Update(T entity)
        {
            var entityToUpdate = _trainingPlanContext.Set<T>().Find(entity.Id);
            if (entityToUpdate == null) return null;
            _trainingPlanContext.Entry(entityToUpdate)
                .CurrentValues
                .SetValues(entity);
            _trainingPlanContext.SaveChanges();

            return entityToUpdate;
        }

        public T Delete(int id)
        {
            var entityToDelete = _trainingPlanContext.Set<T>().Find(id);
            // ReSharper disable once InvertIf
            if (entityToDelete != null)
            {
                _trainingPlanContext.Set<T>().Remove(entityToDelete);
                _trainingPlanContext.SaveChanges();
            }

            return entityToDelete;
        }
    }
}