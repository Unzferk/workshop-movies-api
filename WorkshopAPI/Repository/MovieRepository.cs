using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WorkshopAPI.Data;
using WorkshopAPI.Models;

namespace WorkshopAPI.Repository.IRepository
{
    public class MovieRepository : IMovieRepository
     {
        private readonly ApplicationDbContext _db;

        public MovieRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool DeleteMovie(Movie movie)
        {
            _db.Movie.Remove(movie);
            return Save();
        }

        public Movie GetMovieById(int MovieId)
        {
            return _db.Movie.FirstOrDefault(m => m.Id == MovieId);
        }

        public Movie GetMovieByName(string name)
        {
            return _db.Movie.FirstOrDefault(m => m.Name == name);
        }

        public ICollection<Movie> GetMovies()
        {
            return _db.Movie.OrderBy(c => c.Name).ToList();
        }

        public ICollection<Movie> GetMoviesByCategory(int categoryId)
        {
            return _db.Movie.Include(m => m.Category).Where(cat => cat.Id == categoryId).ToList();
        }

        public bool MovieExist(string name)
        {
            bool res = _db.Movie.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
            return res;
        }

        public bool MovieExists(int MovieId)
        {
            bool res = _db.Movie.Any(c => c.Id == MovieId);
            return res;
        }

        public bool PostMovie(Movie movie)
        {
            _db.Movie.Add(movie);
            return Save();
        }

        public bool PutMovie(Movie movie)
        {
            _db.Movie.Update(movie);
            return Save();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

    }
}
