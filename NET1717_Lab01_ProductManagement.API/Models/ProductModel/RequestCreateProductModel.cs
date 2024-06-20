using NET1717_Lab01_ProductManagement.Repository.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NET1717_Lab01_ProductManagement.API.Models.ProductModel
{
    public class RequestCreateProductModel
    {
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Units in stock cannot be negative.")]
        public int UnitsInStock { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be a positive value.")]
        public decimal UnitPrice { get; set; }
    }
}
