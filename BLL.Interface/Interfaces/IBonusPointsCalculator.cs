using BLL.Interface;

namespace BLL.Interface
{
    /// <summary>
    /// Calculates account bonus points for deposit/withdrawal
    /// </summary>
    public interface IBonusPointsCalculator
    {
        /// <summary>
        /// calculates deposit bonus points for <paramref name="account"/>
        /// </summary>
        /// <param name="account">bank account</param>
        /// <param name="depositAmount">deposit amount</param>
        /// <returns>bonus points</returns>
        float CalculateDepositBonus(BankAccount account, decimal depositAmount);

        /// <summary>
        /// calculates withdrawal bonus points for <paramref name="account"/>
        /// </summary>
        /// <param name="account">bank account</param>
        /// <param name="withdrawalAmount">withdrawal amount</param>
        /// <returns>bonus points</returns>
        float CalculateWithdrawalBonus(BankAccount account, decimal withdrawalAmount);
    }
}
