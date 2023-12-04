using Entities;

namespace Entities.CategorySystem;

public abstract class CategoryType
{
    public CategoryCollection Parent { get; set; }

    public string Name { get; set; }
}