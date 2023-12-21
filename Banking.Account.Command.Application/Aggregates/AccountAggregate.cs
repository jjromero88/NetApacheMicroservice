using Banking.Account.Command.Application.Features.BankAccounts.Commands.OpenAccount;
using Banking.Cqrs.Core.Domain;
using Banking.Cqrs.Core.Events;

namespace Banking.Account.Command.Application.Aggregates
{
    public class AccountAggregate : AggregateRoot
    {
        public bool Active { get; set; }
        public double Balance { get; set; }

        public AccountAggregate()
        {
        }

        public AccountAggregate(OpenAccountCommand comand)
        {
            var accountOpenedEvent = new AccountOpenedEvent(
                    comand.Id,
                    comand.AccountHolder,
                    comand.AccountType,
                    DateTime.Now,
                    comand.OpeningBalance
                );

            RaiseEvent(accountOpenedEvent);
        }

        public void Apply(AccountOpenedEvent @event)
        {
            Id = @event.Id;
            Active = true;
            Balance = @event.OpeningBalance;
        }

        public void DepositFunds(double amount)
        {
            if (!Active)
            {
                throw new Exception("Los fondos no pueden ser depositados en una cuenta cancelada");
            }

            if (amount <= 0)
            {
                throw new Exception("El depósito de dinero debe ser mayor que cero");
            }

            var fundsDepositEvent = new FundsDepositedEvent(Id)
            {
                Id = Id,
                Amount = amount
            };

            RaiseEvent(fundsDepositEvent);

        }

        public void Apply(FundsDepositedEvent @event)
        {
            Id = @event.Id;
            Balance += @event.Amount;
        }

        public void WithDrawFunds(double amount)
        {
            if (!Active)
            {
                throw new Exception("La cuenta bancaria esta cerrada");
            }

            var fundsWithDrawEvent = new FundsWithdrawnEvent(Id)
            {
                Id = Id,
                Amount = amount
            };

            RaiseEvent(fundsWithDrawEvent);
        }

        public void Apply(FundsWithdrawnEvent @event)
        {
            Id = @event.Id;
            Balance -= @event.Amount;
        }

        public void CloseAccount()
        {
            if (!Active)
            {
                throw new Exception("La cuenta está cerrada");
            }

            var accountClosedEvent = new AccountClosedEvent(Id);
            RaiseEvent(@accountClosedEvent);
        }

        public void Apply(AccountClosedEvent @event)
        {
            Id = @event.Id;
            Active = false;
        }

    }
}
