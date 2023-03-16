using System.Data.SQLite;
using System.Data;
using Dapper;
using RouletteAPI4.Interfaces;

namespace RouletteAPI4
{

    public class BetRepository : IBetRepository
{
    private readonly IDbConnection _dbConnection;

    public BetRepository(string connectionString)
    {
        _dbConnection = new SQLiteConnection(connectionString);
        _dbConnection.Open();

        // create table if not exists
        _dbConnection.Execute(@"CREATE TABLE IF NOT EXISTS Bets
                                (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    PlayerId TEXT NOT NULL,
                                    Amount DECIMAL(10,2) NOT NULL,
                                    Type INT NOT NULL,
                                    Value TEXT NOT NULL,
                                    IsWinningBet INTEGER NOT NULL,
                                    Payout DECIMAL(10,2) NOT NULL
                                )");
    }

    public void AddBet(Bet bet)
    {
        using var transaction = _dbConnection.BeginTransaction();

        try
        {
            // insert the bet record
            _dbConnection.Execute(@"INSERT INTO Bets (PlayerId, Amount, Type, Value, IsWinningBet, Payout)
                                     VALUES (@PlayerId, @Amount, @Type, @Value, @IsWinningBet, @Payout)",
                                    new
                                    {
                                        PlayerId = bet.PlayerId,
                                        Amount = bet.Amount,
                                        Type = (int)bet.Type,
                                        Value = bet.Value,
                                        IsWinningBet = bet.IsWinningBet ? 1 : 0,
                                        Payout = bet.Payout
                                    });

            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw ex;
        }
    }

    public List<Bet> GetAllBets()
    {
        return _dbConnection.Query<Bet>("SELECT * FROM Bets").ToList();
    }

    public void ClearAllBets()
    {
        _dbConnection.Execute("DELETE FROM Bets");
    }

    public void Dispose()
    {
        _dbConnection.Dispose();
    }
}

}