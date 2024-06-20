using System.Text.Json.Serialization;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

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
                    object data = null;

                    if (IsValidJson(responseBodyText)) 
                    {
                        data = System.Text.Json.JsonSerializer.Deserialize<object>(responseBodyText);
                    }
                    BaseResponseMiddleware<object> baseResponse;
                    baseResponse = new BaseResponseMiddleware<object>(
                        context.Response.StatusCode,
                        "Successfully",
                        data: data
                    );
                    var json = System.Text.Json.JsonSerializer.Serialize(baseResponse, new JsonSerializerOptions
                    {
                        //DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
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
        private bool IsValidJson(string strInput)
        {
            if (string.IsNullOrWhiteSpace(strInput)) return false;

            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || // For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) // For array
            {
                try
                {
                    JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException)
                {
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }

    }
}
