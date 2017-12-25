using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    /// <summary>
    /// calculates account bonus points
    /// </summary>
    public class BonusCalculator : IBonusPointsCalculator
    {
        /// <summary>
        /// calculates deposit bonus points for <paramref name="account"/>
        /// </summary>
        /// <param name="account">bank account</param>
        /// <param name="depositAmount">deposit amount</param>
        /// <returns>bonus points</returns>
        public float CalculateDepositBonus(BankAccount account, decimal depositAmount)
        {
            return (float)depositAmount / BankAccount.EffectiveAmount * (account.BalanceValue + account.DepositValue);
        }

        /// <summary>
        /// calculates withdrawal bonus points for <paramref name="account"/>
        /// </summary>
        /// <param name="account">bank account</param>
        /// <param name="withdrawalAmount">withdrawal amount</param>
        /// <returns>bonus points</returns>
        public float CalculateWithdrawalBonus(BankAccount account, decimal withdrawalAmount)
        {
            return (float)withdrawalAmount / BankAccount.EffectiveAmount * account.BalanceValue;
        }
    }
}
