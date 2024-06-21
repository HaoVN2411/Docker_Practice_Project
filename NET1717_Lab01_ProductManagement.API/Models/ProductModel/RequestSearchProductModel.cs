using NET1717_Lab01_ProductManagement.Repository.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace NET1717_Lab01_ProductManagement.API.Models.ProductModel
{
    public class RequestSearchProductModel : IValidatableObject 
    {
        public string? ProductName { get; set; }
        public int? CategoryId { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "The price must be a positive value.")]
        [AllowNull]
        public decimal? FromUnitPrice { get; set; } = decimal.Zero;
        [Range(0, double.MaxValue, ErrorMessage = "The price must be a positive value.")]
        [AllowNull]
        public decimal? ToUnitPrice { get; set; } = null;
        [AllowNull]
        public SortProductByEnum? sortProductBy { get; set; }
        [AllowNull]
        public SortProductTypeEnum? sortProductType { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Page index must be a positive integer.")]
        [AllowNull]
        public int pageIndex { get; set; } = 1;
        [Range(1, int.MaxValue, ErrorMessage = "Page size must be a positive integer.")]
        [AllowNull]
        public int pageSize { get; set; } = 10;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Custom validation logic for enums
            if (sortProductBy.HasValue && !Enum.IsDefined(typeof(SortProductByEnum), sortProductBy.Value))
            {
                yield return new ValidationResult("Invalid value for SortProductBy.", new[] { nameof(sortProductBy) });
            }

            if (sortProductType.HasValue && !Enum.IsDefined(typeof(SortProductTypeEnum), sortProductType.Value))
            {
                yield return new ValidationResult("Invalid value for SortProductType.", new[] { nameof(sortProductType) });
            }

            if (FromUnitPrice >= ToUnitPrice && FromUnitPrice.HasValue && ToUnitPrice.HasValue)
            {
                yield return new ValidationResult("From price need to be higher than to price", new[] { "invalid-price" });
            }
        }
    }

    public enum SortProductByEnum
    {
        ProductId = 1,
        ProductName = 2,
        CategoryId = 3,
        UnitsInStock = 4,
        UnitPrice = 5,
    }
    public enum SortProductTypeEnum
    {
        Ascending = 1,
        Descending = 2,
    }
}
