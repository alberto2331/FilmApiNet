namespace FilmAPI.Entities
{
    public class FilmActor
    {
        public int FilmId { get; set; }
        public int ActorId { get; set; }
        public string Character { get; set; }
        public int Order { get; set; }
        public Film Film { get; set; }
        public Actor Actor { get; set; }
    }
}
