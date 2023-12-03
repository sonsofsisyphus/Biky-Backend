using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Category
    {
        public Category Parent { get; set; }

        public string Identifier { get; set; }
    }
}
