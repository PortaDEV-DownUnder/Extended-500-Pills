using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Features;
using Exiled.Events.Handlers;
using MEC;
using PlayerRoles;

namespace ExtendedPills.Items
{
    [CustomItem(ItemType.SCP500)]
    public class SCP_500_A : CustomItem
    {
        public override uint Id { get; set; } = 40001;
        public override string Name { get; set; } = "SCP-500-A";
        public override string Description { get; set; } = "Drugs that make everyone come back.";
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

        public Exiled.API.Features.Broadcast NoPlayers { get; set; } = new()
        {
            Content = "No Players to revive!",
            Duration = 10
        };

        protected override void SubscribeEvents()
        {
            Player.UsingItem += OnUsingItem;
            base.SubscribeEvents();
        }
        
        protected override void UnsubscribeEvents()
        {
            Player.UsingItem -= OnUsingItem;
            base.UnsubscribeEvents();
        }

        private void OnUsingItem(UsingItemEventArgs ev)
        {
            if (!Check(ev.Player.CurrentItem)) return;

            Timing.CallDelayed(0.6f, () =>
            {
                if (!Check(ev.Player.CurrentItem)) return;
                Timing.CallDelayed(0.9f, () =>
                {
                    RoleTypeId RTID;
                    switch (ev.Player.Role.Team)
                    {
                        case Team.ClassD:
                            RTID = RoleTypeId.ClassD;
                            break;
                        case Team.ChaosInsurgency:
                            RTID = RoleTypeId.ChaosConscript;
                            break;
                        case Team.FoundationForces:
                            RTID = RoleTypeId.NtfPrivate;
                            break;
                        default:
                            RTID = RoleTypeId.NtfPrivate;
                            break;
                    }

                    List<Exiled.API.Features.Player> list = Exiled.API.Features.Player.List.Where(x => x.IsDead)
                        .ToList();
                    if (list.Count() == 0) ev.Player.Broadcast(this.NoPlayers, false);
                    else
                    {
                        Random random = new();
                        Exiled.API.Features.Player player = list[random.Next(list.Count())];
                        player.Role.Set(RTID, SpawnReason.None);
                        player.Position = ev.Player.Position;
                    }
                });
            });


        }
    }
}