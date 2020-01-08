using System.Collections.Generic;
using Grenades;
using MEC;

namespace SamplePlugin
{
	public class EventHandlers
	{
		public Plugin plugin;
		public EventHandlers(Plugin plugin) => this.plugin = plugin;

		public void OnRoundStart()
		{
			foreach (ReferenceHub hub in Plugin.GetHubs())
			{
				//Since this event fires before everyone has initially spawned, you need to wait before doing things like changing their health, adding items, etc
				Timing.RunCoroutine(GiveBall(hub));
			}
		}

		public IEnumerator<float> GiveBall(ReferenceHub hub)
		{
			//Wait 3 seconds to make sure everyone is spawned in correctly
			yield return Timing.WaitForSeconds(3f);
			//Give everybody 5 balls
			for (int i = 0; i < 5; i ++)
				hub.inventory.AddNewItem(ItemType.SCP018);
		}

		public void OnRoundEnd()
		{
			//Broadcast a message to all players when the round ends
			foreach (ReferenceHub hub in Plugin.GetHubs())
				hub.Broadcast(5, "Thanks for playing on the server!");
		}

		public void OnGrenadeThrown(ref GrenadeManager gm, ref int id, ref bool slow, ref double fuse, ref bool allow)
		{
			if (gm.inv.curItem == ItemType.SCP018)
			{
				//This hurts a player for 10 damage anytime they throw a ball
				ReferenceHub hub = Plugin.GetPlayer(gm.ccm.gameObject);
				hub.playerStats.HurtPlayer(
					new PlayerStats.HitInfo(10, hub.nicknameSync.MyNick, DamageTypes.Grenade,
						hub.queryProcessor.PlayerId), hub.gameObject);
			}
		}
	}
}