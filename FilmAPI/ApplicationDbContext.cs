using FilmAPI.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FilmAPI
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FilmActor>()
                .HasKey(e => new { e.ActorId, e.FilmId});
            modelBuilder.Entity<FilmGender>()
                .HasKey(e => new { e.GenderId, e.FilmId });

            modelBuilder.Entity<FilmCinema>()
                .HasKey(e => new { e.CinemaId, e.FilmId });

            SeedData(modelBuilder);
            
            base.OnModelCreating(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {

            var aventura = new Gender() { Id = 3, Name = "Aventura" };
            var animation = new Gender() { Id = 4, Name = "Animación" };
            var suspenso = new Gender() { Id = 5, Name = "Suspenso" };
            var romance = new Gender() { Id = 6, Name = "Romance" };

            modelBuilder.Entity<Gender>()
                .HasData(new List<Gender>
                {
                    aventura, animation, suspenso, romance
                });

            var jimCarrey = new Actor() { Id = 4, Name = "Jim Carrey", Birthdate = new DateTime(1962, 01, 17) };
            var robertDowney = new Actor() { Id = 5, Name = "Robert Downey Jr.", Birthdate = new DateTime(1965, 4, 4) };
            var chrisEvans = new Actor() { Id = 6, Name = "Chris Evans", Birthdate = new DateTime(1981, 06, 13) };

            modelBuilder.Entity<Actor>()
                .HasData(new List<Actor>
                {
                    jimCarrey, robertDowney, chrisEvans
                });

            var endgame = new Film()
            {
                Id = 5,
                Title = "Avengers: Endgame",
                InTheaters = true,
                ReleaseDate = new DateTime(2019, 04, 26)
            };

            var iw = new Film()
            {
                Id = 6,
                Title = "Avengers: Infinity Wars",
                InTheaters = false,
                ReleaseDate = new DateTime(2019, 04, 26)
            };

            var sonic = new Film()
            {
                Id = 7,
                Title = "Sonic the Hedgehog",
                InTheaters = false,
                ReleaseDate = new DateTime(2020, 02, 28)
            };
            var emma = new Film()
            {
                Id = 8,
                Title = "Emma",
                InTheaters = false,
                ReleaseDate = new DateTime(2020, 02, 21)
            };
            var wonderwoman = new Film()
            {
                Id = 9,
                Title = "Wonder Woman 1984",
                InTheaters = false,
                ReleaseDate = new DateTime(2020, 08, 14)
            };

            modelBuilder.Entity<Film>()
                .HasData(new List<Film>
                {
                    endgame, iw, sonic, emma, wonderwoman
                });

            modelBuilder.Entity<FilmGender>().HasData(
                new List<FilmGender>()
                {
                    new FilmGender(){FilmId = endgame.Id, GenderId = suspenso.Id},
                    new FilmGender(){FilmId = endgame.Id, GenderId = aventura.Id},
                    new FilmGender(){FilmId = iw.Id, GenderId = suspenso.Id},
                    new FilmGender(){FilmId = iw.Id, GenderId = aventura.Id},
                    new FilmGender(){FilmId = sonic.Id, GenderId = aventura.Id},
                    new FilmGender(){FilmId = emma.Id, GenderId = suspenso.Id},
                    new FilmGender(){FilmId = emma.Id, GenderId = romance.Id},
                    new FilmGender(){FilmId = wonderwoman.Id, GenderId = suspenso.Id},
                    new FilmGender(){FilmId = wonderwoman.Id, GenderId = aventura.Id},
                });

            modelBuilder.Entity<FilmActor>().HasData(
                new List<FilmActor>()
                {
                    new FilmActor(){FilmId = endgame.Id, ActorId = robertDowney.Id, Character = "Tony Stark", Order = 1},
                    new FilmActor(){FilmId = endgame.Id, ActorId = chrisEvans.Id, Character = "Steve Rogers", Order = 2},
                    new FilmActor(){FilmId = iw.Id, ActorId = robertDowney.Id, Character = "Tony Stark", Order = 1},
                    new FilmActor(){FilmId = iw.Id, ActorId = chrisEvans.Id, Character = "Steve Rogers", Order = 2},
                    new FilmActor(){FilmId = sonic.Id, ActorId = jimCarrey.Id, Character = "Dr. Ivo Robotnik", Order = 1}
                });
        }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<FilmActor> FilmsActors { get; set; }
        public DbSet<FilmGender> FilmsGenders { get; set; }
        public DbSet<FilmCinema> FilmsCinemas { get; set; }
    }
}
