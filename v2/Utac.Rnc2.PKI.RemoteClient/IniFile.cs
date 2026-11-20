using System;
using System.Runtime.InteropServices;
using System.Text;

namespace RemoteClientPKI
{

    internal class CIniFile
    {

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
            string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
                 string key, string def, StringBuilder retVal,
            int size, string filePath);

        private readonly string IniFile;

        public CIniFile()
        {
            IniFile = Environment.GetEnvironmentVariable("windir") + "\\rnc2.ini";

        }

        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.IniFile);
        }

        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp,
                                            255, this.IniFile);
            return temp.ToString();

        }

    }
}

