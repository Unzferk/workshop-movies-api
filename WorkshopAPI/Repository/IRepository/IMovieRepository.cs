using System.Collections.Generic;
using WorkshopAPI.Models;

namespace WorkshopAPI.Repository.IRepository
{
    public interface IMovieRepository
    {
        ICollection<Movie> GetMovies();
        ICollection<Movie> GetMoviesByCategory(int categoryId);

        Movie GetMovieById(int MovieId);
        Movie GetMovieByName(string name);

        bool MovieExist(string name);
        bool MovieExists(int MovieId);

        bool PostMovie(Movie movie);
        bool PutMovie(Movie movie);
        bool DeleteMovie(Movie movie);

        bool Save();
    }
}
