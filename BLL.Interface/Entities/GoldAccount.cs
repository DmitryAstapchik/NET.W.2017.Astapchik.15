namespace BLL.Interface
{
    /// <summary>
    /// A bank account of gold type
    /// </summary>
    public class GoldAccount : BankAccount
    {
        /// <summary>
        /// constructs a gold account with specified IBAN, owner, balance and bonus points
        /// </summary>
        /// <param name="iban">account IBAN</param>
        /// <param name="owner">account owner</param>
        /// <param name="balance">account balance</param>
        /// <param name="bonusPoints">account bonus points</param>
        public GoldAccount(string iban, string owner, decimal balance, float bonusPoints) : base(iban, owner, balance, bonusPoints)
        {
            this.BalanceValue = 5;
            this.DepositValue = 4;
        }
    }
}
