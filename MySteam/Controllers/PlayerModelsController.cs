using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySteam.Models;
using MySteam.Data;

namespace MySteam.Controllers
{
    public class PlayerModelsController : Controller
    {
        private readonly MySteamContext _context;

        public PlayerModelsController(MySteamContext context)
        {
            _context = context;
        }

        // GET: PlayerModels
        public async Task<IActionResult> Index()
        {
            return View(new List<UserModel>());
            // return View(await _context.PlayerModel.ToListAsync());
        }

        // GET: PlayerModels/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserModel playerModel;

            try
            {
                playerModel = await _context.GetPlayerAsync(id);
                // await _context.PlayerModel.SingleOrDefaultAsync(m => m.steamid == id);
            }
            catch (MissingSteamIDException ex)
            {
                return NotFound();
                // return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            catch (InvalidSteamIDException ex)
            {
                return NotFound();
            }

            if (playerModel == null)
            {
                return NotFound();
            }

            return View(playerModel);
        }

        // GET: PlayerModels/Create
        public async Task<IActionResult> Games(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<SimpleGameModel> games;

            try
            {
                ApiHelper.Instance.SetKey(Constants.API_KEY);
                games = await ApiHelper.Instance.GetGamesForUser(id, true);
                // await _context.PlayerModel.SingleOrDefaultAsync(m => m.steamid == id);
            }
            catch (MissingSteamIDException ex)
            {
                return NotFound();
                // return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            catch (InvalidSteamIDException ex)
            {
                return NotFound();
            }

            if (games == null)
            {
                return NotFound();
            }

            return View(games);
        }

        // GET: PlayerModels/Create
        public async Task<IActionResult> DetailedGames(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            GameViewModel info = new GameViewModel();

            try
            {
                // Retrieve all raw data 
                ApiHelper.Instance.SetKey(Constants.API_KEY);
                var userGames = await ApiHelper.Instance.GetGamesForUser(id, true);
                var ids = userGames.Select(sgm => sgm.appid).ToList();
                var detailedGames = await _context.GetDetailedGameDatas(ids);

                // Compile into something easier to work with
                var overview = userGames.ToDictionary(
                    sgm => sgm.appid, 
                    sgm => new GameValue() { simpleGame = sgm }
                );
                foreach (DetailedGameData dgd in detailedGames)
                    overview[dgd.steam_appid].detailedGame = dgd;

                // Calculate necessary values
                var count = userGames.Count;
                var totalTime = DataAnalyzer.CalculateTotalTimePlayed(userGames);
                var totalWorth = DataAnalyzer.CalculateTotalGameWorth(detailedGames);
                var mostExpensive = DataAnalyzer.FindMostExpensiveGame(detailedGames);
                var leastExpensive = DataAnalyzer.FindLeastExpensiveGame(detailedGames);
                var mostWorth = DataAnalyzer.FindMostWorthGame(userGames, detailedGames);
                var leastWorth = DataAnalyzer.FindMostWorthGame(userGames, detailedGames);

                // Construct view model
                info.RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier;
                info.GameCount = count;
                info.TotalTimePlayed = totalTime + " minutes";
                info.TotalWorth = "$" + totalWorth;
                info.MostExpensiveGame = overview[mostExpensive.steam_appid];
                info.LeastExpensiveGame = overview[leastExpensive.steam_appid];
                info.MostWorthGame = mostWorth;
                info.LeastWorthGame = leastWorth;
                info.Games = overview.Select(o => o.Value).ToList();
            }
            catch (MissingSteamIDException ex)
            {
                return NotFound();
            }
            catch (InvalidSteamIDException ex)
            {
                return NotFound();
            }

            if (info == null)
            {
                return NotFound();
            }

            return View(info);
        }
       

        // POST: PlayerModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("steamid,communityvisibilitystate,profilestate,personaname,lastlogoff,profileurl,avatar,avatarmedium,avatarfull,personastate,realname,primaryclanid,timecreated,personastateflags,loccountrycode,locstatecode,loccityid")] UserModel playerModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(playerModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(playerModel);
        }

        // GET: PlayerModels/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playerModel = await _context.PlayerModel.SingleOrDefaultAsync(m => m.steamid == id);
            if (playerModel == null)
            {
                return NotFound();
            }
            return View(playerModel);
        }

        // POST: PlayerModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("steamid,communityvisibilitystate,profilestate,personaname,lastlogoff,profileurl,avatar,avatarmedium,avatarfull,personastate,realname,primaryclanid,timecreated,personastateflags,loccountrycode,locstatecode,loccityid")] UserModel playerModel)
        {
            if (id != playerModel.steamid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(playerModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerModelExists(playerModel.steamid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(playerModel);
        }

        // GET: PlayerModels/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playerModel = await _context.PlayerModel
                .SingleOrDefaultAsync(m => m.steamid == id);
            if (playerModel == null)
            {
                return NotFound();
            }

            return View(playerModel);
        }

        // POST: PlayerModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var playerModel = await _context.PlayerModel.SingleOrDefaultAsync(m => m.steamid == id);
            _context.PlayerModel.Remove(playerModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlayerModelExists(string id)
        {
            return _context.PlayerModel.Any(e => e.steamid == id);
        }
    }
}
