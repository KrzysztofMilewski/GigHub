using GigHub.Models;
using System.Collections.Generic;

namespace GigHub.ViewModels
{
    public class GigViewModel
    {
        public string Venue { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public int GenreId { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
    }
}