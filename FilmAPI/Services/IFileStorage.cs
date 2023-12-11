namespace FilmAPI.Services
{
    public interface IFileStorage
    {
        Task<string> SaveFile( byte[] content, string length, string container, string contentType );
        Task<string> EditFile( byte[] content, string length, string container, string route, string contentType );
        Task DeleteFile( string route, string container );
    }
}
