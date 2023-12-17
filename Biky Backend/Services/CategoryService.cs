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

        public CategorySendRequest GetCategories()
        {
            try
            {
                CategorySendRequest csrHead = CategorySendRequest.ToCategorySendRequest(_dbConnector.Categories.
                    FirstOrDefault(c=>c.CategoryID == 1));
                FillChildren(csrHead);
                return csrHead;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error getting category.", ex);
            }
        }

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
