using Microsoft.AspNetCore.Mvc;
using RouletteAPI4.Interfaces;
using System.Xml.Linq;

namespace RouletteAPI4 { 
public class RouletteController : ControllerBase
{
        private IBetRepository _betRepository;
        private  IRouletteGame game;

        public RouletteController(IBetRepository betRepository)
          {
        _betRepository = betRepository;
            game = new RouletteGame(betRepository);
        }

        [HttpGet]
        [Route("GetAllBetsFromDB")]
        public IActionResult GetBetsFromDB()
        {
            return Ok(new { message = "Saved Bets: " + game.GetSavedBetsFromDB() });
        }

        [HttpGet]
        [Route("GetCurrentBets")]
        public IActionResult GetCurrentBets()
        {
            return Ok(new { message = "Current Bets: " + DisplayCurrentBets() });
        }

        [HttpDelete]
        [Route("ClearAllBetsFromDB")]
        public IActionResult ClearBetsFromDB()
        {
            _betRepository.ClearAllBets();
            return Ok(new { message = "All Bets Cleared" });
        }

        [HttpPost]
    [Route("PlaceBet")]
    public IActionResult PlaceBet(string playerId, decimal amount, BetType type, string value)
    {
        try
        {
           
                    game.PlaceBet(new Bet(playerId, amount, type, value));
               
                
            return Ok(new { message = "Bet of " + amount + " placed successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost]
    [Route("Spin")]
    public IActionResult Spin()
    {
        try
        {
                if (game.GetBets().Count == 0)
                {
                    return Ok(new { message = "No bets have placed, please place some bets before spinning the wheel" });
                }
                game.Spin();
                return Ok(new { message = "Wheel spun successfully, bet payouts are as follows: " + DisplayBetPayouts() + " All Bets have now been reset." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost]
    [Route("RequestPayout")]
    public IActionResult Payout(string playerId)
    {

            // In a full implementation with DB stored players with ID's, this method would check the total oustanding payout balance of each
            // player and then pay that out to them

        try
        {
            return Ok(new { message = "Payout completed successfully (In a full implementation with DB stored players with ID's, this method would check the total oustanding payout balance of each player and then pay that out to them)" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet]
    [Route("ShowPreviousSpins")]
    public IActionResult ShowPreviousSpins()
    {
        var previousSpins = game.ShowPreviousSpins();
        return Ok(previousSpins);
    }

        private string DisplayBetPayouts()
        {
            string betDisplay = "";

            foreach (Bet bet in game.GetBets())
            {
                betDisplay += "Player " + bet.PlayerId + " Payout amount is " + bet.Payout + ". ";
            }

            game.ClearBets();
            return betDisplay;
        }

        private string DisplayCurrentBets()
        {
            string betDisplay = "";

            foreach (Bet bet in game.GetBets())
            {
                betDisplay += "Player " + bet.PlayerId + " Current bet amount for bet ID " + bet.Id + " is " + bet.Amount + ". ";
            }

            if (betDisplay == "")
            {
                return "No Current Bets made";
            }

            return betDisplay;
        }


    }

}