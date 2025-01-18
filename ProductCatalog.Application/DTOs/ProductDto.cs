using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace ProductCatalog.Application.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        [StringLength(100, ErrorMessage = "The Name must be at most 100 characters long.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "The Description must be at most 500 characters long.")]
        public string Description { get; set; }

        
        public DateTime CreationDate { get; set; }= DateTime.Now;

        public string? CreatedByUserId { get; set; }

        [Required(ErrorMessage = "The Start Date field is required.")]
        public DateTime StartDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "The Duration field is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The Duration must be a positive number.")]
        public int Duration { get; set; } // Duration in days

        [Required(ErrorMessage = "The Price field is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "The Price must be greater than 0.")]
        public decimal Price { get; set; }

        public string? CategoryName { get; set; } = "";

        public string? ImagePath { get; set; } 
        public IFormFile? ImageFile { get; set; }
 

        [Required(ErrorMessage = "The Category field is required.")]
        public int CategoryId { get; set; }
    }
}