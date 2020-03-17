using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace dcld
{
    class clsMPLABXHandler
    {
        // Additional classes
        clsFilePathHandler ConvertFilePath = new clsFilePathHandler();

        // Buffer variables 
        private string _dsp = System.IO.Path.DirectorySeparatorChar.ToString();
        private string _adsp = System.IO.Path.AltDirectorySeparatorChar.ToString();

        // Project Path (= .X Directory)
        private string _mplabx_project_dir = "";
        internal string MPLABXProjectDirectory
        {
            get { return (_mplabx_project_dir); }
            set { _mplabx_project_dir = value; return; }
        }

        private string _asm_file ;
        internal string AssemblerSourceFile
        {
            get { return (_asm_file); }
            set { _asm_file = value; return; }
        }

        private string _c_source_file = "";
        internal string CSourceFile
        {
            get { return (_c_source_file); }
            set { _c_source_file = value; return; }
        }

        private string _c_header_file = "";
        internal string CHeaderFile
        {
            get { return (_c_header_file); }
            set { _c_header_file = value; return; }
        }

        private string _lib_header_file = "";
        internal string LibHeaderFile
        {
            get { return (_lib_header_file); }
            set { _lib_header_file = value; return; }
        }
        
        //private string _project_path = "";
        //internal string ProjectPath
        //{
        //    get { return (_project_path); }
        //    set { _project_path = value; return; }
        //}

        private bool ParseMPLABXProjectFile(string filename)
        {
            XmlDocument mplabx_project_file = new XmlDocument();
            string str_dum = "";

            if ((filename.Substring(filename.Length - 1, 1)) != _dsp)
            {
                filename += _dsp + "configurations.xml";
                if (File.Exists(filename))
                {
                    mplabx_project_file.Load(filename);
                    XmlTextReader reader = new XmlTextReader(filename);
                    XmlNode node = mplabx_project_file.DocumentElement["sourceRootList"];
                    str_dum = node.InnerXml;
                    Console.WriteLine(node.InnerXml);
                }
            }

            return (true);
        }



        internal bool SetMPLABXProjectDirectory(string NewMPLABXProjectPath, string DefaultPath)
        {
            string str_path = "", ref_path = "";
            string[] dum_sep = new string[1];

            // Read new MPLAB X Project Directory 
            str_path = NewMPLABXProjectPath.Trim();
            if ((str_path.Length > 3) && (str_path.Substring(str_path.Length - _dsp.Length, _dsp.Length) != _dsp)) str_path += _dsp;

            // Set MPLAB X Project Directory Path Reference (always absolute)
            _mplabx_project_dir = str_path;

            // Buffer file paths as absolute paths
            if (_mplabx_project_dir.Length > 0)
            { ref_path = _mplabx_project_dir; }
            else
            { ref_path = DefaultPath; }


            return(true);

        }

    }
}
