using System;

namespace GigHub.Dtos
{
    public class GigDto
    {
        // id is for adding a link to page where user can see the deatails of the gig
        public int Id { get; set; }
        public bool IsCanceled { get; set; }
        public UserDto Artist { get; set; }
        public DateTime DateTime { get; set; }
        public string Venue { get; set; }
        public GenreDto Genre { get; set; }
    }
}