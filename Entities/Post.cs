﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Post
    {
        public Guid PostID { get; set; }

        public Guid AuthorID { get; set; }

        public string ContentText { get; set; }

        public ImageCollection Images { get; set; }

        public DateTime PostTime { get; set; }

    }
}
