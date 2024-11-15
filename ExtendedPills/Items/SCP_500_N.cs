using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using UnityEngine;

namespace ExtendedPills.Items;

public class SCP_500_N : CustomItem
{
    public override uint Id { get; set; } = 40007;
    public override string Name { get; set; } = "SCP-500-N";
    public override string Description { get; set; } = "A pill that will make you emit radiation, damaging everyone around you.";
    public override float Weight { get; set; } = 1f;
    public override ItemType Type { get; set; } = ItemType.SCP500;

    public int Radius { get; set; } = 30;
    public int DamagePerSecond { get; set; } = 10;
    public bool DamageSCP { get; set; } = false;
    public bool ShouldDamageSelf { get; set; } = false;
    public bool AlwaysDoDirectDamage { get; set; } = false;
    
    public override SpawnProperties SpawnProperties { get; set; } = new()
    {
        Limit = 1,
        DynamicSpawnPoints =
        [
            new() { Chance = 33, Location = SpawnLocationType.InsideHczArmory },
            new() { Chance = 66, Location = SpawnLocationType.InsideIntercom },
            new() { Chance = 100, Location = SpawnLocationType.InsideSurfaceNuke }
        ]
    };
    private List<Player> GetPlayersInRadius(Player source, int radius, bool includeSelf = false)
    {
        List<Player> players = new();
        foreach (var player in Player.List)
        {
            if (!player.IsAlive || player.Id == source.Id && !includeSelf) continue;
            if (Vector3.Distance(source.Position, player.Position) <= radius)
            {
                players.Add(player);
            }
        }
        return players;
    }
    
    protected override void SubscribeEvents()
    {
        Exiled.Events.Handlers.Player.UsingItem += OnUsingItem;
        base.SubscribeEvents();
    }

    protected override void UnsubscribeEvents()
    {
        Exiled.Events.Handlers.Player.UsingItem -= OnUsingItem;
        base.UnsubscribeEvents();
    }

    private void OnUsingItem(UsingItemEventArgs ev)
    {
        if (!Check(ev.Player.CurrentItem)) return;
        Timing.CallDelayed(1f, () =>
        {
            for (int i = 0; i < 10; i++)
            {
                Timing.CallDelayed(i, () =>
                {
                    foreach (var p in GetPlayersInRadius(ev.Player, Radius, ShouldDamageSelf))
                    {
                        if (p.Role == RoleTypeId.Scp079) continue;
                        if (!DamageSCP && p.IsScp) continue;
                        if (AlwaysDoDirectDamage)
                        {
                            if (p.Health < DamagePerSecond) p.Kill("SCP-500-N");
                            else
                            {
                                if (ShouldDamageSelf && p ==  ev.Player) p.Health -= DamagePerSecond + 7;
                                else 
                                    p.Health -= DamagePerSecond;
                            };
                        }
                        else
                        {
                            if (ShouldDamageSelf && p ==  ev.Player) p.Hurt(DamagePerSecond + 7, DamageType.Asphyxiation);
                            else 
                                p.Hurt(DamagePerSecond, DamageType.Asphyxiation);
                        }
                    }
                });
            }
        });
    }
    
}