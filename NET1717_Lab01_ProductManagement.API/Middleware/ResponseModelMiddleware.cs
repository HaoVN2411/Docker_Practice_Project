
using NET1717_Lab01_ProductManagement.API.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace NET1717_Lab01_ProductManagement.API.Middleware
{
    public class ResponseModelMiddleware : IMiddleware
    {

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;
                await next(context);
                context.Response.Body = originalBodyStream;

                if (context.Response.StatusCode == StatusCodes.Status200OK)
                {
                    responseBody.Seek(0, SeekOrigin.Begin);
                    var responseBodyText = await new StreamReader(responseBody).ReadToEndAsync();

                    object data = JsonSerializer.Deserialize<object>(responseBodyText);
                    BaseResponse<object> baseResponse;

                    if (data is PagedResponse<object> pagedData)
                    {
                        baseResponse = new PagedResponse<object>(
                            pagedData.Status,
                            pagedData.Message,
                            pagedData.Data,
                            pagedData.PageIndex,
                            pagedData.PageSize,
                            pagedData.TotalPages,
                            pagedData.TotalCount
                        );
                    }
                    else
                    {
                        baseResponse = new BaseResponse<object>(
                            context.Response.StatusCode,
                            "Success",
                            data
                        );
                    }

                    var json = JsonSerializer.Serialize(baseResponse, new JsonSerializerOptions
                    {
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    await context.Response.WriteAsync(json);
                }
                else
                {
                    responseBody.Seek(0, SeekOrigin.Begin);
                    await responseBody.CopyToAsync(originalBodyStream);
                }
            }
        }
    }
}
