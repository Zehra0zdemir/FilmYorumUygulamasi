using MovieReviewApp.Models;

namespace MovieReviewApp.Utils
{
    public static class EnumHelper
    {
        public static string GetGenreText(MovieGenre genre)
        {
            switch (genre)
            {
                case MovieGenre.BilimKurgu: return "Bilim Kurgu";
                default: return genre.ToString();
            }
        }

        public static MovieGenre GetGenreFromText(string text)
        {
            switch (text)
            {
                case "Bilim Kurgu": return MovieGenre.BilimKurgu;
                default: return (MovieGenre)System.Enum.Parse(typeof(MovieGenre), text);
            }
        }
    }
}
