using System.Collections.Generic;
using System.Linq;
using WorkshopAPI.Data;
using WorkshopAPI.Models;
using WorkshopAPI.Repository.IRepository;

namespace WorkshopAPI.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db)
        {
            _db=db;
        }

        public bool CategoryExist(string name)
        {
            bool res = _db.Category.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
            return res;
        }

        public bool CategoryExists(int CategoryId)
        {
            bool res = _db.Category.Any(c => c.Id == CategoryId);
            return res;
        }

        public bool DeleteCategory(Category category)
        {
            _db.Category.Remove(category);
            return Save();
        }

        public ICollection<Category> GetCategories()
        {
            return _db.Category.OrderBy(c => c.Name).ToList();
        }

        public Category GetCategoryById(int CategoryId)
        {
            return _db.Category.FirstOrDefault(c => c.Id == CategoryId);
        }

        public bool PostCategory(Category category)
        {
            _db.Category.Add(category);
            return Save();
        }

        public bool PutCategory(Category category)
        {
            _db.Category.Update(category);
            return Save();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }
    }
}
