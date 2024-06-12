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
        public int UnitsInStock { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
