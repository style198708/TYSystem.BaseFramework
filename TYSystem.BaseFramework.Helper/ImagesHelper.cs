using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TYSystem.BaseFramework.Common.Enum;

namespace TYSystem.BaseFramework.Common.Helper
{
    public enum MakeImagesMode : short
    {
        /// <summary>
        /// 无
        /// </summary>
        Nothing = 0,
        /// <summary>
        /// 指定高宽缩放（可能变形）
        /// </summary>
        HW = 1,
        /// <summary>
        /// 指定宽，高按比例
        /// </summary>
        W = 2,
        /// <summary>
        /// 指定高，宽按比例
        /// </summary>
        H = 3,
        /// <summary>
        /// 指定高宽裁减（不变形）
        /// </summary>
        Cut = 4,

        /// <summary>
        /// 按照宽度成比例缩放后，按照指定的高度进行裁剪
        /// </summary>
        W_HCut = 5,
    }

    public class ThumbImage
    {
        public MakeImagesMode MakeImagesMode { get; set; }
        public string ThumbnailPath { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string EndFix { get; set; }
    }

    public class ImagesHelper
    {
        //public static void MakeThumbImage(string originalImagePath, List<ThumbImage> ThumbImageList)
        //{
        //    foreach (ThumbImage item in ThumbImageList)
        //    {
        //        MakeThumbImage(originalImagePath, item.ThumbnailPath, item.Width, item.Height, item.MakeImagesMode);
        //    }
        //}

        #region 根据流生成指定大小图片的方法
        /// <summary>
        /// 根据流生成指定大小图片的方法
        /// </summary>
        /// <param name="stream">上传的图片流</param>
        /// <param name="path">生成图片存放的路径</param>
        /// <param name="width">生成图片的宽度</param>
        /// <param name="height">生成图片的高度</param>
        /// <param name="mode">生成的模式（ "HW":指定高宽缩放；"W"://指定宽度，计算缩略图；"H":指定高度，计算缩略图；"CUT":）</param>
        /// <returns></returns>
        public static string MakeThumbImage(Stream stream, string path, int width, int height, MakeImagesMode ImageMode)
        {
            System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
            return MakeImagePath(image, path, width, height, ImageMode);
        }
        #endregion

        #region 根据原始图片生成缩略图的方法
        /// <summary>
        /// 生成缩略图的方法
        /// </summary>
        /// <param name="originalImagePath">原始图片路径</param>
        /// <param name="thumbnailPath">生成图片存放的路径</param>
        /// <param name="width">生成图片的宽度</param>
        /// <param name="height">生成图片的高度</param>
        /// <param name="mode">生成的模式（ "HW":指定高宽缩放；"W"://指定宽度，计算缩略图；"H":指定高度，计算缩略图；"CUT":）</param>
        /// <returns></returns>
        public static string MakeThumbImage(string originalImagePath, string thumbnailPath, int width, int height, MakeImagesMode ImageMode)
        {
            if (string.IsNullOrEmpty(thumbnailPath) || string.IsNullOrEmpty(originalImagePath))
            {
                return "路径不能为空";
            }
            string messages = string.Empty;
            System.Drawing.Image image = System.Drawing.Image.FromFile(originalImagePath);
            if (image == null)
                return "图片创建失败";


            return MakeImagePath(image, thumbnailPath, width, height, ImageMode);
        }
        #endregion

