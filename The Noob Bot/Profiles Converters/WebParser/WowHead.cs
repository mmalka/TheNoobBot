using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using nManager.Helpful;

namespace Profiles_Converters.WebParser
{
    internal class WowHead
    {
        private static string RetrieveContent(int questId)
        {
            var request = (HttpWebRequest) WebRequest.Create("http://www.wowhead.com/quest=" + questId);
            request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.2.12) Gecko/20101026 Firefox/3";
            request.Accept = "Accept: text/html,application/xhtml+xml,application/xml";

            string strData = "";
            try
            {
                request.Proxy = WebProxy.GetDefaultProxy();
                var response = (HttpWebResponse) request.GetResponse();
                Stream stream = response.GetResponseStream();
                Encoding ec = Encoding.GetEncoding("utf-8");
                var reader = new StreamReader(stream, ec);
                strData = reader.ReadToEnd();
                if (strData.Contains("Error"))
                {
                    var e = new Exception(strData);
                    throw e;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return strData;
        }

        public static QuestInfo GetQuestObject(int questId)
        {
            string content = RetrieveContent(questId);
            string beginning = "'};\n$.extend(true, g_quests, _);\n_ = g_quests;\n$.extend(g_quests[" + questId + "], ";
            const string end = ");\nvar _ = {};";
            const string beginning2 = "<div id=\"infobox-contents0\"></div>";
            const string end2 = "'infobox-contents0'";
            const string beginning3 = "ext/javascript\">\n\t\t\tMarkup.printHtml('\\";
            const string end3 = @"', ";
            string beginning4 = @"color\x3Dr4\x5D";
            const string end4 = @"\x5B\";
            int start = content.IndexOf(beginning, StringComparison.Ordinal) + beginning.Length;
            if (start <= 0 || start == beginning.Length - 1)
            {
                Logging.WriteError("Quest " + questId + " is marked as invalid;");
                return new QuestInfo();
            }
            int start2 = content.IndexOf(beginning2, StringComparison.Ordinal) + beginning2.Length;
            int stop = content.IndexOf(end, start, StringComparison.Ordinal);
            int stop2 = content.IndexOf(end2, start2, StringComparison.Ordinal);
            string result = content.Substring(start, stop - start);
            string result2 = content.Substring(start2, stop2 - start2);
            int start3 = result2.IndexOf(beginning3, StringComparison.Ordinal) + beginning3.Length;
            int stop3 = result2.IndexOf(end3, start3, StringComparison.Ordinal);
            string result3 = result2.Substring(start3, stop3 - start3);
            int start4 = result3.IndexOf(beginning4, StringComparison.Ordinal) + beginning4.Length;
            int result4Add = -1;
            if (start4 <= 0 || start4 == beginning4.Length - 1)
            {
                beginning4 = @"color\x3Dr3\x5D";
                start4 = result3.IndexOf(beginning4, StringComparison.Ordinal) + beginning4.Length;
                result4Add = 1;
            }
            int stop4 = result3.IndexOf(end4, start4, StringComparison.Ordinal);
            string result4 = result3.Substring(start4, stop4 - start4);

            var qInfo = new QuestInfo();
            var qObject = JsonConvert.DeserializeObject<RootObject>(result);
            qInfo.Category = qObject.category;
            qInfo.Category2 = qObject.category2;
            qInfo.Classs = qObject.classs;
            qInfo.Id = qObject.id;
            qInfo.Itemchoices = qObject.itemchoices;
            qInfo.Itemrewards = qObject.itemrewards;
            qInfo.Level = qObject.level;
            qInfo.Money = qObject.money;
            qInfo.Name = qObject.name;
            qInfo.Race = qObject.race;
            qInfo.Reprewards = qObject.reprewards;
            qInfo.Reqclass = qObject.reqclass;
            qInfo.ReqMinLevel = qObject.reqlevel;
            qInfo.ReqMaxLevel = Others.ToInt32(result4) + result4Add;
            qInfo.Side = qObject.side;
            qInfo.Wflags = qObject.wflags;
            qInfo.Type = qObject.type;
            qInfo.XP = qObject.xp;
            qInfo.IsValid = true;
            return qInfo;
        }

        public class QuestInfo
        {
            public QuestInfo()
            {
                IsValid = false;
            }

            public bool IsValid { get; set; }
            public int Category { get; set; }
            public int Category2 { get; set; }
            public int Classs { get; set; }
            public int Id { get; set; }
            public List<List<int>> Itemchoices { get; set; }
            public List<List<int>> Itemrewards { get; set; }
            public int Level { get; set; }
            public int Money { get; set; }
            public string Name { get; set; }
            public int Race { get; set; }
            public List<List<int>> Reprewards { get; set; }
            public int Reqclass { get; set; }
            public int ReqMinLevel { get; set; }
            public int ReqMaxLevel { get; set; }
            public int Reqrace { get; set; }
            public int Side { get; set; }
            public int Wflags { get; set; }
            public int Type { get; set; }
            public int XP { get; set; }
        }

        public class RootObject
        {
            public int category { get; set; }
            public int category2 { get; set; }
            public int classs { get; set; }
            public int id { get; set; }
            public List<List<int>> itemchoices { get; set; }
            public List<List<int>> itemrewards { get; set; }
            public int level { get; set; }
            public int money { get; set; }
            public string name { get; set; }
            public int race { get; set; }
            public List<List<int>> reprewards { get; set; }
            public int reqclass { get; set; }
            public int reqlevel { get; set; }
            public int reqrace { get; set; }
            public int side { get; set; }
            public int wflags { get; set; }
            public int type { get; set; }
            public int xp { get; set; }
        }
    }
}