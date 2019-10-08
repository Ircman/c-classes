public static string ToSecureString(SecureString securePassword)
        {
            if (securePassword == null)
                throw new ArgumentNullException("securePassword");

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        public static SecureString SecureStringToString(string password)
        {
            if (password == null)
                throw new ArgumentNullException("password");

            char[] chars=  password.ToCharArray(0, password.Length);
          

            SecureString securePassword = new SecureString();
            foreach (var c in chars)
            {
                securePassword.AppendChar(c);
            }
              securePassword.MakeReadOnly();
            return securePassword;
                
            
        }
