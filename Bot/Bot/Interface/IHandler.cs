namespace Bot.Interface
{
    public interface IHendler<Type>
    {
        Task Hendle(Type request, CancellationToken cancellationToken);
    }
}