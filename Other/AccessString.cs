/// <summary>
    /// Access string class works with access string from db
    /// </summary>
    class AccessString
    {
        Dictionary<string, string> _accesslist = new Dictionary<string, string>();

        private string _token;
        private string _fullAccessString;

        /// <summary>
        /// Action list
        /// </summary>
        [Flags]
        public  enum Actions
        {
          View = 1,
          Add = 10,
          Edit = 100,
          Delete = 1000,
          User = 10000,
          End = 100000,
        }

        /// <summary>
        /// Contains Size of Actions enum
        /// </summary>
        private readonly int _actionSize;  

        /// <summary>
        /// Attributes that can be added 
        /// </summary>
        [Flags]
        public enum Attributes
        {
            Remove = 0,
            Add =1
        }
        /// <summary>
        /// name of Action or Method name
        /// </summary>
        public enum AccessName
        {
            Drivers,
            Users,
            Orders,
        }
     
        /// <summary>
        /// Is current data Valid
        /// </summary>
        private bool IsValid { get;}

        //example "Drivers=100000,Users=110001,Orders=111111"
        /// <summary>
        /// Method parse string and add data to dictionary
        /// </summary>
        /// <param name="str">full access string</param>
        /// <returns>return true if parse was success</returns>
        private bool ParseString(string str)
        {

            if (string.IsNullOrEmpty(str))
            {
                return false;
            }

            if (str.Contains(','))
            {
                if (str.Contains('='))
                {
                    string[] splitStr = str.Split(',');
                    for (int i = 0; i < splitStr.Length; i++)
                    {
                        string[] subSplit = splitStr[i].Split('=');
                        string name;
                        string access;
                        if (subSplit.Length == 2)
                        {
                            name = subSplit[0];
                            access = subSplit[1];
                        }
                        else
                        {
                            //todo error
                            _accesslist = new Dictionary<string, string>();
                            return false;
                        }

                        if (name.Length > 1 && access.Length == _actionSize)
                        {
                            _accesslist.Add(name,access);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Method check access to actions
        /// </summary>
        /// <param name="accessName">Method or Action Name</param>
        /// <param name="action">Action</param>
        /// <returns>return true if action is valid</returns>
        public bool CheckAccess(AccessName accessName,Actions action)
        {
            string name = $"{accessName}";
            if (!_accesslist.ContainsKey(name) || !IsValid)
            {
                return false;
            }
            
            switch (action)
            {
                case Actions.View:
                {
                    string acc;
                    if (_accesslist.TryGetValue(name,out acc))
                    {
                        return Check(acc, Actions.View);
                    }
                    return false;
                }
                case Actions.Add:
                {
                    string acc;
                    if (_accesslist.TryGetValue(name, out acc))
                    {
                        return Check(acc, Actions.Add);
                    }
                    return false;
                    }
                case Actions.Edit:
                {
                    string acc;
                    if (_accesslist.TryGetValue(name, out acc))
                    {
                        return Check(acc, Actions.Edit);
                    }
                    return false;
                    }
                case Actions.Delete:
                {
                    string acc;
                    if (_accesslist.TryGetValue(name, out acc))
                    {
                        return Check(acc, Actions.Delete);
                    }
                    return false;
                    }
                case Actions.User:
                {
                    string acc;
                    if (_accesslist.TryGetValue(name, out acc))
                    {
                        return Check(acc, Actions.User);
                    }
                    return false;
                }
                default: return false;
            }
        }

        /// <summary>
        /// Method check access to action
        /// </summary>
        /// <param name="str">access string</param>
        /// <param name="action">Enum Actions</param>
        /// <returns></returns>
        private bool Check(string str,Actions action)
        {
            if (!IsValid)
            {
                return IsValid;
            }

            ushort read = 0; //view 
            ushort write=0; //add
            ushort edit=0; //edit
            ushort delete=0; //delete
            ushort user=0; //user

            if (str.Length == _actionSize)
            {
                user = Convert.ToUInt16(str[1].ToString());
                delete = Convert.ToUInt16(str[2].ToString());
                edit = Convert.ToUInt16(str[3].ToString());
                write = Convert.ToUInt16(str[4].ToString());
                read = Convert.ToUInt16(str[5].ToString());
            }

            switch (action)
            {
                case Actions.View:
                {
                    if (read == 1)
                    {
                        return true;
                    }

                    return false;
                }
                case Actions.Add:
                {
                    if (write == 1)
                    {
                        return true;
                    }

                    return false;
                }
                case Actions.Edit:
                {
                    if (edit == 1)
                    {
                        return true;
                    }

                    return false;
                }
                case Actions.Delete:
                {
                    if (delete == 1)
                    {
                        return true;
                    }

                    return false;
                }
                case Actions.User:
                {
                    if (user == 1)
                    {
                        return true;
                    }
                    return false;
                }
                default: return false;
            }

        }

        /// <summary>
        /// Ctor 
        /// </summary>
        /// <param name="token">user token</param>
        public AccessString(string token)
        {
            //Todo need to set _fullAccessString from sql

            _token = token;
            _actionSize = Enum.GetNames(typeof(Actions)).Length;
            //todo must get data from sql and set IsValid to true if data exist
            if (IsValid)
            {
                IsValid = ParseString(_fullAccessString);
            }
        }
        /// <summary>
        /// Method Add or remove access to action 
        /// </summary>
        /// <param name="accessName">Enum AccessName</param>
        /// <param name="action">enum Action</param>
        /// <param name="attribute">Enum Attributes</param>
        /// <returns> returns true if success</returns>
        public bool SetOptions(AccessName accessName, Actions action, Attributes attribute )
        {
            string accessMethod = $"{accessName}";
            if (IsValid)
            {
                string acc;

                if (_accesslist.TryGetValue(accessMethod, out acc))
                {
                    ushort read = 0; //view 
                    ushort write = 0; //add
                    ushort edit = 0; //edit
                    ushort delete = 0; //delete
                    ushort user = 0; //user
                    ushort en = 1;

                    if (acc.Length == _actionSize)
                    {
                        en = Convert.ToUInt16(acc[0].ToString());
                        user = Convert.ToUInt16(acc[1].ToString());
                        delete = Convert.ToUInt16(acc[2].ToString());
                        edit = Convert.ToUInt16(acc[3].ToString());
                        write = Convert.ToUInt16(acc[4].ToString());
                        read = Convert.ToUInt16(acc[5].ToString());
                    }

                    switch (action)
                    {
                        case Actions.View:
                        {
                            read = (ushort)attribute;
                           _accesslist[accessMethod]=$"{en}{user}{delete}{edit}{write}{read}";
                            return true;
                        }
                        case Actions.Add:
                        {
                            write = (ushort)attribute;
                            _accesslist[accessMethod] = $"{en}{user}{delete}{edit}{write}{read}";
                            return true;
                        }
                        case Actions.Edit:
                        {
                            edit = (ushort)attribute;
                            _accesslist[accessMethod] = $"{en}{user}{delete}{edit}{write}{read}";
                            return true;
                        }
                        case Actions.Delete:
                        {
                            delete = (ushort)attribute;
                            _accesslist[accessMethod] = $"{en}{user}{delete}{edit}{write}{read}";
                            return true;
                        }
                        default: return false;
                    }

                }
                return false;
            }
            return false;
        }
        /// <summary>
        /// Method returns new string after modifications 
        /// </summary>
        /// <returns>Access String</returns>
        public string GetAccessString()
        {
            //todo update access string with new options 
            StringBuilder accessString = new StringBuilder();
            for (int i = 0; i < _accesslist.Count; i++)
            {
                accessString.Append($"{_accesslist.ElementAt(i).Key}={_accesslist.ElementAt(i).Value},");
            }
            return accessString.ToString();
        }
        /// <summary>
        /// Method return AccessString to all methods with no rights 
        /// </summary>
        /// <returns></returns>
        public string CreateEmptyAccessString()
        {
            string emptyAccessString="";
            int enumCount = Enum.GetNames(typeof(AccessName)).Length;
            for (int i = 0; i < enumCount; i++)
            {
                emptyAccessString += $"{(AccessName) i}={(int)Actions.End},";
            }
            return emptyAccessString; //$"{AccessName.Drivers}={(int)Actions.End},{AccessName.Orders}={(int)Actions.End}";
        }

    }
