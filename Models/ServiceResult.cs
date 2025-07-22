namespace LoginApi.Models
{
    public enum ServiceResultType
    {
        Ok,
        NotFound,
        BadRequest,
        Unauthorized,
        Forbidden
    }
    public class ServiceResult
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public ServiceResultType Type { get; set; }

        public static ServiceResult Ok() => new() { Success = true, Type = ServiceResultType.Ok };
        public static ServiceResult NotFound(string error) => new() { Success = false, Type = ServiceResultType.NotFound, Error = error };
        public static ServiceResult BadRequest(string error) => new() { Success = false, Type = ServiceResultType.BadRequest, Error = error };
    }

}
