using System;
using System.Diagnostics;
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
            set { 
                _mplabx_project_dir = value;
                _mplabx_include_dir_abs = ConvertFilePath.ToAbsoluteFilePath(_mplabx_include_dir, _mplabx_project_dir); 
                return; 
            }
        }

        // Project Netbeans Path (= .X/nbproject Directory)
        private string _mplabx_nbproject_dir = "";
        internal string NetBeansProjectDirectory
        {
            get { return (_mplabx_nbproject_dir); }
            set { _mplabx_nbproject_dir = value; return; }
        }

        // Project Netbeans Path (= .X/nbproject Directory)
        private string _mplabx_makefile_path = "";
        internal string MakefilePath
        {
            get { return (_mplabx_makefile_path); }
            set { _mplabx_makefile_path = value; return; }
        }

        // Project name (= .X Directory)
        private string _mplabx_project_name = "";
        internal string MPLABXProjectName
        {
            get { return (_mplabx_project_name); }
            set { _mplabx_project_name = value; return; }
        }

        private string _mplabx_include_dir = "";
        internal string MPLABXIncludeDirectory
        {
            get { return (_mplabx_include_dir); }
            set {
                string _str = "";
                _str = value.Trim();
                if ((_str.Length > 0) &&
                    (_str.Substring(0, 1 + _adsp.Length) != "." + _adsp) &&
                    (_str.Substring(0, 2 + _adsp.Length) != ".." + _adsp) && 
                    (!System.IO.Path.IsPathRooted(_str))
                    )
                    _str = "./" + _str;
                _mplabx_include_dir = _str;
                return; 
            }
        }

        private string _mplabx_include_dir_abs = "";
        internal string MPLABXIncludeDirectoryAbsolute
        {
            get {
                _mplabx_include_dir_abs = ConvertFilePath.ToAbsoluteFilePath(_mplabx_include_dir, _mplabx_project_dir);
                return (_mplabx_include_dir_abs); 
            }
//            set { _mplabx_include_dir_abs = value; return; }
        }

        private clsMPLABXConfiguration[] _mplabx_configuration;
        internal clsMPLABXConfiguration[] MPLABXConfiguration
        {
            get { return (_mplabx_configuration); }
            set { _mplabx_configuration = value; return; }
        }

        private int _active_configuration;
        internal int ActiveConfiguration
        {
            get { return (_active_configuration); }
            set { _active_configuration = value; return; }
        }

        private string _c_header_include = "";
        internal string CHeaderIncludePath
        {
            get { return (_c_header_include); }
            set { _c_header_include = value; return; }
        }


        private string _lib_header_include = "";
        internal string LibHeaderIncludePath
        {
            get { return (_lib_header_include); }
            set { _lib_header_include = value; return; }
        }
        

        private bool ParseMPLABXProjectFiles(string filename)
        {
            int _i = 0;
            string str_dum = "";
            string[] dum_sep = new string[1];
            XmlDocument mplabx_project_file = new XmlDocument();

            if ((filename.Substring(filename.Length - 1, 1)) != _dsp)
                filename += _dsp;

            if (!File.Exists(filename + "configurations.xml"))
                return (false);

            // Read the project's configurations project file
            mplabx_project_file.Load(filename + "configurations.xml");

            XmlNode node = mplabx_project_file.DocumentElement["confs"];
            if (node == null)
                return(false);

            _mplabx_configuration = new clsMPLABXConfiguration[node.ChildNodes.Count];
            _i = 0;

            foreach (XmlNode nd in node.ChildNodes)
            {
                _mplabx_configuration[_i] = new clsMPLABXConfiguration();
                _mplabx_configuration[_i].Name = nd.Attributes["name"].InnerText;

                foreach (XmlNode cnd in nd.ChildNodes)
                {
                    switch (cnd.Name)
                    {
                        case "C30Global":

                            foreach (XmlNode ccnd in cnd.ChildNodes)
                            {
                                if (ccnd.Attributes["key"].InnerText == "common-include-directories")
                                {
                                    str_dum = ccnd.Attributes["value"].InnerText;
                                    dum_sep[0] = ";";
                                    _mplabx_configuration[_i].CommonIncludeDirectories = str_dum.Split(dum_sep, StringSplitOptions.RemoveEmptyEntries);
                                    break;
                                }
                            }

                            break;

                        case "C30":

                            foreach (XmlNode ccnd in cnd.ChildNodes)
                            {
                                if (ccnd.Attributes["key"].InnerText == "extra-include-directories")
                                {
                                    str_dum = ccnd.Attributes["value"].InnerText;
                                    dum_sep[0] = ";";
                                    _mplabx_configuration[_i].ExtraIncludeDirectories = str_dum.Split(dum_sep, StringSplitOptions.RemoveEmptyEntries);
                                    break;
                                }
                            }

                            break;

                        case "C30-AS":

                            foreach (XmlNode ccnd in cnd.ChildNodes)
                            {
                                if (ccnd.Attributes["key"].InnerText == "extra-include-directories-for-assembler")
                                {
                                    str_dum = ccnd.Attributes["value"].InnerText;
                                    dum_sep[0] = ";";
                                    _mplabx_configuration[_i].ExtraIncludeDirectoriesAssembler = str_dum.Split(dum_sep, StringSplitOptions.RemoveEmptyEntries);
                                    break;
                                }
                            }

                            break;

                        case "toolsSet":

                            foreach (XmlNode ccnd in cnd.ChildNodes)
                            {
                                switch (ccnd.Name)
                                {
                                    case "targetDevice":
                                        _mplabx_configuration[_i].TargetDevice = ccnd.InnerText;
                                        break;
                                    case "languageToolchain":
                                        _mplabx_configuration[_i].LanguageToolchain = ccnd.InnerText;
                                        break;
                                    case "languageToolchainVersion":
                                        _mplabx_configuration[_i].LanguageToolchainVersion = ccnd.InnerText;
                                        break;
                                }
                            }

                            break;

                    }
                    
                }

                _i++; // Go to next node
                
            }

            // Read the project's active configuration from private configurations file
            if (!File.Exists(filename + "private\\configurations.xml"))
                return (false);
            mplabx_project_file.Load(filename + "private\\configurations.xml");
            node = mplabx_project_file.DocumentElement["defaultConf"];
            if (node == null)
                return (false);
            _active_configuration = Convert.ToInt32(node.InnerText);

            return (true);
        }

        internal bool SetMPLABXProject(string NewMPLABXProjectPath)
        {
            string str_path = "", str_dum = "";
            string[] dum_sep = new string[1];

            // Read new MPLAB X Project Directory 
            str_path = NewMPLABXProjectPath.Trim();

            // In case path is not the "nbproject/project.xml" path...
            if (!str_path.Contains("project.xml") && (str_path.Length > 7))
                if (str_path.Substring(str_path.Length-3, 3).Contains(".X"))    // Path is MPLAB X project folder
                {
                    if (str_path.Substring(str_path.Length - 1, 1) != "\\")
                        str_path += "\\";
                    str_path += "nbproject\\project.xml";
                }

            // Return FALSE if path is empty or file does not exist
            if ((str_path.Length == 0) || (!File.Exists(str_path)))
                return (false);

            // Extract Netbeans project directory 
            _mplabx_nbproject_dir = System.IO.Directory.GetParent(str_path).FullName;
            if ((_mplabx_nbproject_dir.Length > 3) && (_mplabx_nbproject_dir.Substring(_mplabx_nbproject_dir.Length - _dsp.Length, _dsp.Length) != _dsp))
                _mplabx_nbproject_dir += _dsp;

            // Extract the MPLAB X project directory
            str_dum = System.IO.Directory.GetParent(_mplabx_nbproject_dir).FullName;
            _mplabx_project_dir = System.IO.Directory.GetParent(str_dum).FullName;
            if ((_mplabx_project_dir.Length > 3) && (_mplabx_project_dir.Substring(_mplabx_project_dir.Length - _dsp.Length, _dsp.Length) != _dsp))
                _mplabx_project_dir += _dsp;

            // Extract/check MAKEFILE directory
            if (File.Exists(_mplabx_project_dir + "Makefile"))
                _mplabx_makefile_path = _mplabx_project_dir + "Makefile";

            // Extract the MPLAB X project directory name
            _mplabx_project_name = System.IO.Directory.GetParent(_mplabx_project_dir).Name;
            Debug.WriteLine(_mplabx_project_name);


            // Read XC16 C include directory from MPLAB X project files
            if (ParseMPLABXProjectFiles(_mplabx_nbproject_dir))
            {
                // foreach (string _str in _mplabx_configuration[0])
                if (_mplabx_configuration[_active_configuration].CommonIncludeDirectories.Length > 0) //[0] != null)
                    MPLABXIncludeDirectory = _mplabx_configuration[_active_configuration].CommonIncludeDirectories[0]; // This is the user-defined common include path
                else if (_mplabx_configuration[_active_configuration].ExtraIncludeDirectories.Length > 0) //[0] != null)
                    MPLABXIncludeDirectory = _mplabx_configuration[_active_configuration].ExtraIncludeDirectories[0]; // This is the user-defined C-include path
                else
                    MPLABXIncludeDirectory = System.IO.Directory.GetParent(_mplabx_makefile_path).FullName; // this is where the Makefile is located

                // Capture absolute path of MPLAB include path
                _mplabx_include_dir_abs = ConvertFilePath.ToAbsoluteFilePath(_mplabx_include_dir, _mplabx_project_dir);

                }

            return(true);

        }

    }

    class clsMPLABXConfiguration
    {
        private string _name = "";
        internal string Name
        {
            get { return(_name); }
            set { _name = value; return; }
        }

        private string _languageToolchain = "";
        internal string LanguageToolchain
        {
            get { return (_languageToolchain); }
            set { _languageToolchain = value; return; }
        }

        private string _languageToolchainVersion = "";
        internal string LanguageToolchainVersion
        {
            get { return (_languageToolchainVersion); }
            set { _languageToolchainVersion = value; return; }
        }

        private string _languageToolchainDir = "";
        internal string LanguageToolchainDir
        {
            get { return (_languageToolchainDir); }
            set { _languageToolchainDir = value; return; }
        }

        private string _targetDevice = "";
        internal string TargetDevice
        {
            get { return(_targetDevice); }
            set { _targetDevice = value; return; }
        }

        private string[] _commonIncludeDir;
        internal string[] CommonIncludeDirectories
        {
            get { return (_commonIncludeDir); }
            set { _commonIncludeDir = value; return; }
        }

        private string[] _extraIncludeDir;
        internal string[] ExtraIncludeDirectories
        {
            get { return (_extraIncludeDir); }
            set { _extraIncludeDir = value; return; }
        }

        private string[] _extraIncludeDirASM;
        internal string[] ExtraIncludeDirectoriesAssembler
        {
            get { return (_extraIncludeDirASM); }
            set { _extraIncludeDirASM = value; return; }
        }

        private string[] _extraIncludeDirASMCount;
        internal string[] ExtraIncludeDirectoriesAssemblerCount
        {
            get { return (_extraIncludeDirASMCount); }
            set { _extraIncludeDirASMCount = value; return; }
        }
    
    }

}
