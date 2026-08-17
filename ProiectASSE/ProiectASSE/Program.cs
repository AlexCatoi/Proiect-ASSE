// <copyright file="Program.cs" company="Transilvania University of Brasov">
// Catoi Mihai-Alexandru
// </copyright>

namespace ProiectASSE
{
    using System;
    using log4net;
    using log4net.Config;
    using log4net.Util;

    internal class Program
    {
        // Creezi loggerul pentru această clasă
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            // Activezi debugging intern (opțional)
            LogLog.InternalDebugging = true;
            System.Diagnostics.Debug.Listeners.Add(new System.Diagnostics.TextWriterTraceListener(Console.Out));

            // Configurezi log4net
            XmlConfigurator.Configure();

            // Scrii un log de test
            Log.Info("Aplicatia a pornit cu succes! Test log4net OK.");
            Log.Warn("Acesta este un mesaj de avertizare.");
            Log.Error("Acesta este un mesaj de eroare de test.");

            Console.WriteLine("Logurile au fost scrise. Verifica fisierul logfile.txt");
        }
    }
}
