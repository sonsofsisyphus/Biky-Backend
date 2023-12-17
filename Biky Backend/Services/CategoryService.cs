using Biky_Backend.Services.DTO;
using Entities;
using Services;
using Services.DTO;

namespace Biky_Backend.Services
{
    public class CategoryService
    {
        private readonly DBConnector _dbConnector;

        public CategoryService(DBConnector dbConnector)
        {
            _dbConnector = dbConnector;
        }

        // Method to add a new category to the database.
        public void AddCategory(Category category)
        {
            try
            {
                _dbConnector.Categories.Add(category);
                _dbConnector.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error adding category.", ex);
            }
        }

        // Method to retrieve all categories and their hierarchical structure.
        public CategorySendRequest GetCategories()
        {
            try
            {
                // Retrieve the root category (e.g., "csrHead").
                CategorySendRequest csrHead = CategorySendRequest.ToCategorySendRequest(_dbConnector.Categories.
                    FirstOrDefault(c=>c.CategoryID == 1));

                // Fill the hierarchical structure with children categories.
                FillChildren(csrHead);
                return csrHead;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error getting category.", ex);
            }
        }

        // Recursive method to fill the children of a category.
        private void FillChildren(CategorySendRequest csr)
        {
            try
            {
                if (HasChildren(csr))
                {
                    csr.Children = _dbConnector.
                         Categories.Where(c => c.ParentID == csr.CategoryID)
                         .Select(CategorySendRequest.ToCategorySendRequest).ToList();
                    foreach(var child in csr.Children) { 
                        FillChildren(child);
                    }
                }
               

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error getting category.", ex);
            }
        }

        private bool HasChildren(CategorySendRequest csr)
        {
            return _dbConnector.Categories.Any(c => c.ParentID == csr.CategoryID);
        }

        // Method to retrieve category name by given id
        public string GetCategoryName(int id)
        {
            try
            {
                string s = _dbConnector.Categories.FirstOrDefault(c => c.CategoryID == id).Name;
                return s;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error getting category.", ex);
                return "";
            }
        }

    }
}
