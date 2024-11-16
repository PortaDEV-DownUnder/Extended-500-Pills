using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using UnityEngine;

namespace ExtendedPills.Items;

[CustomItem(ItemType.SCP500)]
public class SCP_500_B : CustomItem
{
    public override uint Id { get; set; } = 40002;
    public override string Name { get; set; } = "SCP-500-B";
    public override string Description { get; set; } = "Drugs that makes you a traitor of your team.";
    public override float Weight { get; set; } = 1f;
    public override ItemType Type { get; set; } = ItemType.SCP500;
    public override SpawnProperties SpawnProperties { get; set; } = new()
    {
        Limit = 1,
        DynamicSpawnPoints = new()
        {
            new()
            {
                Chance = 100f,
                Location = SpawnLocationType.Inside330
            }
        }
    };

    protected override void SubscribeEvents()
    {
        Exiled.Events.Handlers.Player.UsingItem += UsingItem;
        base.SubscribeEvents();
    }
    
    protected override void UnsubscribeEvents()
    {
        Exiled.Events.Handlers.Player.UsingItem -= UsingItem;
        base.UnsubscribeEvents();
    }
    
    private void UsingItem(UsingItemEventArgs ev)
    {
        if (!Check(ev.Player.CurrentItem)) return;
        Timing.CallDelayed(0.6f, () =>
        {
            if (!Check(ev.Player.CurrentItem)) return;
            Timing.CallDelayed(0.9f, () =>
            {
            switch (ev.Player.Role.Team)
            {
                case Team.FoundationForces:
                    ev.Player.Role.Set(RoleTypeId.ChaosRifleman, RoleSpawnFlags.None);
                    break;
                case Team.ChaosInsurgency:
                    ev.Player.Role.Set(RoleTypeId.NtfPrivate, RoleSpawnFlags.None);
                    break;
                case Team.Scientists:
                    ev.Player.Role.Set(RoleTypeId.ClassD, RoleSpawnFlags.None);
                    break;
                case Team.ClassD:
                    ev.Player.Role.Set(RoleTypeId.Scientist, RoleSpawnFlags.None);
                    break;
            }
            ev.Player.EnableEffect<AmnesiaVision>(10f, true);
        });
        });
    }
}