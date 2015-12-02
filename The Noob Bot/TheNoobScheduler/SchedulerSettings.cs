using System;
using System.Collections.Generic;

namespace TheNoobScheduler
{
    [Serializable]
    public class SchedulerSettings
    {
        public List<BattleNetAccounts> Accounts = new List<BattleNetAccounts>();
    }

    [Serializable]
    public class BattleNetAccounts
    {
        public string AccountEmail;
        public string AccountName = "WoW1";
        public string AccountPassword;
        public List<Character> Characters = new List<Character>();
        public bool IsActive;

        public int SessId;

        public bool ShouldSerializeSessId()
        {
            return false;
        }
    }

    [Serializable]
    public class Character
    {
        public string CharacterFaction;
        public string CharacterName;
        public string CharacterNote;
        public string CharacterRealm;
        public bool IsActive;
    }
}