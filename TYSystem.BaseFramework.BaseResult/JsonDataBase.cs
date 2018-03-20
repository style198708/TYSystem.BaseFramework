using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TYSystem.BaseFramework.BaseResult
{
    public class JsonDataBase
    {
        //
        // 摘要:
        //     状态，0=成功，非0失败
        [Description("状态，0=成功，非0失败")]
        public int? errno { get; set; }
        //
        // 摘要:
        //     提示信息
        [Description("提示信息")]
        public string errmsg { get; set; }
        //
        // 摘要:
        //     扩展字段1
        [Description("扩展字段1")]
        public string Ext1 { get; set; }
    }
}
