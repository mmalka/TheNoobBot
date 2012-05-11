using System.Collections.Generic;
using System.Threading;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Helpers;

namespace nManager.Wow.Bot.States
{
    public class Regeneration : State
    {
        public override string DisplayName
        {
            get { return "Regeneration"; }
        }


        public override int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        private int _priority;

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
                    Usefuls.IsLoadingOrConnecting ||
                    ObjectManager.ObjectManager.Me.IsDeadMe ||
                    !ObjectManager.ObjectManager.Me.IsValid ||
                    ObjectManager.ObjectManager.Me.InCombat ||
                    Usefuls.IsFlying ||
                    !Products.Products.IsStarted)
                    return false;

                if (ObjectManager.ObjectManager.Me.IsMounted)
                    return false;

                // Need Regeneration
                // Hp:
                if (ObjectManager.ObjectManager.Me.HealthPercent <= nManagerSetting.CurrentSetting.foodPercent)
                    return true;
                // Mana:
                if (ObjectManager.ObjectManager.Me.BarTwoPercentage <= nManagerSetting.CurrentSetting.drinkPercent && nManagerSetting.CurrentSetting.restingMana)
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
                if ((ObjectManager.ObjectManager.Me.HealthPercent <= nManagerSetting.CurrentSetting.foodPercent) ||
                    // HP
                    (ObjectManager.ObjectManager.Me.BarTwoPercentage <= nManagerSetting.CurrentSetting.drinkPercent && nManagerSetting.CurrentSetting.restingMana))
                    // MANA
                {
                    Logging.Write("Regen started");
                    MovementManager.StopMove();
                    Thread.Sleep(500);

                    // Use food:
                    if (ObjectManager.ObjectManager.Me.HealthPercent <= nManagerSetting.CurrentSetting.foodPercent &&
                        nManagerSetting.CurrentSetting.foodName != "")
                    {
                        ObjectManager.ObjectManager.Me.forceIsCast = true;
                        ItemsManager.UseItem(nManagerSetting.CurrentSetting.foodName);
                        Thread.Sleep(500);
                    }

                    // Use Water:
                    if (ObjectManager.ObjectManager.Me.BarTwoPercentage <= nManagerSetting.CurrentSetting.drinkPercent &&
                        nManagerSetting.CurrentSetting.drinkName != "" && nManagerSetting.CurrentSetting.restingMana)
                    {
                        ObjectManager.ObjectManager.Me.forceIsCast = true;
                        ItemsManager.UseItem(nManagerSetting.CurrentSetting.drinkName);
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

                    while (ObjectManager.ObjectManager.Me.BarTwoPercentage <= 95 &&
                           Products.Products.IsStarted && nManagerSetting.CurrentSetting.restingMana) // Wait Mana
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
                            Interact.InteractGameObjectBeta23(ObjectManager.ObjectManager.Pet.GetBaseAddress);
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
