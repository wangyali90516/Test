using System.ComponentModel;
using UserToAssetTool;

namespace RedisToTableTool.Model
{
    /// <summary>
    ///     用户信息
    /// </summary>
    [Description("用户信息")]
    public class UserInfo : AssetBaseDto
    {
        /// <summary>
        ///     账号状态
        /// </summary>
        [Description("账号状态")]
        public bool AccountStatus { get; set; }

        /// <summary>
        ///     手机号
        /// </summary>
        [Description("手机号")]
        public string Cellphone { get; set; }

        /// <summary>
        ///     身份证号
        /// </summary>
        [Description("身份证号")]
        public string CredentialNo { get; set; }

        /// <summary>
        ///     用户编号
        /// </summary>
        [Description("用户编号")]
        public string UserId { get; set; }

        /// <summary>
        ///     用户名称
        /// </summary>
        [Description("用户名称")]
        public string UserName { get; set; }
    }
}