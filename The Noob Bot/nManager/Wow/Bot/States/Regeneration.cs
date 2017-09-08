using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using nManager.FiniteStateMachine;
using nManager.Helpful;
using nManager.Wow.Bot.Tasks;
using nManager.Wow.Class;
using nManager.Wow.Enums;
using nManager.Wow.Helpers;
using nManager.Wow.ObjectManager;
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
                if (!Usefuls.InGame || Usefuls.IsLoading || ObjectManager.ObjectManager.Me.IsDeadMe || !ObjectManager.ObjectManager.Me.IsValid ||
                    ObjectManager.ObjectManager.Me.InCombat || Usefuls.IsFlying || !Products.Products.IsStarted)
                    return false;

                if (!nManagerSetting.CurrentSetting.ActivateRegenerationSystem)
                    return false;

                if (Math.Abs(ObjectManager.ObjectManager.Me.HealthPercent) < 0.001f)
                    return false; // Workarround for the "0% HP" issue.

                if (ObjectManager.ObjectManager.Me.HealthPercent > 85)
                    return false;

                if (!Usefuls.IsSwimming)
                {
                    if (!string.IsNullOrEmpty(nManagerSetting.CurrentSetting.FoodName) && ItemsManager.GetItemCount(nManagerSetting.CurrentSetting.FoodName) > 0)
                        if (ObjectManager.ObjectManager.Me.HealthPercent <= nManagerSetting.CurrentSetting.EatFoodWhenHealthIsUnderXPercent)
                            return true;

                    if (nManagerSetting.CurrentSetting.DoRegenManaIfLow && !string.IsNullOrEmpty(nManagerSetting.CurrentSetting.BeverageName) &&
                        ItemsManager.GetItemCount(nManagerSetting.CurrentSetting.BeverageName) > 0)
                        if (ObjectManager.ObjectManager.Me.ManaPercentage <= nManagerSetting.CurrentSetting.DrinkBeverageWhenManaIsUnderXPercent)
                            return true;
                    if (ObjectManager.ObjectManager.Me.WowClass == WoWClass.Mage)
                    {
                        if (_mageConjureSpell == null || ObjectManager.ObjectManager.Me.Level >= 13 && !_mageConjureSpell.KnownSpell)
                            _mageConjureSpell = new Spell("Conjure Refreshment");
                        if (_mageConjureSpell.KnownSpell)
                            return true;
                    }
                }
                return CombatClass.GetLightHealingSpell != null && CombatClass.GetLightHealingSpell.IsSpellUsable;
            }
        }


        public void EatOrDrink(string itemName, bool isMana = false)
        {
            try
            {
                if (FishingTask.IsLaunched)
                    FishingTask.StopLoopFish();
                // isMana = false => Health
                if (ObjectManager.ObjectManager.Me.IsMounted)
                    Usefuls.DisMount();
                MovementManager.StopMove();
                Thread.Sleep(500);

                ObjectManager.ObjectManager.Me.ForceIsCasting = true; // Make the bot believe it's casting something. (we set it to false in Finally())
                ItemsManager.UseItem(itemName);
                for (int i = 0; i < 30; i++)
                {
                    if (ObjectManager.ObjectManager.Me.IsDeadMe || ObjectManager.ObjectManager.Me.InCombat)
                        return;
                    if (!isMana && ObjectManager.ObjectManager.Me.HealthPercent > 95 || isMana && ObjectManager.ObjectManager.Me.ManaPercentage > 95)
                        break;
                    Thread.Sleep(500);
                    // Eat/Drink for 15 seconds or until we get to high HP/Mana
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("public void EatOrDrink(string itemName, bool isMana = false): " + e);
            }
            finally
            {
                ObjectManager.ObjectManager.Me.ForceIsCasting = false;
                // In case we set it to true and crash, prevent the bot from believing that its constantly casting.
            }
        }

        private Spell _mageConjureSpell;
        private int _mageConjureCount;
        private bool _mageConjureActive = true;
        private List<int> _mageFoodList = new List<int> {65499, 43523, 43518, 65517, 65516, 65515, 65500, 80610, 113509};

        private int GetBestMageFoodInBags()
        {
            List<WoWItem> items = ItemsManager.GetAllFoodItems();
            List<int> foodAvailable = new List<int>();
            foreach (var item in items)
            {
                if (_mageFoodList.Contains(item.Entry))
                    foodAvailable.Add(item.Entry);
            }
            int targetItem = 0;
            foreach (var i in _mageFoodList.Where(foodAvailable.Contains))
            {
                targetItem = i; // get the highest level food as the list is ordered.
                break;
            }
            return targetItem;
        }

        public override void Run()
        {
            #region Player

            try
            {
                if (!string.IsNullOrEmpty(nManagerSetting.CurrentSetting.FoodName) && ItemsManager.GetItemCount(nManagerSetting.CurrentSetting.FoodName) > 0)
                {
                    if (ObjectManager.ObjectManager.Me.HealthPercent <= nManagerSetting.CurrentSetting.EatFoodWhenHealthIsUnderXPercent)
                    {
                        Logging.Write("Health regeneration started: Food mode");
                        EatOrDrink(nManagerSetting.CurrentSetting.FoodName);
                        Logging.Write("Health regeneration done: Food mode");
                    }
                }

                // We check for Mana -after- we eat food, to make sure we still need it since most food now gives mana too.
                if (!string.IsNullOrEmpty(nManagerSetting.CurrentSetting.BeverageName) && ItemsManager.GetItemCount(nManagerSetting.CurrentSetting.BeverageName) > 0)
                {
                    if (nManagerSetting.CurrentSetting.DoRegenManaIfLow && ObjectManager.ObjectManager.Me.ManaPercentage <= nManagerSetting.CurrentSetting.DrinkBeverageWhenManaIsUnderXPercent)
                    {
                        Logging.Write("Mana regeneration started: Beverage mode");
                        EatOrDrink(nManagerSetting.CurrentSetting.BeverageName, true);
                        Logging.Write("Mana regeneration done: Beverage mode");
                    }
                }

                if (ObjectManager.ObjectManager.Me.WowClass == WoWClass.Mage && _mageConjureActive)
                {
                    int targetItem = GetBestMageFoodInBags();
                    while (targetItem <= 0)
                    {
                        if (!NeedToRun)
                            return;
                        if (_mageConjureSpell.IsSpellUsable)
                        {
                            _mageConjureCount++;
                            _mageConjureSpell.Cast(true);
                            Thread.Sleep(300);
                        }
                        Thread.Sleep(1000);
                        targetItem = GetBestMageFoodInBags();
                        if (_mageConjureCount > 4)
                            _mageConjureActive = false;
                        if (ObjectManager.ObjectManager.Me.InInevitableCombat)
                            return;
                    }
                    if (targetItem > 0)
                    {
                        _mageConjureCount = 0;
                        Logging.Write("Health regeneration started: Mage Food mode");
                        EatOrDrink(ItemsManager.GetItemNameById(targetItem));
                        Logging.Write("Health regeneration done: Mage Food mode");
                    }
                }

                if (CombatClass.GetLightHealingSpell == null)
                    return;
                // In case we did not use food/beverage, we check if we need to heal ourself.
                if (CombatClass.GetLightHealingSpell.IsSpellUsable && NeedToRun)
                {
                    Logging.Write("Regeneration started: Light Heal mode");
                    if (ObjectManager.ObjectManager.Me.IsMounted)
                        Usefuls.DisMount();
                    MovementManager.StopMove();
                    Thread.Sleep(500);
                    var timer = new Timer(10000);
                    while (CombatClass.GetLightHealingSpell.IsSpellUsable && NeedToRun)
                    {
                        Thread.Sleep(250);
                        if (ObjectManager.ObjectManager.Me.IsCasting)
                            continue;
                        if (Math.Abs(ObjectManager.ObjectManager.Me.HealthPercent) < 0.001f)
                            return;
                        if (ObjectManager.ObjectManager.Me.InCombat || ObjectManager.ObjectManager.Me.IsDeadMe)
                            return;
                        if (timer.IsReady)
                            break;
                        CombatClass.GetLightHealingSpell.CastOnSelf(true);
                    }

                    Logging.Write(!NeedToRun ? "Regeneration done (success): Light Heal mode" : "Regeneration done (timer): Light Heal mode");
                }
            }
            catch (Exception e)
            {
                Logging.WriteError("Regeneration.Run(): " + e);
            }

            #endregion
        }
    }
}