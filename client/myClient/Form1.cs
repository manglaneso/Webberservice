using myClient.service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Services;
using System.Windows.Forms;

namespace myClient
{
    public partial class Client : Form
    {
        public Client()
        {
            InitializeComponent();
        }

        //Adding action on click button
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //Create instance to work with the web service
                service.Service1 myClient = new service.Service1();

                //Get path of the SVG file to convert with OpenFileDialog
                string path = "";
                OpenFileDialog getfile = new OpenFileDialog();
                getfile.Title = "Open file..";
                getfile.Filter = "Images|*.svg";

                //If everything OK, save the path
                if (getfile.ShowDialog() == DialogResult.OK)
                {
                    path = getfile.FileName;
                }

                /*Open the file with the path obtained in the previous step 
                using a FileStream and convert it to an array of bytes with
                filestream.Read*/
                string filename = path;
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                byte[] ImageData = new byte[fs.Length];
                fs.Read(ImageData, 0, (int)fs.Length);
                fs.Close();

                /*Declare and initialize an array of bytes to store the 
                result of calling the web service*/
                byte[] result = new byte[100000];
                System.Threading.Thread.Sleep(5000);
                //Call the web service to convert the SVG file to PNG and transform it
                result = myClient.SVGtoPNGandTransform(ImageData);

                //Obtain a path to save the PNG file with SaveFileDialog
                string savepath = "";
                SaveFileDialog setfile = new SaveFileDialog();
                setfile.Title = "Save as..";
                setfile.Filter = "Images|*.png";

                //If everything OK, save the path, the name of the file and the extension of the file
                if (setfile.ShowDialog() == DialogResult.OK)
                {
                    string ext = System.IO.Path.GetExtension(setfile.FileName);
                    savepath = setfile.FileName;
                }

                //Open a file stream to save the converted and transformed id to the file system
                string savefileName = savepath;
                FileStream fileStream2 = new FileStream(savefileName, FileMode.Create);
                //Write it to the file system
                fileStream2.Write(result, 0, result.Length);
                //Print the result image in a pictureBox in the app
                pictureBox1.Image = System.Drawing.Image.FromStream(fileStream2);

                fileStream2.Close();

                //If everthing OK, show a message on the richTextBox in the app
                richTextBox1.AppendText("Well done!");

            }
            catch (Exception E)
            {
                //If any error, print the exception on the richTextBox
                richTextBox1.AppendText("error : \n" + E.ToString());
            }
        
        }

    }
}
