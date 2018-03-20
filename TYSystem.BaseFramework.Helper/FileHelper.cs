using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using TYSystem.BaseFramework.Extension;
using TYSystem.BaseFramework.Common;
using System.Net.Http;

namespace TYSystem.BaseFramework.Helper
{
    /// <summary>
    /// �ļ�������
    /// </summary>
    public class FileHelper : IDisposable
    {
        private bool _alreadyDispose = false;

        #region ���캯��
        /// <summary>
        /// FileObj
        /// </summary>
        public FileHelper()
        {
            //
            // TODO: �ڴ˴���ӹ��캯���߼�
            //
        }
        /// <summary>
        /// ~FileObj �ͷ���Դ
        /// </summary>
        ~FileHelper()
        {
            Dispose(); ;
        }
        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="isDisposing"></param>
        protected virtual void Dispose(bool isDisposing)
        {
            if (_alreadyDispose) return;
            //if (isDisposing)
            //{
            //     if (xml != null)
            //     {
            //         xml = null;
            //     }
            //}
            _alreadyDispose = true;
        }
        #endregion

        #region IDisposable ��Ա
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region �ж��ļ��Ƿ����
        public static bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }
        #endregion

        #region �����ļ�
        public static bool Create(string filePath)
        {
            try
            {
                File.Create(filePath);
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion

        #region ȡ���ļ���׺��
        /// <summary>
        /// ȡ��׺��
        /// </summary>
        /// <param name="filename">�ļ���</param>
        /// <returns>.gif|.html��ʽ</returns>
        public static string GetFileExtName(string fileName)
        {
           return Path.GetExtension(fileName).ToLower();
        }
        #endregion

        #region д�ļ�
        /// <summary>
        /// д�ļ�
        /// </summary>
        /// <param name="Path">�ļ�·��</param>
        /// <param name="Strings">�ļ�����</param>
        public static void WriteFile(string Path, string Strings)
        {
            if (!System.IO.File.Exists(Path))
            {
                System.IO.FileStream f = System.IO.File.Create(Path);
                f.Close();
            }
            System.IO.StreamWriter f2 = new System.IO.StreamWriter(Path, false, System.Text.Encoding.GetEncoding("gb2312"));
            f2.Write(Strings);
            f2.Close();
            f2.Dispose();
        }
        #endregion

        #region ���ļ�
        /// <summary>
        /// ���ļ�
        /// </summary>
        /// <param name="Path">�ļ�·��</param>
        /// <returns></returns>
        public static string ReadFile(string Path)
        {
            string s = "";
            if (!System.IO.File.Exists(Path))
                s = "��������Ӧ��Ŀ¼";
            else
            {
                StreamReader f2 = new StreamReader(Path, System.Text.Encoding.GetEncoding("UTF-8"));
                s = f2.ReadToEnd();
                f2.Close();
                f2.Dispose();
            }

            return s;
        }
        #endregion

        #region ׷���ļ�
        /// <summary>
        /// ׷���ļ�
        /// </summary>
        /// <param name="Path">�ļ�·��</param>
        /// <param name="strings">����</param>
        public static void FileAdd(string Path, string strings)
        {
            StreamWriter sw = File.AppendText(Path);
            sw.Write(strings);
            sw.Flush();
            sw.Close();
        }
        #endregion

        #region �����ļ�
        /// <summary>
        /// �����ļ�
        /// </summary>
        /// <param name="orignFile">ԭʼ�ļ�</param>
        /// <param name="NewFile">���ļ�·��</param>
        public static void FileCoppy(string orignFile, string NewFile)
        {
            File.Copy(orignFile, NewFile, true);
        }

        #endregion

        #region ɾ���ļ�
        /// <summary>
        /// ɾ���ļ�
        /// </summary>
        /// <param name="Path">·��</param>
        //public static void FileDel(string Path)
        //{
        //    string file = HttpContext.Current.Server.MapPath(Path);

        //    if (File.Exists(file))
        //    {
        //        File.Delete(file);
        //    }
        //}
        /// <summary>
        /// ɾ���ļ�
        /// </summary>
        /// <param name="Path">����·��</param>
        public static void FileDel1(string Path)
        {
            if (File.Exists(Path))
            {
                File.Delete(Path);
            }
        }
        #endregion

        #region �ƶ��ļ�
        /// <summary>
        /// �ƶ��ļ�
        /// </summary>
        /// <param name="orignFile">ԭʼ·��</param>
        /// <param name="NewFile">��·��</param>
        public static void FileMove(string orignFile, string NewFile)
        {
            File.Move(orignFile, NewFile);
        }
        #endregion

        #region �ڵ�ǰĿ¼�´���Ŀ¼
        /// <summary>
        /// �ڵ�ǰĿ¼�´���Ŀ¼
        /// </summary>
        /// <param name="orignFolder">��ǰĿ¼</param>
        /// <param name="NewFloder">��Ŀ¼</param>
        public static void FolderCreate(string orignFolder, string NewFloder)
        {
            Directory.SetCurrentDirectory(orignFolder);
            Directory.CreateDirectory(NewFloder);
        }
        #endregion

        #region �ݹ�ɾ���ļ���Ŀ¼���ļ�
        /****************************************
          * �������ƣ�DeleteFolder
          * ����˵�����ݹ�ɾ���ļ���Ŀ¼���ļ�
          * ��     ����dir:�ļ���·��
          * ����ʾ�У�
          *            string dir = Server.MapPath("test/");  
          *            EC.FileObj.DeleteFolder(dir);       
         *****************************************/
        /// <summary>
        /// �ݹ�ɾ���ļ���Ŀ¼���ļ�
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static void DeleteFolder(string dir)
        {
            if (Directory.Exists(dir)) //�����������ļ���ɾ��֮ 
            {
                foreach (string d in Directory.GetFileSystemEntries(dir))
                {
                    if (File.Exists(d))
                        File.Delete(d); //ֱ��ɾ�����е��ļ� 
                    else
                        DeleteFolder(d); //�ݹ�ɾ�����ļ��� 
                }
                Directory.Delete(dir); //ɾ���ѿ��ļ��� 
            }

        }

        #endregion

        #region ��ָ���ļ����������������copy��Ŀ���ļ������� ��Ŀ���ļ���Ϊֻ�����Ծͻᱨ��
        /****************************************
          * �������ƣ�CopyDir
          * ����˵������ָ���ļ����������������copy��Ŀ���ļ������� ��Ŀ���ļ���Ϊֻ�����Ծͻᱨ��
          * ��     ����srcPath:ԭʼ·��,aimPath:Ŀ���ļ���
          * ����ʾ�У�
          *            string srcPath = Server.MapPath("test/");  
          *            string aimPath = Server.MapPath("test1/");
          *            EC.FileObj.CopyDir(srcPath,aimPath);   
         *****************************************/
        /// <summary>
        /// ָ���ļ����������������copy��Ŀ���ļ�������
        /// </summary>
        /// <param name="srcPath">ԭʼ·��</param>
        /// <param name="aimPath">Ŀ���ļ���</param>
        public static void CopyDir(string srcPath, string aimPath)
        {
            try
            {
                // ���Ŀ��Ŀ¼�Ƿ���Ŀ¼�ָ��ַ�����������������֮
                if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                    aimPath += Path.DirectorySeparatorChar;
                // �ж�Ŀ��Ŀ¼�Ƿ����������������½�֮
                if (!Directory.Exists(aimPath))
                    Directory.CreateDirectory(aimPath);
                // �õ�ԴĿ¼���ļ��б��������ǰ����ļ��Լ�Ŀ¼·����һ������
                //�����ָ��copyĿ���ļ�������ļ���������Ŀ¼��ʹ������ķ���
                //string[] fileList = Directory.GetFiles(srcPath);
                string[] fileList = Directory.GetFileSystemEntries(srcPath);
                //�������е��ļ���Ŀ¼
                foreach (string file in fileList)
                {
                    //�ȵ���Ŀ¼��������������Ŀ¼�͵ݹ�Copy��Ŀ¼������ļ�

                    if (Directory.Exists(file))
                        CopyDir(file, aimPath + Path.GetFileName(file));
                    //����ֱ��Copy�ļ�
                    else
                        File.Copy(file, aimPath + Path.GetFileName(file), true);
                }

            }
            catch (Exception ee)
            {
                throw new Exception(ee.ToString());
            }
        }


        #endregion

        #region Kill�ļ�����
        /// <summary>
        /// Kill�ļ�����
        /// </summary>
        /// <param name="processName">��������</param>
        private void KillFileProcess(string processName)
        {
            System.Diagnostics.Process[] ps = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process p in ps)
            {
                if (p.ProcessName.ToLower().Equals(processName.ToLower()))
                    p.Kill();
            }
        }
        #endregion



        #region ����Ŀ¼�����ļ�
        /// <summary>
        /// ����Ϊָ����Ŀ¼
        /// </summary>
        /// <param name="dir">Ŀ¼������c:\\test\\</param>
        /// <param name="fileName">�ļ����ƣ�����rain.jpg</param>
        /// <param name="searchPattern">ƥ�����ͣ�����*.*/*.jpg</param>
        /// <returns></returns>
        public static string FindFile(string dir,string fileName,string searchPattern) 
        {
            //��ָ��Ŀ¼����Ŀ¼�²����ļ�,��listBox1���г���Ŀ¼���ļ� 
            DirectoryInfo Dir = new DirectoryInfo(dir);
            try
            {
                foreach (DirectoryInfo d in Dir.GetDirectories()) //������Ŀ¼ 
                {
                    var result = FindFile(Dir + d.ToString() + "\\", fileName, searchPattern);
                    if (result != "")
                    {
                        return result;
                    }
                }
                foreach (FileInfo f in Dir.GetFiles(searchPattern)) //�����ļ� 
                {
                    if (f.Name == fileName) return f.FullName;
                }
            }
            catch (Exception e)
            {
                return "error:"+e.Message;
            }
            return "";
        }
        /// <summary>
        /// ����Ϊָ����Ŀ¼
        /// </summary>
        /// <param name="dir">Ŀ¼������c:\\test\\</param>
        /// <param name="fileName">�ļ����ƣ�����rain.jpg</param>
        /// <returns></returns>
        public static string FindFile(string dir, string fileName)
        {
           return FindFile(dir, fileName, "*.*");
        }
        #endregion








        #region ��ȡ��ƷͼƬ
        private static string GetThumbnailProduct(string fullName)
        {
            string[] fullNames = fullName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (fullNames.Length > 0)
                return fullNames[0];
            return fullName;
        }
        /// <summary>
        /// ��ȡ����ͼ
        /// </summary>
        /// <param name="fullName">ԭʼ�ļ���</param>
        /// <param name="mark">����ͼ��ʶ����</param>
        /// <param name="imageDir">����Ŀ¼��EnumSysImageDir.PRODUCT</param>
        /// <returns></returns>
        private static string GetThumbnail(string fullName, string mark, string imageDir)
        {
            string result = string.Empty;
            string productImgPath = imageDir;
            if (!string.IsNullOrEmpty(fullName))
            {
                if (!string.IsNullOrEmpty(mark))
                {
                    result = string.Format("{0}{1}{2}/{3}", DomainManageConfigInfo.ImgServerUrl, productImgPath, mark, fullName);
                    return result;
                }
                result = string.Format("{0}{1}{2}", DomainManageConfigInfo.ImgServerUrl, productImgPath, fullName);
            }
            return result;
        }
        /// <summary>
        /// ��ȡ��ƷͼƬ
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="mark"></param>
        /// <returns></returns>
        public static string GetProductThumbnail(string fullName, string mark, string imageDir)
        {
            return GetThumbnail(GetThumbnailProduct(fullName), mark, imageDir);
        }

        #endregion

    }
}