        #region MakeThumbImage方法私有调用
        public static string MakeImagePath(System.Drawing.Image image, string savePath, int width, int height, MakeImagesMode ImageMode)
        {
            string messages = string.Empty;
            if (string.IsNullOrEmpty(savePath))
            {
                return "路径不能为空";
            }
            System.Drawing.Image bitmap = MakeImage(image, width, height, ImageMode, ref messages);
            if (bitmap == null || !string.IsNullOrEmpty(messages))
            {
                return messages;
            }
            try
            {
                FileInfo file = new FileInfo(savePath);
                string dirPath = file.Directory.FullName;
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                ImageCodecInfo ici; EncoderParameters ep;
                MakeImageCodecInfo(out ici, out ep);
                bitmap.Save(savePath, ici, ep);
            }
            catch (Exception ex)
            {
                messages = ex.Message;
            }
            finally
            {
                image.Dispose();
                bitmap.Dispose();
                if (messages == string.Empty)
                {
                    messages = "成功生成！";
                }

            }
            return messages;
        }
        public static System.Drawing.Image MakeImage(System.Drawing.Image image, int width, int height, MakeImagesMode ImageMode, ref string errMsg)
        {
            int tw = width;
            int th = height;
            //原始图片的宽度和高度
            int sw = image.Width;
            int sh = image.Height;
            if (tw == 0 && th == 0)//如果设置小于0,不进行缩放
            {
                tw = sw;
                th = sh;
            }
            int x = 0, y = 0;
            switch (ImageMode)
            {
                case MakeImagesMode.HW://指定高宽缩放
                    break;
                case MakeImagesMode.W://按比例缩放，指定宽度，计算缩略图的高度
                    th = image.Height * tw / image.Width;
                    break;
                case MakeImagesMode.H://按比例缩放，指定高度，计算缩略图的高度
                    tw = image.Width * th / image.Height;
                    break;
                case MakeImagesMode.Cut:
                    if (width > 0 && height > 0 && (double)tw / (double)th < (double)width / (double)height)
                    {
                        sw = image.Width;
                        sh = image.Width * height / tw;
                        x = 0;
                        y = (image.Height - sh) / 2;
                    }
                    else
                    {
                        sh = image.Height;
                        sw = image.Height * width / th;
                        y = 0;
                        x = (image.Width - sw) / 2;
                    }
                    break;
                default:
                    break;
            }
            //if (Math.Abs(sw - tw) < 5 && Math.Abs(sh - th) < 5)
            //{
            //    tw = sw;
            //    th = sh;
            //}

            if (sw < tw)//如果原图片小于要裁剪的图片,就不进行裁剪 
            {
                tw = sw;
                th = sh;
            }
            //关键质量控制
            //获取系统编码类型数组,包含了jpeg,bmp,png,gif,tiff

            System.Drawing.Image bitmap = new System.Drawing.Bitmap(tw, th, PixelFormat.Format32bppArgb);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            try
            {

                g.FillRectangle(Brushes.White, 0, 0, tw, th);
                //设置高质量插值法
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                g.Clear(System.Drawing.Color.Transparent);
                g.DrawImage(image,
                    new System.Drawing.Rectangle(x, y, tw, th),
                    new System.Drawing.Rectangle(x, y, sw, sh),
                    System.Drawing.GraphicsUnit.Pixel);
                return bitmap;

            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                g.Dispose();
                return null;
            }
            finally
            {
                if (g != null)
                {
                    g.Dispose();
                    System.GC.Collect();
                }
            }
        }
        private static void MakeImageCodecInfo(out ImageCodecInfo ici, out EncoderParameters ep)
        {
            ici = null;
            ep = null;
            ImageCodecInfo[] icis = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo i in icis)
            {
                if (i.MimeType == "image/jpeg")
                {
                    ici = i;
                }
            }
            ep = new EncoderParameters(1);
            ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Convert.ToInt32(100));
        }
        #endregion










