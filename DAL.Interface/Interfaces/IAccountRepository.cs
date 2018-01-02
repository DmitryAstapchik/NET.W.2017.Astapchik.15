using System.Collections.Generic;

namespace DAL.Interface
{
    /// <summary>
    /// Allows to work with a storage of bank accounts
    /// </summary>
    public interface IAccountRepository
    {
        /// <summary>
        /// Adds an account to a storage
        /// </summary>
        /// <param name="account">an account to add</param>
        void Create(BankAccountDTO account);

        /// <summary>
        /// Removes an account from a storage
        /// </summary>
        /// <param name="iban">IBAN of an account</param>
        /// <returns>account balance</returns>
        void Delete(BankAccountDTO account);

        /// <summary>
        /// Gets an account from a storage
        /// </summary>
        /// <param name="iban">IBAN of an account</param>
        /// <returns>bank account instance</returns>
        BankAccountDTO GetByIban(string iban);

        /// <summary>
        /// Rewrites an account in a storage
        /// </summary>
        /// <param name="account">account to rewrite</param>
        void Update(BankAccountDTO account);

        IEnumerable<BankAccountDTO> GetByOwner(AccountOwnerDTO owner);
    }
}
