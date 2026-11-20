using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using System.Web.Mvc;

namespace Utac.Rnc2.UI.Web.Models
{
    public class CaptchaImageResult : ActionResult
    {
        public string GetCaptchaString(int length)
        {
            int intZero = '0';
            int intNine = '9';
            int intA = 'A';
            int intZ = 'Z';
            int intCount = 0;
            int intRandomNumber = 0;
            string strCaptchaString = "";

            Random random = new Random(System.DateTime.Now.Millisecond);

            while (intCount < length)
            {
                intRandomNumber = random.Next(intZero, intZ);
                if (((intRandomNumber >= intZero) && (intRandomNumber <= intNine) || (intRandomNumber >= intA) && (intRandomNumber <= intZ)))
                {
                    strCaptchaString = strCaptchaString + (char)intRandomNumber;
                    intCount = intCount + 1;
                }
            }
            return strCaptchaString;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            Bitmap bmp = new Bitmap(100, 30);
            Graphics g = Graphics.FromImage(bmp);
            //g.Clear(Color.Navy);
            g.Clear(ColorTranslator.FromHtml("#3ea1cf"));
            string randomString = GetCaptchaString(6);
            context.HttpContext.Session["captchastring"] = randomString;
            SolidBrush brush = new SolidBrush(Color.WhiteSmoke);
            g.DrawString(randomString, new Font("Courier", 16), new SolidBrush(Color.WhiteSmoke), 2, 2);
            Random random = new Random(System.DateTime.Now.Millisecond);
            for (int i = 1; i <= 40; i++)
            {
                g.FillRectangle(brush, random.Next(4, 95), random.Next(2, 24), random.Next(1, 3), random.Next(1, 3));
            }
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "image/jpeg";
            bmp.Save(response.OutputStream, ImageFormat.Jpeg);
            bmp.Dispose();
        }
    }
}