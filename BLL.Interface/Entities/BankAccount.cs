﻿using System;

namespace BLL.Interface
{
    /// <summary>
    /// Represents a bank account with IBAN, owner, balance, bonus points and account type
    /// </summary>
    public abstract class BankAccount
    {
        #region fields
        /// <summary>
        /// an amount of money that considers effective for a bank
        /// </summary>
        internal const ushort EffectiveAmount = 10000;

        /// <summary>
        /// maximum possible bonus points
        /// </summary>
        private const ushort MaxBonusPoints = 100;

        /// <summary>
        /// Bonus points of an account.
        /// </summary>
        private float bonusPoints;
        #endregion

        #region constructors
        /// <summary>
        /// Creates a bank account instance with specified IBAN, owner, balance and bonus points.
        /// </summary>
        /// <param name="iban">International Bank Account Number</param>
        /// <param name="owner">owner</param>
        /// <param name="balance">balance</param>
        /// <param name="bonusPoints">bonus points</param>
        protected BankAccount(string iban, AccountOwner owner, decimal balance, float bonusPoints, BankAccountStatus status)
        {
            if (string.IsNullOrWhiteSpace(iban))
            {
                throw new ArgumentException("String is not significant.", nameof(iban));
            }

            if (bonusPoints < 0 || bonusPoints > MaxBonusPoints)
            {
                throw new ArgumentOutOfRangeException("bonusPoints", $"Bonus points range is from 0 to {MaxBonusPoints}.");
            }

            this.IBAN = iban;
            this.Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            this.Balance = balance;
            this.BonusPoints = bonusPoints;
            this.Status = status;
        }
        #endregion

        public enum BankAccountStatus
        {
            Active,
            Inactive
        }

        #region properties
        public static IBonusPointsCalculator Calculator { get; set; }

        /// <summary>
        /// International Bank Account Number of an account.
        /// </summary>
        public string IBAN { get; private set; }

        /// <summary>
        /// Owner of an account.
        /// </summary>
        public AccountOwner Owner { get; private set; }

        /// <summary>
        /// Gets an account balance.
        /// </summary>
        public decimal Balance { get; private set; }

        /// <summary>
        /// Gets publicly and sets privately account bonus points using calculation.
        /// </summary>
        public float BonusPoints
        {
            get => (float)Math.Round(bonusPoints, 2);
            set => bonusPoints = value < 0 ? 0 : value > MaxBonusPoints ? MaxBonusPoints : value;
        }

        public BankAccountStatus Status { get; private set; }

        /// <summary>
        /// 'value' of a balance
        /// </summary>
        protected internal byte BalanceValue { get; protected set; }

        /// <summary>
        /// 'value' of a deposit
        /// </summary>
        protected internal byte DepositValue { get; protected set; }
        #endregion

        #region methods
        /// <summary>
        /// Gets full account info string.
        /// </summary>
        /// <returns>account info as vertical list</returns>
        public override string ToString()
        {
            return string.Join(Environment.NewLine, $"IBAN: {IBAN}", $"Owner: {Owner}", $"Balance: {Balance.ToString("C")}", $"Bonus points: {BonusPoints}");
        }

        public decimal Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("deposit amount must be greater than zero");
            }

            Balance += amount;
            BonusPoints += Calculator.CalculateDepositBonus(this, amount);
            return Balance;
        }

        public decimal Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("withdraw amount must be greater than zero");
            }

            if (Balance < amount)
            {
                throw new ArgumentException("balance is lesser than withdrawal amount");
            }

            Balance -= amount;
            BonusPoints -= Calculator.CalculateWithdrawalBonus(this, amount);
            return Balance;
        }
        #endregion
    }
}
