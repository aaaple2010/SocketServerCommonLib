using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;

/// <summary>
/// 验证码
/// </summary>
public class CodeImage
{

    #region Private Fields
    private const double PI = 3.1415926535897932384626433832795;
    private const double PI2 = 6.283185307179586476925286766559;
    private static int _len = 4;
    private static readonly Single _jianju = (float)18.0;
    private static readonly Single _height = (float)24.0;
    public static string _checkCode;
    #endregion

    #region Constructors

    #endregion

    #region Private Methods

    private static string GenerateNumbers()
    {
        string strOut = "";
        System.Random random = new Random();
        for (int i = 0; i < _len; i++)
        {
            string num = Convert.ToString(random.Next(10000) % 10);
            strOut += num;
        }
        return strOut.Trim();
    }

    private static string GenerateCharacters()
    {
        string strOut = "";
        System.Random random = new Random();
        for (int i = 0; i < _len; i++)
        {
            string num = Convert.ToString((char)(65 + random.Next(10000) % 26));
            strOut += num;
        }
        return strOut.Trim();
    }

    private static string GenerateAlphas()
    {
        string strOut = "";
        string num = "";
        System.Random random = new Random();
        for (int i = 0; i < _len; i++)
        {
            if (random.Next(500) % 2 == 0)
            {
                num = Convert.ToString(random.Next(10000) % 10);
            }
            else
            {
                num = Convert.ToString((char)(65 + random.Next(10000) % 26));
            }
            strOut += num;
        }
        return strOut.Trim();
    }

    private static System.Drawing.Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
    {
        System.Drawing.Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);
        // 将位图背景填充为白色 
        System.Drawing.Graphics graph = System.Drawing.Graphics.FromImage(destBmp);
        graph.FillRectangle(new SolidBrush(System.Drawing.Color.White), 0, 0, destBmp.Width, destBmp.Height);
        graph.Dispose();
        double dBaseAxisLen = bXDir ? (double)destBmp.Height : (double)destBmp.Width;
        for (int i = 0; i < destBmp.Width; i++)
        {
            for (int j = 0; j < destBmp.Height; j++)
            {
                double dx = 0;
                dx = bXDir ? (PI2 * (double)j) / dBaseAxisLen : (PI2 * (double)i) / dBaseAxisLen;
                dx += dPhase;
                double dy = Math.Sin(dx);
                // 取得当前点的颜色 
                int nOldX = 0, nOldY = 0;
                nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                nOldY = bXDir ? j : j + (int)(dy * dMultValue);
                System.Drawing.Color color = srcBmp.GetPixel(i, j);
                if (nOldX >= 0 && nOldX < destBmp.Width
                 && nOldY >= 0 && nOldY < destBmp.Height)
                {
                    destBmp.SetPixel(nOldX, nOldY, color);
                }
            }
        }
        return destBmp;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// 验证时转为小写副本
    /// </summary>
    /// <returns></returns>
    public static Image CreateCheckCodeImage()
    {
        string checkCode;

        checkCode = GenerateAlphas();
        _checkCode = checkCode.ToLower();
        MemoryStream ms = null;
        if (checkCode == null || checkCode.Trim() == String.Empty)
            return null;

        Bitmap image = new System.Drawing.Bitmap((int)Math.Ceiling((checkCode.Length * _jianju)), (int)_height);
        Graphics g = Graphics.FromImage(image);
        try
        {
            Random random = new Random();
            g.Clear(Color.White);
            // 画图片的背景噪音线 
            for (int i = 0; i < 4; i++)
            {
                int x1 = random.Next(image.Width);
                int x2 = random.Next(image.Width);
                int y1 = random.Next(image.Height);
                int y2 = random.Next(image.Height);
                g.DrawLine(new Pen(Color.FromArgb(random.Next()), 1), x1, y1, x2, y2);
            }
            Font font = new System.Drawing.Font("Times New Roman", 14, System.Drawing.FontStyle.Bold);
            LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);

            for (int i = 0; i < checkCode.Length; i++)
            {
                g.DrawString(checkCode.Substring(i, 1), font, brush, 2 + i * _jianju, 1);
            }

            // 画图片的前景噪音点 
            for (int i = 0; i < 40; i++)
            {
                int x = random.Next(image.Width);
                int y = random.Next(image.Height);
                image.SetPixel(x, y, Color.FromArgb(random.Next()));
            }
            // 画图片的波形滤镜效果 
            image = TwistImage(image, true, 3, 1);
            // 画图片的边框线 
            g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
            ms = new System.IO.MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
        }
        finally
        {
            g.Dispose();
            image.Dispose();
        }
        return Bitmap.FromStream(ms);
    }
    #endregion
}

