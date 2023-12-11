using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FilmAPI.Migrations
{
    public partial class populatingTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Actors",
                columns: new[] { "Id", "Birthdate", "Name", "Photo" },
                values: new object[,]
                {
                    { 4, new DateTime(1962, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jim Carrey", null },
                    { 5, new DateTime(1965, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Robert Downey Jr.", null },
                    { 6, new DateTime(1981, 6, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chris Evans", null }
                });

            migrationBuilder.InsertData(
                table: "Films",
                columns: new[] { "Id", "InTheaters", "Poster", "ReleaseDate", "Title" },
                values: new object[,]
                {
                    { 5, true, null, new DateTime(2019, 4, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Avengers: Endgame" },
                    { 6, false, null, new DateTime(2019, 4, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Avengers: Infinity Wars" },
                    { 7, false, null, new DateTime(2020, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sonic the Hedgehog" },
                    { 8, false, null, new DateTime(2020, 2, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Emma" },
                    { 9, false, null, new DateTime(2020, 8, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Wonder Woman 1984" }
                });

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 3, "Aventura" },
                    { 4, "Animación" },
                    { 5, "Suspenso" },
                    { 6, "Romance" }
                });

            migrationBuilder.InsertData(
                table: "FilmsActors",
                columns: new[] { "ActorId", "FilmId", "Character", "Order" },
                values: new object[,]
                {
                    { 4, 7, "Dr. Ivo Robotnik", 1 },
                    { 5, 5, "Tony Stark", 1 },
                    { 5, 6, "Tony Stark", 1 },
                    { 6, 5, "Steve Rogers", 2 },
                    { 6, 6, "Steve Rogers", 2 }
                });

            migrationBuilder.InsertData(
                table: "FilmsGenders",
                columns: new[] { "FilmId", "GenderId" },
                values: new object[,]
                {
                    { 5, 3 },
                    { 6, 3 },
                    { 7, 3 },
                    { 9, 3 },
                    { 5, 5 },
                    { 6, 5 },
                    { 8, 5 },
                    { 9, 5 },
                    { 8, 6 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FilmsActors",
                keyColumns: new[] { "ActorId", "FilmId" },
                keyValues: new object[] { 4, 7 });

            migrationBuilder.DeleteData(
                table: "FilmsActors",
                keyColumns: new[] { "ActorId", "FilmId" },
                keyValues: new object[] { 5, 5 });

            migrationBuilder.DeleteData(
                table: "FilmsActors",
                keyColumns: new[] { "ActorId", "FilmId" },
                keyValues: new object[] { 5, 6 });

            migrationBuilder.DeleteData(
                table: "FilmsActors",
                keyColumns: new[] { "ActorId", "FilmId" },
                keyValues: new object[] { 6, 5 });

            migrationBuilder.DeleteData(
                table: "FilmsActors",
                keyColumns: new[] { "ActorId", "FilmId" },
                keyValues: new object[] { 6, 6 });

            migrationBuilder.DeleteData(
                table: "FilmsGenders",
                keyColumns: new[] { "FilmId", "GenderId" },
                keyValues: new object[] { 5, 3 });

            migrationBuilder.DeleteData(
                table: "FilmsGenders",
                keyColumns: new[] { "FilmId", "GenderId" },
                keyValues: new object[] { 6, 3 });

            migrationBuilder.DeleteData(
                table: "FilmsGenders",
                keyColumns: new[] { "FilmId", "GenderId" },
                keyValues: new object[] { 7, 3 });

            migrationBuilder.DeleteData(
                table: "FilmsGenders",
                keyColumns: new[] { "FilmId", "GenderId" },
                keyValues: new object[] { 9, 3 });

            migrationBuilder.DeleteData(
                table: "FilmsGenders",
                keyColumns: new[] { "FilmId", "GenderId" },
                keyValues: new object[] { 5, 5 });

            migrationBuilder.DeleteData(
                table: "FilmsGenders",
                keyColumns: new[] { "FilmId", "GenderId" },
                keyValues: new object[] { 6, 5 });

            migrationBuilder.DeleteData(
                table: "FilmsGenders",
                keyColumns: new[] { "FilmId", "GenderId" },
                keyValues: new object[] { 8, 5 });

            migrationBuilder.DeleteData(
                table: "FilmsGenders",
                keyColumns: new[] { "FilmId", "GenderId" },
                keyValues: new object[] { 9, 5 });

            migrationBuilder.DeleteData(
                table: "FilmsGenders",
                keyColumns: new[] { "FilmId", "GenderId" },
                keyValues: new object[] { 8, 6 });

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
