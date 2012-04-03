


namespace Questing_Bot
{
    class ClassTeste
    {
        //X="2505.115" Y="-5563.931" Z="420.6445"
        public static void ClassTestel()
        {
  
            WowManager.WoW.PlayerManager.ClickToMove.CGPlayer_C__ClickToMove(1654.104f, -5996.521f, 183.0229f, 0, (int)WowManager.MiscEnums.ClickToMoveType.Move, 0.5f);
            System.Threading.Thread.Sleep(1000);
            while (WowManager.WoW.PlayerManager.ClickToMove.GetClickToMoveTypePush() == WowManager.MiscEnums.ClickToMoveType.Move)
            {
                System.Threading.Thread.Sleep(100);
            }
            var spell = new WowManager.MiscStructs.Spell(52694);
            System.Threading.Thread.Sleep(5000);
            spell.Launch();
            System.Threading.Thread.Sleep(7000);

            WowManager.WoW.PlayerManager.ClickToMove.CGPlayer_C__ClickToMove(1799.286f, -6003.341f, 170.4593f, 0, (int)WowManager.MiscEnums.ClickToMoveType.Move, 0.5f);
            System.Threading.Thread.Sleep(1000);
            while (WowManager.WoW.PlayerManager.ClickToMove.GetClickToMoveTypePush() == WowManager.MiscEnums.ClickToMoveType.Move)
            {
                System.Threading.Thread.Sleep(100);
            }
            System.Threading.Thread.Sleep(5000);
            spell.Launch();
            System.Threading.Thread.Sleep(7000);

            WowManager.WoW.PlayerManager.ClickToMove.CGPlayer_C__ClickToMove(1592.047f, -5735.208f, 196.1772f, 0, (int)WowManager.MiscEnums.ClickToMoveType.Move, 0.5f);
            System.Threading.Thread.Sleep(1000);
            while (WowManager.WoW.PlayerManager.ClickToMove.GetClickToMoveTypePush() == WowManager.MiscEnums.ClickToMoveType.Move)
            {
                System.Threading.Thread.Sleep(100);
            }
            System.Threading.Thread.Sleep(5000);
            spell.Launch();
            System.Threading.Thread.Sleep(7000);

            WowManager.WoW.PlayerManager.ClickToMove.CGPlayer_C__ClickToMove(1384.774f, -5701.124f, 199.2797f, 0, (int)WowManager.MiscEnums.ClickToMoveType.Move, 0.5f);
            System.Threading.Thread.Sleep(1000);
            while (WowManager.WoW.PlayerManager.ClickToMove.GetClickToMoveTypePush() == WowManager.MiscEnums.ClickToMoveType.Move)
            {
                System.Threading.Thread.Sleep(100);
            }
            System.Threading.Thread.Sleep(5000);
            spell.Launch();
            System.Threading.Thread.Sleep(7000);

            WowManager.WoW.Useful.Keybindings.PressKeybindings(WowManager.MiscEnums.Keybindings.ACTIONBUTTON5);
            System.Threading.Thread.Sleep(50*1000);

            Questing_Bot.API.Useful.DisableRepairAndVendor = true;
            Questing_Bot.API.Useful.DisableLoot = true;

        }
    }
}
