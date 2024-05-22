using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1717_Lab01_ProductManagement.Repository.Entities;
using NET1717_Lab01_ProductManagement.Repository;
using NET1717_Lab01_ProductManagement.API.Models.CategoryModel;
using NET1717_Lab01_ProductManagement.API.Models.ProductModel;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace NET1717_Lab01_ProductManagement.API.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public ProductController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// SortBy (ProductId = 1,ProductName = 2,CategoryId = 3,UnitsInStock = 4,UnitPrice = 5,)
        /// 
        /// SortType (Ascending = 1,Descending = 2,)        
        /// </summary>
        /// <param name="requestSearchProductModel"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult SearchProduct([FromQuery] RequestSearchProductModel requestSearchProductModel)
        {
            var sortBy = requestSearchProductModel.SortContent != null ? requestSearchProductModel.SortContent?.sortProductBy.ToString() : null;
            var sortType = requestSearchProductModel.SortContent != null ? requestSearchProductModel.SortContent?.sortProductType.ToString() : null;
            Expression<Func<ProductEntity, bool>> filter = x =>
                (string.IsNullOrEmpty(requestSearchProductModel.ProductName) || x.ProductName.Contains(requestSearchProductModel.ProductName)) &&
                (x.CategoryId == requestSearchProductModel.CategoryId || requestSearchProductModel.CategoryId == null) && 
                x.UnitPrice >= requestSearchProductModel.FromUnitPrice && 
                (x.UnitPrice <= requestSearchProductModel.ToUnitPrice || requestSearchProductModel.ToUnitPrice == null) ;
            Func<IQueryable<ProductEntity>, IOrderedQueryable<ProductEntity>> orderBy = null;

            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortType == SortProductTypeEnum.Ascending.ToString())
                {
                    orderBy = query => query.OrderBy(p => EF.Property<object>(p, sortBy));
                }
                else if (sortType == SortProductTypeEnum.Descending.ToString())
                {
                    orderBy = query => query.OrderByDescending(p => EF.Property<object>(p, sortBy));
                }
            }
            var responseCategorie = _unitOfWork.ProductRepository.Get(
                null,
                orderBy,
                includeProperties: "",
                pageIndex: requestSearchProductModel.pageIndex,
                pageSize: requestSearchProductModel.pageSize
            );
            return Ok(responseCategorie);
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var responseCategories = _unitOfWork.ProductRepository.GetByID(id);
            return Ok(responseCategories);
        }
        [HttpPost]
        public IActionResult CreateProduct(RequestCreateProductModel requestCreateProductModel)
        {
            var productEntity = new ProductEntity
            {
                CategoryId = requestCreateProductModel.CategoryId,
                ProductName = requestCreateProductModel.ProductName,
                UnitPrice = requestCreateProductModel.UnitPrice,
                UnitsInStock = requestCreateProductModel.UnitsInStock,
            };
            _unitOfWork.ProductRepository.Insert(productEntity);
            _unitOfWork.Save();
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, RequestCreateProductModel requestCreateProductModel)
        {
            var existedProductEntity = _unitOfWork.ProductRepository.GetByID(id);
            if (existedProductEntity != null)
            {
                existedProductEntity.CategoryId = requestCreateProductModel.CategoryId;
                existedProductEntity.ProductName = requestCreateProductModel.ProductName;
                existedProductEntity.UnitPrice = requestCreateProductModel.UnitPrice;
                existedProductEntity.UnitsInStock = requestCreateProductModel.UnitsInStock;
            }
            _unitOfWork.ProductRepository.Update(existedProductEntity);
            _unitOfWork.Save();
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var existedCategoryEntity = _unitOfWork.ProductRepository.GetByID(id);
            _unitOfWork.ProductRepository.Delete(existedCategoryEntity);
            _unitOfWork.Save();
            return Ok();
        }
    }
}
