using LumenWorks.Framework.IO.Csv;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using CsvHelper;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CsvReader = LumenWorks.Framework.IO.Csv.CsvReader;
using System.Diagnostics;

namespace ConsoleApp50
{
    class Program
    {

        static List<FileData> Filedatas = new List<FileData>();

        static void ReadCsvFile()
        {

            var csvTable = new DataTable();
            using (var csvReader = new CsvReader(new StreamReader(System.IO.File.OpenRead(@"C:\Users\itay.m\Downloads\SR-5543\MNN2_FAILS.csv")), true))
            {
                csvTable.Load(csvReader);
            }
            
            var csv = new StringBuilder();
            /*
            var firstHL = "WebSite";
            var secondHL = "IP";
            var thirdHL = "TotalOrderCost";
            var fourthHL = "ShippingCountry";
            var fifthHL = "IPLocationName";
            var sixthHL = "Num ShippingCountry <> Order IP Location";
            var Headines = string.Format("{0},{1},{2},{3},{4},{5}", firstHL, secondHL, thirdHL, fourthHL, fifthHL, sixthHL);

            csv.Append(string.Format("{0},{1},{2},{3},{4},{5}", firstHL, secondHL, thirdHL, fourthHL, fifthHL, sixthHL, Environment.NewLine));
            
            File.WriteAllText(@"C:\Users\itay.m\Downloads\SR-5543\KR_Get_IP_list_from_customers_10.14.2020_NEW.csv", csv.ToString());   */
            int count = 0;
            for (int i = 0; i < csvTable.Rows.Count; i++)
            {
                Filedatas.Add(new FileData { WebSite = csvTable.Rows[i][0].ToString(), IP = csvTable.Rows[i][1].ToString(), TotalOrderCost = csvTable.Rows[i][2].ToString(),
                    ShippingCountry = csvTable.Rows[i][3].ToString(), IPLocationName = csvTable.Rows[i][4].ToString() });
                if (i == count + 199)
                {
                    Console.WriteLine($"Counter:{i}");
                    count = count + 199;
                }

                try
                {
                    GetLocationIP(Filedatas[i]);
                    Console.WriteLine(Filedatas[i]);
                    var first = Filedatas[i].WebSite;
                    var second = Filedatas[i].IP;
                    var third = Filedatas[i].TotalOrderCost;
                    var fourth = Filedatas[i].ShippingCountry;
                    var fifth = Filedatas[i].IPLocationName;

                    var newLine = string.Format("{0},{1},{2},{3},{4}", first, second, third,fourth,fifth);
                    csv.Append(newLine + Environment.NewLine);
                }
                catch (Exception)
                {
                    var first = Filedatas[i].WebSite;
                    var second = Filedatas[i].IP;
                    var third = Filedatas[i].TotalOrderCost;
                    var fourth = Filedatas[i].ShippingCountry;
                    csv.Append(string.Format("{0},{1},{2},{3},fail{4}", first, second, third, fourth, Environment.NewLine));
                    continue;
                }

                File.WriteAllText(@"C:\Users\itay.m\Downloads\SR-5543\EXPORTS\MNN2_FAILS_NEW.csv", csv.ToString());

            }
            

        }


        


        static FileData GetLocationIP(FileData filedata)
        {
            string url = "https://freegeoip.app/json/";

            string read_str = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + filedata.IP);

            // read data
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                read_str = reader.ReadToEnd();
            }
            string ip = filedata.IP;
            string result = (url + ip);

            JObject o = JObject.Parse(read_str);
           // Console.WriteLine(o);
            if (o.GetValue("country_name") != null)
            {
                string country2 = o.GetValue("country_name").ToString();
                filedata.IPLocationName = country2;
            }
            else
            {
                filedata.IPLocationName = null;
            }
            //   Console.WriteLine(filedata);
            //    Console.WriteLine(country2);
            return filedata;
        }





        static void Main(string[] args)
        {
            ReadCsvFile();
            Console.WriteLine("Finished!");
            Console.ReadLine();
        }






    }




}
