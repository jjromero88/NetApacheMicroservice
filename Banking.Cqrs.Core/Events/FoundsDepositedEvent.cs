
namespace Banking.Cqrs.Core.Events
{
    public class FoundsDepositedEvent : BaseEvent
    {
        public double Amount { get; set; }
        public FoundsDepositedEvent(string id) : base(id)
        {
        }
    }
}
