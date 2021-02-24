using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Management;

namespace fipur
{
    public static class fipur
    {
        public static bool FindPE(string prog)
        {

            RegistryKey _key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Compatibility Assistant\Store\");
            if (_key != null)
            {
                var keys = _key.GetValueNames();
                foreach (string k in keys)
                {

                    k.ToLower();
                    if (k.Contains(prog.ToLower()))
                    {
                        return true;
                    }
                    else if (k.ToUpper().Contains(prog.ToUpper()))
                    {
                        return true;
                    }
                }

            }
            return false;

        }

        public static List<string> GetAllR()
        {

            List<string> regKeys = new List<string>() {
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
            @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
            };
            List<string> installedList = new List<string>();

            foreach (string regKey in regKeys)
            {
                using (RegistryKey OpenKey = Registry.CurrentUser.OpenSubKey(regKey))
                {
                    foreach (string subkey in OpenKey.GetSubKeyNames())
                    {
                        using (RegistryKey sk = OpenKey.OpenSubKey(subkey))
                        {
                            try
                            {
                                installedList.Add(Convert.ToString(sk.GetValue("DisplayName")));
                            }
                            catch { }
                        }
                    }
                }
            }

            return installedList;
        }

        public static bool CheckSingleR(string prog)
        {
            List<string> regKeys = new List<string>() {
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
            @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
            };

            foreach (string regKey in regKeys)
            {
                using (RegistryKey OpenKey = Registry.CurrentUser.OpenSubKey(regKey))
                {
                    foreach (string subkey in OpenKey.GetSubKeyNames())
                    {
                        using (RegistryKey sk = OpenKey.OpenSubKey(subkey))
                        {
                            try
                            {
                                if (sk.GetValue("DisplayName").ToString() == prog)
                                {
                                    return true;
                                }
                            }
                            catch { }
                        }
                    }
                }
            }
            return false;

        }

        public static bool CheckSingleWMI(string prog)
        {
            ManagementObjectSearcher objectSearch = new ManagementObjectSearcher("SELECT * FROM Win32_Product");
            foreach (ManagementObject mo in objectSearch.Get())
            {
                try
                {
                    if (mo.GetPropertyValue("Name").ToString().Contains(prog))
                    {

                        return true;
                    }
                }
                catch (Exception e) when (mo == null || mo.GetPropertyValue("Name").ToString() == string.Empty)
                {

                }
            }
            return false;

        }

        public static List<string> GetAllWMI()
        {
            List<string> installed = new List<string>();

            ManagementObjectSearcher objectSearch = new ManagementObjectSearcher("SELECT * FROM Win32_Product");
            foreach (ManagementObject mo in objectSearch.Get())
            {
                try
                {
                    if (mo.GetPropertyValue("Name").ToString() != string.Empty)
                    {

                        installed.Add(mo.GetPropertyValue("Name").ToString());
                    }
                }
                catch (Exception e) when (mo == null || mo.GetPropertyValue("Name").ToString() == string.Empty)
                {

                }
            }
            return installed;

        }
    }
}
