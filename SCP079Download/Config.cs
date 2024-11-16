using Exiled.API.Interfaces;

namespace SCP079Download
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
    }
}