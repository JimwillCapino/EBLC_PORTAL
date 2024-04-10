
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Dynamic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Globalization;
namespace Basecode.Data
{
    public static class Helper
    {
        public static string GetFullMonthName(string abbreviatedMonth)
        {

            DateTimeFormatInfo dtfi = CultureInfo.CurrentCulture.DateTimeFormat;
            for (int i = 1; i <= 12; i++)
            {
                if (dtfi.GetAbbreviatedMonthName(i).Equals(abbreviatedMonth, StringComparison.OrdinalIgnoreCase))
                {
                    return dtfi.GetMonthName(i);
                }
            }
            return null; // Indicate failure to find a matching month abbreviation
        }

        public static IQueryable<T> OrderByPropertyName<T>(this IQueryable<T> query, string attribute, string direction)
        {
            return ApplyOrdering(query, attribute, direction, "OrderBy");
        }

        public static IQueryable<T> ThenBy<T>(this IQueryable<T> query, string attribute, string direction)
        {
            return ApplyOrdering(query, attribute, direction, "ThenBy");
        }

        private static IQueryable<T> ApplyOrdering<T>(IQueryable<T> query, string attribute, string direction, string orderMethodName)
        {
            try
            {
                if (direction == Constants.SortDirection.Descending) orderMethodName += Constants.SortDirection.Descending;

                var t = typeof(T);

                var param = Expression.Parameter(t);
                var property = t.GetProperty(attribute);

                if (property != null)
                    return query.Provider.CreateQuery<T>(
                        Expression.Call(
                            typeof(Queryable),
                            orderMethodName,
                            new[] { t, property.PropertyType },
                            query.Expression,
                            Expression.Quote(
                                Expression.Lambda(
                                    Expression.Property(param, property),
                                    param))
                        ));
                else
                    return query;
            }
            catch (Exception) // Probably invalid input, you can catch specifics if you want
            {
                return query; // Return unsorted query
            }
        }

        public static HttpResponseMessage ComposeResponse(HttpStatusCode statusCode, object responseData)
        {
            var jsonResponse = JsonConvert.SerializeObject(responseData);
            var resp = new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
            };

            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return resp;
        }

        public static Dictionary<string, string[]> GetModelStateErrors(ModelStateDictionary modelState)
        {
            return modelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            );
        }
        public static void GetErrors(Exception ex, out HttpStatusCode responseCode, out object responseData)
        {
            var message = Constants.Exception.ErrorProcessing;

            responseCode = HttpStatusCode.BadRequest;
            responseData = message;
        }           
    }
}
