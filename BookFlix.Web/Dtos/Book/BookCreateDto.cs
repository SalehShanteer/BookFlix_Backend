using System.ComponentModel.DataAnnotations;

namespace BookFlix.Web.Dtos.Book
{
    public class BookCreateDto
    {
        [Required(ErrorMessage = "TitleRequired")]
        [StringLength(150, ErrorMessage = "TitleLengthExceed")]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "DescriptionLengthExceed")]
        public string Description { get; set; }

        [RegularExpression(@"^(\d{10}|\d{13})$", ErrorMessage = "InvalidISBN")]
        public string ISBN { get; set; }

        [Url(ErrorMessage = "InvalidCoverImageUrl")]
        public string CoverImageUrl { get; set; }

        public DateTime PublicationDate { get; set; }

        [StringLength(100, ErrorMessage = "PublisherLengthExceed")]
        public string Publisher { get; set; }

        [Range(1, 10000, ErrorMessage = "PageCountLengthExceed")]
        public int PageCount { get; set; }
        
        public bool IsAvailable { get; set; } = true;

        public List<Guid> AuthorIDs { get; set; }

        [Required(ErrorMessage = "At least one Genre is required")]
        [MinLength(1, ErrorMessage = "At least one Genre is required")]
        public List<int> GenreIDs { get; set; }

        [StringLength(50, ErrorMessage = "FileLocationLengthExceed")]
        public string FileLocation { get; set; }

    }
}
