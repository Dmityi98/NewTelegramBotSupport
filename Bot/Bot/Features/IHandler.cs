
namespace SupportBot.Features
{
    public interface IHendler<Type>
    {
        Task Hendle(Type request, CancellationToken cancellationToken);
    }
}