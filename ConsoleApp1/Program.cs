using System;

using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using System.Net;
using System.Drawing.Imaging;
using System.Xml;
using System.Threading;

namespace Desktop_wallpaper
{
    
    class Program
    {

        [DllImport("User32", CharSet = CharSet.Auto)]
     
        public static extern int SystemParametersInfo(int uiAction, int uiParam, string pvParam, uint fWinIni);
     
        public static void SaveImage(string filename, ImageFormat format, string url)
        {
            WebClient client = new WebClient();
            System.Threading.Thread.Sleep(5000);
            string path = getPathForDownload(url);
            using (Stream stream = client.OpenRead(path))
            {
                
                Bitmap bitmap; bitmap = new Bitmap(stream);

                if (bitmap != null)
                {
                    bitmap.Save(filename, format);
                }
            }
            //stream.Flush();
            //stream.Close();
            //client.Dispose();
        }
     
        public static string getPathForDownload(string url)
        {
            string summary = "";

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(url);
            XmlElement root = xmlDocument.DocumentElement;
            foreach (XmlNode node in root.ChildNodes)
            {

                if (node.Name == "entry")
                {
                    foreach (XmlNode entry in node.ChildNodes)
                    {
                        if (entry.Name == "summary")
                        {
                            foreach (XmlNode toParse in entry)
                            {
                                summary = toParse.Value;

                            }

                        }
                    }

                }

            }

            string[] parse = summary.Split(new char[] { '=', '"' });
            string previousPath = parse[35];
            string[] almostDone = previousPath.Split('"');
            string path = almostDone[0];
            string path1 = "";
            if (path.Contains("300px"))
            {
                path1 = path.Replace("300px", "2000px");
            }

            return path1;
        }
       
        static void Main(string[] args)
        {
            try
            {
                const string url = "https://commons.wikimedia.org/w/api.php?action=featuredfeed&feed=potd&feedformat=atom&language=uk";
                string path = getPathForDownload(url);
                Thread.Sleep(100000);
                SaveImage($@"C:\Users\STEPAN\source\repos\ConsoleApp1\ConsoleApp1\bin\Debug\image3.jpeg", ImageFormat.Jpeg, url);
                SystemParametersInfo(0x0014, 0, $@"C:\Users\STEPAN\source\repos\ConsoleApp1\ConsoleApp1\bin\Debug\image3.jpeg", 0x0001);
                Console.WriteLine("Done");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();

        }
    }
}