using System;
using System.ComponentModel;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;

namespace ExtendedPills.Items;

[CustomItem(ItemType.SCP500)]
public class SCP_500_S : CustomItem
{
    public override uint Id { get; set; } = 40005;
    public override string Name { get; set; } = "SCP-500-S";
    public override string Description { get; set; } = "I can't read the label this pills make me go Faster or Slower?";
    public override float Weight { get; set; } = 1f;
    public override ItemType Type { get; set; } = ItemType.SCP500;
    public override SpawnProperties SpawnProperties { get; set; } = new()
    {
        Limit = 1,
        DynamicSpawnPoints = new() { new() { Chance = 100f, Location = SpawnLocationType.Inside049Armory } }
    };
    [Description("For how much time you can use the effect")]
    public float Duration { get; set; } = -1f;
    [Description("Min value for speed")]
    public byte MinValue { get; set; } = 1;
    [Description("Max value for speed")]
    public byte MaxValue { get; set; } = byte.MaxValue;

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
            ev.Player.SyncEffect(Duration < 0
                ? new Effect(EffectType.MovementBoost, float.MaxValue, (byte)new Random().Next(MinValue, MaxValue))
                : new Effect(EffectType.MovementBoost, Duration, (byte)new Random().Next(MinValue, MaxValue)));
        });
    }

    protected override void UnsubscribeEvents()
    {
        Exiled.Events.Handlers.Player.UsingItem -= UsingItem;
        base.UnsubscribeEvents();
    }
}