using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace dcld
{
    class clsMPLABXHandler
    {
        // Project Path (= .X Directory)
        private string _project_dir = "";
        internal string ProjectDirectory
        {
            get { return (_project_dir); }
            set { _project_dir = value; return; }
        }

        //private string _project_path = "";
        //internal string ProjectPath
        //{
        //    get { return (_project_path); }
        //    set { _project_path = value; return; }
        //}
    
    }
}
