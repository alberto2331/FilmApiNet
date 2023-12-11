namespace FilmAPI.Entities
{
    public class FilmGender
    {
        public int FilmId { get; set; }
        public int GenderId { get; set; }
        public Film Film { get; set; }
        public Gender Gender { get; set; }
    }
}
