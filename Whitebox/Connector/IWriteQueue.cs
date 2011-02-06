namespace Whitebox.Connector
{
    public interface IWriteQueue
    {
        void Enqueue(object message);
    }
}