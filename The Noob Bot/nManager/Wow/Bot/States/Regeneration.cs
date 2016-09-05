using System.Collections.Generic;
using System.Threading;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Helpers;
using Math = System.Math;
using Timer = nManager.Helpful.Timer;

namespace nManager.Wow.Bot.States
{
    public class Regeneration : State
    {
        public override string DisplayName
        {
            get { return "Regeneration"; }
        }


        public override int Priority { get; set; }

        public override List<State> NextStates
        {
            get { return new List<State>(); }
        }

        public override List<State> BeforeStates
        {
            get { return new List<State>(); }
        }

        public override bool NeedToRun
        {
            get
            {
                if (!Usefuls.InGame ||
                    Usefuls.IsLoading ||
                    ObjectManager.ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.ObjectManager.Me.IsValid ||
                    ObjectManager.ObjectManager.Me.InCombat ||
                    Usefuls.IsFlying ||
                    !Products.Products.IsStarted)
                    return false;

                if (ObjectManager.ObjectManager.Me.IsMounted)
                    return false;

                if (Math.Abs(ObjectManager.ObjectManager.Me.HealthPercent) < 0.001f)
                    return false;

                // Need Regeneration
                // Hp:
                if (ObjectManager.ObjectManager.Me.HealthPercent <=
                    nManagerSetting.CurrentSetting.EatFoodWhenHealthIsUnderXPercent)
                    return true;
                // Mana:
                if (ObjectManager.ObjectManager.Me.ManaPercentage <=
                    nManagerSetting.CurrentSetting.DrinkBeverageWhenManaIsUnderXPercent &&
                    nManagerSetting.CurrentSetting.DoRegenManaIfLow)
                    return true;

                if (CombatClass.GetLightHealingSpell.IsSpellUsable && ObjectManager.ObjectManager.Me.HealthPercent <= 85)
                    return true;
                // Pet:
                //if (ObjectManager.ObjectManager.Pet.HealthPercent <= Config.Bot.FormConfig.RegenPetMinHp && ObjectManager.ObjectManager.Pet.IsAlive && ObjectManager.ObjectManager.Pet.IsValid && Config.Bot.FormConfig.RegenPet)
                //    return true;

                return false;
            }
        }

        public override void Run()
        {
            #region Player

            try
            {
                if ((ObjectManager.ObjectManager.Me.HealthPercent <=
                     nManagerSetting.CurrentSetting.EatFoodWhenHealthIsUnderXPercent) ||
                    // HP
                    (ObjectManager.ObjectManager.Me.ManaPercentage <=
                     nManagerSetting.CurrentSetting.DrinkBeverageWhenManaIsUnderXPercent &&
                     nManagerSetting.CurrentSetting.DoRegenManaIfLow))
                    // MANA
                {
                    Logging.Write("Regen started");
                    MovementManager.StopMove();
                    Thread.Sleep(500);

                    // Use food:
                    if (ObjectManager.ObjectManager.Me.HealthPercent <=
                        nManagerSetting.CurrentSetting.EatFoodWhenHealthIsUnderXPercent &&
                        nManagerSetting.CurrentSetting.FoodName != "")
                    {
                        ObjectManager.ObjectManager.Me.forceIsCast = true;
                        ItemsManager.UseItem(nManagerSetting.CurrentSetting.FoodName);
                        Thread.Sleep(500);
                    }

                    // Use Water:
                    if (ObjectManager.ObjectManager.Me.ManaPercentage <=
                        nManagerSetting.CurrentSetting.DrinkBeverageWhenManaIsUnderXPercent &&
                        nManagerSetting.CurrentSetting.BeverageName != "" &&
                        nManagerSetting.CurrentSetting.DoRegenManaIfLow)
                    {
                        ObjectManager.ObjectManager.Me.forceIsCast = true;
                        ItemsManager.UseItem(nManagerSetting.CurrentSetting.BeverageName);
                        Thread.Sleep(500);
                    }


                    while (ObjectManager.ObjectManager.Me.HealthPercent <= 95 && Products.Products.IsStarted)
                        // Wait health
                    {
                        if (ObjectManager.ObjectManager.Me.IsDeadMe || ObjectManager.ObjectManager.Me.InCombat)
                        {
                            ObjectManager.ObjectManager.Me.forceIsCast = false;
                            return;
                        }
                        Thread.Sleep(500);
                    }

                    while (ObjectManager.ObjectManager.Me.ManaPercentage <= 95 &&
                           Products.Products.IsStarted && nManagerSetting.CurrentSetting.DoRegenManaIfLow) // Wait Mana
                    {
                        if (ObjectManager.ObjectManager.Me.IsDeadMe || ObjectManager.ObjectManager.Me.InCombat)
                        {
                            ObjectManager.ObjectManager.Me.forceIsCast = false;
                            return;
                        }
                        Thread.Sleep(500);
                    }
                    ObjectManager.ObjectManager.Me.forceIsCast = false;
                    Logging.Write("Regen finished");
                }
                if (CombatClass.GetLightHealingSpell.KnownSpell && ObjectManager.ObjectManager.Me.HealthPercent <= 85)
                {
                    Logging.Write("Regeneration started with Light Heal started.");
                    MovementManager.StopMove();
                    Thread.Sleep(500);
                    var timer = new Timer(10000);
                    while (CombatClass.GetLightHealingSpell.IsSpellUsable && ObjectManager.ObjectManager.Me.HealthPercent <= 85)
                    {
                        if (Math.Abs(ObjectManager.ObjectManager.Me.HealthPercent) < 0.001f)
                            return;
                        if (timer.IsReady)
                            break;
                        CombatClass.GetLightHealingSpell.CastOnSelf(true);
                        Thread.Sleep(100);
                    }

                    Logging.Write(ObjectManager.ObjectManager.Me.HealthPercent > 85 ? "Regeneration terminated with success" : "Regeneration terminated by timer");
                }
            }
            catch
            {
            }

            #endregion

            /*
            #region Pet

            try
            {
                if (Config.Bot.FormConfig.RegenPet && ObjectManager.ObjectManager.Pet.IsAlive && ObjectManager.ObjectManager.Pet.IsValid &&
                    Products.Products.IsStarted)
                {
                    if (ObjectManager.ObjectManager.Pet.HealthPercent <= Config.Bot.FormConfig.RegenPetMinHp)
                    {
                        Logging.Write("Regen Pet");
                        nManager.Navigation.MovementManager.StopMove();
                        Thread.Sleep(1000);

                        // Use macro food
                        if (Config.Bot.FormConfig.RegenUsePetMacro)
                        {
                            Interact.InteractGameObject(ObjectManager.ObjectManager.Pet.GetBaseAddress);
                            Thread.Sleep(500);
                            Keyboard.PressBarAndSlotKey(Config.Bot.FormConfig.PetBar + ";" + Config.Bot.FormConfig.PetSlot);
                            Thread.Sleep(500);
                        }

                        while (ObjectManager.ObjectManager.Pet.HealthPercent <= Config.Bot.FormConfig.RegenPetMaxHp  && Products.Products.IsStarted)
                        {
                            if (!ObjectManager.ObjectManager.Pet.IsAlive || ObjectManager.ObjectManager.Me.IsDeadMe || ObjectManager.ObjectManager.Me.InCombat ||
                                ObjectManager.ObjectManager.Pet.InCombat)
                            {
                                return;
                            }
                            Thread.Sleep(500);
                        }
                        Logging.Write("Regen Pet finished");
                    }
                }
            }
            catch
            {
            }

            #endregion
             * */
        }
    }
}