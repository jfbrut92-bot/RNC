using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

using RemoteCaInterface;

namespace RemoteClientPKI
{

    class RemoteInitClientPKI
    {
        public static CRemoteCaInterface GetRemoteObject(string sURL, bool sEnsureSecurity, string sUserName, string sPassword, string sDomain)
        {
            CIniFile iniFile = new CIniFile();

            if (sURL == "")
            {
                throw new Exception("-2");
            }

            TcpChannel chan;

            try
            {
                if (sEnsureSecurity == true)
                {
                    IDictionary props = new Hashtable();
                    //if (string.IsNullOrEmpty(sUserName))
                    //{
                    //    sUserName = "rnc2remote";
                    //}
                    props["username"] = sUserName;

                    props["password"] = sPassword;

                    props["domain"] = sDomain;

                    chan = new TcpChannel(props, null, null);

                    ChannelServices.RegisterChannel(chan, true);
                }
                else
                {
                    chan = new TcpChannel();
                    ChannelServices.RegisterChannel(chan, false);
                }
            }
            catch (RemotingException)
            {
                //Le canal est déjà inscrit
            }

            CRemoteCaInterface obj =
            (CRemoteCaInterface)Activator.GetObject(typeof(CRemoteCaInterface), sURL);

            if (obj == null)
            {
                throw new Exception("-1");
            }

            return obj;
        }

        public static CRemoteCaInterface GetRemoteObject()
        {

            CIniFile iniFile = new CIniFile();
            string sURL = iniFile.IniReadValue("PKI_REMOTE", "URL");
            string sEnsureSecurity = iniFile.IniReadValue("PKI_REMOTE", "ENSURESECURITY");
            bool security = (sEnsureSecurity == "true") ? true : false;

            CRemoteCaInterface obj = null;
            try
            {
                string sUserName = "";
                sUserName = iniFile.IniReadValue("PKI_REMOTE", "USERNAME").Trim();

                string sPassword = "";
                sPassword = iniFile.IniReadValue("PKI_REMOTE", "PASSWORD");

                string sDomain = "";
                sDomain = iniFile.IniReadValue("PKI_REMOTE", "DOMAIN");

                obj = GetRemoteObject(sURL, security, sUserName, sPassword, sDomain);
            }
            catch (RemotingException)
            {
                //Le canal est déjà inscrit
            }

            if (obj == null)
            {
                throw new Exception("-1");
            }

            return obj;
        }
    }
}
