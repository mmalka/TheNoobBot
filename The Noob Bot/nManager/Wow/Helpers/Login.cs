using System;
using System.Threading;
using System.Windows.Forms;
using nManager.Helpful;
using Timer = nManager.Helpful.Timer;

namespace nManager.Wow.Helpers
{
    public class Login
    {
        public struct SettingsLogin
        {
            public string Realm;
            public string Login;
            public string Character;
            public string Password;
            public string BNetName;

            public SettingsLogin(string realm, string login, string character, string password, string bNetName)
                : this()
            {
                try
                {
                    Realm = realm;
                    Login = login;
                    Character = character;
                    Password = password;
                    BNetName = bNetName;
                }
                catch (Exception exception)
                {
                    Logging.WriteError(
                        "SettingsLogin(string realm, string login, string character, string password, string bNetName): " +
                        exception);
                }
            }
        }

        /*
        private static int _lastUse;
        private static string _randomString = "";
        public static void PulseOld(SettingsLogin settings)
        {
            try
            {
                if (_randomString == "")
                    _randomString = Others.GetRandomString(Others.Random(4, 10));

                if (_lastUse + 5000 > Others.Times)
                    return;

                _lastUse = Others.Times;

                if (Memory.WowMemory.Memory.ReadInt(Memory.WowProcess.WowModule + (uint)Addresses.Login.battlerNetWindow) == 1) // Player Select bn account
                {
                    Keyboard.PressKey(Memory.WowProcess.MainWindowHandle, Keys.Enter);
                    return;
                }
                if (Memory.WowMemory.Memory.ReadUTF8String(Memory.WowProcess.WowModule + (uint)Addresses.GameInfo.consoleExecValue) == _randomString)
                {
                    Lua.LuaDoString(" ConsoleExec(\"" + Others.GetRandomString(Others.Random(1, 4)) + "\") ", true);
                    Keyboard.PressKey(Memory.WowProcess.MainWindowHandle, Keys.Enter);
                    return;
                }

                string t = "";
                foreach (var b in Encoding.UTF8.GetBytes(settings.Character))
                {
                    if (t != "")
                        t = t + ", ";
                    t = t + Convert.ToInt32(b);
                }
                string playerName = "string.char(" + t + ")";

                t = "";
                foreach (var b in Encoding.UTF8.GetBytes(settings.Realm))
                {
                    if (t != "")
                        t = t + ", ";
                    t = t + Convert.ToInt32(b);
                }
                string realmName = "string.char(" + t + ")";

                t = "";
                foreach (var b in Encoding.UTF8.GetBytes(settings.Password))
                {
                    if (t != "")
                        t = t + ", ";
                    t = t + Convert.ToInt32(b);
                }
                string password = "string.char(" + t + ")";

                t = "";
                foreach (var b in Encoding.UTF8.GetBytes(settings.Login))
                {
                    if (t != "")
                        t = t + ", ";
                    t = t + Convert.ToInt32(b);
                }
                string username = "string.char(" + t + ")";

                string ss = "if ScriptErrorsFrame and ScriptErrorsFrame:IsVisible() then ScriptErrorsFrame:Hide() end " +

                            "if StaticPopup1 and StaticPopup1:IsVisible() then StaticPopup1:Hide() end " +

                            //"if WoWAccountSelectDialog then ret = WoWAccountSelectDialog:IsShown() else ret = false end " +
                    //"if ret then  " +
                    //"	for i = 1, GetNumGameAccounts() do " +
                    //"		if string.lower(GetGameAccountInfo(i)) == string.lower(\"" + settings.Account + "\") or i == " + settings.Account + " then " +
                    //"			WoWAccountSelect_SelectAccount(i) " +
                    //"			break  " +
                    //"		end  " +
                    //"	end " +					
                    //"end " +

                            "if AccountLoginUI then ret =  AccountLoginUI:IsVisible() else ret = false end " +
                            "if ret then  " +
                            "	DefaultServerLogin(" + username + ", " + password + ") " +
                            "end " +

                            "if RealmList then ret = RealmList:IsVisible() else ret = false end  " +
                            "if ret then " +
                            "	for i = 1, select('#',GetRealmCategories()) do " +
                            "		for j = 1, GetNumRealms(i) do " +
                            "			if string.lower(GetRealmInfo(i, j)) == string.lower(" + realmName + ")  then " +
                            "				RealmList:Hide() " +
                            "				ChangeRealm(i, j) " +
                            "				break " +
                            "			end " +
                            "		end " +
                            "	end " +
                            "end " +


                            "if CharacterSelectUI then ret = CharacterSelectUI:IsVisible() else ret = false end  " +
                            "if ret then " +
                            "	if string.lower(GetServerName()) == string.lower(" + realmName + ") or tonumber(" + realmName + ") ~= nil then  " +
                            "		if GetNumCharacters() > 0 then " +
                            "			for i = 1,GetNumCharacters() do  " +
                            "				if string.lower(GetCharacterInfo(i)) == string.lower(" + playerName + ") then " +
                            "					CharacterSelect_SelectCharacter(i) " +
                    //  "					EnterWorld() " +
                            "					ConsoleExec(\"" + _randomString + "\") " +
                            "					break " +
                            "				end " +
                            "			end " +
                            "		end " +
                            "	else " +
                            "		RequestRealmList(1) " +
                            "	end " +
                            "end ";

                if (!Usefuls.InGame && !Usefuls.IsLoadingOrConnecting)
                {
                    Thread.Sleep(1000);
                    Lua.LuaDoString(Others.ToUtf8(ss), true);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Login > Pulse(SettingsLogin settings): " + exception);
            }
        }
        */


