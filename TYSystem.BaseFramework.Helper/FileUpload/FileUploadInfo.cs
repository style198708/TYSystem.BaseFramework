using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace TYSystem.BaseFramework.Common.Helper
{
    /// <summary>
    /// 上传文件参数
    /// </summary>
    public class FileUploadParam
    {
        public FileUploadParam()
        {
            this.maxattachsize = 500 * 1024;//500KB
            this.upext = ".jpg,.jpeg,.png,.wmv,.avi,.xls,.xlsx,.doc,.docx,.pdf,.ppt,.pptx";
            this.fileDirectory = "/";
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        public HttpRequest request { get; set; }
        public System.IO.Stream stream { get; set; }
        /// <summary>
        /// 上传文件扩展
        /// </summary>
        public string upext { get; set; }
        /// <summary>
        /// 上传文件大小，默认500KB
        /// </summary>
        public int maxattachsize { get; set; }
        /// <summary>
        /// 上传目录，默认根目录
        /// </summary>
        public string fileDirectory { get; set; }
        /// <summary>
        /// 上传文件类型，默认 FileType.Image
        /// </summary>
        public FileType fileType { get; set; }

    }

    #region FileType
    /// <summary>
    /// 文件类型
    /// </summary>
    public enum FileType
    {
        /// <summary>
        /// 图片
        /// </summary>
        Image = 0,
        /// <summary>
        /// 视频
        /// </summary>
        Video = 1,
        /// <summary>
        /// 上传的文件
        /// </summary>
        File = 2,
        /// <summary>
        /// 其他文件
        /// </summary>
        Other
    }
    #endregion

    /// <summary>
    /// 上传公用方法返回数据集合
    /// </summary>
    public class FileUploadInfoResult
    {
        public FileUploadInfoResult()
        {
            this.localname = new List<string>();
            this.newname = new List<string>();
            this.fullpath = new List<string>();
            this.err = new List<string>();
        }
        public List<string> localname { get; set; }
        public List<string> newname { get; set; }
        public List<string> fullpath { get; set; }
        public List<string> err { get; set; }

        public FileType filetype { get; set; }

    }








    #region 前端返回结果对象
    /// <summary>
    /// 返回上传结果实体
    /// </summary>
    [Serializable]
    public class UploadFileResult
    {
        public UploadFileResult()
        {
            this.errno = 0;
            this.list = new List<UploadFileInfo>();
        }
        /// <summary>
        /// 是否成功
        /// </summary>
        public int errno { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public string errmsg { get; set; }

        /// <summary>
        /// 多张图片返回结果集
        /// </summary>
        public List<UploadFileInfo> list { get; set; }
    }

    /// <summary>
    /// 返回上传文件信息
    /// </summary>
    [Serializable]
    public class UploadFileInfo
    {
        /// <summary>
        /// 原始文件名称
        /// </summary>
        public string ClientFileName { get; set; }

        /// <summary>
        /// 新文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 访问路径
        /// </summary>
        public string FullPath { get; set; }
    }
    #endregion

}
