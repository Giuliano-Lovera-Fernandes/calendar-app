using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dima.Core
{
    public static class Configuration
    {
        public const int DEFAULTSTATUSCODE = 200;
        public const int DefaultPageNumber = 1;
        public const int DefaultPageSize = 25;

        public static string ConnectionString { get; set; } = String.Empty;
        public static string BackendUrl { get; set; } = String.Empty;
        public static string FrontendUrl { get; set; } = String.Empty;

    }
}
