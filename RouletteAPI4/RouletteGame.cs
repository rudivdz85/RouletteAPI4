
using NUnit.Framework.Internal;
using RouletteAPI4.Interfaces;

namespace RouletteAPI4
{
    public class RouletteGame : IRouletteGame
{
        private static int spinIndex = 0;
    private static RouletteGame instance;
    private static List<string> previousSpins = new List<string>();
    private static List<Bet> bets = new List<Bet>();
    private bool isSpinning = false;
        private static decimal totalCurrentPayout = 0;  

        private IBetRepository _betRepository;

        public RouletteGame(IBetRepository betRepository)
        {
            _betRepository = betRepository;
        }

        public void PlaceBet(Bet bet)
    {
        if (isSpinning)
        {
            throw new Exception("Cannot place bet while the wheel is spinning");
        }

        bets.Add(bet);
            _betRepository.AddBet(bet);
        }

        public string GetSavedBetsFromDB()
        {
            string betsFromDBString = "";
            List<Bet> betsFromDB = _betRepository.GetAllBets();
            foreach (Bet bet in betsFromDB)
            {
                betsFromDBString += "Bet ID: " + bet.Id + " Payout was " + bet.Payout;
            }
            return betsFromDBString;
        }

        public List<Bet> GetBets()
        {
            return bets;
        }
    public void Spin()
    {
            totalCurrentPayout = 0;
            spinIndex++;
            if (isSpinning)
        {
            throw new Exception("Cannot spin the wheel while it is already spinning");
        }

        // Generate a random number between 0 and 36 (inclusive) to represent the winning number on the roulette wheel.
        Random random = new Random();
        int winningNumber = random.Next(0, 37);

        // Add the winning number to the list of previous spins.
        previousSpins.Add("Spin number " + spinIndex.ToString() + " : The winning number was " +  winningNumber.ToString());

        // Iterate over all bets and calculate the payout for each one.
        foreach (Bet bet in bets)
        {
            switch (bet.Type)
            {
                    

                    case BetType.Split:
                        string[] splitValues = bet.Value.Split('-');
                        int firstNumber = int.Parse(splitValues[0]);
                        int secondNumber = int.Parse(splitValues[1]);
                        if ((winningNumber == firstNumber) || (winningNumber == secondNumber))
                        {
                            // Bet on a split: payout is 17 to 1
                            bet.Payout = bet.Amount * 17;
                        }
                        else
                        {
                            // Bet on a split but didn't win: no payout
                            bet.Payout = 0;
                        }
                        break;

                    case BetType.Street:
                        int streetNumber = int.Parse(bet.Value);
                        if ((winningNumber >= streetNumber && winningNumber <= streetNumber + 2) && (winningNumber % 3 == 1))
                        {
                            // Bet on a street: payout is 11 to 1
                            bet.Payout = bet.Amount * 11;
                        }
                        else
                        {
                            // Bet on a street but didn't win: no payout
                            bet.Payout = 0;
                        }
                        break;

                    case BetType.Corner:
                        string[] cornerValues = bet.Value.Split('-');
                        int cornerStart = int.Parse(cornerValues[0]);
                        if ((winningNumber == cornerStart) || (winningNumber == cornerStart + 1) || (winningNumber == cornerStart + 3) || (winningNumber == cornerStart + 4))
                        {
                            // Bet on a corner: payout is 8 to 1
                            bet.Payout = bet.Amount * 8;
                        }
                        else
                        {
                            // Bet on a corner but didn't win: no payout
                            bet.Payout = 0;
                        }
                        break;

                    case BetType.FiveNumber:
                        if (winningNumber == 0 || winningNumber == 00 || (winningNumber >= 1 && winningNumber <= 3))
                        {
                            // Bet on 0, 00, 1, 2, or 3: payout is 6 to 1
                            bet.Payout = bet.Amount * 6;
                        }
                        else
                        {
                            // Bet on any other number: no payout
                            bet.Payout = 0;
                        }
                        break;

                    case BetType.Line:
                        int lineStart = int.Parse(bet.Value);
                        if ((lineStart == 1 && winningNumber >= 1 && winningNumber <= 6) ||
                            (lineStart == 4 && winningNumber >= 4 && winningNumber <= 9) ||
                            (lineStart == 7 && winningNumber >= 7 && winningNumber <= 12) ||
                            (lineStart == 10 && winningNumber >= 10 && winningNumber <= 15) ||
                            (lineStart == 13 && winningNumber >= 13 && winningNumber <= 18) ||
                            (lineStart == 16 && winningNumber >= 16 && winningNumber <= 21) ||
                            (lineStart == 19 && winningNumber >= 19 && winningNumber <= 24) ||
                            (lineStart == 22 && winningNumber >= 22 && winningNumber <= 27) ||
                            (lineStart == 25 && winningNumber >= 25 && winningNumber <= 30) ||
                            (lineStart == 28 && winningNumber >= 28 && winningNumber <= 33) ||
                            (lineStart == 31 && winningNumber >= 31 && winningNumber <= 36))
                        {
                            // Bet on a line of numbers: payout is 5 to 1
                            bet.Payout = bet.Amount * 5;
                        }
                        else
                        {
                            // Bet on any other number: no payout
                            bet.Payout = 0;
                        }
                        break;

                    case BetType.Dozen:
                        int dozen = int.Parse(bet.Value);
                        if ((dozen == 1 && winningNumber >= 1 && winningNumber <= 12) ||
                            (dozen == 2 && winningNumber >= 13 && winningNumber <= 24) ||
                            (dozen == 3 && winningNumber >= 25 && winningNumber <= 36))
                        {
                            // Bet on a dozen: payout is 2 to 1
                            bet.Payout = bet.Amount * 2;
                        }
                        else
                        {
                            // Bet on any other number: no payout
                            bet.Payout = 0;
                        }
                        break;

                    case BetType.StraightUp:
                        if (bet.Value == winningNumber.ToString())
                        {
                            // Bet on a specific number: payout is 35 to 1
                            bet.Payout = bet.Amount * 35;
                        }
                        else
                        {
                            // Bet on a specific number but didn't win: no payout
                            bet.Payout = 0;
                        }
                        break;

                    case BetType.Column:
                        int column = (winningNumber - 1) % 3 + 1;
                        if (bet.Value == column.ToString())
                        {
                            // Bet on a column: payout is 2 to 1
                            bet.Payout = bet.Amount * 2;
                        }
                        else
                        {
                            // Bet on a column but didn't win: no payout
                            bet.Payout = 0;
                        }
                        break;

                    case BetType.EvenMoney:
                        if ((winningNumber % 2 == 0 && bet.Value == "even") ||
                            (winningNumber % 2 == 1 && bet.Value == "odd"))
                        {
                            // Bet on even or odd: payout is 1 to 1
                            bet.Payout = bet.Amount * 2;
                        }
                        else if ((winningNumber <= 18 && bet.Value == "low") ||
                                 (winningNumber >= 19 && bet.Value == "high"))
                        {
                            // Bet on low or high: payout is 1 to 1
                            bet.Payout = bet.Amount * 2;
                        }
                        else if ((winningNumber % 2 == 0 && bet.Value == "red") ||
                                 (winningNumber % 2 == 1 && bet.Value == "black"))
                        {
                            // Bet on red or black: payout is 1 to 1
                            bet.Payout = bet.Amount * 2;
                        }
                        else
                        {
                            // Bet on even money but didn't win: no payout
                            bet.Payout = 0;
                        }
                        break;

                    case BetType.Red:
                        if (winningNumber % 2 == 1 && bet.Value == "red")
                        {
                            // Bet on red: payout is 1 to 1
                            bet.Payout = bet.Amount * 2;
                        }
                        else
                        {
                            // Bet on red but didn't win: no payout
                            bet.Payout = 0;
                        }
                        break;

                    case BetType.Black:
                        if (winningNumber % 2 == 0 && bet.Value == "black")
                        {
                            // Bet on black: payout is 1 to 1
                            bet.Payout = bet.Amount * 2;
                        }
                        else
                        {
                            // Bet on black but didn't win: no payout
                            bet.Payout = 0;
                        }
                        break;

                    case BetType.Low:
                        if (winningNumber >= 1 && winningNumber <= 18)
                        {
                            // Bet on low numbers (1-18): payout is 1 to 1
                            bet.Payout = bet.Amount * 2;
                        }
                        else
                        {
                            // Bet on low numbers but didn't win: no payout
                            bet.Payout = 0;
                        }
                        break;

                    case BetType.High:
                        if (winningNumber >= 19 && winningNumber <= 36)
                        {
                            // Bet on high numbers (19-36): payout is 1 to 1
                            bet.Payout = bet.Amount * 2;
                        }
                        else
                        {
                            // Bet on high numbers but didn't win: no payout
                            bet.Payout = 0;
                        }
                        break;

                    case BetType.Odd:
                        if (winningNumber % 2 == 1)
                        {
                            // Bet on odd numbers: payout is 1 to 1
                            bet.Payout = bet.Amount * 2;
                        }
                        else
                        {
                            // Bet on odd numbers but didn't win: no payout
                            bet.Payout = 0;
                        }
                        break;

                    case BetType.Even:
                        if (winningNumber % 2 == 0)
                        {
                            // Bet on even numbers: payout is 1 to 1
                            bet.Payout = bet.Amount * 2;
                        }
                        else
                        {
                            // Bet on even numbers but didn't win: no payout
                            bet.Payout = 0;
                        }
                        break;
                }

            }
        
        isSpinning = false;
          
    }

        public void ClearBets()
        {
            bets.Clear();
          
        }

        public void Payout()
        {

        }

    public List<string> ShowPreviousSpins()
    {
        return previousSpins; 
    }

    }

}