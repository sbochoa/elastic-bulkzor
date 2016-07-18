using System.Linq;
using Bulkzor.Executor.Helpers;

namespace Bulkzor.Executor
{
    class Program
    {
        static void Main(string[] args)
        {
            var configurationFilePath = args[0];

            var tasks = new ConfigurationFileReader(configurationFilePath, new FileManager()).CreateTasks();

            Bulkzor.RunBulks(tasks.ToArray());
        }
    }
}
