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
				case RoleType.ChaosInsurgency:
					return "008f1c";
				case RoleType.ClassD:
					return "ffae00";
				case RoleType.FacilityGuard:
					return "bfbfbf";
				case RoleType.NtfCadet:
					return "6ab9f1";
				case RoleType.NtfCommander:
					return "003dcb";
				case RoleType.NtfLieutenant:
					return "058df1";
				case RoleType.NtfScientist:
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
