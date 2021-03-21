using Exiled.API.Features;
using Exiled.Loader;

namespace StatsDisplay
{
	public class StatsDisplay : Plugin<Config>
	{
		internal static StatsDisplay singleton;
		private EventHandlers ev;

		internal static bool isScp035 = false;

		public override void OnEnabled()
		{
			base.OnEnabled();

			singleton = this;
			ev = new EventHandlers();

			Check035();

			Exiled.Events.Handlers.Server.RoundStarted += ev.OnRoundStart;
			Exiled.Events.Handlers.Player.Escaping += ev.OnEscaping;
			Exiled.Events.Handlers.Player.Died += ev.OnPlayerDeath;
			Exiled.Events.Handlers.Server.RoundEnded += ev.OnRoundEnd;
		}

		public override void OnDisabled()
		{
			base.OnDisabled();

			Exiled.Events.Handlers.Server.RoundStarted -= ev.OnRoundStart;
			Exiled.Events.Handlers.Player.Escaping -= ev.OnEscaping;
			Exiled.Events.Handlers.Player.Died -= ev.OnPlayerDeath;
			Exiled.Events.Handlers.Server.RoundEnded -= ev.OnRoundEnd;

			ev = null;
		}

		public override string Name => "StatsDisplay";
		public override string Author => "Cyanox";

		internal void Check035()
		{
			foreach (var plugin in Loader.Plugins)
			{
				if (plugin.Name == "scp035")
				{
					isScp035 = true;
					return;
				}
			}
		}
	}
}
