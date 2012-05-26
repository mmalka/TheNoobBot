using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace nManager.Helpful
{
    public static class Others
    {
        /// <summary>
        /// Text To Utf8.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToUtf8(string text)
        {
            try
            {
                if (string.IsNullOrEmpty(text))
                {
                    return text;
                }

                var bytes = Encoding.Default.GetBytes(text);
                return Encoding.UTF8.GetString(bytes);
            }
            catch (Exception exception)
            {
                Logging.WriteError("ToUtf8(string text): " + exception);
            }
            return "";
        }

        public static string[] TextToArrayByLine(string text)
        {
            try
            {
                var ret = new List<string>();
                string[] split = text.Split(Environment.NewLine.ToCharArray());

                foreach (string s in split)
                {
                    if (s.Trim() != "")
                        ret.Add(s);
                }
                return ret.ToArray();
            }
            catch (Exception exception)
            {
                Logging.WriteError("TextToArrayByLine(string text): " + exception);
                return new string[0];
            }
        }

        public static string ArrayToTextByLine(string[] array)
        {
            try
            {
                string ret = "";
                foreach (var l in array)
                {
                    ret += l + Environment.NewLine;
                }
                return ret;
            }
            catch (Exception exception)
            {
                Logging.WriteError("ArrayToTextByLine(string[] array): " + exception);
                return "";
            }
        }

        /// <summary>
        /// Open Web Browser
        /// </summary>
        /// <param name="urlOrPath"></param>
        /// <returns></returns>
        public static void OpenWebBrowserOrApplication(string urlOrPath)
        {
            try
            {
                var p =
                    new Process();
                var pi =
                    new ProcessStartInfo
                    {
                        FileName = urlOrPath
                    };
                p.StartInfo = pi;
                p.Start();
            }
            catch (Exception exception)
            {
                Logging.WriteError("OpenWebBrowser(string url): " + exception);
            }
        }

        private static readonly UTF8Encoding Utf8 = new UTF8Encoding();
        public static string ToUtf8(byte[] bytes)
        {
            try
            {
                var s = Utf8.GetString(bytes, 0, bytes.Length);

                if (s.IndexOf("\0", StringComparison.Ordinal) != -1)
                    s = s.Remove(s.IndexOf("\0", StringComparison.Ordinal), s.Length - s.IndexOf("\0", StringComparison.Ordinal));

                return s;
            }
            catch (Exception exception)
            {
                Logging.WriteError("ToUtf8(byte[] bytes): " + exception);
            }
            return "";
        }

        public static int HardDriveID()
        {
            try
            {
                var dsk = new ManagementObject(@"win32_logicaldisk.deviceid=""c:""");
                dsk.Get();
                string volumeSerial = dsk["VolumeSerialNumber"].ToString();
                return volumeSerial.Sum(c => Convert.ToInt32(c));
            }
            catch (Exception e)
            {
                Logging.WriteError("HardDriveID(): " + e);
                return 0;
            }
        }

        /// <summary>
        /// Return the MD5 checksun of the file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileMd5CheckSum(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    FileStream st = null;
                    try
                    {
                        var check = new MD5CryptoServiceProvider();
                        st = File.Open(filePath, FileMode.Open, FileAccess.Read);
                        byte[] somme = check.ComputeHash(st);
                        string ret = "";
                        foreach (byte a in somme)
                        {
                            if ((a < 16))
                            {
                                ret += "0" + a.ToString("X");
                            }
                            else
                            {
                                ret += a.ToString("X");
                            }
                        }
                        return ret;
                    }
                    catch (Exception exception)
                    {
                        Logging.WriteError("GetFileMd5CheckSum(string filePath)#1: " + exception);
                    }
                    finally
                    {
                        if (st != null) st.Close();
                    }
                }
                else
                {
                    return "";
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetFileMd5CheckSum(string filePath)#2: " + exception);
            }
            return "";
        }

        public static string DelSpecialChar(string stringSpecialChar)
        {
            try
            {
                var l = new List<string>();
                foreach (var c in stringSpecialChar)
                {
                    if (c < 65 || c > 122 && (c > 90 || c < 97))
                    {
                        // 
                    }
                    else
                    {
                        l.Add(Convert.ToString(c));
                    }
                }
                return String.Concat(l);
            }
            catch (Exception e)
            {
                Logging.WriteError("DelSpecialChar(string stringSpecialChar): " + e);
                return "";
            }
        }

        public static string GetRandomString(int maxSize)
        {
            try
            {
                var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
                var data = new byte[1];
                var crypto = new RNGCryptoServiceProvider();
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
                var result = new StringBuilder(maxSize);
                foreach (byte b in data)
                {
                    result.Append(chars[b % (chars.Length - 1)]);
                }
                return result.ToString();
            }
            catch (Exception e)
            {
                Logging.WriteError("GetRandomString(int maxSize): " + e);
            }
            return "abcdef";
        }

        /// <summary>
        /// Shut Down the Computer.
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public static void ShutDownPc()
        {
            try
            {
                var proc = new Process { StartInfo = { FileName = "shutdown.exe", Arguments = " -s -f" } };
                proc.Start();
                proc.Close();
            }
            catch (Exception exception)
            {
                Logging.WriteError("ShutDownPc(): " + exception);
            }
        }

        /// <summary>
        /// Return Random number.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static int Random(int from, int to)
        {
            try
            {
                var r = new Random(unchecked((int)DateTime.Now.Ticks));
                return r.Next(from, to);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Random(int from, int to): " + exception);
            }
            return 0;
        }

        /// <summary>
        /// int Seconde to string HH:MM:SS.
        /// </summary>
        /// <param name ="sec"></param>
        /// <returns></returns>
        public static string SecToHour(int sec)
        {
            try
            {
                var houre = (sec / 3600) + "H";
                sec = sec - ((sec / 3600) * 3600);

                if ((sec / 60) < 10)
                    houre = houre + "0" + (sec / 60) + "M";
                else
                    houre = houre + (sec / 60) + "M";

                sec = sec - ((sec / 60) * 60);

                if (sec < 10)
                    houre = houre + "0" + sec + "";
                else
                    houre = houre + sec + "";

                return houre;
            }
            catch (Exception e)
            {
                Logging.WriteError("SecToHour(int sec): " + e);
                return "00H00M00";
            }
        }

        /// <summary>
        /// Read file and return array.
        /// </summary>
        /// <param name ="path"></param>
        /// <returns></returns>
        public static string[] ReadFileAllLines(string path)
        {
            try
            {
                var coll = new StringCollection();

                using (var sr = new StreamReader(path))
                {
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        coll.Add(line);
                    }
                }


                var lines = new string[coll.Count];
                coll.CopyTo(lines, 0);
                return lines;
            }
            catch (Exception e)
            {
                Logging.WriteError("ReadFileAllLines(string path): " + e);
                return new string[0];
            }
        }

        /// <summary>
        /// Time on milisec.
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public static int Times { get { return Environment.TickCount; } }

        /// <summary>
        /// Time on sec.
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public static int TimesSec { get { return Environment.TickCount/1000; } }

        static string _httpUrl = "";
        static string _fileDest = "";
        static bool _downloadFinish;
        static readonly object LockerDownload = new object();
        /// <summary>
        /// Download file from http address, return true if sucess.
        /// </summary>
        /// <param name ="httpUrl"></param>
        /// <param name ="fileDest"></param>
        /// <returns></returns>
        public static bool DownloadFile(string httpUrl, string fileDest)
        {
            try
            {
                lock (LockerDownload)
                {
                    _httpUrl = httpUrl;
                    _fileDest = fileDest;

                    var checkUpdateThreadLaunch = new Thread(DownloadThread) { Name = "DownloadFile" };
                    checkUpdateThreadLaunch.Start();

                    Thread.Sleep(200);

                    while (_downloadFinish)
                    {
                        Application.DoEvents();
                        Thread.Sleep(100);
                    }
                    if (ExistFile(fileDest))
                        return true;
                    return false;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("DownloadFile(string httpUrl, string fileDest): " + exception);
                _downloadFinish = false;
                return false;
            }
        }
        static void DownloadThread()
        {
            try
            {
                _downloadFinish = true;
                try
                {
                    var client = new WebClient();
                    client.DownloadFile(_httpUrl, _fileDest);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("DownloadThread()#1: " + exception);
                }
                _downloadFinish = false;
            }
            catch (Exception exception)
            {
                Logging.WriteError("DownloadThread()#2: " + exception);
            }
        }

        /// <summary>
        /// Return a list of the file.
        /// </summary>
        /// <param name ="pathDirectory"></param>
        /// <param name ="searchPattern"></param>
        /// <returns></returns>
        public static List<String> GetFilesDirectory(string pathDirectory, string searchPattern = "")
        {
            try
            {
                string path = "";
                if (!pathDirectory.Contains(":"))
                    path = Application.StartupPath;
                return Directory.GetFiles(path + pathDirectory, searchPattern).Select(subfolder =>
                {
                    var name = Path.GetFileName(subfolder);
                    return name != null ? name.ToString(CultureInfo.InvariantCulture) : null;
                }).ToList();
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetFilesDirectory(string pathDirectory, string searchPattern = \"\"): " + exception);
            }
            return new List<string>();
        }

        /// <summary>
        /// Return the code source of the page. Sample: GetRequest("http://www.google.com/index.php", "a=5&amp;b=10" )
        /// </summary>
        /// <param name ="url"></param>
        /// <param name ="data"></param>
        /// <returns></returns>
        public static string GetRequest(string url, string data)
        {
            HttpWebResponse httpWResponse = null;
            StreamReader sr = null;
            string result;
            try
            {
                if (!string.IsNullOrWhiteSpace(data))
                    url = url + "?" + data;
                var httpWRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWRequest.UserAgent = "TheNoobBot";
                httpWResponse = (HttpWebResponse)httpWRequest.GetResponse();
                sr = new StreamReader(httpWResponse.GetResponseStream(), Encoding.GetEncoding("iso-8859-1"));
                result = sr.ReadToEnd();

            }

            catch (Exception ex)
            {
                Logging.WriteError("GetRequest(string url, string data): " + ex);
                result = "";
            }

            finally
            {
                if (httpWResponse != null) httpWResponse.Close();
                if (sr != null) sr.Close();
            }

            return result;
        }

        /// <summary>
        /// Return the code source of the page. Sample: PostRequest("http://www.google.com/index.php", "a=5&amp;b=10" )
        /// </summary>
        /// <param name ="url"></param>
        /// <param name ="parameters"></param>
        /// <returns></returns>
        public static string PostRequest(string url, string parameters)
        {
            try
            {
                // parameters: name1=value1&name2=value2	
                WebRequest webRequest = WebRequest.Create(url);
                //string ProxyString = 
                //   System.Configuration.ConfigurationManager.AppSettings
                //   [GetConfigKey("proxy")];
                //webRequest.Proxy = new WebProxy (ProxyString, true);
                //Commenting out above required change to App.Config
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.Method = "POST";
                ((HttpWebRequest)webRequest).UserAgent = "TheNoobBot";
                byte[] bytes = Encoding.UTF8.GetBytes(parameters);
                Stream os = null;
                try
                { // send the Post
                    webRequest.ContentLength = bytes.Length;   //Count bytes to send
                    os = webRequest.GetRequestStream();
                    os.Write(bytes, 0, bytes.Length);         //Send it
                }
                catch (WebException ex)
                {
                    Logging.WriteError("PostRequest(string url, string parameters)#1: " + ex);
                }
                finally
                {
                    if (os != null)
                    {
                        os.Close();
                    }
                }

                try
                { // get the response
                    WebResponse webResponse = webRequest.GetResponse();
                    var sr = new StreamReader(webResponse.GetResponseStream());
                    string name = sr.ReadToEnd().Trim();
                    return name;
                }
                catch (WebException ex)
                {
                    Logging.WriteError("PostRequest(string url, string parameters)#2: " + ex.Message);
                    MessageBox.Show(ex.Message, "HttpPost: Response error",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("PostRequest(string url, string parameters)#3: " + exception);
            }
            return null;
        }

        /// <summary>
        /// Return the path of bot directory.
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        public static string GetCurrentDirectory { get { return Application.StartupPath; } }

        /// <summary>
        /// Return MD5.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string EncrypterMD5(string value)
        {
            try
            {
                byte[] data = new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(value));

                var hashedString = new StringBuilder();

                for (int i = 0; i < data.Length; i++)

                    hashedString.Append(data[i].ToString("x2"));

                return hashedString.ToString();
            }
            catch (Exception exception)
            {
                Logging.WriteError("EncrypterMD5(string value): " + exception);
            }
            return "";
        }

        /// <summary>
        /// Open et dialog box Open File and return a path file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="typeFile"></param>
        /// <returns></returns>
        public static string DialogBoxOpenFile(string path, string typeFile)
        {
            try
            {
                var chooseFile = new OpenFileDialog
                                     {InitialDirectory = path, Filter = typeFile};
                chooseFile.ShowDialog();
                return chooseFile.FileName;
            }
            catch (Exception exception)
            {
                Logging.WriteError("DialogBoxOpenFile(string path, string typeFile): " + exception);
            }
            return "";
        }

        public static string[] DialogBoxOpenFileMultiselect(string path, string typeFile)
        {
            try
            {
                var chooseFile = new OpenFileDialog { InitialDirectory = path, Filter = typeFile, Multiselect = true };
                chooseFile.ShowDialog();
                return chooseFile.FileNames;
            }
            catch (Exception exception)
            {
                Logging.WriteError("DialogBoxOpenFile(string path, string typeFile): " + exception);
            }
            return new string[0];
        }

        /// <summary>
        /// Open et dialog box Save File and return a path file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="typeFile"></param>
        /// <returns></returns>
        public static string DialogBoxSaveFile(string path, string typeFile)
        {
            try
            {
                var saveFile = new SaveFileDialog { InitialDirectory = path, Filter = typeFile };
                saveFile.ShowDialog();
                return saveFile.FileName;
            }
            catch (Exception exception)
            {
                Logging.WriteError("DialogBoxSaveFile(string path, string typeFile): " + exception);
            }
            return "";
        }

        /// <summary>
        /// Gets if visual C++2010 is installed.
        /// </summary>
        public static void GetVisualCpp2010()
        {
            try
            {
                if (File.Exists(Environment.SystemDirectory + "\\mfc100.dll"))
                {
                    return;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetVisualCpp2010() #1: " + exception);
            }

            try
            {
                var resulMb =
                    MessageBox.Show(
                        Translate.Get(Translate.Id
                        .Visual_C________redistributable_X___package_is_requis_for_this_tnb__It_is_not_installed_on_your_computer__do_you_want_install_this_now___If_this_is_not_installed_on_your_computer_the_tnb_don_t_work_correctly),
                        "Visual C++ 2010 redistributable X86 " + Translate.Get(Translate.Id.Requis), MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (resulMb == DialogResult.Yes)
                {
                    Process.Start("http://www.microsoft.com/downloads/en/details.aspx?FamilyID=a7b7a05e-6de6-4d3a-a423-37bf0912db84");
                }
                Process.GetCurrentProcess().Kill();
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetVisualCpp2010() #2: " + exception);
            }
        }

        /// <summary>
        /// Return true if File exist.
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public static bool ExistFile(string strPath)
        {
            try
            {
                return File.Exists(strPath);
            }
            catch (Exception exception)
            {
                Logging.WriteError("ExistFile(string strPath): " + exception);
            }
            return false;
        }

        /// <summary>
        /// Return a string text ecncrypted.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string StringToEncryptString(string text)
        {
            try
            {
                string chaineEncrypt = "";
                string chaine = text;
                byte[] test = Encoding.UTF8.GetBytes(chaine);
                int key = Convert.ToInt32(Convert.ToInt32(HardDriveID()));
                for (int i = 0; i <= test.Length - 1; i++)
                {
                    if (chaineEncrypt != "")
                    {
                        chaineEncrypt = chaineEncrypt + "-";
                    }
                    chaineEncrypt = chaineEncrypt + (test[i] + key + 15);
                }
                return chaineEncrypt;
            }
            catch (Exception exception)
            {
                Logging.WriteError("StringToEncryptString(string text): " + exception);
            }
            return "";
        }

        /// <summary>
        /// Return a string text decrypted.
        /// </summary>
        /// <param name="encryptText"></param>
        /// <returns></returns>
        public static string EncryptStringToString(string encryptText)
        {
            try
            {
                string[] texte2 = encryptText.Split(Convert.ToChar("-"));
                var listBytes = new List<Byte>();
                int key = Convert.ToInt32(Convert.ToInt32(HardDriveID()));

                for (int i = 0; i <= texte2.Length - 1; i++)
                {
                    listBytes.Add(Convert.ToByte(Convert.ToInt32(texte2[i]) - key - 15));
                }
                byte[] arrayBytes = listBytes.ToArray();
                return Encoding.UTF8.GetString(arrayBytes, 0, arrayBytes.Length);
            }
            catch (Exception exception)
            {
                Logging.WriteError("EncryptStringToString(string encryptText): " + exception);
            }
            return "";
        }

        /// <summary>
        /// Read file, Return a string.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="writeNewLine"> </param>
        /// <returns></returns>
        public static string ReadFile(string filePath, bool writeNewLine = false)
        {
            try
            {
                var monStreamReader = new StreamReader(filePath);
                string ligne = monStreamReader.ReadLine();
                string returnText = "";

                while (ligne != null)
                {
                    returnText = returnText + ligne;
                    if (writeNewLine)
                        returnText += Environment.NewLine;
                    ligne = monStreamReader.ReadLine();
                    Application.DoEvents();
                    //Thread.Sleep(1);
                }

                monStreamReader.Close();
                return returnText;
            }
            catch (Exception e)
            {
                Logging.WriteError("ReadFile(string filePath, bool writeNewLine = false): " + e);
                return "";
            }
        }

        /// <summary>
        /// Write file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void WriteFile(string filePath, string value)
        {
            try
            {
                var monStreamWriter = new StreamWriter(filePath);

                monStreamWriter.Write(value);
                monStreamWriter.Close();
            }
            catch (Exception e)
            {
                Logging.WriteError("WriteFile(string filePath, string value): " + e);
            }
        }

        /// <summary>
        /// Wait.
        /// </summary>
        /// <param name="milsecToWait"></param>
        /// <returns></returns>
        public static void Wait(int milsecToWait)
        {
            try
            {
                var num = GetTickCount() + milsecToWait;
                while (GetTickCount() < num)
                {
                    Application.DoEvents();
                    Thread.Sleep(5);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Wait(int milsecToWait): " + exception);
            }
        }

        [DllImport("kernel32")]
        private static extern int GetTickCount();

        public static List<string> GetReqWithAuthHeader(string url, String userName, String userPassword)
        {
            try
            {
                var req = WebRequest.Create(ToUtf8(url));
                if (userName != "" && userPassword != "")
                {
                    var authInfo = ToUtf8(userName) + ":" + ToUtf8(userPassword);
                    authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                    req.Headers["Authorization"] = "Basic " + authInfo;
                }
                ((HttpWebRequest)req).UserAgent = "TheNoobBot";

                var response = req.GetResponse();
                string headerResult = "";
                if (userName != "" && userPassword != "")
                {
                    headerResult = response.Headers.Get("retn");
                }
                var stm = response.GetResponseStream();
                var r = new StreamReader(stm);
                var sourceResult = r.ReadToEnd();
                return new List<string> { ToUtf8(headerResult), ToUtf8(sourceResult) };
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetReqWithAuthHeader(string url, String userName, String userPassword): " + exception);
            }
            return new List<string> { "", "" };
        }
    }
}
