using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GigsController()
        {
            _context = ApplicationDbContext.Create();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _context.Dispose();
            base.Dispose(disposing);
        }

        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new GigViewModel()
            {
                Genres = _context.Genres.ToList(),
                Heading = "Add a new gig"
            };

            return View("GigForm", viewModel);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var currentUser = User.Identity.GetUserId();
            var gig = _context.Gigs.Single(g => g.Id == id && g.ArtistId == currentUser);

            var viewModel = new GigViewModel()
            {
                Genres = _context.Genres.ToList(),
                Date = gig.DateTime.ToString("d MMM yyyy"),
                Time = gig.DateTime.ToString("HH:mm"),
                GenreId = gig.GenreId,
                Venue = gig.Venue,
                Heading = "Edit a gig",
                Id = gig.Id
            };

            return View("GigForm", viewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GigViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();
                return View("GigForm", viewModel);
            }

            var gig = new Gig()
            {
                ArtistId = User.Identity.GetUserId(),
                Venue = viewModel.Venue,
                GenreId = viewModel.GenreId,
                DateTime = viewModel.GetDateTime()
            };

            _context.Gigs.Add(gig);
            _context.SaveChanges();

            return RedirectToAction("Mine", "Gigs");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Update(GigViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();
                return View("GigForm", viewModel);
            }

            var currentUser = User.Identity.GetUserId();
            var gig = _context.Gigs.Single(g => g.Id == viewModel.Id && g.ArtistId == currentUser);

            gig.Venue = viewModel.Venue;
            gig.DateTime = viewModel.GetDateTime();
            gig.GenreId = viewModel.GenreId;
            _context.SaveChanges();

            return RedirectToAction("Mine", "Gigs");
        }

        [Authorize]
        public ActionResult Attending()
        {
            string currentUser = User.Identity.GetUserId();
            var myGigs = _context.Attendances.Where(a => a.AttendeeId == currentUser).Select(a => a.Gig).Include(g=>g.Artist).Include(g=>g.Genre).ToList();

            var viewModel = new GigsViewModel()
            {
                Gigs = myGigs,
                ShowActions = User.Identity.IsAuthenticated
            };

            return View("Gigs", viewModel);
        }

        public ActionResult Mine()
        {
            string currentUser = User.Identity.GetUserId();
            var myGigs = _context.Gigs.Where(g => g.ArtistId == currentUser && g.DateTime > DateTime.Now && !g.IsCancelled).Include(g => g.Genre).ToList();

            return View(myGigs);
        }
    }
}