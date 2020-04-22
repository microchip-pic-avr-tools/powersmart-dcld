using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace dcld
{
    [System.Runtime.InteropServices.GuidAttribute("3952432A-6B37-4A47-8A68-A42D879C5A0D")]
    class clsINIFileHandler
    {
        // Settings file handling
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString", CallingConvention = CallingConvention.StdCall)]
        static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);
        [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileString", CallingConvention = CallingConvention.StdCall)]
        static extern int WritePrivateProfileString(string lpAppName, string lpKeyName, StringBuilder lpString, int nSize, string lpFileName);
        [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileString", CallingConvention = CallingConvention.StdCall)]
        static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);
        [DllImport("Kernel32.dll")]
        static extern bool Beep(Int32 dwFreq, Int32 dwDuration);


        private System.IO.FileInfo _fi;

        private string _default_directory = "";
        public string DefaultDirectory
        {
            get {
                string sdum = "", _dsp = "";

                _dsp = System.IO.Path.DirectorySeparatorChar.ToString();
                sdum = Application.StartupPath;
                if (sdum.Substring(sdum.Length - _dsp.Length, _dsp.Length) != _dsp)
                { sdum = sdum + _dsp; }
                _default_directory = sdum;
                return (_default_directory); 
            }
        }

        private string _directory = "";
        public string Directory
        {
            get { return (_directory); }
        }

        private string _directory_name = "";
        public string DirectoryName
        {
            get { return (_directory_name); }
        }

        private string _filename = "";
        public string FileName
        {
            get { return (_filename); }
        }

        private string _file_title = "";
        public string FileTitle
        {
            get { return (_file_title); }
        }

        private string _file_extension = "";
        public string FileExtension
        {
            get { return (_file_extension); }
        }

        private string _file_version = "";
        public string FileVersion
        {
            get { return (_file_version); }
        }

        private string _file_version_date = "";
        public string FileVersionDate
        {
            get { return (_file_version_date); }
        }

        // ========================================================================================
        // File operation handling functions
        // ========================================================================================

        public bool Create(string FileName, string InitialSection, string InitialKey, string InitialValue)
        {
            bool fres=false;
            fres = WritePrivateProfileString(InitialSection, InitialKey, InitialValue, FileName);
            if(fres)
                fres &= SetFilename(FileName);
            return (fres);
        }

        public bool SetFilename(string FileName)
        {
            bool b_res = false;

            if (System.IO.File.Exists(FileName))
            { 
                _fi = new System.IO.FileInfo(FileName);

                if (System.IO.Directory.Exists(_fi.Directory.ToString()))
                {
                    _directory = _fi.Directory.ToString();
                    _directory_name = _fi.DirectoryName;
                    _filename = _fi.FullName; // FileName.Replace("." + _dsp, Application.StartupPath + _dsp).Trim();
                    _file_title = _fi.Name; // GetFileTitle(FileName);
                    _file_extension = _fi.Extension; // GetFileExtension(FileName);

                    _file_version = ReadKey("generic", "Version", "n/a");
                    _file_version_date = ReadKey("generic", "Date", "n/a");

                    b_res = true;
                }
                else { b_res = false; }
            }
            
            if(!b_res)
            {
                _directory = "";
                _filename = "";
                _file_title = "";
                _file_extension = "";
            }

            return (b_res);
    
        }

        public bool Clear()
        {
            _default_directory = "";
            _directory = "";
            _directory_name = "";
            _filename = "";
            _file_title = "";
            _file_extension = "";
            return (true);
        }

        // ========================================================================================
        // INI file structure handling functions
        // ========================================================================================
        public string ReadKey(string Section, string Key, string DefaultValue)
        {
            int rc;
            StringBuilder sb = new StringBuilder(65536);
            rc = GetPrivateProfileString(Section, Key, DefaultValue, sb, 65535, _filename);
            if (rc > 0){
                sb.Replace("\\n", "\r\n");
                return (sb.ToString());
            }
            else
                return DefaultValue;
        }

        public bool WriteKey(string Section, string Key, string Value)
        {
            bool rc;
            rc = WritePrivateProfileString(Section, Key, Value, _filename);

            return rc;
        }

        public bool DeleteKey(string Section, string Key)
        {
            bool rc;
            rc = WritePrivateProfileString(Section, Key, null, _filename);

            return rc;
        }

        public bool KeyExists(string Section, string Key)
        {
            int rc;
            StringBuilder sb = new StringBuilder(65536);
            rc = GetPrivateProfileString(Section, Key, "", sb, 65535, _filename);
            if (rc > 0)
                return (true);
            else
                return (false);
        }


    }
}
