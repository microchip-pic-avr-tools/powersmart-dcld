using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace dcld
{
    class clsFilePathHandler
    {

        // Buffer variables 
        private string _dsp = System.IO.Path.DirectorySeparatorChar.ToString();
        private string _adsp = System.IO.Path.AltDirectorySeparatorChar.ToString();


        /* ***************************************************************************************** 
         * Builds the absolute path of the MPLAB X project directory the current DCLD project
         * is associated with.
         * ***************************************************************************************** */
        internal string ToAbsoluteFilePath(string RelativeFilePath, string ReferencePath)
        {
            int i = 0, up_steps = 0;
            string str_dum = "", source_path = "", reference_path = "";
            string[] str_arr_anchor_path;
            string[] str_arr_relative_path;
            string[] dum_sep = new string[1];
            bool IsFile = false;

            System.IO.FileInfo _fi_src = new System.IO.FileInfo(RelativeFilePath.Trim());
            System.IO.FileInfo _fi_ref = new System.IO.FileInfo(ReferencePath.Trim());

            // ==============================

            // Check if path points to file or directory
            IsFile = (bool)(System.IO.Directory.Exists(RelativeFilePath) && System.IO.File.Exists(RelativeFilePath));

            // Check if given parameter RelativeFilePath is really a relative path
            RelativeFilePath = RelativeFilePath.Trim();
            ReferencePath = ReferencePath.Trim();

            if (RelativeFilePath.Length == 0) return (RelativeFilePath); // Path is empty
            if (ReferencePath.Length == 0) ReferencePath = Application.StartupPath; // Path is empty
            if ((RelativeFilePath.Substring(0, 1 + _adsp.Length) != "." + _adsp) &&
                (RelativeFilePath.Substring(0, 1 + _dsp.Length) != "." + _dsp))
                if ((RelativeFilePath.Substring(0, 2 + _adsp.Length) != ".." + _adsp) &&
                    (RelativeFilePath.Substring(0, 2 + _dsp.Length) != ".." + _dsp))
                    return (RelativeFilePath); // File path is not a relative path
            if ((ReferencePath.Substring(0, 1 + _adsp.Length) == "." + _adsp) ||
                (ReferencePath.Substring(0, 1 + _dsp.Length) == "." + _dsp))
                if ((ReferencePath.Substring(0, 2 + _adsp.Length) == ".." + _adsp) ||
                    (ReferencePath.Substring(0, 2 + _dsp.Length) == ".." + _dsp))
                    return (RelativeFilePath); // File path is not an absolute path

            // ==============================

            // Format paths
            source_path = RelativeFilePath; // Transform relative path into absolute
            source_path = source_path.Replace(_dsp, _adsp); // Make sure the standard path separator character is used
            dum_sep[0] = (_adsp); // Set Path Separator
            str_arr_relative_path = source_path.Split(dum_sep, StringSplitOptions.RemoveEmptyEntries); // Split Path

            reference_path = _fi_ref.FullName; // Transform relative path into absolute
            reference_path = reference_path.Replace(_adsp, _dsp); // Make sure the standard path separator character is used
            dum_sep[0] = (_dsp); // Set Path Separator
            str_arr_anchor_path = reference_path.Split(dum_sep, StringSplitOptions.RemoveEmptyEntries); // Split Path

            // Capture element counter
            up_steps = 0;

            for (i = str_arr_relative_path.Length; i > 0; i--)
            {
                if (str_arr_relative_path[i - 1] == ".")
                { break; }
                else if (str_arr_relative_path[i - 1] == "..")
                { up_steps++; }
            }

            // Build absolute path
            for (i = 0; i < (str_arr_anchor_path.Length - up_steps); i++)
            {
                str_dum += str_arr_anchor_path[i] + _dsp;
            }

            // Add relative path
            for (i = 0; i < str_arr_relative_path.Length; i++)
            {
                if ((str_arr_relative_path[i] != ".") && (str_arr_relative_path[i] != ".."))
                {
                    // If path points to a file, remove the last backslash
                    if ((i == (str_arr_relative_path.Length - 1)) && IsFile)
                        str_dum += str_arr_relative_path[i];
                    else
                        str_dum += str_arr_relative_path[i] + _dsp;
                }

            }

            // Return result
            return (str_dum.Trim());

        }

        internal string ToRelativeFilePath(string AbsoluteFilePath, string ReferencePath)
        {
            int i = 0, up_steps = 0, fork = 0;
            bool IsFile = false;
            string str_path = "";
            string source_path = "", reference_path = "";
            string[] str_arr_anchor_path;
            string[] str_arr_absolute_path;
            string[] dum_sep = new string[1];

            System.IO.FileInfo _fi_src = new System.IO.FileInfo(AbsoluteFilePath.Trim());
            System.IO.FileInfo _fi_ref = new System.IO.FileInfo(ReferencePath.Trim());

            try
            {

                // ==============================

                // Check if path points to file or directory
                IsFile = (bool)(System.IO.Directory.Exists(AbsoluteFilePath) && System.IO.File.Exists(AbsoluteFilePath));

                // Check if given parameter RelativeFilePath is really a relative path
                AbsoluteFilePath = AbsoluteFilePath.Trim();
                ReferencePath = ReferencePath.Trim();

                if (AbsoluteFilePath.Length == 0) return (AbsoluteFilePath); // Path is empty
                if (ReferencePath.Length == 0) ReferencePath = Application.StartupPath; // Path is empty
                if ((AbsoluteFilePath.Substring(0, 1 + _adsp.Length) == "." + _adsp) ||
                    (AbsoluteFilePath.Substring(0, 1 + _dsp.Length) == "." + _dsp))
                    if ((AbsoluteFilePath.Substring(0, 2 + _adsp.Length) != ".." + _adsp) ||
                        (AbsoluteFilePath.Substring(0, 2 + _dsp.Length) != ".." + _dsp))
                        return (AbsoluteFilePath); // File path is not an absolute path
                if ((ReferencePath.Substring(0, 1 + _adsp.Length) == "." + _adsp) ||
                    (ReferencePath.Substring(0, 1 + _dsp.Length) == "." + _dsp))
                    if ((ReferencePath.Substring(0, 2 + _adsp.Length) == ".." + _adsp) ||
                        (ReferencePath.Substring(0, 2 + _dsp.Length) == ".." + _dsp))
                        return (AbsoluteFilePath); // File path is not an absolute path

                // ==============================

                // Format paths
                source_path = _fi_src.FullName; // Transform relative path into absolute
                source_path = source_path.Replace(_dsp, _adsp); // Make sure the standard path separator character is used
                dum_sep[0] = (_adsp); // Set Path Separator
                str_arr_absolute_path = source_path.Split(dum_sep, StringSplitOptions.RemoveEmptyEntries); // Split Path

                reference_path = _fi_ref.FullName; // Transform relative path into absolute
                reference_path = reference_path.Replace(_adsp, _dsp); // Make sure the standard path separator character is used
                dum_sep[0] = (_dsp); // Set Path Separator
                str_arr_anchor_path = reference_path.Split(dum_sep, StringSplitOptions.RemoveEmptyEntries); // Split Path


                // Build relative path
                for (i = 0; i < str_arr_anchor_path.Length; i++)
                {
                    if (i == str_arr_absolute_path.Length)
                        break;
                    if (str_arr_absolute_path[i] != str_arr_anchor_path[i])
                        up_steps++;
                }
                fork = (str_arr_anchor_path.Length - up_steps);
                for (i = fork; i < str_arr_anchor_path.Length; i++)
                {
                    str_path += ".." + _adsp;
                }
                for (i = fork; i < str_arr_absolute_path.Length; i++)
                {
                    // If path points to a file, remove the last backslash
                    if ((i == (str_arr_absolute_path.Length - 1)) && IsFile)
                        str_path += str_arr_absolute_path[i];
                    else
                        str_path += str_arr_absolute_path[i] + _adsp;
                }

                // Check if root needs to be added
                if (str_path.Length > 1)
                    if (str_path.Substring(0, 2) != "..")
                        str_path = "." + _adsp + str_path;
                if (str_path.Length == 0)
                    str_path = "." + _adsp;

                return (str_path.Trim());

            }
            catch
            {
                return (AbsoluteFilePath);
            }

        }

        internal string Win2Unix(string FilePath)
        {
            string f_path = "";

            f_path = FilePath.Trim();

            if (f_path.Length > 0)
            {
                while (f_path.Contains(_dsp + _dsp))
                { f_path = f_path.Replace(_dsp + _dsp, _dsp); }

                f_path = f_path.Replace(_dsp, _adsp);

                while (f_path.Contains(_adsp + _adsp))
                { f_path = f_path.Replace(_adsp + _adsp, _adsp); }

            }


            return (f_path);
        }

        internal string Unix2Win(string FilePath)
        {
            string f_path = "";

            f_path = FilePath.Trim();

            if (f_path.Length > 0)
            {
                while (f_path.Contains(_adsp + _adsp))
                { f_path = f_path.Replace(_adsp + _adsp, _adsp); }

                f_path = f_path.Replace(_adsp, _dsp);

                while (f_path.Contains(_dsp + _dsp))
                { f_path = f_path.Replace(_dsp + _dsp, _dsp); }

            }

            return (f_path);
        }    
    
    }
}
