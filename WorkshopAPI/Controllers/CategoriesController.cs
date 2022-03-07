using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WorkshopAPI.Models;
using WorkshopAPI.Repository.IRepository;

namespace WorkshopAPI.Controllers
{
    [Route("category-management/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class CategoriesController : Controller
    {

        private readonly ICategoryRepository catrep;
        private readonly IMapper mapper;


        public CategoriesController(ICategoryRepository _catrep, IMapper _mapper)
        {
            catrep = _catrep;
            mapper = _mapper;
        }


        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200,Type =typeof(List<CategoryDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetCategories()
        {
            var categoryList = catrep.GetCategories();

            var categoryDtoList = new List<CategoryDto>();

            foreach (var category in categoryList)
            {
                categoryDtoList.Add(mapper.Map<CategoryDto>(category));
            }

            return Ok(categoryDtoList);
        }


        /// <summary>
        /// Get just one category
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        [ProducesResponseType(200, Type = typeof(CategoryDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        [HttpGet ("{CategoryId:int}", Name ="GetCategory")]
        public IActionResult GetCategory(int CategoryId)
        {   
            var category = catrep.GetCategoryById(CategoryId);

            if (category == null)
            {
                return NotFound();
            }
            var categoryDto = mapper.Map<CategoryDto>(category);

            return Ok(categoryDto);
        }

        /// <summary>
        /// Create a new Category
        /// </summary>
        /// <param name="categoryDto"></param>
        /// <returns></returns>
        [ProducesResponseType(201, Type = typeof(CategoryDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public IActionResult PostCategory([FromBody] CategoryDto categoryDto)
        {
            if(categoryDto == null)
            {
                return BadRequest(ModelState);
            }
            if (catrep.CategoryExist(categoryDto.Name))
            {
                ModelState.AddModelError(string.Empty, "Category Already Exist");
                return StatusCode(404, ModelState);
            }
            var category = mapper.Map<Category>(categoryDto);

            if (!catrep.PostCategory(category))
            {
                ModelState.AddModelError("", $"Something went wrong saving: {category.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCategory", new { categoryId = category.Id }, category);
        }

        /// <summary>
        /// Update data of a sigle Category
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <param name="categoryDto"></param>
        /// <returns></returns>
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPatch("{CategoryId:int}", Name = "PatchCategory")]
        public IActionResult PatchCategory(int CategoryId, [FromBody] CategoryDto categoryDto)
        {
            if(categoryDto == null || CategoryId != categoryDto.Id)
            {
                return BadRequest(ModelState);
            }
            var category = mapper.Map<Category>(categoryDto);

            if (!catrep.PutCategory(category))
            {
                ModelState.AddModelError("", $"Something went wrong updating: {category.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Delete a Category
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{CategoryId:int}", Name = "DeleteCategory")]
        public IActionResult DeleteCategory(int CategoryId)
        {
            if (!catrep.CategoryExists(CategoryId))
            {
                return NotFound();
            }

            var category = catrep.GetCategoryById(CategoryId);

            if (!catrep.DeleteCategory(category)){

                ModelState.AddModelError("", $"Something went wrong deleting {category.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }
    }
}
