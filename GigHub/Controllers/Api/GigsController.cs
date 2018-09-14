using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class GigsController : ApiController
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

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var currentUser = User.Identity.GetUserId();
            var gig = _context.Gigs.Include(g=>g.Attendances.Select(a=>a.Attendee)).SingleOrDefault(g => g.Id == id && g.ArtistId == currentUser);

            if (gig.IsCancelled)
                return NotFound();

            gig.Cancel();

            _context.SaveChanges();

            return Ok();
        }
    }
}
