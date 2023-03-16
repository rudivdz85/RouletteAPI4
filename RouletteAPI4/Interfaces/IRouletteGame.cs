namespace RouletteAPI4.Interfaces
{
    public interface IRouletteGame
    {
        void PlaceBet(Bet bet);
        void Spin();
        void Payout();
        List<string> ShowPreviousSpins();
        string GetSavedBetsFromDB();
        List<Bet> GetBets();
        void ClearBets();
    }
}
