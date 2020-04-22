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
        string _dsp = System.IO.Path.DirectorySeparatorChar.ToString();

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

        private int _selected_item = 0;
        internal int SelectedItem
        {
            get { return (_selected_item); }
            set { _selected_item = value; return; }
        }

        private clsRecentFileListItem[] _items;
        internal clsRecentFileListItem[] Items
        {
            get { return (_items); }
            set { _items = value; return; }
        }

        private void WriteParentFileList()
        {
            int _i = 0;

            // Write updated recent file list to file
            for (_i = 0; _i < _items.Length; _i++)
            {
                _settings_file.WriteKey("recent_files", "file" + _i.ToString(), _items[_i].Path);
            }
            _settings_file.WriteKey("recent_files", "count", _items.Length.ToString());

            return;
        }

        private void ClearParentFileList()
        {
            int _i = 0, file_count = 0;

            // Clear recent file list in file
            file_count = Convert.ToInt32(_settings_file.ReadKey("recent_files", "count", "0"));

            for (_i = 0; _i < file_count; _i++)
            {
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
            string[] dum_sep = new string[1];
            string[] str_arr;

            if(_settings_file == null)
                return(false);

            file_count = Convert.ToInt32(_settings_file.ReadKey("recent_files", "count", "0"));
            if (file_count == 0)
                return(true);

            _items = new clsRecentFileListItem[file_count];

            for (_i = 0; _i < _items.Length; _i++)
            {
                // Read full string from file
                _items[_i] = new clsRecentFileListItem();
                _items[_i].Path = _settings_file.ReadKey("recent_files", "file" + _i.ToString(), "").Trim();

                // Split off file name
                dum_sep[0] = _dsp;
                str_arr = _items[_i].Path.Split(dum_sep, StringSplitOptions.RemoveEmptyEntries);
                _items[_i].Title = str_arr[str_arr.GetUpperBound(0)];

            }

            return(true);
        }

        internal void AddNew(string Filename)
        {
            string str_dum = Filename.Trim();
            string[] dum_sep = new string[1];
            string[] str_arr;
            clsRecentFileListItem[] _item_list;
            clsRecentFileListItem new_item;

            if (str_dum.Length == 0)
                return;

            if (!File.Exists(str_dum))
                return;

            // Create new item
            dum_sep[0] = _dsp;
            str_arr = str_dum.Split(dum_sep, StringSplitOptions.RemoveEmptyEntries);

            new_item = new clsRecentFileListItem();
            new_item.Path = str_dum;
            new_item.Title = str_arr[str_arr.GetUpperBound(0)];
            
            // Extend items array by one element
            _item_list = _items;
            Array.Resize(ref _item_list, (_item_list.Length + 1));
            Array.Copy(_items, 0, _item_list, 1, _items.Length);
            _item_list[0] = new_item;

            // Copy new list back into list
            _items = _item_list;
            _item_list = null;

            Trim(); // Trim new list

            // Clear file list in file
            ClearParentFileList();

            // Write updated list to settings file
            WriteParentFileList();

            return;
        }

        internal void Remove(string Filename)
        {
            int _i = 0;

            for (_i = 0; _i < _items.Length; _i++)
            {
                if(_items[_i].Path.Trim().ToLower() == Filename.Trim().ToLower())
                {
                    _items[_i] = null;
                    break;
                }
            }

            Trim();
            WriteParentFileList();
            GetFileList();

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
        private string _path = "";
        internal string Path
        {
            get { return (_path); }
            set { _path = value; return; }
        }

        private string _title = "";
        internal string Title
        {
            get { return (_title); }
            set { _title = value; return; }
        }
    
    }
}
