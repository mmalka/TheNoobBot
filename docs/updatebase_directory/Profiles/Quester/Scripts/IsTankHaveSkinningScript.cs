if (Skill.GetValue(SkillLine.Skinning) <= 0)
  return false;
switch (ObjectManager.Me.WowSpecialization())
{
  case WoWSpecialization.PaladinProtection:
  case WoWSpecialization.WarriorProtection:
  case WoWSpecialization.MonkBrewmaster:
  case WoWSpecialization.DruidGuardian:
  case WoWSpecialization.DemonHunterVengeance:
  case WoWSpecialization.DeathknightBlood:
	return true;
  default:
	return false;
}

// Usage: <ScriptCondition>=IsTankHaveSkinningScript.cs</ScriptCondition>