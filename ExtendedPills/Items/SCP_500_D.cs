using System.ComponentModel;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;

namespace ExtendedPills.Items;

[CustomItem(ItemType.SCP500)]
public class SCP_500_D : CustomItem
{
    public override uint Id { get; set; } = 40003;
    public override string Name { get; set; } = "SCP-500-D";
    public override string Description { get; set; } = "Drugs that makes you different for everyone.";
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
    
    [Description("Duration of the role change (0 is infinite)")]
    public float Duration { get; set; } = 7f;

    public Exiled.API.Features.Broadcast ChangeApperance { get; set; } = new()
    {
        Content = "You temporarily look like a %role%",
        Duration = 3
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
            Timing.CallDelayed(0.4f, () =>
            {
            switch (ev.Player.Role.Team)
            {
                case Team.FoundationForces:
                    ev.Player.Broadcast(new Exiled.API.Features.Broadcast
                    {
                        Content = this.ChangeApperance.Content.Replace("%role%", "<color=green>Chaos Rifleman</color>"),
                        Duration = this.ChangeApperance.Duration
                    }, false);
                    ev.Player.ChangeAppearance(RoleTypeId.ChaosRifleman, true, 0);
                    break;
                case Team.ChaosInsurgency:
                    ev.Player.Broadcast(new Exiled.API.Features.Broadcast
                    {
                        Content = this.ChangeApperance.Content.Replace("%role%", "<color=blue>Ntf Private</color>"),
                        Duration = this.ChangeApperance.Duration
                    }, false);
                    ev.Player.ChangeAppearance(RoleTypeId.NtfPrivate, true, 0);
                    break;
                case Team.Scientists:
                    ev.Player.Broadcast(new Exiled.API.Features.Broadcast
                    {
                        Content = this.ChangeApperance.Content.Replace("%role%", "<color=orange>ClassD</color>"),
                        Duration = this.ChangeApperance.Duration
                    }, false);
                    ev.Player.ChangeAppearance(RoleTypeId.ClassD, true, 0);
                    break;
                case Team.ClassD:
                    ev.Player.Broadcast(new Exiled.API.Features.Broadcast
                    {
                        Content = this.ChangeApperance.Content.Replace("%role%", "<color=yellow>Scientist</color>"),
                        Duration = this.ChangeApperance.Duration
                    }, false);
                    ev.Player.ChangeAppearance(RoleTypeId.Scientist, true, 0);
                    break;
            }
        });
        });
        if (Duration > 0)
        {
            Timing.CallDelayed(Duration, () =>
            {
                ev.Player.ChangeAppearance(ev.Player.Role, true, 0);
                ev.Player.Broadcast(new Exiled.API.Features.Broadcast
                {
                    Content = "You are back to your original role",
                    Duration = 3
                }, false);
            });
        }
    }

    
}