namespace ProjectChronos.Common.Models
{
    public class ServiceResult<T> : BaseServiceResult
    {
        public T Data { get; set; }
    }
}