        public static void StopLogin()
        {
            _login = false;
        }

        private static bool _login;

        public static bool Pulse(SettingsLogin settings)
        {
            try
            {
                _login = true;
                Timer timeout = new Timer(120000);
                while ((ObjectManager.ObjectManager.Me.GetBaseAddress == 0 || ObjectManager.ObjectManager.Me.Guid == 0L ||
                        !ObjectManager.ObjectManager.Me.IsValid) && _login)
                {
                    if (timeout.IsReady)
                    {
                        return false;
                    }
                    string loggingIn = "if (WoWAccountSelectDialog and WoWAccountSelectDialog:IsShown()) then " +
                                       "for i = 0, GetNumGameAccounts() do " +
                                       "if GetGameAccountInfo(i) == '" + settings.BNetName + "' then " +
                                       "WoWAccountSelect_SelectAccount(i) " +
                                       "end " +
                                       "end " +
                                       "elseif (AccountLoginUI and AccountLoginUI:IsVisible()) then " +
                                       "local editbox = AccountLoginPasswordEdit; editbox:SetText('" + settings.Password + "'); " +
                                       "DefaultServerLogin('" + settings.Login + "', editbox); " +
                                       "AccountLoginUI:Hide() " +
                                       "elseif (RealmList and RealmList:IsVisible()) then " +
                                       "for i = 1, select('#',GetRealmCategories()) do " +
                                       "for j = 1, GetNumRealms(i) do " +
                                       "if GetRealmInfo(i, j) == '" + settings.Realm.Replace("'", @"\'") + "' then " +
                                       "RealmList:Hide() " +
                                       "ChangeRealm(i, j) " +
                                       "end " +
                                       "end " +
                                       "end " +
                                       "end ";
                    string charLoggingIn = "if (CharacterSelectUI and CharacterSelectUI:IsVisible()) then " +
                                           "if GetServerName() ~= '" + settings.Realm.Replace("'", @"\'") +
                                           "' and (not RealmList or not RealmList:IsVisible()) then " +
                                           "RequestRealmList(1) " +
                                           "else " +
                                           "for i = 0,GetNumCharacters() do " +
                                           "if (GetCharacterInfo(i) == '" + settings.Character + "') then " +
                                           "CharacterSelect_SelectCharacter(i) " +
                                           //"EnterWorld(); " +
                                           "end " +
                                           "end " +
                                           "end " +
                                           "end ";
                    Lua.LuaDoString(loggingIn, true);
                    Thread.Sleep(5000);
                    Lua.LuaDoString(charLoggingIn, true);
                    Thread.Sleep(2500);
                    Keyboard.PressKey(Memory.WowProcess.MainWindowHandle, Keys.Enter);
                    /*if ((tickCount + 45000) < Environment.TickCount)
                    {
                        Lua.LuaDoString(Others.ToUtf8("SetCVar(\"realmName\",\"" + settings.Realm.Replace("'", @"\'") + "\"); SetCVar(\"accountList\", \"\");"), true);
                        Lua.LuaDoString(Others.ToUtf8("DefaultServerLogin(\"" + settings.Login + "\",\"" + settings.Password + "\");"), true);
                        tickCount = Environment.TickCount;
                    }
                    Lua.LuaDoString(Others.ToUtf8("if (CharacterSelectUI:IsShown()) then for i=0,GetNumCharacters() do local name = GetCharacterInfo(i); if (name ~= nil and string.lower(name) == '" + settings.Character.ToLower() + "') then CharacterSelect_SelectCharacter(i); EnterWorld(); end end end"), true);
                    Thread.Sleep(500);
                    Lua.LuaDoString(Others.ToUtf8("if (WoWAccountSelectDialog:IsShown()) then for i=0, GetNumGameAccounts() do local name = GetGameAccountInfo(i); if (name ~= nil and string.lower(name) == '" + settings.BNetName.ToLower() + "') then selectedIndex = i; WoWAccountSelect_SelectAccount(selectedIndex); WoWAccountSelect_Accept(); end end end"), true);
                    */
                    Thread.Sleep(8000);
                    Application.DoEvents();
                }
                return true;
            }
            catch (Exception exception)
            {
                Logging.WriteError("Login > Pulse(SettingsLogin settings): " + exception);
                return false;
            }
        }
    }
}