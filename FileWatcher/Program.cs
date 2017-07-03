using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace FileWatcher
{
    class Program
    {
        static SecurityEntities db = new SecurityEntities();
        static void Main(string[] args)
        {
            readXML();
        }

        static void readXML()
        { 

           
            string obj = ((from item in db.tests
                      select item.data).FirstOrDefault()) ;



            XDocument xdoc = XDocument.Parse(obj);



            var test = xdoc.Descendants().Where(d => d.Name == "book"
                               && d.Descendants().Any(e => e.Name == "genre"));
                               //  && e.Value == "Fantasy"));

            foreach (var item in test)
            {
                Console.WriteLine(item);
            }


            Console.WriteLine(test);
            Console.Read();
        }

        void readDirectory()
        {
            try
            {
                DirectoryInfo d = new DirectoryInfo(@"C:\test");//Assuming Test is your Folder

                while (true)
                {
                    var Files = d.GetFiles("*.*").ToList();
                    foreach (var file in Files)
                    {
                        //Move to some other location and delete from original location
                        file.CopyTo(@"C:\test1\" + file.Name, true);
                        file.Delete();

                        //Process


                        // complete the operation complete
                    }
                    Console.WriteLine("Sleeping 10s...");
                    Thread.Sleep(10000);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
                Console.ReadLine();

            }
        }
    }
}
