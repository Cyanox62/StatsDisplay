using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Exiled.Loader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StatsDisplay
{
	partial class EventHandlers
	{
		private EscapeData escapeData;

		private Dictionary<Player, int> scpKills = new Dictionary<Player, int>();
		private Dictionary<Player, KillData> humanKills = new Dictionary<Player, KillData>();

		private KeyValuePair<Player, int> defaultScp = default(KeyValuePair<Player, int>);
		private KeyValuePair<Player, KillData> defaultHuman = default(KeyValuePair<Player, KillData>);

		internal void OnRoundStart()
		{
			scpKills.Clear();
			humanKills.Clear();
			escapeData = null;
		}

		internal void OnEscaping(EscapingEventArgs ev)
		{
			if (escapeData == null)
			{
				TimeSpan t = TimeSpan.FromSeconds(Round.ElapsedTime.TotalSeconds);
				escapeData = new EscapeData()
				{
					playerName = ev.Player.Nickname,
					role = ev.Player.Role,
					time = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds)
				};
			}
		}

		private List<Player> TryGet035()
		{
			List<Player> scp035 = null;
			if (StatsDisplay.isScp035)
			{
				try
				{
					scp035 = (List<Player>)Loader.Plugins.First(pl => pl.Name == "scp035").Assembly.GetType("scp035.API.Scp035Data").GetMethod("GetScp035s", BindingFlags.Public | BindingFlags.Static).Invoke(null, null);
				}
				catch (Exception e)
				{
					Log.Debug("Failed getting 035s: " + e);
					scp035 = new List<Player>();
				}
			}
			else
			{
				scp035 = new List<Player>();
			}
			return scp035;
		}

		internal void OnPlayerDeath(DiedEventArgs ev)
		{
			if (ev.Handler.Type == Exiled.API.Enums.DamageType.PocketDimension)
			{
				Player scp106 = Player.List.FirstOrDefault(x => x.Role == RoleType.Scp106);
				if (scp106 != null)
				{
					IncrementScpKill(scp106);
				}
			}

			if (ev.Killer.Id == ev.Target.Id) return;

			Player scp035 = null;
			if (StatsDisplay.isScp035)
			{
				scp035 = TryGet035().FirstOrDefault();
			}

			if (ev.Killer.Role.Team == Team.SCP || ev.Killer.Id == scp035?.Id)
			{
				IncrementScpKill(ev.Killer);
			}
			else if (ev.Target.Role.Team == Team.SCP || ev.Target.Id == scp035?.Id)
			{
				if (!humanKills.ContainsKey(ev.Killer)) humanKills.Add(ev.Killer, new KillData()
				{
					kills = 1,
					lastRole = ev.Killer.Role
				});
				else
				{
					humanKills[ev.Killer].kills++;
					humanKills[ev.Killer].lastRole = ev.Killer.Role;
				}
			}
		}

		internal void OnRoundEnd(RoundEndedEventArgs ev)
		{
			scpKills.OrderByDescending(key => key.Value);
			humanKills.OrderByDescending(key => key.Value);

			KeyValuePair<Player, int> scpTop = scpKills.FirstOrDefault();
			KeyValuePair<Player, KillData> humanTop = humanKills.FirstOrDefault();

			Map.Broadcast(15,
				$"{(scpTop.Equals(defaultScp) ? "<i>SCPs failed to obtain a kill.</i>" : $"<b><color=#FF0000>{scpTop.Key.Nickname}</color></b> <i>killed the most humans with</i> <b>{scpTop.Value} kill{(scpTop.Value > 1 ? "s" : "")}!</b>")}\n" +
				$"{(humanTop.Equals(defaultHuman) ? "<i>Nobody was able to contain any SCPs.</i>" : $"<b><color=#{GetColor(humanTop.Value.lastRole)}>{humanTop.Key.Nickname}</color></b> <i>killed the most SCPs with</i> <b>{humanTop.Value.kills} kill{(humanTop.Value.kills > 1 ? "s" : "")}!</b>")}\n" +
				$"{(escapeData == null ? "<i>Nobody was able to escape.</i>" : $"<b><color=#{GetColor(escapeData.role)}>{escapeData.playerName}</color></b> <i>escaped first at</i> <b>{escapeData.time}!</b>")}");
		}
	}
}
