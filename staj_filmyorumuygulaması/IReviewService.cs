using System.Collections.Generic;
using MovieReviewApp.Models;

namespace MovieReviewApp.Services.Interfaces
{
    public interface IReviewService
    {
        void SaveReviewsToFile(List<Movie> movies);
    }
}
