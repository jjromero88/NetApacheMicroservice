using Banking.Cqrs.Core.Domain;

namespace Banking.Cqrs.Core.Handlers
{
    public interface EventSourcingHandler<T>
    {
        Task Save(AggregateRoot agregar);
        Task<T> GetById(string id);
    }
}
