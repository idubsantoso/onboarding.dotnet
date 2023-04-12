using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Authorization;
using WebApi.Dto;
using WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BookAuthorController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookAuthorController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<BookDto>>>> Get(){
            return Ok(await _bookService.GetAllBooks());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<BookDto>>> GetSingle(int id){
            return Ok(await _bookService.GetBookById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<BookDto>>>> AddBook(BookDto newBook)
        {
            return Ok(await _bookService.AddNewBook(newBook));
        }
        
    }
}