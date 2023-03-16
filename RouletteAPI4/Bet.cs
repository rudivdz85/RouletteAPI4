using System;
using System.Data.Entity.Core.Metadata.Edm;


namespace RouletteAPI4
{
    public enum BetType
    {
        StraightUp = 1,
        Split = 2,
        Street = 3,
        Corner = 4,
        FiveNumber = 5
        Line = 6,
        Dozen = 7,
        Column = 8,
        EvenMoney = 9,
        Red = 10,
        Black = 11,
        Low = 12,
        High = 13,
        Odd = 14,
        Even = 15
    }

    public class Bet
    {
        public int Id { get; set; }
        public string PlayerId { get; set; }
        public decimal Amount { get; set; }
        public BetType Type { get; set; }
        public string Value { get; set; }
        public bool IsWinningBet { get; set; }
        public decimal Payout { get; set; }

        public Bet(string playerId, decimal amount, BetType type, string value)
        {
            Random random = new Random();
            Id = random.Next(0, 999999);
            PlayerId = playerId;
            Amount = amount;
            Type = type;
            Value = value;
            IsWinningBet = false;
            Payout = 0;
        }

        public Bet()
        {

        }

        public Bet(int iD, string playerId, decimal amount, int type, string value, int isWinningBet, decimal payout)
        {

            Id = iD; 
            PlayerId = playerId;
            Amount = amount;
            Type = (BetType)type;
            Value = value;
            IsWinningBet = false;
            Payout = 0;
            Value = value;
            IsWinningBet = isWinningBet == 0 ? false : true;
            Payout = 0;
        }

        public void CalculatePayout(decimal payoutMultiplier)
        {
            Payout = Amount * payoutMultiplier;
            IsWinningBet = true;
        }
    }

}