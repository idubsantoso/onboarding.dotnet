using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Dto;
using WebApi.Entities;
using WebApi.Models;
using WebApi.Exceptions;

namespace WebApi.Services
{
    public class BookService : IBookService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public BookService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }


        public async Task<ServiceResponse<BookDto>> AddNewBook(BookDto dto)
        {
            var serviceResponse = new ServiceResponse<BookDto>();

            // var book = _mapper.Map<Book>(dto);
            var newBook = new Book()
            {
                Author = dto.Author,
                Description = dto.Description,
                Title = dto.Title,
                Category = dto.Category,
                TotalPages = dto.TotalPages.HasValue ? dto.TotalPages.Value : 0,
            };
            var dbBook = await _context.Books.AddAsync(newBook);
            await _context.SaveChangesAsync();
            serviceResponse.Data = dto;
            return serviceResponse;
        }

        public async Task<ServiceResponse<BookDto>> UpdateBook(UpdateBook updateBook)
        {
            var serviceResponse = new ServiceResponse<BookDto>();

            try 
            {
                var dbBook = await _context.Books.FirstOrDefaultAsync(c => c.Id == updateBook.Id);
                if(dbBook is null)
                    throw new NotFoundException($"Character with Id '{updateBook.Id}' not found.");
                
                // _mapper.Map(updateCharacter, character);

                dbBook.Author = updateBook.Author;
                dbBook.Description = updateBook.Description;
                dbBook.Title = updateBook.Title;
                dbBook.Category = updateBook.Category;
                dbBook.TotalPages = updateBook.TotalPages;

                await _context.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<BookDto>(dbBook);

                return serviceResponse;
            }
            catch (Exception ex)
            {
                throw new InternalServerException(ex.Message);
            }
        }

        public async Task<ServiceResponse<List<BookDto>>> GetAllBooks()
        {
            var serviceResponse = new ServiceResponse<List<BookDto>>();
            var dbBook = await _context.Books.ToListAsync();
            serviceResponse.Data = dbBook.Select(c => _mapper.Map<BookDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<BookDto>> GetBookById(int id)
        {
            var serviceResponse = new ServiceResponse<BookDto>();
            var dbBook = await _context.Books.FirstOrDefaultAsync(c => c.Id == id);
            serviceResponse.Data = _mapper.Map<BookDto>(dbBook);
            return serviceResponse;
        }
    }
}