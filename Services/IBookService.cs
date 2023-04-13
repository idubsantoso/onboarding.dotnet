using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Dto;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IBookService
    {
        Task<ServiceResponse<BookDto>> AddNewBook(BookDto dto);
        Task<ServiceResponse<BookDto>> UpdateBook(UpdateBook updateBook);
        Task<ServiceResponse<List<BookDto>>> GetAllBooks();
        Task<ServiceResponse<BookDto>> GetBookById(int id);
    }
}