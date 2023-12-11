using Microsoft.EntityFrameworkCore;

namespace FilmAPI.Helpers
{
    public static class HttpContextExtensions
    {
        public async static Task InsertParameterPaginator<T>(
            this HttpContext httpContext, // This is to add the Http responses in the header
            IQueryable<T> queryable, // The iQueryable is to determine the number of records in the table
            int numberOfRecordsPerPage) 
        {
            double quantity = await queryable.CountAsync();
            double quantityOfPages = Math.Ceiling(quantity / numberOfRecordsPerPage);
            httpContext.Response.Headers.Add("quantityOfPages", quantityOfPages.ToString());
        }
    }
}
