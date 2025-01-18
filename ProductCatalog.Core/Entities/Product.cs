using Microsoft.AspNetCore.Http;
using ProductCatalog.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace ProductCatalog.Core.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime StartDate { get; set; }
        public int Duration { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; } = "";
        public Category Category { get; set; }

        public bool IsCurrentlyVisible()
        {
            var now = DateTime.UtcNow;
            return now >= StartDate && now <= StartDate.AddDays(Duration) && Category != null;
        }

        public void Validate()
        {
            if (Price < 0)
            {
                throw new InvalidPriceException("Price cannot be negative.");
            }

            if (Duration <= 0)
            {
                throw new InvalidDurationException("Duration must be a positive number.");
            }

            //if (StartDate < DateTime.Today)
            //{
            //    throw new InvalidStartDateException("Start date cannot be in the past.");
            //}
        }

        public void ValidateImage(IFormFile imageFile)
        {
            if (imageFile == null)
            {
                throw new ArgumentNullException(nameof(imageFile), "Image file is required.");
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(imageFile.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new InvalidImageException("Image must be in JPG, JPEG, or PNG format.");
            }

            if (imageFile.Length > 1 * 1024 * 1024) // 1MB
            {
                throw new InvalidImageException("Image size cannot exceed 1MB.");
            }
        }

        public void LogUpdate(string userId)
        {
            var logMessage = $"Product {Id} updated by user {userId} at {DateTime.UtcNow}.";
            // Use a logging framework like Serilog or NLog to log the message.
        }
    }
}