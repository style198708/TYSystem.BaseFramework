using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;


namespace TYSystem.BaseFramework.Common.Helper
{

    /// <summary>
    /// 上传文件，同时上传多张图片
    /// </summary>
    public class FileUploadHelper
    {
        private static XmlDocument xmlDoc;

        /// <summary>
        /// 
        /// </summary>
        static FileUploadHelper()
        {
            if (xmlDoc == null)
            {
                xmlDoc = FileHandler.FileHandlerXmlDoc();
            }
        }


        /// <summary>
        /// 根据目录代码取水印设置
        /// </summary>
        /// <param name="DirectoryCode">目录代码,TempFile,AdminAvatar</param>
        /// <param name="WaterImagePath">水印图片路径</param>
        /// <param name="WaterText">水印文字</param>
        internal static void GetDirectoryConfigInfo(string DirectoryCode, ref string WaterImagePath, ref string WaterText)
        {
            if (xmlDoc == null)
                xmlDoc = FileHandler.FileHandlerXmlDoc();

            if (xmlDoc != null)
            {
                WaterImagePath = "";
                WaterText = "";

                XmlNodeList xmlNodeList = xmlDoc.SelectNodes("//FileHandler/DirectoryConfigs/DirectoryConfig[@Code='" + DirectoryCode + "']");
                if (xmlNodeList.Count < 1) return;

                //水印图片位置
                try
                {
                    WaterImagePath = xmlNodeList[0].Attributes["WaterImagePath"].Value;
                    //转换为绝对路径
                    WaterImagePath = FileHandler.GetConfigPath + WaterImagePath;
                }
                catch
                {
                }
                //水印文字
                try
                {
                    WaterText = xmlNodeList[0].Attributes["WaterText"].Value;
                }
                catch
                {
                }
            }
        }
        #region 验证是否是有效图片
        /// <summary>
        /// 验证是否是有效图片
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool ValidateImage(byte[] streamByte)
        {
            //System.Drawing.Image sourceImage;
            try
            {
                //sourceImage = System.Drawing.Image.FromStream(stream, true, true);
                //sourceImage.Dispose();
                //return true;

                System.IO.MemoryStream ms = new System.IO.MemoryStream(streamByte);
                System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                img.Dispose();
                return true;

            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 随机产生临时文件名
        /// <summary>
        /// 随机产生临时文件名
        /// 规则：YYYYMMDDhhnnss + 6位随机串 + 扩展名
        /// </summary>
        /// <param name="extName">文件扩展名</param>
        public static string GetRndFileName(string extName)
        {
            return GetRndFileName(extName, 0);
        }

        public static string GetRndFileName(string extName, int typeID)
        {
            string rndStr = DateTime.Now.ToString("yyyyMMddHHmmss") + RandomHelper.RandCode(10) + typeID + extName;
            return rndStr;
        }
        public static string GetRndOnlyFileName(string extName)
        {
            string rndStr = DateTime.Now.ToString("yyyyMM") + RandomHelper.GetRandomID() + extName;
            return rndStr;
        }
        #endregion

        /// <summary>
        /// 转换KB/MB
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static String SizeFormatNum2String(long size)
        {
            double pers = 1048576; //1024*1024  
            String s = "";
            if (size > 1024 * 1024)
                s = String.Format("{0:F}", (double)size / pers) + "M";
            else
                s = String.Format("{0:F}", (double)size / (1024)) + "KB";
            return s;
        }


        /// <summary>
        /// 上传文件，同时上传多张文件
        /// </summary>
        /// <param name="param"></param>
        /// <param name="append"></param>
        /// <param name="fullpath"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public static bool UploadFileByHTML(FileUploadParam param, ref FileUploadInfoResult uploadResult)
        {
            //uploadResult = new FileUploadResult()
            //{
            //    localname = new List<string>(),
            //    newname = new List<string>(),
            //    fullpath = new List<string>(),
            //    err = new List<string>(),
            //};

            HttpFileCollection filecollection = param.request.Files;
            for (int index = 0; index < filecollection.Count; index++)
            {
                HttpPostedFile postedfile = filecollection.Get(index);
                // 读取原始文件名
                string localname = postedfile.FileName;
                byte[] file = new Byte[postedfile.ContentLength];  // 统一转换为byte数组处理 // 初始化byte长度.

                // 转换为byte类型
                System.IO.Stream stream = postedfile.InputStream;
                stream.Read(file, 0, postedfile.ContentLength);
                stream.Close();

                if (file.Length == 0)
                {
                    uploadResult.err.Add("上传的文件无效");
                    return false;
                }

                if (file.Length > param.maxattachsize)
                {
                    uploadResult.err.Add("文件大小超过" + SizeFormatNum2String(param.maxattachsize));
                    return false;
                }

                uploadResult.filetype = param.fileType;

                #region 上传文件
                if (param.fileType == FileType.Image)
                {
                    if (!FileUploadHelper.ValidateImage(file))
                    {
                        uploadResult.err.Add("请上传有效的文件");
                        return false;
                    }
                }

                // 取上载文件后缀名
                string extension = FileHelper.GetFileExtName(localname);
                if (param.upext.Split(',').Where(p => p.Contains(extension)).Count() <= 0)
                {
                    uploadResult.err.Add("上传文件扩展名必需为：" + param.upext);
                    return false;
                }
                else
                {
                    string fileName = FileUploadHelper.GetRndOnlyFileName(extension);
                    bool resultFlag = Upload(param.fileDirectory, localname, fileName, file, ref uploadResult);
                    if (!resultFlag)
                    {
                        //上传失败直接返回
                        return false;
                    }
                }
                #endregion

                file = null;
            }
            filecollection = null;
            return true;
        }

        private static bool Upload(string dir, string localname, string fileName, byte[] file, ref FileUploadInfoResult uploadResult)
        {
            if (string.IsNullOrEmpty(dir)) return false;
            FileInfo formalFileInfo = new FileInfo(Path.Combine(dir, fileName));
            //判断正式目录是否存在,不存在则创建
            try
            {
                if (!Directory.Exists(formalFileInfo.DirectoryName))
                {
                    Directory.CreateDirectory(formalFileInfo.DirectoryName);
                }
            }
            catch (Exception ex)
            {
                uploadResult.err.Add("执行Exists或CreateDirectory失败，" + ex.Message.ToString());
                return false;
            }

            //判断是否在正式目录存在同名文件，存在则删除
            if (formalFileInfo.Exists)
            {
                try
                {
                    formalFileInfo.Delete();
                }
                catch (Exception ex)
                {
                    uploadResult.err.Add("执行Delete失败，" + ex.Message.ToString());
                    return false;
                }
            }

            try
            {
                System.IO.FileStream fs = new System.IO.FileStream(formalFileInfo.FullName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                fs.Write(file, 0, file.Length);
                fs.Flush();
                fs.Close();
            }
            catch (Exception ex)
            {
                uploadResult.err.Add(ex.Message.ToString());//错误信息
                return false;
            }
            uploadResult.localname.Add(localname);//原始文件名
            uploadResult.newname.Add(fileName);//新文件名称
            uploadResult.fullpath.Add(formalFileInfo.FullName);//全路径
            return true;
        }











    }

}
