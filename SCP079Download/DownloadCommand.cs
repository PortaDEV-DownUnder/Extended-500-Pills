using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using PlayerRoles;

namespace SCP079Download
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class DownloadCommand : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            if (player.Role != RoleTypeId.Scp079)
            {
                response = "You must be SCP-079 to use this command.";
                return false;
            };
            Scp079Role role = (Scp079Role)player.Role;
            if (role.Level != 5)
            {
                response = "You must be Level 5 to use this command.";
                return false;
            }

            if (Math.Round(role.AuxManager.CurrentAux) <= 199)
            {
                response = "You must have at least 200 power to use this command.";
                return false;
            }

            if (Warhead.IsInProgress)
            {
                response = "The warhead has already detonating.";
                return false;
            }

            if (DownloadState.State != EDownload.NOT_STARTED)
            {
                response = "The download has already begun.";
                return false;
            }
            
            DownloadState.Begin();
            response = "Download has begun.";
            return true;
        }
    
        public string Command { get; } = "download";
        public string[] Aliases { get; } = new []{ "dl" };
        public string Description { get; } = "Begin the Download Prcoeess as SCP-079.";
    }
}