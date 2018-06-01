namespace RedisToTableTool.Model
{
    public class CommonResult<T>
    {
        /// <summary>
        ///     结果编码
        /// </summary>
        /// <value>The code.</value>
        public int Code { get; set; }

        public string Message { get; set; }

        public T Result { get; set; }
    }

    public class ResponseResult<T>
    {
        public T data { get; set; }
        public string errorCode { get; set; }
        public string errorDesc { get; set; }

        public bool IsSuccess { get; set; }
    }
}