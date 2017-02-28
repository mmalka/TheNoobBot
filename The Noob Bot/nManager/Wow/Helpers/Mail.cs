using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using nManager.Helpful;

namespace nManager.Wow.Helpers
{
    public class Mail
    {
        public static void SendMessage(string target, string titleMsg, string txtMsg, List<String> itemSend,
            List<string> itemNoSend, List<Enums.WoWItemQuality> itemQuality,
            out bool mailSendingCompleted)
        {
            try
            {
                string syntaxSellItem = itemSend.Aggregate("", (current, s) => current + " or namei == \"" + s + "\" ");

                string syntaxQualityItem = itemQuality.Aggregate(" 1 == 2 ",
                    (current, s) => current + " or r == " + (uint) s + " ");

                string syntaxNoSellItem = "";
                string syntaxNoSellItemEnd = "";
                if (itemNoSend.Count > 0)
                {
                    syntaxNoSellItemEnd = " end ";
                    syntaxNoSellItem = itemNoSend.Aggregate(" if ",
                        (current, s) => current + " and namei ~= \"" + s + "\" ");
                    syntaxNoSellItem = syntaxNoSellItem.Replace("if  and", "if ");
                    syntaxNoSellItem = syntaxNoSellItem + " then ";
                }

                string scriptLua = "";


                scriptLua = scriptLua + "MailFrameTab_OnClick(0,2) ";
                scriptLua = scriptLua + "local c,l,r,_=0 ";

                scriptLua = scriptLua + "for b=0,4 do ";
                scriptLua = scriptLua + "for s=1,40 do  ";
                scriptLua = scriptLua + "local l=GetContainerItemLink(b,s) ";
                scriptLua = scriptLua + "if l then namei,_,r=GetItemInfo(l) ";
                scriptLua = scriptLua + "if " + syntaxQualityItem + " " + syntaxSellItem + " then ";
                scriptLua = scriptLua + syntaxNoSellItem;
                scriptLua = scriptLua + "UseContainerItem(b,s) ";
                scriptLua = scriptLua + syntaxNoSellItemEnd;
                scriptLua = scriptLua + " end ";
                scriptLua = scriptLua + "end ";
                scriptLua = scriptLua + "end ";
                scriptLua = scriptLua + "end ";

                scriptLua = scriptLua +
                            " numAttachments = 0; for i=1, ATTACHMENTS_MAX_SEND do local itemName, itemTexture, stackCount, quality = GetSendMailItem(i); if itemName then numAttachments = numAttachments + 1; end end ";

                scriptLua = scriptLua + "if numAttachments>0 then ";
                if (titleMsg != "" && txtMsg != "")
                    scriptLua = scriptLua + "SendMail(\"" + target + "\", \"" + titleMsg + "\", \"" + txtMsg + "\") ";
                else
                    scriptLua = scriptLua + "SendMail(\"" + target + "\", \"" + titleMsg + " \", \"" + titleMsg +
                                " \") ";

                scriptLua = scriptLua + "end ";

                mailSendingCompleted = Others.ToInt32(Lua.LuaDoString(scriptLua, "numAttachments")) <= 0;
                Thread.Sleep(Usefuls.Latency + 1000);
                if (Others.IsFrameVisible("SecureTransferDialog"))
                {
                    Thread.Sleep(2000);
                    Lua.LuaDoString("SecureCapsuleGet('C_SecureTransfer').SendMail();");
                    Lua.LuaDoString("SecureTransferDialog:Hide();");
                    Thread.Sleep(Usefuls.Latency + 500);
                }
            }
            catch (Exception exception)
            {
                mailSendingCompleted = true;
                Logging.WriteError(
                    "Mail > SendMessage(string target, string titleMsg, string txtMsg, List<String> itemSend, List<string> itemNoSend, List<Enums.WoWItemQuality> itemQuality): " +
                    exception);
            }
        }

        public static int GetNumAttachments()
        {
            try
            {
                return
                    Others.ToInt32(
                        Lua.LuaDoString(
                            "numAttachments = 0; for i=1, ATTACHMENTS_MAX_SEND do local itemName, itemTexture, stackCount, quality = GetSendMailItem(i); if itemName then numAttachments = numAttachments + 1; end end",
                            "numAttachments"));
            }
            catch (Exception exception)
            {
                Logging.WriteError("Mail > GetNumAttachments(): " + exception);
                return 0;
            }
        }
    }
}