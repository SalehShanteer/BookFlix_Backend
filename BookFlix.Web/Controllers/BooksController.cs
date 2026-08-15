using BookFlix.Core.Service_Interfaces;
using BookFlix.Core.Services.Validation;
using BookFlix.Core.Models;
using BookFlix.Web.Dtos.Book;
using BookFlix.Web.Mapper_Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookFlix.Web.Controllers
{
    [Authorize]
    [Route("api/books")]
    public class BooksController : ApiController
    {
        private readonly IBookService _bookService;
        private readonly IFileService<Book> _fileService;
        private readonly IBookMapper _bookMapper;

        public BooksController(IBookService bookService, IFileService<Book> fileService, IBookMapper bookMapper)
        {
            _bookService = bookService;
            _fileService = fileService;
            _bookMapper = bookMapper;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBookByIDAsync(Guid id)
        {
            var book = await _bookService.GetBookByIDAsync(id);

            if (book is null) return HandleFailure(Result.Failure(Error.NotFound("BookNotFound")));
         
            return Ok(_bookMapper.ToBookDto(book));
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BookDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBooksAsync()
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(_bookMapper.ToBookDtos(books.ToList()));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AddBookAsync(BookCreateDto createBookDto)
        {
           var book = await _bookMapper.ToBook(createBookDto);

            var result = await _bookService.AddBookAsync(book);

            if (result.IsFailure) return HandleFailure(result);

            var bookDto = _bookMapper.ToBookDto(result.Value);

            return CreatedAtAction(nameof(GetBookByIDAsync), new { id = bookDto.ID }, bookDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBookAsync(Guid id, BookUpdateDto bookUpdateDto)
        {
            var book = await _bookMapper.ToBook(bookUpdateDto);
            book.ID = id;

            var result = await _bookService.UpdateBookAsync(book);

            if (result.IsFailure) return HandleFailure(result);

            return Ok(_bookMapper.ToBookDto(result.Value));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBookAsync(Guid id)
        {
            var result = await _bookService.DeleteBookAsync(id);

            if (result.IsFailure) return HandleFailure(result);

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/Upload")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadBookAsync(Guid id, IFormFile file)
        {
            await using var fileModel = file.ToFileUploadModel();
            var result = await _fileService.UploadFileAsync(id, fileModel);

            if (result.IsFailure) return HandleFailure(result);

            return Ok(new FileUploadResultDto { FileID = result.Value });
        }

        [AllowAnonymous]
        [HttpGet("{id}/File")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBookFileAsync(Guid id, [FromQuery] bool download = false)
        {
            var bookFileResult = await _bookService.GetBookFileStreamAsync(id);
            if (bookFileResult.IsFailure) return HandleFailure(bookFileResult);

            if (download)
            {
                return File(bookFileResult.Value.Stream, bookFileResult.Value.ContentType, bookFileResult.Value.FileName);
            }

            return File(bookFileResult.Value.Stream, bookFileResult.Value.ContentType);
        }

    }
}