using System;

using System.ComponentModel.DataAnnotations;

namespace RazorPagesMovie.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? ProfileUrl { get; set; }

        public string? TG_addr { get; set; }

        public string? Phone { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        public string? State { get; set; }

        public string? Manager { get; set; }

        public string? Extra { get; set; }
    }
}