using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Authorization;
using WebApi.Dto;
using WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Queue;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BookAuthorController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IBackgroundTaskQueue<BookDto> _queue;
        public BookAuthorController(IBookService bookService, IBackgroundTaskQueue<BookDto> queue)
        {
            _bookService = bookService;
            _queue = queue;
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

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<List<BookDto>>>> UpdateBook(UpdateBook updateBook)
        {
            return Ok(await _bookService.UpdateBook(updateBook));
        }

        [HttpPost("queue")]
        public async Task<IActionResult> saveBook([FromBody] List<BookDto> books)
        {
            for(int i = 0; i < books.Capacity; i++){
                _queue.Enqueue(books[i]);
            }
            
            return Accepted();
        }
        
    }
}