        #region  需要测试
        public static void CreateCheckCodeImage(HttpContext context, string checkCode)
        {
            if (checkCode == null || checkCode.Trim() == String.Empty)
                return;
            int iWordWidth = 20;
            int iImageWidth = checkCode.Length * iWordWidth;
            Bitmap image = new Bitmap(iImageWidth, 30);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器 
                Random random = new Random();
                //清空图片背景色 
                g.Clear(Color.White);

                //画图片的背景噪音点
                for (int i = 0; i < 20; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }

                //画图片的背景噪音线 
                for (int i = 0; i < 2; i++)
                {
                    int x1 = 0;
                    int x2 = image.Width;
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    if (i == 0)
                    {
                        g.DrawLine(new Pen(Color.Gray, 2), x1, y1, x2, y2);
                    }

                }


                for (int i = 0; i < checkCode.Length; i++)
                {

                    string Code = checkCode[i].ToString();
                    int xLeft = iWordWidth * (i);
                    random = new Random(xLeft);
                    int iSeed = DateTime.Now.Millisecond;
                    int iValue = random.Next(iSeed) % 4;
                    if (iValue == 0)
                    {
                        Font font = new Font("Arial", 13, (FontStyle.Bold | System.Drawing.FontStyle.Italic));
                        Rectangle rc = new Rectangle(xLeft, 0, iWordWidth, image.Height);
                        System.Drawing.Drawing2D.LinearGradientBrush brush = new LinearGradientBrush(rc, Color.Blue, Color.Red, 1.5f, true);
                        g.DrawString(Code, font, brush, xLeft, 2);
                    }
                    else if (iValue == 1)
                    {
                        Font font = new System.Drawing.Font("楷体", 13, (FontStyle.Bold));
                        Rectangle rc = new Rectangle(xLeft, 0, iWordWidth, image.Height);
                        LinearGradientBrush brush = new LinearGradientBrush(rc, Color.Blue, Color.DarkRed, 1.3f, true);
                        g.DrawString(Code, font, brush, xLeft, 2);
                    }
                    else if (iValue == 2)
                    {
                        Font font = new System.Drawing.Font("宋体", 13, (System.Drawing.FontStyle.Bold));
                        Rectangle rc = new Rectangle(xLeft, 0, iWordWidth, image.Height);
                        LinearGradientBrush brush = new LinearGradientBrush(rc, Color.Green, Color.Blue, 1.2f, true);
                        g.DrawString(Code, font, brush, xLeft, 2);
                    }
                    else if (iValue == 3)
                    {
                        Font font = new System.Drawing.Font("黑体", 13, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Bold));
                        Rectangle rc = new Rectangle(xLeft, 0, iWordWidth, image.Height);
                        LinearGradientBrush brush = new LinearGradientBrush(rc, Color.Blue, Color.Green, 1.8f, true);
                        g.DrawString(Code, font, brush, xLeft, 2);
                    }
                }
                //////画图片的前景噪音点 
                //for (int i = 0; i < 8; i++)
                //{
                //    int x = random.Next(image.Width);
                //    int y = random.Next(image.Height);
                //    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                //}
                //画图片的边框线 
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();

