using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BLL.Interface;
using DAL.Interface;

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
        private IAccountRepository repository;

        /// <summary>
        /// Creates a new bank instance with specified storage, IBAN generator and bonus points calculator
        /// </summary>
        /// <param name="repository">bank accounts storage</param>
        /// <param name="generator">IBAN generator</param>
        public Bank(IAccountRepository repository, IIBANGenerator ibanGenerator, IBonusPointsCalculator calculator)
        {
            this.repository = repository;
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

            var acc = repository.GetByIban(iban);
            repository.Delete(acc);
            return acc.Balance;
        }

        public IEnumerable<BankAccount> GetUserAccounts(string email)
        {
            foreach (var account in this.repository.GetUserAccounts(email))
            {
                yield return account.FromDTO();
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

            var account = repository.GetByIban(iban).FromDTO();
            account.Deposit(amount);
            account.BonusPoints += bonusCalculator.CalculateDepositBonus(account, amount);
            repository.Update(account.ToDTO());

            return account.Balance;
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

            var account = BankAccountMapper.FromDTO(repository.GetByIban(iban));

            if (amount > account.Balance)
            {
                throw new InvalidOperationException("account balance is lesser than withdrawal amount");
            }

            account.Withdraw(amount);
            account.BonusPoints -= bonusCalculator.CalculateWithdrawalBonus(account, amount);
            repository.Update(account.ToDTO());

            return account.Balance;
        }

        /// <summary>
        /// Opens a new account for <paramref name="holder"/> with <paramref name="startBalance"/>.
        /// </summary>
        /// <param name="holder">person's full name</param>
        /// <param name="startBalance">first deposit amount</param>
        /// <returns>IBAN of a new account</returns>
        /// <exception cref="ArgumentException">Start balance is lesser than minimal.</exception>
        public string OpenAccount(string holder, decimal startBalance)
        {
            if (string.IsNullOrWhiteSpace(holder))
            {
                throw new ArgumentException("No significant characters are given.", "holder");
            }

            //if (startBalance < MinDeposit)
            //{
            //    throw new ArgumentException($"Cannot create a bank account with balance lesser than {MinDeposit}");
            //}

            BankAccount account;
            if (startBalance < 1000)
            {
                account = new StandardAccount(ibanGenerator.GenerateIBAN(), holder, startBalance, bonusPoints: 0);
            }
            else if (startBalance < 10000)
            {
                account = new GoldAccount(ibanGenerator.GenerateIBAN(), holder, startBalance, bonusPoints: 5);
            }
            else
            {
                account = new PlatinumAccount(ibanGenerator.GenerateIBAN(), holder, startBalance, bonusPoints: 10);
            }

            repository.Create(account.ToDTO());

            return account.IBAN;
        }
    }
}