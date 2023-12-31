﻿using Services;

namespace Biky_Backend.Services
{
    public class ImageCollectionService
    {
        private readonly DBConnector _dbConnector;

        public ImageCollectionService(DBConnector dbConnector)
        {
            _dbConnector = dbConnector;
        }

        // Method to retrieve a list of image URLs associated with a specific post.
        public List<String>? GetImagesByPost(Guid postID)
        {
            return _dbConnector.ImageCollections.Where(i => i.PostID == postID).Select(i => i.Image).ToList();
        }
    }
}