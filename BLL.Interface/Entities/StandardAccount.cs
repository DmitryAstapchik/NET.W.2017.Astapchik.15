namespace BLL.Interface
{
    /// <summary>
    /// A bank account of standard type
    /// </summary>
    public class StandardAccount : BankAccount
    {
        /// <summary>
        /// constructs a standard account with specified IBAN, owner, balance and bonus points
        /// </summary>
        /// <param name="iban">account IBAN</param>
        /// <param name="owner">account owner</param>
        /// <param name="balance">account balance</param>
        /// <param name="bonusPoints">account bonus points</param>
        public StandardAccount(string iban, AccountOwner owner, decimal balance, float bonusPoints, BankAccountStatus status = BankAccountStatus.Active) : base(iban, owner, balance, bonusPoints, status)
        {
            this.BalanceValue = 1;
            this.DepositValue = 3;
        }
    }
}
