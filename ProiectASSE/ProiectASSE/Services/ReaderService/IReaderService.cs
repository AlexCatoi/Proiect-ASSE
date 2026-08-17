// <copyright file="IReaderService.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.Services.ReaderService
{
    using System.Collections.Generic;
    using ProiectASSE.DomainModel.Entities;

    /// <summary>
    /// Provides operations for managing readers.
    /// </summary>
    public interface IReaderService
    {
        /// <summary>
        /// Retrieves a reader by its unique identifier.
        /// </summary>
        /// <param name="id">The identifier of the reader.</param>
        /// <returns>The reader with the specified ID.</returns>
        Reader GetReader(int id);

        /// <summary>
        /// Retrieves all readers stored in the system.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="Reader"/> objects.
        /// </returns>
        IEnumerable<Reader> GetAllReaders();

        /// <summary>
        /// Registers a new reader in the system.
        /// </summary>
        /// <param name="reader">The reader entity to be added.</param>
        void RegisterReader(Reader reader);

        /// <summary>
        /// Updates the information of an existing reader.
        /// </summary>
        /// <param name="reader">The reader entity containing updated information.</param>
        void UpdateReader(Reader reader);

        /// <summary>
        /// Deletes a reader from the system.
        /// </summary>
        /// <param name="id">The identifier of the reader to delete.</param>
        void DeleteReader(int id);
    }
}
