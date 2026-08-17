// <copyright file="ReaderRepository.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.DataAccess.Repository
{
    using System.Collections.Generic;
    using System.Linq;
    using ProiectASSE.DomainModel.Context;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Provides data access operations for <see cref="Reader"/> entities.
    /// This repository encapsulates CRUD operations and interacts with the database context.
    /// </summary>
    public class ReaderRepository : IReaderRepository
    {
        /// <summary>
        /// The database context used to access reader data.
        /// </summary>
        private readonly LibraryContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReaderRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used for data access.</param>
        public ReaderRepository(LibraryContext context)
        {
            this.context = context;
        }

        /// <inheritdoc/>
        public Reader GetById(int id)
        {
            return this.context.Readers.Find(id);
        }

        /// <inheritdoc/>
        public IEnumerable<Reader> GetAll()
        {
            return this.context.Readers.ToList();
        }

        /// <inheritdoc/>
        public void Add(Reader reader)
        {
            this.context.Readers.Add(reader);
        }

        /// <inheritdoc/>
        public void Update(Reader reader)
        {
            var entry = this.context.Entry(reader);

            if (entry.State == System.Data.Entity.EntityState.Detached)
            {
                this.context.Readers.Attach(reader);
            }

            entry.State = System.Data.Entity.EntityState.Modified;
        }

        /// <inheritdoc/>
        public void Delete(int id)
        {
            var reader = this.context.Readers.Find(id);
            if (reader != null)
            {
                this.context.Readers.Remove(reader);
            }
        }

        /// <inheritdoc/>
        public bool EmailExists(string email, int? excludeId = null)
        {
            return this.context.Readers
                .Any(r => r.Email == email && (!excludeId.HasValue || r.Id != excludeId.Value));
        }

        /// <inheritdoc/>
        public bool PhoneExists(string phone, int? excludeId = null)
        {
            return this.context.Readers
                .Any(r => r.Phone == phone && (!excludeId.HasValue || r.Id != excludeId.Value));
        }

        /// <inheritdoc/>
        public void SaveChanges()
        {
            this.context.SaveChanges();
        }
    }
}
