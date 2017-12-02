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
            return View(new List<PlayerModel>());
            // return View(await _context.PlayerModel.ToListAsync());
        }

        // GET: PlayerModels/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PlayerModel playerModel;

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

        // POST: PlayerModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("steamid,communityvisibilitystate,profilestate,personaname,lastlogoff,profileurl,avatar,avatarmedium,avatarfull,personastate,realname,primaryclanid,timecreated,personastateflags,loccountrycode,locstatecode,loccityid")] PlayerModel playerModel)
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
        public async Task<IActionResult> Edit(string id, [Bind("steamid,communityvisibilitystate,profilestate,personaname,lastlogoff,profileurl,avatar,avatarmedium,avatarfull,personastate,realname,primaryclanid,timecreated,personastateflags,loccountrycode,locstatecode,loccityid")] PlayerModel playerModel)
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
