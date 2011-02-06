namespace Whitebox.Core.Application
{
    public interface IApplicationEventQueue
    {
        void Enqueue(object applicationEvent);
    }
}
