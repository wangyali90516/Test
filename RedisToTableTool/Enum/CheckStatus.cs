using System.ComponentModel;

namespace UserToAssetTool.Enum
{
    public enum CheckStatus
    {
        [Description("验证数据错误")]
        CheckFail = 1,

        [Description("验证数据正确")]
        CheckSuccess = 0,

        [Description("验证过程错误")]
        CheckError = 2
    }
}