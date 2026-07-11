using BookFlix.Core.Models;
using BookFlix.Core.Repositories;
using BookFlix.Web.Dtos.Author;
using BookFlix.Web.Dtos.Book;
using BookFlix.Web.Dtos.Genre;
using BookFlix.Web.Mapper_Interfaces;

namespace BookFlix.Web.Mappers
{
    public class BookMapper : IBookMapper
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IGenreRepository _genreRepository;

        public BookMapper(IAuthorRepository authorRepository, IGenreRepository genreRepository)
        {
            _authorRepository = authorRepository;
            _genreRepository = genreRepository;
        }

        public BookDto ToBookDto(Book book)
        {
            return new BookDto
            {
                ID = book.ID,
                Title = book.Title,
                Description = book.Description,
                ISBN = book.ISBN,
                CoverImageUrl = book.CoverImageUrl,
                PublicationDate = book.PublicationDate,
                Publisher = book.Publisher,
                PageCount = book.PageCount,
                AverageRating = book.AverageRating,
                IsAvailable = book.IsAvailable,
                CreatedAt = book.CreatedAt,
                UpdatedAt = book.UpdatedAt,
                FileID = book.FileID,
                Authors = book.Authors?.Select(a => new AuthorDto
                {
                    ID = a.ID,
                    Name = a.Name
                }).ToList() ?? new List<AuthorDto>(),
                Genres = book.Genres?.Select(g => new GenreDto
                {
                    ID = g.ID,
                    Name = g.Name
                }).ToList() ?? new List<GenreDto>()
            };
        }

        public async Task<Book> ToBook(BookCreateDto dto)
        {
            var authors = await GetAuthors(dto.AuthorIDs);
            var genres = await GetGenres(dto.GenreIDs);

            return new Book
            {
                Title = dto.Title,
                Description = dto.Description,
                ISBN = dto.ISBN,
                CoverImageUrl = dto.CoverImageUrl,
                PublicationDate = dto.PublicationDate,
                Publisher = dto.Publisher,
                PageCount = dto.PageCount,
                IsAvailable = dto.IsAvailable,
                Authors = authors,
                Genres = genres
            };
        }

        public async Task<Book> ToBook(BookUpdateDto dto)
        {
            var authors = await GetAuthors(dto.AuthorIDs);
            var genres = await GetGenres(dto.GenreIDs);

            return new Book
            {
                ID = dto.ID,
                Title = dto.Title,
                Description = dto.Description,
                ISBN = dto.ISBN,
                CoverImageUrl = dto.CoverImageUrl,
                PublicationDate = dto.PublicationDate,
                Publisher = dto.Publisher,
                PageCount = dto.PageCount,
                IsAvailable = dto.IsAvailable,
                Authors = authors,
                Genres = genres
            };
        }

        public IList<BookDto> ToBookDtos(IList<Book> books)
        {
            return books.Select(ToBookDto).ToList();
        }

        private async Task<List<Author>> GetAuthors(List<Guid> authorIDs)
        {
            var authors = new List<Author>();
            foreach (var id in authorIDs)
            {
                var author = await _authorRepository.GetByIDAsync(id);
                if (author is null) throw new KeyNotFoundException("AuthorNotFound");
                authors.Add(author);
            }
            return authors;
        }

        private async Task<List<Genre>> GetGenres(List<int> genreIDs)
        {
            var genres = new List<Genre>();
            foreach (var id in genreIDs)
            {
                var genre = await _genreRepository.GetByIDAsync(id);
                if (genre is null) throw new KeyNotFoundException("GenreNotFound");
                genres.Add(genre);
            }
            return genres;
        }
    }
}