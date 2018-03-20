using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using TYSystem.BaseFramework.Common.Cached;

namespace TYSystem.BaseFramework.Common.Helper
{
    /// <summary>
    /// 验证码生成帮助类
    /// </summary>
    public class ValidationCode
    {
        //用户存取验证码字符串
        public string validationCode = String.Empty;

        Graphics g = null;

        int bgWidth = 0;
        int bgHeight = 0;

        public string FontFace = "Comic Sans MS";
        public int FontSize = 9;
        public Color foreColor = Color.FromArgb(220, 220, 220);
        public Color backColor = Color.FromArgb(190, 190, 190);
        public Color mixedLineColor = Color.FromArgb(220, 220, 220);
        public int mixedLineWidth = 1;
        public int mixedLineCount = 5;


        #region 根据指定长度，返回随机验证码
        /// <summary>
        /// 根据指定长度，返回随机验证码
        /// </summary>
        /// <param >制定长度</param>
        /// <returns>随即验证码</returns>
        public string Next(int length)
        {
            this.validationCode = GetRandomCode(length);
            return this.validationCode;
        }
        #endregion


        #region 根据指定长度及背景图片样式，返回带有随机验证码的图片对象
        /// <summary>
        /// 根据指定长度及背景图片样式，返回带有随机验证码的图片对象
        /// </summary>
        /// <param >指定长度</param>
        /// <param >背景图片样式</param>
        /// <returns>Image对象</returns>
        public Image NextImage(int length, HatchStyle hatchStyle, bool allowMixedLines)
        {
            this.validationCode = GetRandomCode(length);

            //校验码字体
            Font myFont = new Font(FontFace, FontSize);

            //根据校验码字体大小算出背景大小
            bgWidth = (int)myFont.Size * length + 4;
            bgHeight = (int)myFont.Size * 2;
            //生成背景图片
            Bitmap myBitmap = new Bitmap(bgWidth, bgHeight);

            g = Graphics.FromImage(myBitmap);


            this.DrawBackground(hatchStyle);
            this.DrawValidationCode(this.validationCode, myFont);
            if (allowMixedLines)
                this.DrawMixedLine();

            return (Image)myBitmap;
        }
        #endregion


        #region 内部方法：绘制验证码背景
        private void DrawBackground(HatchStyle hatchStyle)
        {
            //设置填充背景时用的笔刷
            HatchBrush hBrush = new HatchBrush(hatchStyle, backColor);

            //填充背景图片
            g.FillRectangle(hBrush, 0, 0, this.bgWidth, this.bgHeight);
        }
        #endregion


        #region 内部方法：绘制验证码
        private void DrawValidationCode(string vCode, Font font)
        {
            g.DrawString(vCode, font, new SolidBrush(this.foreColor), 2, 2);
        }
        #endregion


        #region 内部方法：绘制干扰线条
        /// <summary>
        /// 绘制干扰线条
        /// </summary>
        private void DrawMixedLine()
        {
            for (int i = 0; i < mixedLineCount; i++)
            {
                g.DrawBezier(new Pen(new SolidBrush(mixedLineColor), mixedLineWidth), RandomPoint(), RandomPoint(), RandomPoint(), RandomPoint());
            }
        }
        #endregion

        #region 内部方法：返回指定长度的随机验证码字符串
        /// <summary>
        /// 根据指定大小返回随机验证码
        /// </summary>
        /// <param >字符串长度</param>
        /// <returns>随机字符串</returns>
        private string GetRandomCode(int length)
        {
            return RandomHelper.RandCode(length);
        }
        #endregion


        #region 内部方法：产生随机数和随机点


        /// <summary>
        /// 返回一个随机点，该随机点范围在验证码背景大小范围内
        /// </summary>
        /// <returns>Point对象</returns>
        private Point RandomPoint()
        {
            Thread.Sleep(15);
            Random ram = new Random();
            Point point = new Point(ram.Next(this.bgWidth), ram.Next(this.bgHeight));
            return point;
        }
        #endregion


    }

    /// <summary>
    /// 生成验证码
    /// </summary>
    public class ValidationCode2
    {
        /// <summary>
        /// 根据验证码字符生成验证码图片
        /// </summary>
        /// <param name="checkCode"></param>
        /// <returns></returns>
        public static Image CreateCheckCodeImage()
        {
            string checkCode = GenerateCheckCode();
            HttpContext.Current.Session["uuid"] = checkCode; //将字符串保存到Session中，以便需要时进行验证


            System.Drawing.Bitmap image = new System.Drawing.Bitmap(70, 22);
            Graphics g = System.Drawing.Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(System.Drawing.Color.White);
                // 画图片的背景噪音线
                int i;
                for (i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }

                Font font = new System.Drawing.Font("Arial", 12, (System.Drawing.FontStyle.Bold));
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2F, true);
                g.DrawString(checkCode, font, brush, 2, 2);
                //画图片的前景噪音点
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);

            }
            finally
            {
                g.Dispose();
            }
            return image;
        }

        /// <summary>
        /// 保存到缓存
        /// </summary>
        /// <param name="gu"></param>
        /// <returns></returns>
        public static Image CreateAppCheckCodeImage(string gu)
        {
            string checkCode = GenerateCheckCode();
            //HttpContext.Current.Session["uuid"] = checkCode; //将字符串保存到Session中，以便需要时进行验证
          
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(70, 22);
            Graphics g = System.Drawing.Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(System.Drawing.Color.White);
                // 画图片的背景噪音线
                int i;
                for (i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }

                Font font = new System.Drawing.Font("Arial", 12, (System.Drawing.FontStyle.Bold));
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2F, true);
                g.DrawString(checkCode, font, brush, 2, 2);
                //画图片的前景噪音点
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);

            }
            finally
            {
                g.Dispose();
            }
            return image;
        }

        /// <summary>
        /// 生成验证码字符
        /// </summary>
        /// <param name="validateCodeType">(0-字母数字混合,1-数字,2-字母)</param>
        /// <returns></returns>
        public static string GenerateCheckCode(string validateCodeType = "0")
        {
            int number;
            char code;
            string checkCode = String.Empty;
            System.Random random = new Random();
            for (int i = 0; i < 4; i++)
            {
                number = random.Next();
                code = (char)('0' + (char)(number % 10));
                if (number % 2 == 0)
                    code = (char)('0' + (char)(number % 10));
                else
                    code = (char)('A' + (char)(number % 26));
                //去掉0，Modify By KennyPeng On 2008-11-10
                if (code.ToString() == "O" || code.ToString() == "I" || code.ToString() == "o" || code.ToString() == "i")
                {
                    i--;
                    continue;
                }
                //要求全为数字
                if (validateCodeType == "1")
                {
                    if ((int)code < 48 || (int)code > 57)
                    {
                        i--;
                        continue;
                    }
                }

                //要求全为字母
                if (validateCodeType == "2")
                {
                    if ((int)code > 47 || (int)code < 58)
                    {
                        i--;
                        continue;
                    }
                }
                checkCode += code.ToString();
            }
            return checkCode;
        }
    }

}
