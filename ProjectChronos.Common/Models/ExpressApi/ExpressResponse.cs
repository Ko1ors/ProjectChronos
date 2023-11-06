namespace ProjectChronos.Common.Models.ExpressApi
{
    public class ExpressResponse<T>
    {
        public T? Data { get; set; }

        public bool Success { get; set; }

        public required string Message { get; set; }
    }
}
