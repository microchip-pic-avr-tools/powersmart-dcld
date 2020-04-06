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
         * Token Groups:
         * 
         *    Multiple tokens can be combined for more complex comparisons. In the following 
         *    example a code line is generated when 
         *    
         *      option A = true and option B = true while option C = false:
         *
         *      %{(option_id_A) && (option_id_B) && (!option_id_C)}% 
         * 
         *    When multiple %...%-Tokens are used their contents will always be AND-ed. The following 
         *    example generates a code line, if option A = true and EITHER option B OR option C = true
         *      
         *      %{(option_id_A)}% %{(option_id_B) || (option_id_C)}%
         * 
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
                int _i = 0, _k = 0;
                int _id_start = 0, _id_stop = 0;
                string _str_id = "", _str_code_line = "", _str_token = "";

                // Extract Token
                _str_code_line = CodeLine;  // capture code line incl. tokens

                string[] _dum_sep = new string[] { ")}%" }; // Set token separator
                string[] _str_arr = _str_code_line.Split(_dum_sep, StringSplitOptions.None);
                string[] _arr_token_string;
                CCTokenString[] _arr_token;

                for (_i = 0; _i < _str_arr.Length; _i++)
                {
                    // If code line contains a valid TOKEN_START stirng...
                    if (_str_arr[_i].Trim().StartsWith("%{("))
                    {
                        // Trim sub-string
                        _str_arr[_i] = _str_arr[_i].Trim(); // Set token separator ")}%"
                        _str_arr[_i] = _str_arr[_i].Replace(" ", ""); // remove all inner spaces fro more robust parsing
                        _str_token += _str_arr[_i] + _dum_sep[0]; // add the token ending back to token string

                        // Exctact Token ID string (can contain multiple IDs)
                        _id_start = (_str_arr[_i].IndexOf("%{(") + 3); // Set ID Start
                        _id_stop = (_str_arr[_i].Length - _id_start); // Set ID Stop
                        _str_id = _str_arr[_i].Substring(_id_start, _id_stop); // Exctract ID string

                        if (_str_id.Trim().Length == 0)
                        { // if Token is empty, set it as "invalid"
                            _str_id = "-1"; 
                        }
                        else
                        { // Slpit all items in brackets

                            _dum_sep = new string[] { ")" };
                            _arr_token_string = _str_id.Split(_dum_sep, StringSplitOptions.None);
                            _arr_token = new CCTokenString[_arr_token_string.Length];

                            for (_k = 0; _k < _arr_token_string.Length; _k++)
                            {
                                _arr_token[_k] = new CCTokenString();   // Ceate new conditional token object
                                _arr_token_string[_k] = _arr_token_string[_k].Replace(" ", ""); // remove all spaces for robust parsing 
                                //_arr_token_string[_k] += ")"; // Add closing bracket back in

                                if (_arr_token_string[_k].Substring(0, 3) == "&&(")  // AND condition
                                {
                                    _id_start = (_arr_token_string[_k].IndexOf("&&(") + 3);
                                    _id_stop = _arr_token_string[_k].Length - _id_start;
                                    _arr_token[_k].IdString = _arr_token_string[_k].Substring(_id_start, _id_stop).Trim();
                                    _arr_token[_k].CombinatorialConstructor = "&";
                                }
                                else if (_arr_token_string[_k].Substring(0, 3) == "||(")  // OR condition
                                {
                                    _id_start = (_arr_token_string[_k].IndexOf("||(") + 3);
                                    _id_stop = _arr_token_string[_k].Length - _id_start;
                                    _arr_token[_k].IdString = _arr_token_string[_k].Substring(_id_start, _id_stop).Trim();
                                    _arr_token[_k].CombinatorialConstructor = "|";
                                }
                                else // Single ID Token
                                {
                                    _arr_token[_k].IdString = _arr_token_string[_k]; // _str_id;
                                    _arr_token[_k].CombinatorialConstructor = "";
                                }

                                // Check if item is negated (test on NOT xxx)
                                _arr_token[_k].Negate = (bool)(_arr_token[_k].IdString.Substring(0, 1) != "!"); // Capture condition and remove Exlamation Mark
                                if (!_arr_token[_k].Negate) _arr_token[_k].IdString = _arr_token[_k].IdString.Substring(1, (_arr_token[_k].IdString.Length - 1)); // Remove exclamation mark

                                // Capture ID
                                _arr_token[_k].Id = Convert.ToInt32(_arr_token[_k].IdString);

                                // if parsed ID is valid....
                                if (Exists(_arr_token[_k].Id)) 
                                {
                                    switch (_arr_token[_k].CombinatorialConstructor)
                                    {
                                        case "|":
                                            _result.TokenResult |= (_arr_token[_k].Negate == _items[GetIndexOf(_arr_token[_k].Id)].Enabled);
                                            break;
                                        default:
                                            _result.TokenResult &= (_arr_token[_k].Negate == _items[GetIndexOf(_arr_token[_k].Id)].Enabled);
                                            break;
                                    }
                                    
                                }
                                else
                                { // If ID is unknown, return error message instead of code line
                                    _result.CodeLine = "[Conditional Token Error: Token %{(" + _str_id + ")}% is invalid] \r\n";
                                    _result.TokenResult = false;
                                    break;
                                }


                            }
                        
                        }

                    }

                }

                // Complete result
                _result.TokenString = _str_token.Trim();
                if (_result.TokenResult)
                    _result.CodeLine = _str_arr[(_str_arr.Length - 1)];

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

    internal class CCTokenString
    {
        private string _str_id = "";
        internal string IdString
        {
            get { return (_str_id); }
            set { _str_id = value; return; }
        }

        private int _id = 0;
        internal int Id
        {
            get { return (_id); }
            set { _id = value; return; }
        }

        private bool _neg = true;
        internal bool Negate
        {
            get { return(_neg); }
            set { _neg = value; return; }
        }

        private string _comb = "&";
        internal string CombinatorialConstructor
        {
            get { return (_comb); }
            set { _comb = value; return; }
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