                //关键质量控制
                //获取系统编码类型数组,包含了jpeg,bmp,png,gif,tiff
                ImageCodecInfo[] icis = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo ici = null;
                foreach (ImageCodecInfo i in icis)
                {
                    if (i.MimeType == "image/jpeg" || i.MimeType == "image/bmp" || i.MimeType == "image/png" || i.MimeType == "image/gif")
                    {
                        ici = i;
                    }
                }
                EncoderParameters ep = new EncoderParameters(1);
                ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)100);

                image.Save(ms, ici, ep);
                context.Response.ClearContent();

                context.Response.BinaryWrite(ms.ToArray());
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        public static void CreateCheckCodeImage2(HttpContext context, string checkCode)
        {
            if (checkCode == null || checkCode.Trim() == String.Empty)
                return;
            System.Drawing.Bitmap image = new System.Drawing.Bitmap((int)Math.Ceiling((checkCode.Length * 12.5)), 22);
            System.Drawing.Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器 
                Random random = new Random();
                //清空图片背景色 
                g.Clear(Color.White);
                //画图片的背景噪音线 
                for (int i = 0; i < 2; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Black), x1, y1, x2, y2);
                }
                Font font = new System.Drawing.Font("Arial", 12, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic));
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(checkCode, font, brush, 2, 2);
                //画图片的前景噪点 
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线 
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                context.Response.ClearContent();
                context.Response.ContentType = "image/Gif";
                context.Response.BinaryWrite(ms.ToArray());
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }


        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="FileName">生成图片文件夹路径</param>
        /// <returns>生成图片路径</returns>
        public static Tuple<string, string> CreateCheckCodeImage(string FileName)
        {
            string chkCode = string.Empty;
            //颜色列表，用于验证码、噪线、噪点 
            Color[] color = { Color.Black, Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Brown, Color.Brown, Color.DarkBlue };
            //字体列表，用于验证码 
            string[] font = { "Times New Roman", "MS Mincho", "Book Antiqua", "Gungsuh", "PMingLiU", "Impact" };
            //验证码的字符集，去掉了一些容易混淆的字符 
            char[] character = { '2', '3', '4', '5', '6', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'R', 'S', 'T', 'W', 'X', 'Y' };
            Random rnd = new Random();
            //生成验证码字符串 
            for (int i = 0; i < 4; i++)
            {
                chkCode += character[rnd.Next(character.Length)];
            }
            Bitmap bmp = new Bitmap(100, 40);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.LightGray);

            //画验证码字符串 
            for (int i = 0; i < chkCode.Length; i++)
            {
                string fnt = font[rnd.Next(font.Length)];
                Font ft = new Font(fnt, 18);
                Color clr = color[rnd.Next(color.Length)];
                g.DrawString(chkCode[i].ToString(), ft, new SolidBrush(clr), (float)i * 20 + 8, (float)8);
            }
            string file = FileName + Guid.NewGuid().ToString() + ".png";
            bmp.Save(FileName);
            return new Tuple<string, string>(chkCode, file);
        }
        #endregion

        #region 根据固定高度（500）裁剪图片
        /// <summary>
        /// 根据高度裁剪图片
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string CutImageByHeight(Stream stream, string path, string imgName, string ext)
        {
            Image source = Image.FromStream(stream);
            return SplitImage(path, imgName, source, 500, source.Width, ext);//切割
        }

        private static string SplitImage(String path, String file, Image img, Int32 sHeight, Int32 sWidth, string ext)
        {
            StringBuilder sFile = new StringBuilder();


            if ((sWidth - img.Width < 5 || img.Width - sWidth < 5))
            {
                sWidth = img.Width;
            }
            Image i = new Bitmap(sWidth, sHeight);
            Graphics gr = Graphics.FromImage(i);
            int endHeight = img.Height % 200;
            ImageCodecInfo ici; EncoderParameters ep;
            MakeImageCodecInfo(out ici, out ep);

            int iwidth = 0;
            for (Int32 y = 0; y < img.Height; y += sHeight)
            {
                if (y == img.Height - endHeight)
                {
                    i = new Bitmap(sWidth, endHeight);
                    gr = Graphics.FromImage(i);
                    sHeight = endHeight;
                }
                for (Int32 x = 0; x < img.Width; x += sWidth)
                {
                    string fileName = file + y.ToString() + ext;
                    gr.Clear(Color.Transparent);
                    gr.DrawImage(img, new Rectangle(0, 0, i.Width, i.Height), x, y, sWidth, sHeight, GraphicsUnit.Pixel);
                    gr.Save();
                    Image tmp = i.Clone() as Image;
                    if (iwidth % 3 == 0)
                        GraphicHelper.WriteWaterInfo(tmp, Path.Combine(path, fileName), "ProductDesc");
                    else
                        i.Save(Path.Combine(path, fileName), ici, ep);
                    sFile.Append(fileName);
                    sFile.Append(",");
                    iwidth++;
                }
            }
            return sFile.ToString().TrimEnd(',');
        }
        #endregion

        /// <summary>
        /// 无损压缩图片
        /// </summary>
        /// <param name="sourceFile">原图片</param>
        /// <param name="stream">压缩后保存到流中</param>
        /// <param name="height">高度</param>
        /// <param name="width"></param>
        /// <param name="quality">压缩质量 1-100</param>
        /// <param name="type">压缩缩放类型</param>
        /// <returns></returns>
        public static System.Drawing.Image Thumbnail(System.Drawing.Image image, int width, int height, MakeImagesMode type, ref string errMsg)
        {

            //tFormat = iSource.RawFormat;
            //缩放后的宽度和高度
            int toWidth = width;
            int toHeight = height;
            //
            int x = 0;
            int y = 0;
            int oWidth = image.Width;
            int oHeight = image.Height;

            switch (type)
            {
                case MakeImagesMode.HW://指定高宽缩放（可能变形）           
                    {
                        break;
                    }
                case MakeImagesMode.W://指定宽，高按比例     
                    {
                        toHeight = image.Height * width / image.Width;
                        break;
                    }
                case MakeImagesMode.H://指定高，宽按比例
                    {
                        toWidth = image.Width * height / image.Height;
                        break;
                    }
                case MakeImagesMode.Cut://指定高宽裁减（不变形）     
                    {
                        if ((double)image.Width / (double)image.Height > (double)toWidth / (double)toHeight)
                        {
                            oHeight = image.Height;
                            oWidth = image.Height * toWidth / toHeight;
                            y = 0;
                            x = (image.Width - oWidth) / 2;
                        }
                        else
                        {
                            oWidth = image.Width;
                            oHeight = image.Width * height / toWidth;
                            x = 0;
                            y = (image.Height - oHeight) / 2;
                        }
                        break;
                    }
                case MakeImagesMode.W_HCut://按照宽度成比例缩放后，按照指定的高度进行裁剪
                    {
                        toHeight = image.Height * width / image.Width;
                        if (height < toHeight)
                        {
                            oHeight = oHeight * height / toHeight;
                            toHeight = toHeight * height / toHeight;
                        }
                        break;
                    }
                default:
                    break;
            }

            Bitmap ob = new Bitmap(toWidth, toHeight);
            //ImgWaterMark iwm = new ImgWaterMark();
            //iwm.AddWaterMark(ob, towidth, toheight, "www.****.com");
            Graphics g = Graphics.FromImage(ob);
            g.Clear(System.Drawing.Color.WhiteSmoke);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(image
              , new Rectangle(x, y, toWidth, toHeight)
              , new Rectangle(0, 0, oWidth, oHeight)
              , GraphicsUnit.Pixel);
            g.Dispose();

            return ob;


        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>
        public static void MakeThumbnailV2(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            Image originalImage = Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）
                    break;
                case "W"://指定宽，高按比例
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                case "CutA"://指定高宽裁减（不变形）自定义
                    if (ow <= towidth && oh <= toheight)
                    {
                        x = -(towidth - ow) / 2;
                        y = -(toheight - oh) / 2;
                        ow = towidth;
                        oh = toheight;
                    }
                    else
                    {
                        if (ow > oh)//宽大于高
                        {
                            x = 0;
                            y = -(ow - oh) / 2;
                            oh = ow;
                        }
                        else//高大于宽
                        {
                            y = 0;
                            x = -(oh - ow) / 2;
                            ow = oh;
                        }
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以白色背景色填充
            g.Clear(Color.White);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
            new Rectangle(x, y, ow, oh),
            GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图
                FileInfo file = new FileInfo(thumbnailPath);
                string dirPath = file.Directory.FullName;
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }



    }

    public class EnumThumbImageType
    {
        ///// <summary>
        ///// 卡类临时图片地址
        ///// </summary>
        //[EnumAttribute("卡类临时图片地址")]
        //public const string CardTypeImage = "//FileHandler/CardTypeImages/CardTypeImage";

        ///// <summary>
        ///// 活动临时图片地址
        ///// </summary>
        //[EnumAttribute("活动临时图片地址")]
        //public const string ActivityImage = "//FileHandler/ActivityImages/ActivityImage";
        /// <summary>
        /// 活动临时图片地址
        /// </summary>
        [EnumAttribute("活动临时图片地址")]
        public const string ShowOrderImage = "//FileHandler/ShowOrderImages/ShowOrderImage";


        ///// <summary>
        ///// 新闻左侧广告位
        ///// </summary>
        //[EnumAttribute("新闻左侧广告位")]
        //public const string NewLeftImages = "//FileHandler/NewLeftImages/NewLeftImage";

        ///// <summary>
        ///// 新闻右侧广告位
        ///// </summary>
        //[EnumAttribute("新闻右侧广告位")]
        //public const string NewRigthImages = "//FileHandler/NewRigthImages/NewRigthImage";

        /// <summary>
        /// 产品类临时图片地址
        /// </summary>
        [EnumAttribute("产品类临时图片地址")]
        public const string ProductTypeImage = "//FileHandler/ProdcutTypeImages/ProdcutTypeImage";

    }
}
