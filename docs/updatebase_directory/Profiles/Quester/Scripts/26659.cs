
            
                if (questObjective.ExtraObject1 is WoWUnit)
                {
                    _CurrentRock = questObjective.ExtraObject1 as WoWUnit;
                }

                if (questObjective.ExtraObject2 is WoWUnit)
                {
                    _LastRock = questObjective.ExtraObject2 as WoWUnit;
                }

                
                if (ObjectManager.Me.InTransport)
                {
                    if (ObjectManager.Me.Position.Z < 254) //>254 is when we are on the rock at the top to start killing it
                    {
                        Logging.Write("Climb Up");
                        ClimbUp();
                        return false;
                    }

                    if (HasBreathAura) //We have Breath Aura, change rock
                    {
                        Logging.Write("Changing Rock, Breath");
                        ChangeRock();
                        Thread.Sleep(2000);
                    }

                    if (GetDragon.IsValid && nManager.Wow.Helpers.CombatClass.InRange(GetDragon))
                    {
                        Logging.Write("DPS Mob");
                        _worker2 = new System.Threading.Thread(() => nManager.Wow.Helpers.Fight.StartFight(GetDragon.Guid));
                        _worker2.Start();

                        while (!HasBreathAura && !ObjectManager.Me.IsDeadMe && ObjectManager.Me.InCombat && !(nManager.Wow.Helpers.Quest.GetLogQuestIsComplete(26659) || nManager.Wow.Helpers.Quest.GetQuestCompleted(26659)))
                        {
                            Thread.Sleep(1000);
                        }

                        //We got aura, Stop Fighting
                        _worker2 = null;
                        Fight.StopFight();
                     

                        Logging.Write("We have breath, goto changing rock");

                        return false;
                    }
					else{
						Logging.Write("TROP LOIN");
					}
                }
            } //Close the try here
            catch (Exception e)
            {
                Logging.WriteError("Script: " + e);
            }

            finally
            {
                //Save current Level inside the ExtraInt field


                questObjective.ExtraObject1 = _CurrentRock;
                questObjective.ExtraObject2 = _LastRock;
            }
          return false;
		} 

        WoWUnit _CurrentRock = new WoWUnit(0);
        WoWUnit _LastRock = new WoWUnit(0);
        private int _DragonId = 43641;
        private uint _BreathAuraId = 84448;
        System.Threading.Thread _worker2;

        private bool HasBreathAura
        {
            get { return ObjectManager.Me.UnitAura(_BreathAuraId).IsValid; }
        }

        private WoWUnit GetDragon
        {
            get { return ObjectManager.GetWoWUnitByEntry(_DragonId, false).FirstOrDefault(); }
        }

        void ClimbUp()
        {
			Logging.Write("Getting Next Rock Going up");
            WoWUnit nextRock = GetNextRockToGetUp();
			Logging.Write("Next rock IsValid" + nextRock.IsValid + " Dist"  + nextRock.GetDistance);
            Interact.InteractWith(nextRock.GetBaseAddress);
            _CurrentRock = nextRock;
            Thread.Sleep(1500);
        }

        void ChangeRock()
        {
			Logging.Write("Getting Next Rock");
            WoWUnit nextRock = _CurrentRock;
            Interact.InteractWith(GetNextRock().GetBaseAddress);
            _LastRock = _CurrentRock;
            _CurrentRock = nextRock;
            Thread.Sleep(1500);
        }

        private WoWUnit GetNextRock()
        {
			List<WoWUnit> _rockList = new List<WoWUnit>();
			try{
          

            _rockList =
                nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitByEntry(45191).FindAll(x => x.IsValid && x.GetDistance <= 24 && x.GetDistance >= 5  && x != _CurrentRock && x != _LastRock).OrderBy(x => x.Position.DistanceTo(GetDragon.Position)).ToList();
			Logging.Write("Distance Char : " + _rockList[0].GetDistance + " Distance Dragon :" + GetDragon.Position.DistanceTo(_rockList[0].Position));
			
			
            return _rockList[0];
			}
			catch(Exception e)
				{
					Logging.Write("ERREUR" + e.Message);
				}
				return _rockList[0];
        }

        private WoWUnit GetNextRockToGetUp()
        {
			List<WoWUnit> _rockList = new List<WoWUnit>();
			try{
            

            _rockList = nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitByEntry(45191).FindAll(x => x.Position.Z > ObjectManager.Me.Position.Z && x.Position.Z < 254.8 && x.GetDistance <= 24).OrderByDescending(x => x.GetDistance).ToList();
			

            return _rockList[0];
			}
			catch(Exception e )
				{
					Logging.Write("ERREUR" + e.Message);
				}
				return _rockList[0];
        }
    


      public static bool random() //not used, just to close the script
        {
	        try //Bim Try open random !!!
	        {