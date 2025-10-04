using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;

namespace MovieReviewApp.Models
{
    public class Movie
    {
        public string Name { get; set; }
        public MovieGenre Genre { get; set; }
        public int Year { get; set; }
        public string ImagePath { get; set; }
        public List<string> Reviews { get; set; }
        public DateTime AddedDate { get; set; }

        [JsonIgnore]
        public Image PosterImage { get; set; }

        public Movie()
        {
            Reviews = new List<string>();
            AddedDate = DateTime.Now;
        }

        public void LoadPosterImage()
        {
            if (!string.IsNullOrEmpty(ImagePath) && File.Exists(ImagePath))
            {
                try
                {
                    PosterImage = Image.FromFile(ImagePath);
                }
                catch
                {
                    PosterImage = null;
                }
            }
        }

        public bool HasReviews => Reviews != null && Reviews.Count > 0;
        public string GenreDisplayText => Utils.EnumHelper.GetGenreText(Genre);
    }
}
