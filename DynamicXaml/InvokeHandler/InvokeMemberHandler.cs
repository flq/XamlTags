namespace DynamicXaml
{
    public interface InvokeMemberHandler
    {
        bool CanHandle(InvokeContext callContext);
        void Handle(InvokeContext ctx);
    }
}