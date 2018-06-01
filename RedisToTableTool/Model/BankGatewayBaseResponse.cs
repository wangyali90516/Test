namespace RedisToTableTool.Model
{
    /// <summary>
    ///     流标操作调用存管网关返回结果
    /// </summary>
    public class BankGatewayBaseResponse
    {
        /// <summary>
        ///     状态码(1-成功,0-处理中,-1-失败) ,
        /// </summary>
        public int RespCode { get; set; }

        /// <summary>
        ///     返回处理信息 ,
        /// </summary>
        public string RespMsg { get; set; }

        /// <summary>
        ///     回码
        /// </summary>
        public string RespSubCode { get; set; }
    }
}