using System.Threading;
using WowManager;
using WowManager.FiniteStateMachine;
using WowManager.Hardware;
using WowManager.WoW.Interface;
using WowManager.WoW.ObjectManager;
using WowManager.WoW.PlayerManager;

namespace Questing_Bot.Bot.States
{
    internal class Regeneration : State
    {
        public override string DisplayName
        {
            get { return "Regeneration"; }
        }

        public override int Priority
        {
            get { return (int)States.Priority.Regeneration; }
        }

        public override bool NeedToRun
        {
            get
            {
                if (!Useful.InGame ||
                    Useful.isLoadingOrConnecting || 
                    ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.Me.IsValid ||
                    ObjectManager.Me.InCombat ||
                    !Config.Bot.BotIsActive)
                    return false;

                if (ObjectManager.Me.IsMount)
                    return false;

                // Need Regeneration
                // Hp:
                if (ObjectManager.Me.HealthPercent <= Config.Bot.FormConfig.RegenMinHp)
                    return true;
                // Mana:
                if (ObjectManager.Me.BarTwoPercentage <= Config.Bot.FormConfig.RegenMinMp && Config.Bot.FormConfig.RegenMp)
                    return true;
                // Pet:
                if (ObjectManager.Pet.HealthPercent <= Config.Bot.FormConfig.RegenPetMinHp && ObjectManager.Pet.IsAlive && ObjectManager.Pet.IsValid && Config.Bot.FormConfig.RegenPet)
                    return true;

                return false;
            }
        }

        public override void Run()
        {
            #region Player

            try
            {
                if ((ObjectManager.Me.HealthPercent <= Config.Bot.FormConfig.RegenMinHp) || // HP
                    (ObjectManager.Me.BarTwoPercentage <= Config.Bot.FormConfig.RegenMinMp && Config.Bot.FormConfig.RegenMp)) // MANA
                {
                    Log.AddLog(Translation.GetText(Translation.Text.Regen));
                    WowManager.Navigation.MovementManager.StopMove();
                    Thread.Sleep(1000);

                    // Use food:
                    if (ObjectManager.Me.HealthPercent <= Config.Bot.FormConfig.RegenMinHp &&
                        Config.Bot.FormConfig.RegenUseFood)
                    {
                        ObjectManager.Me.forceIsCast = true;
                        string foodName = Tasks.Functions.FoodName();
                        if (foodName != string.Empty)
                        {
                            WowManager.WoW.ItemManager.Item.UseItem(foodName);
                            Thread.Sleep(500);
                        }
                    }

                    // Use Water:
                    if (ObjectManager.Me.BarTwoPercentage <= Config.Bot.FormConfig.RegenMinMp &&
                        Config.Bot.FormConfig.RegenUseWater && Config.Bot.FormConfig.RegenMp)
                    {
                        ObjectManager.Me.forceIsCast = true;
                        string waterName = Tasks.Functions.WaterName();
                        if (waterName != string.Empty)
                        {
                            WowManager.WoW.ItemManager.Item.UseItem(waterName);
                            Thread.Sleep(500);
                        }
                    }

                    if (Config.Bot.FormConfig.RegenMaxHp > 95)
                        Config.Bot.FormConfig.RegenMaxHp = 95;

                    if (Config.Bot.FormConfig.RegenMaxMp > 95)
                        Config.Bot.FormConfig.RegenMaxMp = 95;

                    while (ObjectManager.Me.HealthPercent <= Config.Bot.FormConfig.RegenMinHp && Config.Bot.BotIsActive)
                    // Wait health
                    {
                        if (ObjectManager.Me.IsDeadMe || ObjectManager.Me.InCombat)
                        {
                            ObjectManager.Me.forceIsCast = false;
                            return;
                        }
                        Thread.Sleep(500);
                    }

                    while (ObjectManager.Me.BarTwoPercentage <= Config.Bot.FormConfig.RegenMinMp && Config.Bot.FormConfig.RegenMp &&
                           Config.Bot.BotIsActive) // Wait Mana
                    {
                        if (ObjectManager.Me.IsDeadMe || ObjectManager.Me.InCombat)
                        {
                            ObjectManager.Me.forceIsCast = false;
                            return;
                        }
                        Thread.Sleep(500);
                    }
                    ObjectManager.Me.forceIsCast = false;
                    Log.AddLog(Translation.GetText(Translation.Text.Regen_finish));
                }
            }
            catch
            {
            }

            #endregion

            #region Pet

            try
            {
                if (Config.Bot.FormConfig.RegenPet && ObjectManager.Pet.IsAlive && ObjectManager.Pet.IsValid &&
                    Config.Bot.BotIsActive)
                {
                    if (ObjectManager.Pet.HealthPercent <= Config.Bot.FormConfig.RegenPetMinHp)
                    {
                        Log.AddLog(Translation.GetText(Translation.Text.Regen_Pet));
                        WowManager.Navigation.MovementManager.StopMove();
                        Thread.Sleep(1000);

                        // Use macro food
                        if (Config.Bot.FormConfig.RegenUsePetMacro)
                        {
                            Interact.InteractGameObject(ObjectManager.Pet.GetBaseAddress);
                            Thread.Sleep(500);
                            Keyboard.PressBarAndSlotKey(Config.Bot.FormConfig.PetBar + ";" + Config.Bot.FormConfig.PetSlot);
                            Thread.Sleep(500);
                        }

                        if (Config.Bot.FormConfig.RegenPetMaxHp > 95)
                            Config.Bot.FormConfig.RegenPetMaxHp = 95;

                        while (ObjectManager.Pet.HealthPercent <= Config.Bot.FormConfig.RegenPetMaxHp  && Config.Bot.BotIsActive)
                        {
                            if (!ObjectManager.Pet.IsAlive || ObjectManager.Me.IsDeadMe || ObjectManager.Me.InCombat ||
                                ObjectManager.Pet.InCombat)
                            {
                                return;
                            }
                            Thread.Sleep(500);
                        }
                        Log.AddLog(Translation.GetText(Translation.Text.Regen_Pet_finish));
                    }
                }
            }
            catch
            {
            }

            #endregion
        }
    }
}
