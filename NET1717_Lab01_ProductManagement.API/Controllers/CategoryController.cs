using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NET1717_Lab01_ProductManagement.API.Models.CategoryModel;
using NET1717_Lab01_ProductManagement.Repository;
using NET1717_Lab01_ProductManagement.Repository.Entities;

namespace NET1717_Lab01_ProductManagement.API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public CategoryController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var responseCategories = _unitOfWork.CategoryRepository.Get();
            return Ok(responseCategories);
        }
        [HttpGet("{id}")]
        public IActionResult GetCategoryById(int id)
        {
            var responseCategories = _unitOfWork.CategoryRepository.GetByID(id);
            return Ok(responseCategories);
        }
        [HttpPost]
        public IActionResult CreateCategory(RequestCategoryModel requestCategoryModel)
        {
            var categoryEntity = new CategoryEntity
            {
                CategoryName = requestCategoryModel.CategoryName
            };
            _unitOfWork.CategoryRepository.Insert(categoryEntity);
            _unitOfWork.Save();
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, RequestCategoryModel requestCategoryModel)
        {
            var existedCategoryEntity = _unitOfWork.CategoryRepository.GetByID(id);
            if (existedCategoryEntity != null)
            {
                existedCategoryEntity.CategoryName = requestCategoryModel.CategoryName;
            }
            _unitOfWork.CategoryRepository.Update(existedCategoryEntity);
            _unitOfWork.Save();
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var existedCategoryEntity = _unitOfWork.CategoryRepository.GetByID(id);
            _unitOfWork.CategoryRepository.Delete(existedCategoryEntity);
            _unitOfWork.Save();
            return Ok();
        }

    }
}
