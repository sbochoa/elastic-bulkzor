using System;
using System.Threading.Tasks;

namespace Bulkzor
{
    public class Bulkzor
    {
        public static int NumberOfWorkers { get; set; } = Environment.ProcessorCount;
        public static void RunBulks(params BulkTask[] bulks)
        {
            Parallel.ForEach(bulks, new ParallelOptions() { MaxDegreeOfParallelism = NumberOfWorkers }, bulk =>
            {
                bulk.Run();
            });
        }
    }
}
