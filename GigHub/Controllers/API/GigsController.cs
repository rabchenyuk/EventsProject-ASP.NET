﻿using GigHub.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GigHub.Controllers.API
{
    [Authorize]
    public class GigsController : ApiController
    {
        private readonly ApplicationDbContext _context;
        public GigsController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        [HttpDelete]
        public IHttpActionResult Cancel(int id)
        {
            var userId = User.Identity.GetUserId();
            // Find gig and make sure the user who created the gig can delete this gig
            var gig = _context.Gigs.Single(g => g.Id == id && g.ArtistId == userId);

            if (gig.IsCanceled)
                return NotFound();

            gig.IsCanceled = true;

            var notification = new Notification
            {
                DateTime = DateTime.Now,
                Gig = gig,
                Type = NotificationType.GigCanceled
            };

            // all users that are going to attend this gig
            var attendees = _context.Attendances
                .Where(a => a.GigId == gig.Id)
                .Select(a => a.Attendee).ToList();

            foreach(var attendee in attendees)
            {
                var userNotification = new UserNotification
                {
                    User = attendee,
                    Notification = notification
                };
                _context.UserNotifications.Add(userNotification);
            }

            _context.SaveChanges();

            return Ok();
        }
    }
}
