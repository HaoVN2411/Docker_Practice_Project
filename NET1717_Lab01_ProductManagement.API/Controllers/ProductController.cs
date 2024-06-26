﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1717_Lab01_ProductManagement.Repository.Entities;
using NET1717_Lab01_ProductManagement.Repository;
using NET1717_Lab01_ProductManagement.API.Models.CategoryModel;
using NET1717_Lab01_ProductManagement.API.Models.ProductModel;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NET1717_Lab01_ProductManagement.API.Extentions;
using NET1717_Lab01_ProductManagement.API.Models;
using Microsoft.AspNetCore.Authorization;

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
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("search-product")]
        public IActionResult SearchProduct([ModelBinder(BinderType = typeof(KebabCaseQueryModelBinder))] RequestSearchProductModel requestSearchProductModel)
        {
            var sortBy = requestSearchProductModel.sortProductBy != null ? requestSearchProductModel.sortProductBy?.ToString() : null;
            var sortType = requestSearchProductModel.sortProductType != null ? requestSearchProductModel.sortProductType?.ToString() : null;
            Expression<Func<ProductEntity, bool>> filter = x =>
                (string.IsNullOrEmpty(requestSearchProductModel.ProductName) || x.ProductName.Contains(requestSearchProductModel.ProductName)) &&
                (x.CategoryId == requestSearchProductModel.CategoryId || requestSearchProductModel.CategoryId == null) &&
                x.UnitPrice >= requestSearchProductModel.FromUnitPrice &&
                (x.UnitPrice <= requestSearchProductModel.ToUnitPrice || requestSearchProductModel.ToUnitPrice == null);
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
                filter,
                orderBy,
                includeProperties: "",
                pageIndex: requestSearchProductModel.pageIndex,
                pageSize: requestSearchProductModel.pageSize
            );
            return Ok(new PagedResponse<IEnumerable<ProductEntity>>
            {
                Data = responseCategorie.entities,
                PageIndex = responseCategorie.pageIndex,
                PageSize = responseCategorie.pageSize,
                TotalCount = responseCategorie.totalCount,
                TotalPages = responseCategorie.totalPages,
            });

        }

        [HttpGet("get-product-by-id/{id}")]
        public IActionResult GetProductById(int id)
        {
            var responseCategories = _unitOfWork.ProductRepository.GetByID(id);
            if (responseCategories != null)
            {
                return Ok(responseCategories);
            }
            return NotFound();
        }
        [HttpPost("create-product")]
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
            return Ok(productEntity);
        }

        [HttpPut("update-product-by-id/{id}")]
        public IActionResult UpdateProduct(int id, RequestCreateProductModel requestCreateProductModel)
        {
            var existedProductEntity = _unitOfWork.ProductRepository.GetByID(id);
            if (existedProductEntity == null) return NotFound();

            existedProductEntity.CategoryId = requestCreateProductModel.CategoryId;
            existedProductEntity.ProductName = requestCreateProductModel.ProductName;
            existedProductEntity.UnitPrice = requestCreateProductModel.UnitPrice;
            existedProductEntity.UnitsInStock = requestCreateProductModel.UnitsInStock;

            _unitOfWork.ProductRepository.Update(existedProductEntity);
            _unitOfWork.Save();
            return Ok(existedProductEntity);
        }

        [HttpDelete("delete-product-by-id/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            if (id < 0) return BadRequest("Id must be a positive value.");
            var existedCategoryEntity = _unitOfWork.ProductRepository.GetByID(id);
            if (existedCategoryEntity == null) return NotFound();
            _unitOfWork.ProductRepository.Delete(existedCategoryEntity);
            _unitOfWork.Save();
            return Ok();
        }
    }
}
