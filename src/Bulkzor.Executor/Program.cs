using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulkzor.Executor
{
    class Program
    {
        static void Main(string[] args)
        {
            var configurationFilePath = args[0];

            var tasks = new ConfigurationFileReader(configurationFilePath).CreateTasks();

            Bulkzor.RunBulks(tasks.ToArray());
        }
    }
}
