using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;

namespace FileService
{
    public partial class Service1 : ServiceBase
    {
        string path = ConfigurationManager.AppSettings["WatchPath"]; //"c:\test";
        string workInProgress = ConfigurationManager.AppSettings["WorkInProgress"]; //"c:\test";
        public Service1()
        {
            InitializeComponent();
            MyFileWatcher.Created += MyFileWatcher_Created;
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                MyFileWatcher.Path = path;
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        protected override void OnStop()
        {
        }

        protected override void OnContinue()
        {
          
            //base.OnContinue();
        }

        private void MyFileWatcher_Created(object sender, System.IO.FileSystemEventArgs e)
        {
            try
            {
                Thread.Sleep(10000);
                if (CheckFileExistance(path, e.Name))
                {
                    File.Copy(Path.Combine(path, e.Name) , workInProgress + e.Name, true);
                    File.Delete(Path.Combine(path, e.Name));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool CheckFileExistance(string FullPath, string FileName)
        {
            // Get the subdirectories for the specified directory.'  
            bool IsFileExist = false;
            DirectoryInfo dir = new DirectoryInfo(FullPath);
            if (!dir.Exists)
                IsFileExist = false;
            else
            {
                string FileFullPath = Path.Combine(FullPath, FileName);
                if (File.Exists(FileFullPath))
                    IsFileExist = true;
            }
            return IsFileExist;


        }

       

        private void CreateTextFile(string FullPath, string FileName)
        {
            StreamWriter SW;
            if (!File.Exists(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "txtStatus_" + DateTime.Now.ToString("yyyyMMdd") + ".txt")))
            {
                SW = File.CreateText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "txtStatus_" + DateTime.Now.ToString("yyyyMMdd") + ".txt"));
                SW.Close();
            }
            using (SW = File.AppendText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "txtStatus_" + DateTime.Now.ToString("yyyyMMdd") + ".txt")))
            {
                SW.WriteLine("File Created with Name: " + FileName + " at this location: " + FullPath);
                SW.Close();
            }
        }
    }
}
