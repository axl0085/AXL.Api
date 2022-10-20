namespace AXL.Commons
{
    public class BusinessException:Exception
    {
        public BusinessException(int hResult, string message)
           : base(message)
        {
            base.HResult = hResult;
        }
    }
}