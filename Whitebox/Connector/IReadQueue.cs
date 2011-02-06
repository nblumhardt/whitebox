namespace Whitebox.Connector
{
    public interface IReadQueue
    {
        bool TryDequeue(out object message);
    }
}