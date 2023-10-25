namespace ProjectChronos.Common.Models
{
    public class ServiceResult<T> : BaseServiceResult where T : class
    {
        public T Result { get; set; }
    }
}
