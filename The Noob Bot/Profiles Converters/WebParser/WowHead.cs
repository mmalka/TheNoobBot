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
            string questInfoJsonBlockStart = "'};\n$.extend(true, g_quests, _);\n_ = g_quests;\n$.extend(g_quests[" + questId + "], ";
            const string questInfoJsonBlockEnd = ");\nvar _ = {};";
            const string questInfoJsBlockStart = "<div id=\"infobox-contents0\"></div>";
            const string questInfoJsBlockEnd = "'infobox-contents0'";
            const string questCoreInfoJsBlockStart = "ext/javascript\">\n\t\t\tMarkup.printHtml('\\";
            const string questCoreInfoJsBlockEnd = @"', ";
            string greyLevelColorStart = @"color\x3Dr4\x5D";
            const string greyLevelColorEnd = @"\x5B\";
            const string questPickUpStart = @"\x5DStart\x3A\x20\x5Burl\x3D\x2Fnpc\x3D";
            const string questPickUpEnd = @"\x5D\";
            const string questTurnInStart = @"\x5DEnd\x3A\x20\x5Burl\x3D\x2Fnpc\x3D";
            const string questTurnInEnd = @"\x5D\";
            int questInfoJsonBlockStartIndex = content.IndexOf(questInfoJsonBlockStart, StringComparison.Ordinal) + questInfoJsonBlockStart.Length;
            if (questInfoJsonBlockStartIndex <= 0 || questInfoJsonBlockStartIndex == questInfoJsonBlockStart.Length - 1)
            {
                Logging.WriteError("Quest " + questId + " is marked as invalid;");
                return new QuestInfo();
            }
            int questInfoJsBlockStartIndex = content.IndexOf(questInfoJsBlockStart, StringComparison.Ordinal) + questInfoJsBlockStart.Length;
            int questInfoJsonBlockEndIndex = content.IndexOf(questInfoJsonBlockEnd, questInfoJsonBlockStartIndex, StringComparison.Ordinal);
            int questInfoJsBlockEndIndex = content.IndexOf(questInfoJsBlockEnd, questInfoJsBlockStartIndex, StringComparison.Ordinal);
            string questInfoJsonResult = content.Substring(questInfoJsonBlockStartIndex, questInfoJsonBlockEndIndex - questInfoJsonBlockStartIndex);
            string questInfoJsResult = content.Substring(questInfoJsBlockStartIndex, questInfoJsBlockEndIndex - questInfoJsBlockStartIndex);
            int questCoreInfoJsBlockStartIndex = questInfoJsResult.IndexOf(questCoreInfoJsBlockStart, StringComparison.Ordinal) + questCoreInfoJsBlockStart.Length;
            int questCoreInfoJsBlockEndIndex = questInfoJsResult.IndexOf(questCoreInfoJsBlockEnd, questCoreInfoJsBlockStartIndex, StringComparison.Ordinal);
            string questCoreInfoJsBlockResult = questInfoJsResult.Substring(questCoreInfoJsBlockStartIndex, questCoreInfoJsBlockEndIndex - questCoreInfoJsBlockStartIndex);
            int greyLevelColorStartIndex = questCoreInfoJsBlockResult.IndexOf(greyLevelColorStart, StringComparison.Ordinal) + greyLevelColorStart.Length;
            int maxLevelAdjustment = -1;
            if (greyLevelColorStartIndex <= 0 || greyLevelColorStartIndex == greyLevelColorStart.Length - 1)
            {
                greyLevelColorStart = @"color\x3Dr3\x5D"; // use green instead
                greyLevelColorStartIndex = questCoreInfoJsBlockResult.IndexOf(greyLevelColorStart, StringComparison.Ordinal) + greyLevelColorStart.Length;
                maxLevelAdjustment = 1;
            }
            int greyLevelColorEndIndex = questCoreInfoJsBlockResult.IndexOf(greyLevelColorEnd, greyLevelColorStartIndex, StringComparison.Ordinal);
            ;
            string greyLevelColorResult = questCoreInfoJsBlockResult.Substring(greyLevelColorStartIndex, greyLevelColorEndIndex - greyLevelColorStartIndex);

            int questPickUpStartIndex = questCoreInfoJsBlockResult.IndexOf(questPickUpStart, StringComparison.Ordinal) + questPickUpStart.Length;
            int questPickUpEndIndex = questCoreInfoJsBlockResult.IndexOf(questPickUpEnd, questPickUpStartIndex, StringComparison.Ordinal);
            string questPickUpId = questCoreInfoJsBlockResult.Substring(questPickUpStartIndex, questPickUpEndIndex - questPickUpStartIndex);

            int questTurnInStartIndex = questCoreInfoJsBlockResult.IndexOf(questTurnInStart, StringComparison.Ordinal) + questTurnInStart.Length;
            int questTurnInEndIndex = questCoreInfoJsBlockResult.IndexOf(questTurnInEnd, questTurnInStartIndex, StringComparison.Ordinal);
            string questTurnInId = questCoreInfoJsBlockResult.Substring(questTurnInStartIndex, questTurnInEndIndex - questTurnInStartIndex);

            var qInfo = new QuestInfo();
            var qObject = JsonConvert.DeserializeObject<RootObject>(questInfoJsonResult);
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
            qInfo.ReqMaxLevel = Others.ToInt32(greyLevelColorResult) + maxLevelAdjustment;
            qInfo.Side = qObject.side;
            qInfo.Wflags = qObject.wflags;
            qInfo.Type = qObject.type;
            qInfo.XP = qObject.xp;
            qInfo.PickUp = Others.ToInt32(questPickUpId);
            qInfo.TurnIn = Others.ToInt32(questTurnInId);
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
            public int PickUp { get; set; }
            public int TurnIn { get; set; }
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