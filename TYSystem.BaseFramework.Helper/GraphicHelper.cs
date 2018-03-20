using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TYSystem.BaseFramework.Common.Extension;

namespace TYSystem.BaseFramework.Common.Helper
{
    /// <summary>
    /// Graphic帮助类
    /// 生成缩略图,切割图片
    /// </summary>
    public class GraphicHelper
    {
        /// <summary>
        /// 生成图片缩放，以宽与高建立目录，或以　名称-宽-高结构存储
        /// </summary>
        /// <param name="path">原图路径如：C:\abc.jpg</param>
        /// <param name="iWidth">缩放宽度</param>
        /// <param name="iHeight">缩放高度</param>
        /// <param name="isCreateDirectory">如果为 <c>true</c> 则自动创建文件夹</param>
        /// <param name="compressionRatio">压缩率</param>
        /// <returns></returns>
        public static bool ThumbImg1(string path, int iWidth, int iHeight, bool isCreateDirectory)
        {

            return ThumbImg(path, iWidth, iHeight, isCreateDirectory, 95L);
        }

        /// <summary>
        /// 生成图片缩放，以宽与高建立目录，或以　名称-宽-高结构存储
        /// </summary>
        /// <param name="path">原图路径如：C:\abc.jpg</param>
        /// <param name="iWidth">缩放宽度</param>
        /// <param name="iHeight">缩放高度</param>
        /// <param name="isCreateDirectory">如果为 <c>true</c> 则自动创建文件夹</param>
        /// <param name="compressionRatio">压缩率</param>
        /// <returns></returns>
        public static bool ThumbImg(string path, int iWidth, int iHeight, bool isCreateDirectory, long compressionRatio)
        {
            string dir = System.IO.Path.GetDirectoryName(path);
            if (!dir.EndsWith(@"\")) dir += @"\";
            string filename = System.IO.Path.GetFileName(path);
            string ex = System.IO.Path.GetExtension(path);

            string namestr = filename.Replace(ex, "");
            string thumbPath = "";
            if (isCreateDirectory)
            {
                thumbPath = dir + iWidth.ToString() + "-" + iHeight.ToString() + @"\" + filename;
            }
            else
            {
                thumbPath = dir + namestr + "-W" + iWidth.ToString() + "H" + iHeight.ToString() + ex;
            }
            return ThumbImg(path, thumbPath, iWidth, iHeight, compressionRatio);
        }
        /// <summary>
        /// 生成图片缩放，以宽与高建立目录，或以　名称-宽-高结构存储
        /// </summary>
        /// <param name="path">原图路径如：C:\abc.jpg</param>
        /// <param name="iWidth">缩放宽度</param>
        /// <param name="iHeight">缩放高度</param>
        /// <param name="isCreateDirectory">如果为 <c>true</c> 则自动创建文件夹</param>
        /// <returns></returns>
        public static bool ThumbImg(string path, int iWidth, int iHeight, bool isCreateDirectory, ref string thumbPath, long compressionRatio)
        {
            string dir = System.IO.Path.GetDirectoryName(path);
            if (!dir.EndsWith(@"\")) dir += @"\";
            string filename = System.IO.Path.GetFileName(path);
            string ex = System.IO.Path.GetExtension(path);

            string namestr = filename.Replace(ex, "");

            if (isCreateDirectory)
            {
                thumbPath = dir + iWidth.ToString() + "-" + iHeight.ToString() + @"\" + filename;
            }
            else
            {
                thumbPath = dir + thumbPath + "\\" + namestr + "-W" + iWidth.ToString() + "H" + iHeight.ToString() + ex;
            }
            return ThumbImg(path, thumbPath, iWidth, iHeight, compressionRatio);
        }

        /// <summary>
        /// 生成图片缩放
        /// </summary>
        /// <param name="path">原图路径如：C:\abc.jpg</param>
        /// <param name="thumbPath">缩放路径如：D:\t.jpg</param>
        /// <param name="iWidth">缩放宽度</param>
        /// <param name="iHeight">缩放高度</param>
        /// <returns></returns>
        public static bool ThumbImg(string path, string thumbPath, int iWidth, int iHeight, long compressionRatio)
        {
            string extension = System.IO.Path.GetExtension(thumbPath).ToLower();
            if (".jpg.jpeg.gif.bmp.png".IndexOf(extension) == -1)
            {
                System.IO.File.Copy(path, thumbPath, true);
                return true;
            }

            if (System.IO.File.Exists(thumbPath))
            {
                System.IO.File.Delete(thumbPath);
            }
            //生成缩微图片，以JPEG格式存储
            System.Drawing.Image image = null;
            System.Drawing.Size samize = new System.Drawing.Size();
            //System.Drawing.Bitmap bitmap = null;

            try
            {
                image = System.Drawing.Image.FromFile(path);
                //获得缩微图片的大小
                samize = Calcsamize(image.Width, image.Height, iWidth, iHeight);

                //缩微图片存放路径不存在则生成该路径
                string tempPath = System.IO.Path.GetDirectoryName(thumbPath);
                if (System.IO.Directory.Exists(tempPath) == false) System.IO.Directory.CreateDirectory(tempPath);

                if (extension == ".gif")
                {
                    image = image.GetThumbnailImage(samize.Width, samize.Height, null, IntPtr.Zero);

                    image.Save(thumbPath, ImageFormat.Gif);
                    image.Dispose();
                    return true;
                }

                Bitmap toimg = new Bitmap(samize.Width, samize.Height);
                Graphics g = Graphics.FromImage(toimg);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, new Rectangle(0, 0, samize.Width, samize.Height),
                    new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
                image.Dispose();

                EncoderParameters encParams = new EncoderParameters(1);
                // 65%品质的JPEG格式图片 
                encParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, compressionRatio);
                ImageCodecInfo codeInfo = null;
                ImageCodecInfo[] codeInfos = ImageCodecInfo.GetImageEncoders();
                foreach (ImageCodecInfo info in codeInfos)
                {
                    if (info.MimeType.Equals("image/jpeg"))
                    {
                        codeInfo = info;
                        break;
                    }
                }


                toimg.Save(thumbPath, codeInfo, encParams);

                //if (extension == ".jpg" || extension == ".jpeg")
                //    toimg.Save(thumbPath, System.Drawing.Imaging.ImageFormat.Jpeg,myEncoderParameters);
                //else if (extension == ".gif")
                //    toimg.Save(thumbPath, System.Drawing.Imaging.ImageFormat.Gif);
                //else if (extension == ".bmp")
                //    toimg.Save(thumbPath, System.Drawing.Imaging.ImageFormat.Bmp);
                //else if (extension == ".png")
                //    toimg.Save(thumbPath, System.Drawing.Imaging.ImageFormat.Png);
                //else
                //    toimg.Save(thumbPath, System.Drawing.Imaging.ImageFormat.Gif);

                //bitmap.Dispose();

                toimg.Dispose();
                g.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        /// <summary>
        /// 重设图片大小（不缩放）
        /// </summary>
        /// <param name="path">原图路径如：C:\abc.jpg</param>
        /// <param name="thumbPath">新路径如：D:\t.jpg</param>
        /// <param name="iWidth">宽度</param>
        /// <param name="iHeight">高度</param>
        /// <returns></returns>
        public static bool ReSizeImg(string path, string thumbPath, int iWidth, int iHeight)
        {
            string extension = System.IO.Path.GetExtension(thumbPath).ToLower();
            if (".jpg.jpeg.gif.bmp.png".IndexOf(extension) == -1)
            {
                System.IO.File.Copy(path, thumbPath);
                return true;
            }
            //生成缩微图片，以JPEG格式存储
            System.Drawing.Image image = null;
            System.Drawing.Size samize = new System.Drawing.Size();
            //System.Drawing.Bitmap bitmap = null;

            try
            {
                image = System.Drawing.Image.FromFile(path);
                //获得缩微图片的大小
                samize = new Size(iWidth, iHeight);

                //缩微图片存放路径不存在则生成该路径
                string tempPath = System.IO.Path.GetDirectoryName(thumbPath);
                if (System.IO.Directory.Exists(tempPath) == false) System.IO.Directory.CreateDirectory(tempPath);

                if (extension == ".gif")
                {
                    image.Save(thumbPath, ImageFormat.Gif);
                    image.Dispose();
                    return true;
                }

                Bitmap toimg = new Bitmap(samize.Width, samize.Height);
                Graphics g = Graphics.FromImage(toimg);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, new Rectangle(0, 7, samize.Width, 31),
                    new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
                image.Dispose();

                EncoderParameters encParams = new EncoderParameters(1);
                // 65%品质的JPEG格式图片 
                encParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 95L);
                ImageCodecInfo codeInfo = null;
                ImageCodecInfo[] codeInfos = ImageCodecInfo.GetImageEncoders();
                foreach (ImageCodecInfo info in codeInfos)
                {
                    if (info.MimeType.Equals("image/jpeg"))
                    {
                        codeInfo = info;
                        break;
                    }
                }


                toimg.Save(thumbPath, codeInfo, encParams);

                //if (extension == ".jpg" || extension == ".jpeg")
                //    toimg.Save(thumbPath, System.Drawing.Imaging.ImageFormat.Jpeg,myEncoderParameters);
                //else if (extension == ".gif")
                //    toimg.Save(thumbPath, System.Drawing.Imaging.ImageFormat.Gif);
                //else if (extension == ".bmp")
                //    toimg.Save(thumbPath, System.Drawing.Imaging.ImageFormat.Bmp);
                //else if (extension == ".png")
                //    toimg.Save(thumbPath, System.Drawing.Imaging.ImageFormat.Png);
                //else
                //    toimg.Save(thumbPath, System.Drawing.Imaging.ImageFormat.Gif);

                //bitmap.Dispose();

                toimg.Dispose();
                g.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// 生成图片缩放
        /// </summary>
        /// <param name="path">原图路径如：C:\abc.jpg</param>
        /// <param name="thumbPath">缩放路径如：D:\t.jpg</param>
        /// <param name="iWidth">缩放宽度</param>
        /// <returns></returns>
        public static bool ThumbImg(string path, string thumbPath, int iWidth)
        {
            string extension = System.IO.Path.GetExtension(thumbPath).ToLower();
            if (".jpg.jpeg.gif.bmp.png".IndexOf(extension) == -1)
            {
                System.IO.File.Copy(path, thumbPath);
                return true;
            }
            //生成缩微图片，以JPEG格式存储
            System.Drawing.Image image = null;
            System.Drawing.Size samize = new System.Drawing.Size();
            //System.Drawing.Bitmap bitmap = null;

            try
            {
                image = System.Drawing.Image.FromFile(path);
                //获得缩微图片的大小
                samize = Calcsamize(image.Width, image.Height, iWidth);

                //缩微图片存放路径不存在则生成该路径
                string tempPath = System.IO.Path.GetDirectoryName(thumbPath);
                if (System.IO.Directory.Exists(tempPath) == false) System.IO.Directory.CreateDirectory(tempPath);

                if (extension == ".gif")
                {
                    //image = image.GetThumbnailImage(samize.Width, samize.Height,null, IntPtr.Zero);
                    image.Save(thumbPath, ImageFormat.Gif);
                    image.Dispose();
                    return true;
                }

                System.Drawing.Image toimg = new Bitmap(samize.Width, samize.Height);
                Graphics g = Graphics.FromImage(toimg);
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, new Rectangle(0, 0, samize.Width, samize.Height),
                    new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
                image.Dispose();


                EncoderParameters encParams = new EncoderParameters(1);
                // 65%品质的JPEG格式图片 
                encParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 95L);
                ImageCodecInfo codeInfo = null;
                ImageCodecInfo[] codeInfos = ImageCodecInfo.GetImageEncoders();
                foreach (ImageCodecInfo info in codeInfos)
                {
                    if (info.MimeType.Equals("image/jpeg"))
                    {
                        codeInfo = info;
                        break;
                    }
                }


                toimg.Save(thumbPath, codeInfo, encParams);

                //bitmap.Dispose();

                toimg.Dispose();
                g.Dispose();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///  获得缩略图的宽度和高度不超出指定的临界宽度和临界高度
        /// </summary>
        /// <param name="imgWidth">原宽</param>
        /// <param name="imgHeight">原高</param>
        /// <param name="width">临界宽</param>
        /// <returns></returns>
        public static System.Drawing.Size Calcsamize(int imgWidth, int imgHeight, int width)
        {
            double Multiplier = 0;
            Multiplier = ((double)width / (double)imgWidth);
            return new System.Drawing.Size(width, System.Convert.ToInt32(imgHeight * Multiplier));
        }
        /// <summary>
        /// 获得缩微图的宽度和高度不超出指定的临界宽度和临界高度
        /// </summary>
        /// <param name="imgWidth">原始宽度</param>
        /// <param name="imgHeight">原始高度</param>
        /// <param name="width">缩微的临界宽度</param>
        /// <param name="height">缩微图的临界高度</param>
        /// <returns></returns>
        public static System.Drawing.Size Calcsamize(int imgWidth, int imgHeight, int width, int height)
        {

            if (imgWidth <= width && imgHeight <= height)
            {
                return new System.Drawing.Size(imgWidth, imgHeight);
            }

            double fImgWidth = Convert.ToDouble(imgWidth);
            double fImgHeight = Convert.ToDouble(imgHeight);
            double fWidth = Convert.ToDouble(width);
            double fheight = Convert.ToDouble(height);

            //把fw定为缩放因子
            double fw = fImgWidth / fWidth;
            double fh = fImgHeight / fheight;
            if (fw > fh)
            {
                fheight = fImgHeight / fw;
            }
            else if (fh > fw)
            {
                fWidth = fImgWidth / fh;
            }
            else
            {
                if (imgWidth > width)
                {
                    fWidth = fImgWidth / fw;
                    fheight = fImgHeight / fh;
                }
                else
                {
                    return new System.Drawing.Size(width, height);
                }
            }

            //fWidth = fImgWidth * fw;
            //fheight = fImgHeight * fw;

            fWidth = Math.Round(fWidth);
            fheight = Math.Round(fheight);

            return new System.Drawing.Size(Convert.ToInt32(fWidth), Convert.ToInt32(fheight));
        }


        /// <summary>
        /// 生成图片缩放(不显示长宽)
        /// </summary>
        /// <param name="path">原图路径如：C:\abc.jpg</param>
        /// <param name="thumbPath">缩放路径如：D:\t.jpg</param>
        /// <returns></returns>
        /// <summary>
        public static bool ThumbImg(string path, bool isCreateDirectory, ref string thumbPath, long compressionRatio)
        {
            string dir = System.IO.Path.GetDirectoryName(path);
            if (!dir.EndsWith(@"\")) dir += @"\";
            string filename = System.IO.Path.GetFileName(path);
            string ex = System.IO.Path.GetExtension(path);

            string namestr = filename.Replace(ex, "");

            if (isCreateDirectory)
            {
                thumbPath = dir + @"\" + filename;
            }
            else
            {
                thumbPath = dir + thumbPath + "\\" + namestr + ex;
            }
            return ThumbImg(path, thumbPath, 99999, 99999, compressionRatio);
        }



        #region 水印
        /// <summary>
        /// 写入水印信息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="filePath"></param>
        /// <param name="DirectoryCode"></param>
        /// <returns></returns>
        public static bool WriteWaterInfo(System.Drawing.Image source, string filePath, string DirectoryCode)
        {
            string WaterImagePath = "";
            string WaterText = "";
            //文件不存在,直接退出
            if (source == null) return false;
            //取水印设置
            FileUploadHelper.GetDirectoryConfigInfo(DirectoryCode, ref WaterImagePath, ref WaterText);
            return WriteWaterInfo(source, filePath, WaterImagePath, WaterText);
        }
        /// <summary>
        /// 依据制定的水印文件或水印文字,写入水印到指定文件
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="WaterImagePath"></param>
        /// <param name="WaterText"></param>
        /// <returns></returns>
        public static bool WriteWaterInfo(string SourceFileName, string NewFileName, string WaterImagePath, string WaterText)
        {
            //文件不存在,直接退出
            if (!File.Exists(SourceFileName)) return false;

            bool Result = false;
            //如果有设置水印图片并且图片存在,则写入水印图片到原始文件
            if (WaterImagePath != "" && File.Exists(WaterImagePath))
            {
                System.Drawing.Image image = System.Drawing.Image.FromFile(SourceFileName);

                try
                {
                    AddImageSignPic(image, NewFileName, WaterImagePath, 1, 1, 3);
                    Result = true;
                }
                catch
                {
                    image.Dispose();
                }
            }

            //如果设置水印不成功,并且有设置水印文字,则写入水印文字到原始文件
            if (!Result && WaterText != "")
            {
                System.Drawing.Image image = System.Drawing.Image.FromFile(SourceFileName);

                try
                {
                    Graphics g = Graphics.FromImage(image);
                    Font f = new Font("Verdana", 32);
                    Brush b = new SolidBrush(Color.White);
                    g.DrawString(WaterText, f, b, 10, 10);
                    g.Dispose();
                    image.Save(NewFileName);
                    image.Dispose();

                    Result = true;
                }
                catch
                {
                    image.Dispose();
                }
            }

            //如果水印没有成功,则只拷贝文件
            if (!Result)
            {
                if (!File.Exists(NewFileName))
                    File.Move(SourceFileName, NewFileName);
                Result = true;
            }

            return Result;
        }
        /// <summary>
        /// 写入水印信息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="filePath"></param>
        /// <param name="WaterImagePath"></param>
        /// <param name="WaterText"></param>
        /// <returns></returns>
        private static bool WriteWaterInfo(System.Drawing.Image source, string filePath, string WaterImagePath, string WaterText)
        {
            bool Result = false;
            //如果有设置水印图片并且图片存在,则写入水印图片到原始文件
            if (WaterImagePath != "" && File.Exists(WaterImagePath))
            {
                try
                {
                    AddImageSignPic(source, filePath, WaterImagePath, 5, 1, 3);
                    Result = true;
                }
                catch (Exception)
                {

                }
            }

            //如果设置水印不成功,并且有设置水印文字,则写入水印文字到原始文件
            if (!Result && WaterText != "")
            {
                try
                {
                    int width = source.Width;
                    int height = (source.Height / 2).ToInt();
                    Graphics g = Graphics.FromImage(source);
                    Font f = new Font("Verdana", 9, FontStyle.Bold);
                    Brush b = new SolidBrush(Color.White);
                    Random r = new Random();
                    for (int i = r.Next(1, 30); i < width; i += 200)
                    {
                        g.DrawString(WaterText, f, b, i, height);
                    }
                    g.Dispose();
                    source.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    source.Dispose();

                    Result = true;
                }
                catch
                {
                    source.Dispose();
                }
            }

            //如果水印没有成功,则只拷贝文件
            if (!Result)
            {
                source.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                source.Dispose();
                Result = true;
            }

            return Result;
        }
        /// <summary>
        /// 加图片水印
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="watermarkFilename">水印文件名</param>
        /// <param name="watermarkStatus">图片水印位置</param>
        public static void AddImageSignPic(System.Drawing.Image img, string filename, string watermarkFilename, int watermarkStatus, int quality, int watermarkTransparency)
        {
            Graphics g = Graphics.FromImage(img);
            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            System.Drawing.Image watermark = new Bitmap(watermarkFilename);

            if (watermark.Height >= img.Height || watermark.Width >= img.Width)
                return;

            ImageAttributes imageAttributes = new ImageAttributes();
            ColorMap colorMap = new ColorMap();

            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            float transparency = 0.5F;
            if (watermarkTransparency >= 1 && watermarkTransparency <= 10)
                transparency = (watermarkTransparency / 10.0F);


            float[][] colorMatrixElements = {
												new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
												new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
												new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
												new float[] {0.0f,  0.0f,  0.0f,  transparency, 0.0f},
												new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
											};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            int xpos = 0;
            int ypos = 0;

            switch (watermarkStatus)
            {
                case 1:
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 2:
                    xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 3:
                    xpos = (int)((img.Width * (float).99) - (watermark.Width));
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 4:
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                    break;
                case 5:
                    xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                    ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                    break;
                case 6:
                    xpos = (int)((img.Width * (float).99) - (watermark.Width));
                    ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                    break;
                case 7:
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)((img.Height * (float).99) - watermark.Height);
                    break;
                case 8:
                    xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                    ypos = (int)((img.Height * (float).99) - watermark.Height);
                    break;
                case 9:
                    xpos = (int)((img.Width * (float).99) - (watermark.Width));
                    ypos = (int)((img.Height * (float).99) - watermark.Height);
                    break;
            }

            g.DrawImage(watermark, new Rectangle(xpos, ypos, watermark.Width, watermark.Height), 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel, imageAttributes);

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo ici = null;
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.MimeType.IndexOf("jpeg") > -1)
                    ici = codec;
            }
            EncoderParameters encoderParams = new EncoderParameters();
            long[] qualityParam = new long[1];
            if (quality < 0 || quality > 100)
                quality = 80;

            qualityParam[0] = quality;

            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityParam);
            encoderParams.Param[0] = encoderParam;

            //if (ici != null)
            //    img.Save(filename, ici, encoderParams);
            //else
            img.Save(filename);

            g.Dispose();
            img.Dispose();
            watermark.Dispose();
            imageAttributes.Dispose();
        }








        ///// <summary>
        ///// 加图片水印
        ///// </summary>
        ///// <param name="filename">文件名</param>
        ///// <param name="watermarkFilename">水印文件名</param>
        ///// <param name="watermarkStatus">图片水印位置</param>
        //public static void AddImageSignPic(System.Drawing.Image img, string filename, string watermarkFilename, int watermarkStatus, int quality, int watermarkTransparency)
        //{
        //    Graphics g = Graphics.FromImage(img);
        //    //设置高质量插值法
        //    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
        //    //设置高质量,低速度呈现平滑程度
        //    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        //    System.Drawing.Image watermark = new Bitmap(watermarkFilename);

        //    if (watermark.Height >= img.Height || watermark.Width >= img.Width)
        //        return;

        //    ImageAttributes imageAttributes = new ImageAttributes();
        //    ColorMap colorMap = new ColorMap();

        //    colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
        //    colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
        //    ColorMap[] remapTable = { colorMap };

        //    imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

        //    float transparency = 0.5F;
        //    if (watermarkTransparency >= 1 && watermarkTransparency <= 10)
        //        transparency = (watermarkTransparency / 10.0F);


        //    float[][] colorMatrixElements = {
        //                                        new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
        //                                        new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
        //                                        new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
        //                                        new float[] {0.0f,  0.0f,  0.0f,  transparency, 0.0f},
        //                                        new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
        //                                    };

        //    ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

        //    imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

        //    int xpos = 0;
        //    int ypos = 0;

        //    switch (watermarkStatus)
        //    {
        //        case 1:
        //            xpos = (int)(img.Width * (float).01);
        //            ypos = (int)(img.Height * (float).01);
        //            break;
        //        case 2:
        //            xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
        //            ypos = (int)(img.Height * (float).01);
        //            break;
        //        case 3:
        //            xpos = (int)((img.Width * (float).99) - (watermark.Width));
        //            ypos = (int)(img.Height * (float).01);
        //            break;
        //        case 4:
        //            xpos = (int)(img.Width * (float).01);
        //            ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
        //            break;
        //        case 5:
        //            xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
        //            ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
        //            break;
        //        case 6:
        //            xpos = (int)((img.Width * (float).99) - (watermark.Width));
        //            ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
        //            break;
        //        case 7:
        //            xpos = (int)(img.Width * (float).01);
        //            ypos = (int)((img.Height * (float).99) - watermark.Height);
        //            break;
        //        case 8:
        //            xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
        //            ypos = (int)((img.Height * (float).99) - watermark.Height);
        //            break;
        //        case 9:
        //            xpos = (int)((img.Width * (float).99) - (watermark.Width));
        //            ypos = (int)((img.Height * (float).99) - watermark.Height);
        //            break;
        //    }

        //    g.DrawImage(watermark, new Rectangle(xpos, ypos, watermark.Width, watermark.Height), 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel, imageAttributes);

        //    ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
        //    ImageCodecInfo ici = null;
        //    foreach (ImageCodecInfo codec in codecs)
        //    {
        //        if (codec.MimeType.IndexOf("jpeg") > -1)
        //            ici = codec;
        //    }
        //    EncoderParameters encoderParams = new EncoderParameters();
        //    long[] qualityParam = new long[1];
        //    if (quality < 0 || quality > 100)
        //        quality = 80;

        //    qualityParam[0] = quality;

        //    EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityParam);
        //    encoderParams.Param[0] = encoderParam;

        //    //if (ici != null)
        //    //    img.Save(filename, ici, encoderParams);
        //    //else
        //    img.Save(filename);

        //    g.Dispose();
        //    img.Dispose();
        //    watermark.Dispose();
        //    imageAttributes.Dispose();
        //}

        #endregion

    }
}
