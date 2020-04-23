using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace dcld
{
    class clsRecentFileList
    {
        internal int Count 
        {
            get {
                if (_items != null)
                    return (_items.Length);
                else
                    return (0);
            }
        }

        private clsINIFileHandler _settings_file;
        internal clsINIFileHandler SettingsFile
        {
            get { return (_settings_file); }
            set { _settings_file = value; GetFileList(); return; }
        }

        private int _max_items_absolute = 128;
        private int _max_items_default = 8;
        private int _max_items = 8;
        internal int MaximumItems
        {
            get { return (_max_items); }
            set { _max_items = value; return; }
        }

        private clsRecentFileListItem[] _items;
        internal clsRecentFileListItem[] Items
        {
            get { return (_items); }
            set { _items = value; return; }
        }

        private void WriteParentFileList()
        {
            int _i = 0, _max = 0;

            // Limit number of list items to user-defined maximum
            if (_items.Length > _max_items)
                _max = _max_items;
            else
                _max = _items.Length;

            // Write updated recent file list to file
            for (_i = 0; _i < _max; _i++)
            {
                _settings_file.WriteKey("recent_files", "file" + _i.ToString(), _items[_i].Path);
            }
            _settings_file.WriteKey("recent_files", "count", _i.ToString());

            return;
        }

        private void ClearParentFileList()
        {
            int _i = 0;

            // Clear recent file list in file up to 128 entries in case maximum level has been changed
            // and old entries are still there....
            for (_i = 0; _i < _max_items_absolute; _i++)
            {
                if (_settings_file.ReadKey("recent_files", "file" + _i.ToString(), "").Length > 0)
                    _settings_file.DeleteKey("recent_files", "file" + _i.ToString());
            }
            _settings_file.WriteKey("recent_files", "count", "0");

            return;
        }


        internal void Clear()
        {
            if (_settings_file == null)
                return;

            ClearParentFileList();

            if (_items != null)
            {
                Array.Clear(_items, 0, _items.Length);
                Array.Resize(ref _items, 0);
            }

            return;
        }

        internal bool GetFileList()
        {
            int _i = 0, file_count = 0;

            if(_settings_file == null)
                return(false);

            // read number of saved files
            file_count = Convert.ToInt32(_settings_file.ReadKey("recent_files", "count", "0"));
            if (file_count == 0)
                return(true);

            // read maximum length of file list
            _max_items = Convert.ToInt32(_settings_file.ReadKey("recent_files", "max", _max_items_default.ToString()));
            if (_max_items == 0)
                _max_items = _max_items_default;

            // clamp file count to maximum
            if (file_count > _max_items)
                file_count = _max_items;

            // Crate items array
            _items = new clsRecentFileListItem[file_count];

            // Read file information of file list
            for (_i = 0; _i < _items.Length; _i++)
            {
                // Read full string from file
                _items[_i] = new clsRecentFileListItem();
                _items[_i].Path = _settings_file.ReadKey("recent_files", "file" + _i.ToString(), "").Trim();

            }

            return(true);
        }

        internal void AddNew(string Filename)
        {
            string str_dum = Filename.Trim();
            clsRecentFileListItem[] _item_list;
            clsRecentFileListItem new_item;

            // Exit if filename is empty
            if (str_dum.Length == 0)
                return;

            // Exit if file does not exist
            if (!File.Exists(str_dum))
                return;

            // Create new item
            new_item = new clsRecentFileListItem();
            new_item.Path = str_dum;
            
            // Extend items array by one element
            _item_list = _items;
            Array.Resize(ref _item_list, (_item_list.Length + 1));
            Array.Copy(_items, 0, _item_list, 1, _items.Length);
            _item_list[0] = new_item;

            // Copy new list back into list
            _items = _item_list;
            _item_list = null;

            Trim(); // Trim new list

            // Limit number of items to user-defined maximum
            if (_items.Length > _max_items)
                Array.Resize(ref _items, _max_items);

            // Clear file list in file
            ClearParentFileList();

            // Write updated list to settings file
            WriteParentFileList();

            return;
        }

        internal void Remove(string Filename)
        {
            int _i = 0;

            // Set selected item to NULL
            for (_i = 0; _i < _items.Length; _i++)
            {
                if (_items[_i] != null)
                {
                    if (_items[_i].Path.Trim().ToLower() == Filename.Trim().ToLower())
                    {
                        _items[_i] = null;
                        break;
                    }
                }
            }

            // Clean file list and return result
            Trim();                 // Trim file list
            WriteParentFileList();  // Write cleaned-up list to file
            GetFileList();          // reload file list from file

            return;
        }

        internal void Trim()
        {
            int _i = 0, _j = 0, list_index = 0;
            clsRecentFileListItem itm;

            // Check for duplicates and remove them
            for (_i = 0; _i < _items.Length; _i++)
            {
                itm = _items[_i]; // capture list item

                for (_j = 0; _j < _items.Length; _j++) // search for duplicate
                {
                    if ((itm != null) && (_items[_j] != null)) // Only test non-null items
                    {
                        if ((_i != _j) && (itm.Path.Trim().ToLower() == _items[_j].Path.Trim().ToLower()))  // If duplicate has been found
                            _items[_j] = null;                  // delete duplicate
                    }
                }

            }

            // Reorder file list (reorder valid list items to lowest indices and push gaps to the end)
            for (_i = 0; _i < _items.Length; _i++)
            {
                if (_items[_i] != null)
                {
                    list_index = -1;    // Clear list index
                    for (_j = _i; _j > 0; _j--) // find lowest, empty index
                    {
                        if (_items[_j] == null)
                        { list_index = _j; }
                    }

                    if (list_index > -1) // If a lower, empty list index has been found...
                    {
                        _items[list_index] = _items[_i]; // .. copy Items into lower index
                        _items[_i] = null;               // delete list item
                    }

                }
            }

            // Check for last valid list item (non-null)
            list_index = -1;
            for (_i = 0; _i < _items.Length; _i++) 
            {
                if (_items[_i] == null) // if empty entry has been found...
                {
                    list_index = _i;    // mark list index
                    break;              // stop searching
                }
            }

            // Resize recent file list
            if(list_index > -1)
                Array.Resize(ref _items, list_index);

            return;
        }

    }

    public class clsRecentFileListItem
    {
        string _dsp = System.IO.Path.DirectorySeparatorChar.ToString();
        string _adsp = System.IO.Path.AltDirectorySeparatorChar.ToString();
        
        private string _path = "";
        internal string Path
        {
            get { return (_path); }
            set {
                int arr_ubound = 0;
                string[] dum_sep = new string[1];
                string[] str_arr;
                
                _path = value;

                // Create a label whcih gets shortedned if path is getting too long
                if (_path.Contains(_dsp) && (!_path.Contains(_adsp)))
                    dum_sep[0] = _dsp;
                else if ((!_path.Contains(_dsp)) && (_path.Contains(_adsp)))
                    dum_sep[0] = _adsp;
                str_arr = _path.Split(dum_sep, StringSplitOptions.RemoveEmptyEntries);
                arr_ubound = str_arr.GetUpperBound(0);

                if (str_arr.Length > 6)
                    _display_label = str_arr[0] + _dsp + str_arr[1] + _dsp + "..." + 
                        _dsp + str_arr[arr_ubound - 2] + _dsp + str_arr[arr_ubound - 1] + _dsp + str_arr[arr_ubound];
                else
                    _display_label = _path;

                // Extract file title
                _title = str_arr[arr_ubound];

                return; 
            }
        }

        private string _title = "";
        internal string Title
        {
            get { return (_title); }
        }

        private string _display_label = "";
        internal string DisplayLabel
        {
            get { return (_display_label); }
        }
    

    }
}
