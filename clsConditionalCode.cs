using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dcld
{
    // This defines specal tokens which are used by the applicaiton to enable/disable features
    // by including/skipping generation of taged code lines.
    // The Token Key is used to tag user controls on the main form to identify enable/disable states
    // of a particular option.
    // This class is used by the C-Code and Assembly Code Generator

    class clsConditionalCode
    {
        private ConditionalCodeToken[] _items;
        internal ConditionalCodeToken[] Items
        {
            get { return (_items); }
            set { _items = value; return; }
        }

        private int _count = 0;
        internal int Count
        {
            get { _count = _items.Length; return (_count); }
        }

        // ---------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------

        internal bool Add(string NewToken)
        {
            ConditionalCodeToken[] item_dummy = _items;
            string[] dum_sep = new string[1];
            string[] str_arr;

            // ID Parser
            int _id_start = 0, _id_stop = 0;
            string _str_id = "";

            try
            {
                // Split token string into ID and KEY
                dum_sep[0] = (";"); // Set ID/KEY Separator
                str_arr = NewToken.Split(dum_sep, StringSplitOptions.RemoveEmptyEntries); // Split Token

                // Extract ID
                if (str_arr[0].Contains("%{(") && str_arr[0].Contains(")}%"))
                {
                    _id_start = str_arr[0].IndexOf("%{(") + 3;
                    _id_stop = (str_arr[0].IndexOf(")}%") - _id_start);
                    _str_id = str_arr[0].Substring(_id_start, _id_stop);
                }
                else { _str_id = "0"; }

                // Resize Array adding one new item
                if (item_dummy == null)
                    Array.Resize(ref item_dummy, 1);
                else
                    Array.Resize(ref item_dummy, (item_dummy.Length + 1));

                // Set ID, Key and TOKEN
                item_dummy[item_dummy.Length - 1] = new ConditionalCodeToken();
                item_dummy[item_dummy.Length - 1].Token = NewToken.Trim();
                item_dummy[item_dummy.Length - 1].Id = Convert.ToInt32(_str_id);
                item_dummy[item_dummy.Length - 1].Key = str_arr[1].ToLower().Trim();

                // If everything went OK, copy dummy item array to Item array
                _items = item_dummy;

                return (true);
            }
            catch
            { return (false); }
        }

        // ---------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------

        internal bool Clear()
        {
            try{
                // Reset array to one item
                Array.Resize(ref _items, 0);
                return (true);
            }
            catch
            { return (false); }
        }

        // ---------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------

        internal int GetIdOfKey(string TokenKey)
        {
            int _token_index = 0;
            int _id = 0;
            string _search_token = TokenKey.ToLower().Trim();

            try
            {
                // Reset array to one item
                _token_index = Array.FindIndex(_items, _itm => _itm.Key == _search_token);
                if (_token_index >= 0)
                    _id = _items[_token_index].Id;
                return (_id);
            }
            catch
            { return (_id); }
        }

        // ---------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------

        internal string GetKeyOfId(int TokenID)
        {
            int _token_index = 0;
            string _key = "";
            int _search_token = TokenID;

            try
            {
                // Reset array to one item
                _token_index = Array.FindIndex(_items, _itm => _itm.Id == _search_token);
                if (_token_index >= 0)
                    _key = _items[_token_index].Key;
                return (_key);
            }
            catch
            { return (_key); }
        }

        // ---------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------

        internal int GetIndexOf(int TokenID)
        {
            int _token_index = 0;
            int _search_token = TokenID;

            try
            {
                // Reset array to one item
                _token_index = Array.FindIndex(_items, _itm => _itm.Id == _search_token);
                return (_token_index);
            }
            catch
            { return (0); }
        }

        // ---------------------------------------------------------------------------------------------
        // ---------------------------------------------------------------------------------------------

        internal bool Exists(int TokenID)
        {
            int _token_index = 0;
            int _search_token = TokenID;

            _token_index = Array.FindIndex(_items, _itm => _itm.Id == _search_token);
            return ((bool)(_token_index >= 0));
        }

        /* **********************************************************************************************
         *
         * This function parses all tokes in a code line and checks if the code line should be 
         * generated or not. Tokens can be declared in the following form:
         * 
         *      {(number)}  = code line should be generated if option = true 
         *      {(!number)} = code line should be generated if option = false
         *
         *    Multiple tokens can be set for more complex comparisons. In the following example a 
         *    code line is only generated when 
         *    
         *      option A = true and option B = true while option C = false:
         *
         *      %{(option_id_A)}%%{(option_id_B)}%%{(!option_id_C)}% 
         * 
         * Returns: 
         *
         *    TokenResult = true
         *       CodeLine retuns the unchanged code line without token string
         *       TokenString returns the unparsed token string in the form of %{(1234)}%
         *       
         *    TokenResult = false
         *       CodeLine retuns empty string
         *       TokenString returns the unparsed token string in the form of %{(1234)}%
         *       
         *    Error Message
         *       CodeLine retuns an error message, which cannot be compiled by the target device
         *       This error message contains the invalid token ID.
         *       TokenString returns the unparsed token string in the form of %{(1234)}%
         *       TokenResult is set to 'false'
         *    
         *    
         * *********************************************************************************************/

        internal CodeLineParserResult GetTokenResult(string CodeLine)
        {

            CodeLineParserResult _result = new CodeLineParserResult();

            // Check for Option Tokens
            if (CodeLine.Contains("%{(") && CodeLine.Contains(")}%"))
            {
                // Capture all tokens
                int _i = 0;
                bool _id_result = false;
                int _id = 0, _id_start = 0, _id_stop = 0;
                string _str_id = "", _str_code_line = "", _str_token = "";


                // Extract Token
                _str_code_line = CodeLine;  // capture code line incl. tokens

                string[] dum_sep = new string[] { ")}%" }; // Set token separator
                string[] str_arr = _str_code_line.Split(dum_sep, StringSplitOptions.None);

                for (_i = 0; _i < str_arr.Length; _i++)
                {
                    if (str_arr[_i].Trim().StartsWith("%{("))
                    {
                        // Trim sub-string
                        str_arr[_i] = str_arr[_i].Trim();
                        _str_token += str_arr[_i] + dum_sep[0];

                        // Exctact Token ID
                        _id_start = (str_arr[_i].IndexOf("%{(") + 3); // Set ID Start
                        _id_stop = (str_arr[_i].Length - _id_start); // Set ID Stop
                        _str_id = str_arr[_i].Substring(_id_start, _id_stop); // Exctract ID string
                        if (_str_id.Trim().Length == 0) _str_id = "-1"; // if Token is empty, set it as "invalid"
                        _id_result = (bool)(_str_id.Trim().Substring(0, 1) != "!"); // Capture condition and remove Exlamation Mark
                        if (!_id_result) _str_id = _str_id.Substring(1, (_str_id.Length - 1)); // Remove exclamation mark

                        _id = Convert.ToInt32(_str_id); // Read ID

                        if (Exists(_id)) // if ID is valid....
                        {
                            _result.TokenResult &= (_id_result == _items[GetIndexOf(_id)].Enabled);
                        }
                        else
                        {
                            _result.CodeLine = "[Conditional Token Error: Token %{(" + _str_id + ")}% is invalid] \r\n";
                            _result.TokenResult = false;
                            break;
                        }

                    }
                }

                // Complete result
                _result.TokenString = _str_token.Trim();
                if (_result.TokenResult)
                    _result.CodeLine = str_arr[(str_arr.Length - 1)];

            }

            // if code line does not contain any tokens, pass on code line with generation flag = 'true' 

            else
            {
                _result.CodeLine = CodeLine;    // pass on code line
                _result.TokenString = "";       // clear token string
                _result.TokenResult = true;     // set result = 'true' to generate code line
            }

            return (_result);

        }

        // This function reads all Token-IDs and Token Keys of conditional code blocks from the script
        internal bool GetTokenList(clsINIFileHandler GeneratorScript)
        {



            bool fres = false;
            int _i = 0, _token_count = 0;
            string text_line = "";

            try
            {
                _token_count = Convert.ToInt32(GeneratorScript.ReadKey("option_ids", "count", "0"));

                for (_i = 0; _i < _token_count; _i++)
                {
                    text_line = GeneratorScript.ReadKey("option_ids", (_i.ToString()), "");
                    fres = Add(text_line);
                    if (!fres) break;
                }

                return (fres);
            }
            catch
            { return(false); }

        }

    }


    public class CodeLineParserResult
    {

        private bool _token_result = true;
        internal bool TokenResult
        {
            get { return (_token_result); }
            set { _token_result = value; return; }
        }
        
        private string _token_string = "";
        internal string TokenString
        {
            get { return (_token_string); }
            set { _token_string = value; return; }
        }

        private string _code_line = "";
        internal string CodeLine
        {
            get { return (_code_line); }
            set { _code_line = value; return; }
        }

    }

    public class ConditionalCodeToken
    {
        private int _id = 0;
        internal int Id
        {
            get { return (_id); }
            set { _id = value; return; }
        }

        private string _token = "";
        internal string Token
        {
            get { return (_token); }
            set { _token = value; return; }
        }

        private string _key = "";
        internal string Key
        {
            get { return (_key); }
            set { _key = value; return; }
        }

        private bool _enabled = false;
        internal bool Enabled
        {
            get { return (_enabled); }
            set { _enabled = value; return; }
        }

    }

}
