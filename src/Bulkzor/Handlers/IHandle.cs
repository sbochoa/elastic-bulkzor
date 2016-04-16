namespace Bulkzor.Handlers
{
    public interface IHandle<in T, out TR>
    {
        TR Handle(T message);
    }
}
