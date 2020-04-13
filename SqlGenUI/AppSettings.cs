﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlGenUI
{
    public class AppSettings
    {
        
        [JsonIgnore]
        private static bool isInitialize = false;
        [JsonIgnore]
        private static AppSettings instance = null;
        [JsonIgnore]
        public static AppSettings Instance 
        { 
            get {
                if (instance == null)
                    instance = new AppSettings();

                return instance; 
            } 
        }
        [JsonIgnore]
        private const string appName = "SqlGenUI";
        [JsonIgnore]
        private const string settingFileName = "Settings.json";
        [JsonIgnore]
        private static string fileSettingsPath { get; set; }


        [JsonIgnore]
        public string ConnectionString
        {
            get
            {

                return string.Format($"Data Source={ServerName};Initial Catalog={DefaultDB};User Id={UserName};Password={Password};Application Name=SqlGen;");
            }
        }

        [JsonProperty("Path")]
        public string APIPath { get; set; }

        [JsonProperty("Server")]
        public string ServerName { get; set; }

        [JsonProperty("DB")]
        public string DefaultDB { get; set; }

        [JsonProperty("UserName")]
        public string UserName { get; set; }

        [JsonProperty("Password")]
        public string Password { get; set; }

      



        AppSettings()
        {
            if (!isInitialize)
                isInitialize = true;
            else
                return;

            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), appName);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            fileSettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), appName, settingFileName);

            if (File.Exists(fileSettingsPath))
            {
                try
                {
                    
                    using (StreamReader sr = new StreamReader(fileSettingsPath))
                    {
                        
                        var settings = JsonConvert.DeserializeObject<AppSettings>(sr.ReadToEnd());
                        APIPath = settings.APIPath;
                        DefaultDB = settings.DefaultDB;
                        Password = settings.Password;
                        UserName = settings.UserName;
                        ServerName = settings.ServerName;
                    }
                     
                }
                catch (Exception exc)
                {

                    throw new Exception("Serialization error occured", exc);
                }
            }
        }

        
        public void Save()
        {
            string output = JsonConvert.SerializeObject(instance);
            using (StreamWriter sw = new StreamWriter(fileSettingsPath))
            {
                sw.Write(output);
            }

        }
    }
}
