using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Runtime.Serialization;
using System.Net;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json;



namespace GenGeoMonoDevelop
{
    class MainClass
    {
        //[DataContract]
        class GEO
        {
            public string url0;
            public string nameID;
            public string geonamedevice;
            public double X;
            public double Y;


            //GEO()
            //{
            //    X = 100;
            //    Y = 200;
            //    nameID = "test00";
            //    geonamedevice = "dev00";
            //}

        }

        public static void SendGEO(object n)
        {
            Random rnd = new Random();
            //GEO testgeo = new GEO();

            //string url = "https://localhost:44359/home/inputgeoJSON";
            //string url = "http://random-red.ddns.net:62424/home/inputgeoJSON";
            //string url = printURL();
            string jsonstr = printJSON();
            GEO testgeo = JsonConvert.DeserializeObject<GEO>(jsonstr);

            //Console.WriteLine("url : {0}", url);
            Console.WriteLine("url: {0}", testgeo.url0);
            Console.WriteLine("name: {0}", testgeo.nameID);
            Console.WriteLine("dev: {0}", testgeo.geonamedevice);




            // testgeo.nameID = "GeoGenTest";
            // testgeo.geonamedevice = "devGGT";

            int n1 = int.Parse(n.ToString()); //перобразовываем из n - object в n1 int 

            try
            {
                for (int i = 0; i < n1; i++)
                {

                    //testgeo.nameID = "GeoGenTest";
                    //testgeo.nameID = "GeoGenTest";
                    //testgeo.geonamedevice = "devGGT";
                    //testgeo.X=78.12;
                    testgeo.X = rnd.Next(99)/*+rnd.NextDouble()*/;
                    //testgeo.Y = 980.12;
                    testgeo.Y = rnd.Next(99)/*+rnd.NextDouble()*/;

                    string json = JsonConvert.SerializeObject(testgeo);
                    Console.WriteLine("JSON: {0}", json);

                    var httpRequest = (HttpWebRequest)WebRequest.Create(testgeo.url0);
                    httpRequest.Method = "POST";
                    httpRequest.ContentType = "application/json";
                    using (var requestStream = httpRequest.GetRequestStream())
                    using (var writer = new StreamWriter(requestStream))
                    {
                        writer.Write(json);
                    }
                    using (var httpResponse = httpRequest.GetResponse())
                    using (var responseStream = httpResponse.GetResponseStream())
                    using (var reader = new StreamReader(responseStream))
                    {
                        string response = reader.ReadToEnd();
                    }

                }
            }
            catch (WebException e)
            {
                Console.WriteLine(e); //показываем ошибку
                Console.WriteLine("");
            }
            Console.WriteLine("===>OK<===");
        }



        public static void Main(string[] args)
        {
            Console.WriteLine("Start HTTP request");

            //string url0 = "https://localhost:44359/home/inputgeoJSON";
            initURL();
            //GEO testgeo = new GEO();

            int t = 1;
            while (t == 1)
            {
                Console.Write("Количество отправлений ===> ");
                int n0 = int.Parse(Console.ReadLine());

                // SendGEO(url0,n0);

                Thread ThreadGenGC = new Thread(SendGEO);
                ThreadGenGC.Start(n0);
                Console.WriteLine("Поток ThreadGenGC запущен");
                ThreadGenGC.Join();
                Console.WriteLine("Повторить? (да - 1,нет - 0)");

                t = int.Parse(Console.ReadLine());
            }
            Console.WriteLine("Для закрытия нажмите любую Enter");
            Console.ReadLine();
        }

        private static void initURL()
        {
            GEO initgeo = new GEO();

            Console.Write("Адрес сервера: ");
            initgeo.url0 = Console.ReadLine();
            Console.Write("Пользователь: ");
            initgeo.nameID = Console.ReadLine();
            Console.Write("Устройство: ");
            initgeo.geonamedevice = Console.ReadLine();

            string initjson = JsonConvert.SerializeObject(initgeo);
            Console.WriteLine("initJSON: {0}", initjson);

            string path = @"settings.conf";
            using (StreamWriter conf = File.CreateText(path))
            {
                //conf.WriteLine("url0*" + url); 
                //conf.WriteLine("user0*" + user+"*dev0*"+dev);
                conf.WriteLine(initjson);
            }
        }

        private static string printURL()
        {
            string urlline = "";
            string path = @"settings.conf";
            string line = "";
            using (StreamReader conf = File.OpenText(path))
            {
                while (line != null)
                {
                    line = conf.ReadLine();
                    //Console.WriteLine(line);
                    if (line != null)
                    {
                        string[] ln = line.Split('*');
                        //string[] ln = line.Split(new char[] { '*' }); //разделяем прочитанныю строку на части, разделитель *
                        if (ln[0] == "url0")
                        {
                            urlline = ln[1];
                        }
                    }
                }
            }
            return urlline;
        }

        private static string printJSON()
        {
            string urlline = "";
            string path = @"settings.conf";
            string line = "";
            using (StreamReader conf = File.OpenText(path))
            {
                while (line != null)
                {
                    line = conf.ReadLine();
                    //Console.WriteLine(line);
                    if (line != null)
                    {
                        urlline = line;
                    }
                }
            }
            return urlline;
        }

    }
}
