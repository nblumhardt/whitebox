namespace Whitebox.Core.Application
{
    public interface IApplicationEventHandler<in TEvent>
    {
        void Handle(TEvent applicationEvent);
    }
}
