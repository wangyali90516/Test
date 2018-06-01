namespace RedisToTableTool.Model
{
    public class ResultResponse<T>
    {
        public int BizCode { get; set; }

        public bool IsTrue { get; set; }

        public string Message { get; set; }

        public T Result { get; set; }
    }
}