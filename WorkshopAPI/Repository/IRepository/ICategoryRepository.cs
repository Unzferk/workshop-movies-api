

using System.Collections.Generic;
using WorkshopAPI.Models;

namespace WorkshopAPI.Repository.IRepository
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();

        Category GetCategoryById(int CategoryId);

        bool CategoryExist(string name);
        bool CategoryExists(int CategoryId);

        bool PostCategory(Category category);
        bool PutCategory(Category category);
        bool DeleteCategory(Category category);

        bool Save();
    }
}
