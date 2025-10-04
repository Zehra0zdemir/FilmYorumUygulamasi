using System.Collections.Generic;
using MovieReviewApp.Models;

namespace MovieReviewApp.Services.Interfaces
{
    public interface IMovieService
    {
        List<Movie> GetAllMovies();
        void AddMovie(Movie movie);
        List<Movie> SearchMovies(string name, MovieGenre? genre, int? year);
        void SaveMovies();
        void LoadMovies();
        void AddSampleData();
    }
}
