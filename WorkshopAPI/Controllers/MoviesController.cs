using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WorkshopAPI.Models;
using WorkshopAPI.Models.DTOs;
using WorkshopAPI.Repository.IRepository;

namespace WorkshopAPI.Controllers
{
    [Route("movie-management/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class MoviesController : Controller
    {

        private readonly IMovieRepository movrep;
        private readonly IMapper mapper;


        public MoviesController(IMovieRepository _movrep, IMapper _mapper)
        {
            movrep = _movrep;
            mapper = _mapper;
        }

        /// <summary>
        /// Get all Movies in Database.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<MovieDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetMovies()
        {
            var movieList = movrep.GetMovies();

            var movieDtoList = new List<MovieDto>();

            foreach (var movie in movieList)
            {
                movieDtoList.Add(mapper.Map<MovieDto>(movie));
            }

            return Ok(movieDtoList);
        }

        /// <summary>
        /// Get a single Movie through its Id.
        /// </summary>
        /// <param name="MovieId"></param>
        /// <returns></returns>
        [HttpGet ("{MovieId:int}", Name ="GetMovie")]
        [ProducesResponseType(200, Type = typeof(MovieDto))]
        [ProducesResponseType(404)]
        public IActionResult GetMovie(int MovieId)
        {   
            var movie = movrep.GetMovieById(MovieId);

            if (movie == null)
            {
                return NotFound();
            }
            var movieDto = mapper.Map<MovieDto>(movie);

            return Ok(movieDto);
        }

        /// <summary>
        /// Get the List of Movies of a category.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [ProducesResponseType(200, Type = typeof(List<MovieDto>))]
        [ProducesResponseType(400)]
        [HttpGet("category/{categoryId:int}")]
        public IActionResult getMoviesByCategory(int categoryId)
        {
            var movieList = movrep.GetMoviesByCategory(categoryId);
            if(movieList == null)
            {
                return NotFound();
            }

            var movieItems = new List<MovieDto>();

            foreach(var item in movieList)
            {
                movieItems.Add(mapper.Map<MovieDto>(item));
            }
            return Ok(movieItems);
        }

        /// <summary>
        /// Get a Single Movie through its name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [ProducesResponseType(200, Type = typeof(MovieDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("search")]
        public IActionResult getMovieByName(string name)
        {
            try
            {
                var res = movrep.GetMovieByName(name);
                if (res != null)
                {
                    return Ok(res);
                }
                return NotFound();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error catching data");
            }
        }

        /// <summary>
        /// Create a New Movie.
        /// </summary>
        /// <param name="movieDto"></param>
        /// <returns></returns>
        [ProducesResponseType(201, Type = typeof(MovieDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public IActionResult PostMovie([FromBody] MovieDto movieDto)
        {
            if(movieDto == null)
            {
                return BadRequest(ModelState);
            }

            if (movrep.MovieExist(movieDto.Name))
            {
                ModelState.AddModelError(string.Empty, "Movie Already Exist");
                return StatusCode(404, ModelState);
            }

            var movie = mapper.Map<Movie>(movieDto);

            if (!movrep.PostMovie(movie))
            {
                ModelState.AddModelError("", $"Something went wrong saving: {movie.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetMovie", new { movieId = movie.Id }, movie);
        }

        /// <summary>
        /// Update the Data of a Movie.
        /// </summary>
        /// <param name="MovieId"></param>
        /// <param name="movieDto"></param>
        /// <returns></returns>
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPatch("{MovieId:int}", Name = "PatchMovie")]
        public IActionResult PatchMovie(int MovieId, [FromBody] MovieDto movieDto)
        {
            if(movieDto == null || MovieId != movieDto.Id)
            {
                return BadRequest(ModelState);
            }
            var movie = mapper.Map<Movie>(movieDto);

            if (!movrep.PutMovie(movie))
            {
                ModelState.AddModelError("", $"Something went wrong updating: {movie.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Remove a Movie.
        /// </summary>
        /// <param name="MovieId"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{MovieId:int}", Name = "DeleteMovie")]
        public IActionResult DeleteMovie(int MovieId)
        {
            if (!movrep.MovieExists(MovieId))
            {
                return NotFound();
            }

            var movie = movrep.GetMovieById(MovieId);

            if (!movrep.DeleteMovie(movie)){

                ModelState.AddModelError("", $"Something went wrong deleting {movie.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }
    }
}
