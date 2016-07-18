namespace Bulkzor.Executor.Configurations
{
    public interface IBulkTaskConfiguration
    {
        BulkTask CreateTask(string taskName);
    }
}