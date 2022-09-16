using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subsogator.Common.GlobalConstants
{
    public class ConnectionConstants
    {
        public const string DatabaseConnectionString = @"C:\Users\Plamenna Petrova\AppData\Roaming\Microsoft\" +
            @"UserSecrets\8bdd44b8-a905-42d2-93e9-c85003dab6d1";

        public const string SecretsJSONFileName = "secrets.json";

        public const string SecretsJSONConnectionStringSection = "DefaultConnection:ConnectionString";
    }
}
