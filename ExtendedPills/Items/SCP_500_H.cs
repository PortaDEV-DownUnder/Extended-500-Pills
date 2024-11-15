using System;
using System.ComponentModel;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;

namespace ExtendedPills.Items;

[CustomItem(ItemType.SCP500)]
public class SCP_500_H : CustomItem
{
    public override uint Id { get; set; } = 40004;
    public override string Name { get; set; } = "SCP-500-H";
    public override string Description { get; set; } = "Drugs that makes you stronger?";
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
                Location = SpawnLocationType.Inside049Armory
            }
        }
    };
    
    [Description("Min value for health")]
    public float MinValue { get; set; } = 0.1f;
    [Description("Max value for health")]
    public float MaxValue { get; set; } = 50f;

    protected override void SubscribeEvents()
    {
        Exiled.Events.Handlers.Player.UsingItem += UsingItem;
        base.SubscribeEvents();
    }

    private void UsingItem(UsingItemEventArgs ev)
    {
        if (!Check(ev.Player.CurrentItem)) return;
        Timing.CallDelayed(1f, () =>
        {
            ev.Player.MaxHealth += (float)Util.NDC(new Random(), MinValue, MaxValue);
        });
    }

    protected override void UnsubscribeEvents()
    {
        Exiled.Events.Handlers.Player.UsingItem -= UsingItem;
        base.UnsubscribeEvents();
    }
}