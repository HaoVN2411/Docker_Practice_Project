using NET1717_Lab01_ProductManagement.Repository.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NET1717_Lab01_ProductManagement.API.Models.ProductModel
{
    public class RequestSearchProductModel
    {
        public string? ProductName { get; set; }
        public int? CategoryId { get; set; }
        public decimal? FromUnitPrice { get; set; } = decimal.Zero;
        public decimal? ToUnitPrice { get; set; } = null;
        public SortProductByEnum? sortProductBy { get; set; }
        public SortProductTypeEnum? sortProductType { get; set; }
        public int pageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 10;

    }

    public enum SortProductByEnum
    {
        ProductId,
        ProductName,
        CategoryId,
        UnitsInStock,
        UnitPrice,
    }
    public enum SortProductTypeEnum
    {
        Ascending,
        Descending,
    }
}
