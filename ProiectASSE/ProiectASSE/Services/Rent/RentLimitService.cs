// <copyright file="RentLimitService.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE.Services.RentService
{
    using System;
    using System.Configuration;

    /// <summary>
    /// Provides access to borrowing limit values loaded from application configuration.
    /// This service supplies the rule parameters used by <see cref="RentRulesService"/>
    /// to validate rent requests for both employees and non‑employees.
    /// </summary>
    public class RentLimitService : IRentLimitService
    {
        /// <summary>
        /// The base maximum number of books allowed in a borrowing period.
        /// </summary>
        private readonly int nmc;

        /// <summary>
        /// The number of days defining the borrowing period.
        /// </summary>
        private readonly int per;

        /// <summary>
        /// The maximum number of books allowed in a single request.
        /// </summary>
        private readonly int c;

        /// <summary>
        /// The maximum number of books allowed from the same domain.
        /// </summary>
        private readonly int d;

        /// <summary>
        /// The number of months used to evaluate domain‑based limits.
        /// </summary>
        private readonly int l;

        /// <summary>
        /// The maximum number of extensions allowed.
        /// </summary>
        private readonly int lim;

        /// <summary>
        /// The cooldown period (in days) before borrowing the same book again.
        /// </summary>
        private readonly int delta;

        /// <summary>
        /// The daily borrowing limit for non‑employees.
        /// </summary>
        private readonly int ncz;

        /// <summary>
        /// The maximum number of books an employee may process in a single request.
        /// </summary>
        private readonly int persimp;

        // just for testing
        public RentLimitService(
        int nmc, int per, int c, int d, int l, int lim, int delta, int ncz, int persimp)
        {
            this.nmc = nmc;
            this.per = per;
            this.c = c;
            this.d = d;
            this.l = l;
            this.lim = lim;
            this.delta = delta;
            this.ncz = ncz;
            this.persimp = persimp;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RentLimitService"/> class.
        /// Loads all limit values from application configuration.
        /// </summary>
        public RentLimitService()
        {
            this.nmc = this.ReadInt("NMC");
            this.per = this.ReadInt("PER");
            this.c = this.ReadInt("C");
            this.d = this.ReadInt("D");
            this.l = this.ReadInt("L");
            this.lim = this.ReadInt("LIM");
            this.delta = this.ReadInt("DELTA");
            this.ncz = this.ReadInt("NCZ");
            this.persimp = this.ReadInt("PERSIMP");
        }

        /// <inheritdoc/>
        public int GetNMC(bool isEmployee)
            => isEmployee ? this.nmc * 2 : this.nmc;

        /// <inheritdoc/>
        public int GetPER(bool isEmployee)
            => isEmployee ? Math.Max(1, this.per / 2) : this.per;

        /// <inheritdoc/>
        public int GetC(bool isEmployee)
            => isEmployee ? this.c * 2 : this.c;

        /// <inheritdoc/>
        public int GetD(bool isEmployee)
            => isEmployee ? this.d * 2 : this.d;

        /// <inheritdoc/>
        public int GetL(bool isEmployee)
            => this.l;

        /// <inheritdoc/>
        public int GetLIM(bool isEmployee)
            => isEmployee ? this.lim * 2 : this.lim;

        /// <inheritdoc/>
        public int GetDELTA(bool isEmployee)
            => isEmployee ? Math.Max(1, this.delta / 2) : this.delta;

        /// <inheritdoc/>
        public int GetNCZ(bool isEmployee)
            => isEmployee ? int.MaxValue : this.ncz;

        /// <inheritdoc/>
        public int GetPERSIMP()
            => this.persimp;

        /// <summary>
        /// Reads an integer configuration value from <c>AppSettings</c>.
        /// Returns a default value if the key is missing or invalid.
        /// </summary>
        /// <param name="key">The configuration key to read.</param>
        /// <param name="defaultValue">The fallback value if parsing fails.</param>
        /// <returns>The parsed integer value or the default value.</returns>
        private int ReadInt(string key, int defaultValue = 0)
        {
            var value = ConfigurationManager.AppSettings[key];
            return int.TryParse(value, out int result) ? result : defaultValue;
        }
    }
}
