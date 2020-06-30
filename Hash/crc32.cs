using System;
using System.IO;


namespace ProjectManager.ClassFolder
{
    /// <summary>
    /// Класс для расчета CRC32 
    /// </summary>
    class Crc32
    {
        private byte[] _b;
        uint[] table;

        private uint ComputeChecksum(byte[] bytes) {
            uint crc = 0xffffffff;
            for(int i = 0; i < bytes.Length; ++i) {
                byte index = (byte)(((crc) & 0xff) ^ bytes[i]);
                crc = (uint)((crc >> 8) ^ table[index]);
            }
            return ~crc;
        }

        private byte[] ComputeChecksumBytes(byte[] bytes) {
            return BitConverter.GetBytes(ComputeChecksum(bytes));
        }
        //Констуктор 
        public Crc32()
        {
            try
            {
                _b = null;
                uint poly = 0xedb88320;
                table = new uint[256];
                uint temp = 0;
                for (uint i = 0; i < table.Length; ++i)
                {
                    temp = i;
                    for (int j = 8; j > 0; --j)
                    {
                        if ((temp & 1) == 1)
                        {
                            temp = (uint)((temp >> 1) ^ poly);
                        }
                        else
                        {
                            temp >>= 1;
                        }
                    }
                    table[i] = temp;
                }
            }
            catch (Exception ex)
            {
                
                Error.Msg(ex);
            }
        }

        /// <summary>
        /// Метод счетает CRC32 для файл
        /// </summary>
        /// <param name="pathTofile">Путь к файлу</param>
        /// <returns>Вернет String с CRC32</returns>
        public string GetFileCrc32(string pathTofile)
        {
            try
            {
                if (File.Exists(pathTofile))
                {
                    _b = File.ReadAllBytes(pathTofile);
                    var a2 = ComputeChecksum(_b);
                    var hash16 = Convert.ToString(a2,16).ToUpper(); 
                    if (hash16!="0")
                    {
                        if (hash16.Length < 8)
                        {
                            int miss = 8 - hash16.Length;
                            for (int i = 0; i < miss; i++)
                            {
                                hash16 = hash16.Insert(0, "0");
                            }
                        } 
                    }

                    return hash16;
                }
                return "No file exist";
            }
            catch (Exception e)
            {
                Error.Msg(e);
                return "Hash Error";
            }

        }

    }
}
