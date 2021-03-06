using System;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Object = UnityEngine.Object;
using UnityEngine;
using MEC;

namespace ZombieLunging
{
	public class EventHandlers
	{
		public Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;

		public void OnSetClass(ChangingRoleEventArgs ev)
		{
			if (ev.Player.Nickname == "Dedicated Server") return;

			if (ev.NewRole == RoleType.Scp0492)
			{
				Timing.CallDelayed(1.5f, () => {
					if (ev.Player.GameObject.TryGetComponent(out PlayerSpeeds speed))
					{
						if (plugin.Config.IsDebug) Log.Debug("Tried to add PlayerSpeeds component but player already has it.");
					}
					else
					{
						if (plugin.Config.IsDebug) { Log.Debug("Added component to player."); }
						ev.Player.GameObject.AddComponent<PlayerSpeeds>();
					}

					if (ev.Player.GameObject.TryGetComponent(out CustomZombie cz))
					{
						if (plugin.Config.IsDebug) Log.Debug("Tried to add CustomZombie component but player already has it.");
					}
					else
					{
						if (plugin.Config.IsDebug) { Log.Debug("Added component to player."); }

						ev.Player.GameObject.AddComponent<CustomZombie>();
					}
				});
			}
		}

		public void OnConsoleCommand(SendingConsoleCommandEventArgs ev)
		{
			Log.Debug("A 1");
			if (ev.Player.Role != RoleType.Scp0492) return;

			if (ev.Name.ToLower() == "zr")
			{
				Log.Debug("A 2");
				CustomZombie cz;

				if (ev.Player.GameObject.TryGetComponent(out cz))
				{
				}
				else
				{
					Log.Debug("Tried to get PlayerSpeeds component but couldn't be found.");
					cz = ev.Player.GameObject.AddComponent<CustomZombie>();
				}

				if (cz == null)
					if (cz.cooldown > 0)
					{
						if (!string.IsNullOrEmpty(Plugin.instance.Config.LungeMessage))
						{
							ev.Player.ClearBroadcasts();
							ev.Player.Broadcast(1, Plugin.instance.Config.LungeCooldownMessage.Replace("{time}", Math.Round(cz.cooldown).ToString()));

						}

						if (plugin.Config.IsDebug) Log.Info(cz.cooldown > 0);
					}
					else if (!cz.lunging)
					{
						if (!string.IsNullOrEmpty(Plugin.instance.Config.LungeMessage)) ev.Player.Broadcast(3, Plugin.instance.Config.LungeMessage);
						cz.ActivateSpeedUp();
						ev.ReturnMessage = !string.IsNullOrEmpty(Plugin.instance.Config.LungeMessage) ? Plugin.instance.Config.LungeMessage : "Has activado tu arremetimiento!";
						ev.Color = "Green";
					}
					else
					{
						ev.ReturnMessage = "Estas listo para arremeter de nuevo!";
						ev.Color = "Red";
					}
			}
		}
	}
}