﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Domain.Repositories
{
    public interface IRepository : ITransientDependency
    {

    }

    public interface IRepository<TEntity> : IRepository<TEntity, Guid>
        where TEntity : class, IEntity<Guid>
    {
        
    }

    public interface IRepository<TEntity, TPrimaryKey> : IRepository
        where TEntity : class, IEntity<TPrimaryKey>
    {
        /// <summary>
        /// Get list of all entities without any filtering.
        /// </summary>
        /// <returns>List of entities</returns>
        List<TEntity> GetList();

        /// <summary>
        /// Get list of all entities without any filtering.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>List of entities</returns>
        Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets an entity with given primary key.
        /// Throws <see cref="EntityNotFoundException"/> if can not find an entity with given id.
        /// </summary>
        /// <param name="id">Primary key of the entity to get</param>
        /// <returns>Entity</returns>
        [NotNull]
        TEntity Get(TPrimaryKey id);

        /// <summary>
        /// Gets an entity with given primary key.
        /// Throws <see cref="EntityNotFoundException"/> if can not find an entity with given id.
        /// </summary>
        /// <param name="id">Primary key of the entity to get</param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>Entity</returns>
        [NotNull]
        Task<TEntity> GetAsync(TPrimaryKey id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets an entity with given primary key or null if not found.
        /// </summary>
        /// <param name="id">Primary key of the entity to get</param>
        /// <returns>Entity or null</returns>
        [CanBeNull]
        TEntity Find(TPrimaryKey id);

        /// <summary>
        /// Gets an entity with given primary key or null if not found.
        /// </summary>
        /// <param name="id">Primary key of the entity to get</param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>Entity or null</returns>
        Task<TEntity> FindAsync(TPrimaryKey id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Inserts a new entity.
        /// </summary>
        /// <param name="entity">Inserted entity</param>
        /// <param name="autoSave">
        /// Set true to automatically save changes to database.
        /// This can be used to set database generated Id of an entity for some ORMs (like Entity Framework).
        /// </param>
        [NotNull]
        TEntity Insert([NotNull] TEntity entity, bool autoSave = false);

        /// <summary>
        /// Inserts a new entity.
        /// </summary>
        /// <param name="autoSave">
        /// Set true to automatically save changes to database.
        /// This can be used to set database generated Id of an entity for some ORMs (like Entity Framework).
        /// </param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <param name="entity">Inserted entity</param>
        [NotNull]
        Task<TEntity> InsertAsync([NotNull] TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="entity">Entity</param>
        [NotNull]
        TEntity Update([NotNull] TEntity entity);

        /// <summary>
        /// Updates an existing entity. 
        /// </summary>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <param name="entity">Entity</param>
        [NotNull]
        Task<TEntity> UpdateAsync([NotNull] TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="entity">Entity to be deleted</param>
        void Delete([NotNull] TEntity entity);

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <param name="entity">Entity to be deleted</param>
        Task DeleteAsync([NotNull] TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes an entity by primary key.
        /// </summary>
        /// <param name="id">Primary key of the entity</param>
        void Delete(TPrimaryKey id);

        /// <summary>
        /// Deletes an entity by primary key.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <param name="id">Primary key of the entity</param>
        Task DeleteAsync(TPrimaryKey id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Get list of all entities without any filtering.
        /// </summary>
        /// <returns>List of entities</returns>
        long GetCount();

        /// <summary>
        /// Get list of all entities without any filtering.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>List of entities</returns>
        Task<long> GetCountAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
