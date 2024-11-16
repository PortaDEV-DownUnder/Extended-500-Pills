using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;

namespace ExtendedPills.Items;

[CustomItem(ItemType.SCP500)]
public class SCP_500_T : CustomItem
{
    public override uint Id { get; set; } = 40006;
    public override string Name { get; set; } = "SCP-500-T";
    public override string Description { get; set; } = "Drugs that Teleports you around.";
    public override float Weight { get; set; } = 1f;
    public override ItemType Type { get; set; } = ItemType.SCP500;
    public float Duration { get; set; } = 10f;

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

    private void UsingItem(UsingItemEventArgs ev)
    {
        if (!Check(ev.Player.CurrentItem)) return;
        Timing.CallDelayed(1.5f, () =>
        {
            ev.Player.ShowHint("you start to feel dizzy", 3f);
            ev.Player.EnableEffect<Blinded>(this.Duration, true);
            Timing.WaitForSeconds(2f);
            ev.Player.RandomTeleport(typeof(Room));
        });
    }

    protected override void UnsubscribeEvents()
    {
        Exiled.Events.Handlers.Player.UsingItem -= UsingItem;
        base.UnsubscribeEvents();
    }
}