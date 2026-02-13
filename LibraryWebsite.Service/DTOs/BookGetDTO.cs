namespace LibraryWebsite.Service.DTOs
{
    public class BookGetDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int ISBN { get; set; }
        public string Categoryid { get; set; } = null!;
        public string Aythorid { get; set; } = null!;
        public string Dercription { get; set; } = null!;
        public string PublishYear { get; set; } = null!;
        public int TotalCopies { get; set; }
        public int AvaillableCopies { get; set; }
    }
}
