namespace FilmAPI.Entities
{
    public class Gender
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<FilmGender> FilmGender { get; set; }
    }
}
