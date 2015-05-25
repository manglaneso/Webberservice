using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Services;

namespace service
{
    /// <summary>
    /// Descripción breve de webberservice
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class Service1 : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Holo chabules";
        }

        /*Web method that receives a SVG file as an array of bytes and returns
        a rotated 270º PNG file as an array of bytes*/
        [WebMethod]
        public byte[] SVGtoPNGandTransform(byte[] image)
        {
            //This method also works as a client of the external webservice
            try
            {
                int errorCode = 0;
                string error = "";
                byte[] dataResult = new byte[100000];

                //Create instance to work with the web service
                uniservice.WebServiceRO client = new uniservice.WebServiceRO();

                /*Call the web service to convert the SVG to PNG obtaining a secret key string
                to check if the conversion is done yet*/
                string secret_key = client.startThread_SVGtoPNG(image, ref errorCode, ref error);

                if (errorCode == 0)
                {
                    int response;
                    //If the conversion does not return an error
                    do
                    {
                        //Check if the conversion is completed yet
                        System.Threading.Thread.Sleep(5000);
                        response = client.isCompleteThread(secret_key, ref errorCode, ref error);
                    } while (response == 0);

                    //Read the result and store it in an array of bytes
                    dataResult = new byte[100000];
                    dataResult = client.readResult(secret_key, ref errorCode, ref error);

                    //Convert the array of bytes to a bitmap in order to make the rotation
                    Bitmap bmp;
                    using (var ms = new MemoryStream(dataResult))
                    {
                        bmp = new Bitmap(ms);
                    }

                    //Rotate the image 270º
                    bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);

                    //Reconvert it to an array of bytes
                    ImageConverter converter = new ImageConverter();
                    dataResult = (byte[])converter.ConvertTo(bmp, typeof(byte[]));
                }
                //Return the converted image
                return dataResult;
            }
            catch (Exception E)
            {
                //If any exception found, convert it to an array of bytes and return it
                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream ms;
                using (ms = new MemoryStream())
                {
                    bf.Serialize(ms, E);
                }
                return ms.ToArray();
            }

        }

    }
}