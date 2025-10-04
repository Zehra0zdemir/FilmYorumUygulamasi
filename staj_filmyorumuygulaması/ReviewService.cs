using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MovieReviewApp.Models;

namespace MovieReviewApp.Services
{
    public class ReviewService
    {
        public void SaveReviewsToFile(List<Movie> movies)
        {
            try
            {
                string fileName = $"film_yorumlari_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    var moviesWithReviews = movies.Where(m => m.HasReviews);

                    foreach (var movie in moviesWithReviews)
                    {
                        writer.WriteLine($"Film: {movie.Name}");
                        writer.WriteLine();

                        foreach (var review in movie.Reviews)
                        {
                            writer.WriteLine($"Yorum: {review}");
                            writer.WriteLine();
                        }

                        writer.WriteLine(new string('-', 50));
                        writer.WriteLine();
                    }
                }

                MessageBox.Show($"Yorumlar kaydedildi: {fileName}", "Başarılı",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Yorumlar kaydedilirken hata oluştu: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
