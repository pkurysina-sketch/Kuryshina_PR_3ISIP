using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarConfigurator.Services
{
    internal class ConfiguratorData
    {
        public static string ModelName { get; set; } = "Sedan Basic";
        public static decimal BasePrice { get; set; } = 1_000_000m;
        public static decimal EnginePrice { get; set; }
        public static decimal ColorPrice { get; set; }
        public static decimal OptionsPrice { get; set; }
        public static decimal TotalPrice { get; set; }
        public static decimal DownPayment { get; set; }
        public static decimal MonthlyPayment { get; set; }
        public static int TermMonths { get; set; } = 60;

        public static string FullName { get; set; } = "";
        public static string Phone { get; set; } = "";
        public static string Email { get; set; } = "";

        public static void Reset()
        {
            EnginePrice = 0;
            ColorPrice = 0;
            OptionsPrice = 0;
            TotalPrice = 0;
        }
    }
}

