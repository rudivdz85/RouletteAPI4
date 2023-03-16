namespace RouletteAPI4.Interfaces
{
    public interface IBetRepository
    {
        void AddBet(Bet bet);
        List<Bet> GetAllBets();
        void ClearAllBets();
    }
}
