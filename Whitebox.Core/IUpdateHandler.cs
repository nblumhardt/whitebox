namespace Whitebox.Core
{
    public interface IUpdateHandler<in TMessage>
    {
        void UpdateFrom(TMessage message);
    }
}
