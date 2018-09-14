﻿using GigHub.Dtos;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class AttendancesController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public AttendancesController()
        {
            _context = ApplicationDbContext.Create();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _context.Dispose();
            base.Dispose(disposing);
        }

        [HttpPost]
        public IHttpActionResult Attend(AttendanceDto dto)
        {
            var currentUser = User.Identity.GetUserId();

            if (_context.Attendances.Any(a => a.AttendeeId == currentUser && a.GigId == dto.GigId))
                return BadRequest("The attendance already exists.");

            var attendance = new Attendance()
            {
                GigId = dto.GigId,
                AttendeeId = currentUser
            };

            _context.Attendances.Add(attendance);
            _context.SaveChanges();

            return Ok();
        }
    }
}
