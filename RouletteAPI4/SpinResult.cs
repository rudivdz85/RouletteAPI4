namespace RouletteAPI4
{
    public class SpinResult
    {
        public string Result { get; set; }
        public List<Bet> WinningBets { get; set; }

        public SpinResult(string result, List<Bet> winningBets)
        {
            Result = result;
            WinningBets = winningBets;
        }
    }
}
