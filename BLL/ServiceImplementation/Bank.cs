using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BLL.Interface;
using DAL.Interface;
using DAL.Interface.Interfaces;

namespace BLL
{
    /// <summary>
    /// Represents a bank with functionality of making deposits/withdrawals, creating/closing accounts.
    /// </summary>
    public class Bank : IAccountService
    {
        /// <summary>
        /// Minimum amount of deposit.
        /// </summary>
        private const ushort MinDeposit = 50;

        /// <summary>
        /// Minimum amount of withdrawal.
        /// </summary>
        private const ushort MinWithdrawal = 10;

        /// <summary>
        /// bonus points calculator
        /// </summary>
        private IBonusPointsCalculator bonusCalculator;

        /// <summary>
        /// Bank's IBAN generator
        /// </summary>
        private IIBANGenerator ibanGenerator;

        /// <summary>
        /// bank accounts storage
        /// </summary>
        private IAccountsUnitOfWork accountsRepo;

        /// <summary>
        /// Creates a new bank instance with specified storage, IBAN generator and bonus points calculator
        /// </summary>
        /// <param name="uow">bank accounts storage</param>
        /// <param name="generator">IBAN generator</param>
        public Bank(IAccountsUnitOfWork uow, IIBANGenerator ibanGenerator, IBonusPointsCalculator calculator)
        {
            this.accountsRepo = uow;
            this.ibanGenerator = ibanGenerator;
            this.bonusCalculator = calculator;
        }

        /// <summary>
        /// Removes an account with specified <paramref name="iban"/> from accounts file
        /// </summary>
        /// <param name="iban">IBAN of an account to close</param>
        /// <returns>account balance</returns>
        public decimal CloseAccount(string iban)
        {
            if (string.IsNullOrWhiteSpace(iban))
            {
                throw new ArgumentException("No IBAN is given.", "IBAN");
            }

            var acc = accountsRepo.Accounts.GetByIban(iban);
            acc.Status = BankAccountDTO.AccountStatus.Inactive;
            accountsRepo.Accounts.Update(acc);
            accountsRepo.Save();

            return acc.Balance;
        }

        public IEnumerable<BankAccount> GetPersonalAccounts(AccountOwner owner)
        {
            var accounts = this.accountsRepo.Accounts.GetByOwner(new AccountOwnerDTO { PassportID = owner.PassportID, FullName = owner.FullName, Email = owner.Email });
            if (accounts != null)
            {
                foreach (var account in accounts)
                {
                    yield return account.FromDTO();
                }
            }
        }

        /// <summary>
        /// Makes a deposit of <paramref name="amount"/> to an account with specified <paramref name="iban"/>.
        /// </summary>
        /// <param name="iban">IBAN of an account to make deposit</param>
        /// <param name="amount">deposit amount</param>
        /// <returns>new account balance</returns>
        /// <exception cref="ArgumentException">deposit amount is lesser than minimal</exception>
        public decimal MakeDeposit(string iban, decimal amount)
        {
            if (string.IsNullOrWhiteSpace(iban))
            {
                throw new ArgumentException("No IBAN is given.", "iban");
            }

            if (amount < MinDeposit)
            {
                throw new ArgumentException("Minimum deposit amount is " + MinDeposit.ToString("C"));
            }

            var account = accountsRepo.Accounts.GetByIban(iban).FromDTO();
            account.Deposit(amount);
            account.BonusPoints += bonusCalculator.CalculateDepositBonus(account, amount);
            accountsRepo.Accounts.Update(account.ToDTO());
            accountsRepo.Save();

            return account.Balance;
        }

        public void MakeTransfer(string fromIBAN, string toIBAN, decimal amount)
        {
            var from = accountsRepo.Accounts.GetByIban(fromIBAN).FromDTO();
            var to = accountsRepo.Accounts.GetByIban(toIBAN).FromDTO();
            from.Withdraw(amount);
            to.Deposit(amount);
            accountsRepo.Accounts.Update(from.ToDTO());
            accountsRepo.Accounts.Update(to.ToDTO());
            accountsRepo.Save();
        }

        /// <summary>
        /// Makes a withdrawal of <paramref name="amount"/> from an account with specified <paramref name="iban"/>
        /// </summary>
        /// <param name="iban">IBAN of an account to make withdrawal</param>
        /// <param name="amount">withdrawal amount</param>
        /// <returns>new account balance</returns>
        /// <exception cref="ArgumentException">withdrawal amount is lesser than minimal</exception>
        public decimal MakeWithdrawal(string iban, decimal amount)
        {
            if (string.IsNullOrWhiteSpace(iban))
            {
                throw new ArgumentException("No IBAN is given.", "IBAN");
            }

            if (amount < MinWithdrawal)
            {
                throw new ArgumentException("Minimum withdrawal amount is " + MinWithdrawal.ToString("C"));
            }

            var account = accountsRepo.Accounts.GetByIban(iban).FromDTO();

            if (amount > account.Balance)
            {
                throw new InvalidOperationException("account balance is lesser than withdrawal amount");
            }

            account.Withdraw(amount);
            account.BonusPoints -= bonusCalculator.CalculateWithdrawalBonus(account, amount);
            accountsRepo.Accounts.Update(account.ToDTO());
            accountsRepo.Save();

            return account.Balance;
        }

        /// <summary>
        /// Opens a new account for <paramref name="owner"/> with <paramref name="startBalance"/>.
        /// </summary>
        /// <param name="owner">person's full name</param>
        /// <param name="startBalance">first deposit amount</param>
        /// <returns>IBAN of a new account</returns>
        /// <exception cref="ArgumentException">Start balance is lesser than minimal.</exception>
        public string OpenAccount(AccountOwner owner, decimal startBalance)
        {
            if (owner == null)
            {
                throw new ArgumentNullException(nameof(owner));
            }

            //if (startBalance < MinDeposit)
            //{
            //    throw new ArgumentException($"Cannot create a bank account with balance lesser than {MinDeposit}");
            //}

            BankAccount account;
            if (startBalance < 1000)
            {
                account = new StandardAccount(ibanGenerator.GenerateIBAN(), owner, startBalance, bonusPoints: 0);
            }
            else if (startBalance < 10000)
            {
                account = new GoldAccount(ibanGenerator.GenerateIBAN(), owner, startBalance, bonusPoints: 5);
            }
            else
            {
                account = new PlatinumAccount(ibanGenerator.GenerateIBAN(), owner, startBalance, bonusPoints: 10);
            }

            accountsRepo.Accounts.Create(account.ToDTO());
            accountsRepo.Save();

            return account.IBAN;
        }
    }
}