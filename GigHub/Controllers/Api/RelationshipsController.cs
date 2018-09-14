using GigHub.Dtos;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    public class RelationshipsController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public RelationshipsController()
        {
            _context = ApplicationDbContext.Create();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _context.Dispose();
            base.Dispose(disposing);
        }

        public IHttpActionResult CreateRelationship(RelationshipDto dto)
        {
            string userId = User.Identity.GetUserId();
            if (_context.Relationships.Any(r => r.FollowerId == userId && r.FolloweeId == dto.FolloweeId))
                return BadRequest();

            var relationship = new Relationship()
            {
                FolloweeId = dto.FolloweeId,
                FollowerId = userId
            };

            _context.Relationships.Add(relationship);
            _context.SaveChanges();

            return Ok();
        }
    }
}
