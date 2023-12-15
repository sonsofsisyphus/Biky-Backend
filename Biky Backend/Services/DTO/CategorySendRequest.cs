using Entities;

namespace Biky_Backend.Services.DTO
{
    public class CategorySendRequest
    {
        public int CategoryID { get; set; }

        public string Name { get; set; }

        public List<CategorySendRequest> Children { get; set; }

        public static CategorySendRequest ToCategorySendRequest(Category c)
        {
            return new CategorySendRequest { 
                CategoryID = c.CategoryID,
                Name = c.Name,
                Children = new List<CategorySendRequest>()
            };
        }

    }
}
