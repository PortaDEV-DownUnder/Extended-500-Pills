using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Warhead;
using MEC;
using PlayerRoles;

namespace SCP079Download
{
    public enum EDownload
    {
        NOT_STARTED,
        DOWNLOADING,
        FAILED,
        SUCCESS
    }
    
    public static class DownloadState
    {
        public static EDownload State { get; set; } = EDownload.NOT_STARTED;
        private static CoroutineHandle _downloadCoroutine;
        private static CoroutineHandle _postDetonationCoroutine;

        public static void Reset()
        {
            State = EDownload.NOT_STARTED;
            Timing.KillCoroutines(_downloadCoroutine);
            Timing.KillCoroutines(_postDetonationCoroutine);
        }

        public static void Begin()
        {
            State = EDownload.DOWNLOADING;
            Cassie.Message("WARNING . SCP 0 7 9 ESCAPE ATTEMPT IN PROGRESS . ACTIVATING EMERGENCY CONTAINMENT PROCEDURE NATO_Y NATO_Z 3", isSubtitles: true);
            Timing.CallDelayed(
                Cassie.CalculateDuration(
                    "WARNING . SCP 0 7 9 ESCAPE ATTEMPT IN PROGRESS . ACTIVATING EMERGENCY CONTAINMENT PROCEDURE NATO_Y NATO_Z 3") + 8f,
                () =>
                {
                    
                    Warhead.LeverStatus = true;
                    Warhead.Controller.StartDetonation(suppressSubtitles:true);
                    Timing.CallDelayed(15f,
                        () =>
                        {
                            Cassie.Message(
                                "pitch_0.22 .G3 .G3 pitch_0.85 WARNING . WARHEAD SYSTEMS ERROR . MANUAL OVERRIDE REQUIRED", isSubtitles: true);
                        });
                    
                    
                    _downloadCoroutine = Timing.RunCoroutine(DownloadCoroutine());
                    
                });
        }

        private static IEnumerator<float> DownloadCoroutine()
        {
            while (State == EDownload.DOWNLOADING)
            {
                yield return Timing.WaitForSeconds(0.1f);
                foreach (var p in Get079Players())
                {
                    p.AuxManager.CurrentAux = 5f;
                }
                
            }
        }
        
        public static void WarheadDetonating(DetonatingEventArgs ev)
        {
            if (State == EDownload.DOWNLOADING)
            {
                foreach (var p in Get079TruePlayers())
                {
                    p.IsGodModeEnabled = true;
                }

                State = EDownload.SUCCESS;
            }
        }

        private static IEnumerator<float> PostDetonationCoroutine()
        {
            while (State == EDownload.SUCCESS)
            {
                yield return Timing.WaitForSeconds(0.1f);
                bool OtherSCPs = false;
                foreach (var p in Player.List)
                {
                   if (p.IsScp && p.Role != RoleTypeId.Scp079) OtherSCPs = true;
                }

                if (!OtherSCPs)
                {
                    foreach (var P in Get079TruePlayers())
                    {
                        P.Kill("SUCCESSFUL DOWNLOAD ATTEMPT. (well done)");
                    }
                }
            }
        }

        public static void WarheadStopping(StoppingEventArgs ev)
        {
            if (State == EDownload.DOWNLOADING)
            {
                foreach (var p in Get079TruePlayers())
                {
                    p.Kill("FAILED DOWNLOAD ATTEMPT.");
                }
                State = EDownload.FAILED;
            }
        }

        public static void WarheadDetonated()
        {
            Timing.CallDelayed(1f, () =>
            {
                foreach (var p in Get079TruePlayers())
                {
                    p.IsGodModeEnabled = false;
                }
                Timing.KillCoroutines(_downloadCoroutine);
                _postDetonationCoroutine = Timing.RunCoroutine(PostDetonationCoroutine());
            });
        }

        public static List<Scp079Role> Get079Players()
        {
            List<Scp079Role> players = new List<Scp079Role>();
            foreach (Player player in Player.List)
            {
                if (player.Role == RoleTypeId.Scp079)
                {
                    players.Add((Scp079Role) player.Role);
                }
            }

            return players;
        }
        
        public static List<Player> Get079TruePlayers()
        {
            List<Player> players = new List<Player>();
            foreach (Player player in Player.List)
            {
                if (player.Role == RoleTypeId.Scp079)
                {
                    players.Add(player);
                }
            }

            return players;
        }
    }
}