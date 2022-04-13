using Exiled.API.Features;

namespace StatsDisplay
{
	partial class EventHandlers
	{
		private void IncrementScpKill(Player player)
		{
			if (!scpKills.ContainsKey(player)) scpKills.Add(player, 1);
			else scpKills[player]++;
		}

		private string GetColor(RoleType role)
		{
			switch (role)
			{
				case RoleType.ChaosConscript:
					return "03811a";
				case RoleType.ChaosMarauder:
					return "045d22";
				case RoleType.ChaosRepressor:
					return "0c7732";
				case RoleType.ChaosRifleman:
					return "07771a";
				case RoleType.ClassD:
					return "ffae00";
				case RoleType.FacilityGuard:
					return "bfbfbf";
				case RoleType.NtfPrivate:
					return "6ab9f1";
				case RoleType.NtfCaptain:
					return "003dcb";
				case RoleType.NtfSergeant:
					return "058df1";
				case RoleType.NtfSpecialist:
					return "0390f5";
				case RoleType.Scientist:
					return "ffff7c";
				case RoleType.Scp049:
				case RoleType.Scp0492:
				case RoleType.Scp079:
				case RoleType.Scp096:
				case RoleType.Scp106:
				case RoleType.Scp173:
				case RoleType.Scp93953:
				case RoleType.Scp93989:
					return "ff0000";
				case RoleType.Tutorial:
					return "00ff58";
				default:
					return "ffffff";
			}
		}
	}
}
