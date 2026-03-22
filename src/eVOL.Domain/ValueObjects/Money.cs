namespace eVOL.Domain.ValueObjects
{
    public class Money
    {
        public double Balance { get; private set; } 
        public string Currency { get; private set;}
        
        public Money(double balance, string currency)
        {

            if (balance < 0)
            {
                throw new ArgumentException("Balance can't be negative");
            }

            if (string.IsNullOrEmpty(currency)) throw new ArgumentException("Currency is required");


            Balance = balance;
            Currency = currency;
        }

        private Money() { }
    }
}
