using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MovieReviewApp.Models;
using MovieReviewApp.Services.Interfaces;
using Newtonsoft.Json;

namespace MovieReviewApp.Services
{
    public class MovieService : IMovieService
    {
        private List<Movie> _movies;
        private const string DATA_FILE = "movies_data.json";

        public MovieService()
        {
            _movies = new List<Movie>();
        }

        public List<Movie> GetAllMovies()
        {
            return _movies.OrderByDescending(m => m.AddedDate).ToList();
        }

        public void AddMovie(Movie movie)
        {
            if (movie == null) throw new ArgumentNullException(nameof(movie));

            _movies.Add(movie);
            SaveMovies();
        }

        public List<Movie> SearchMovies(string name, MovieGenre? genre, int? year)
        {
            var result = _movies.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                result = result.Where(m =>
                    m.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (genre.HasValue)
            {
                result = result.Where(m => m.Genre == genre.Value);
            }

            if (year.HasValue)
            {
                result = result.Where(m => m.Year == year.Value);
            }

            return result.OrderByDescending(m => m.AddedDate).ToList();
        }

        public void SaveMovies()
        {
            try
            {
                string json = JsonConvert.SerializeObject(_movies, Formatting.Indented);
                File.WriteAllText(DATA_FILE, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veriler kaydedilirken hata oluştu: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadMovies()
        {
            try
            {
                if (File.Exists(DATA_FILE))
                {
                    string json = File.ReadAllText(DATA_FILE);
                    _movies = JsonConvert.DeserializeObject<List<Movie>>(json) ?? new List<Movie>();

                    foreach (var movie in _movies)
                    {
                        movie.LoadPosterImage();
                    }
                }
                else
                {
                    AddSampleData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veriler yüklenirken hata oluştu: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                AddSampleData();
            }
        }

        public void AddSampleData()
        {
            _movies.Clear();
            _movies.AddRange(new[]
            {
                new Movie { Name = "The Matrix", Genre = MovieGenre.BilimKurgu, Year = 1999 },
                new Movie { Name = "Inception", Genre = MovieGenre.Gerilim, Year = 2010 },
                new Movie { Name = "The Dark Knight", Genre = MovieGenre.Aksiyon, Year = 2008 },
                new Movie { Name = "Forrest Gump", Genre = MovieGenre.Dram, Year = 1994 },
                new Movie { Name = "The Lion King", Genre = MovieGenre.Animasyon, Year = 1994 }
            });
        }
    }
}
