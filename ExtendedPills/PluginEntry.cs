using Exiled.API.Features;
using Exiled.CustomItems.API;
using MEC;

namespace ExtendedPills
{
    public class PluginEntry : Plugin<Config.Config>
    {
        public override string Name => "ExtendedPills";

        public override void OnEnabled()
        {
            RegisterItems();
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            UnregisterCustomItems();
            base.OnDisabled();
        }

        private void RegisterItems()
        {
            Timing.CallDelayed(5f, () =>
            {
                Config.LoadConfigs();
                Config.ItemConfigs.A.Register();
                Config.ItemConfigs.B.Register();
                Config.ItemConfigs.D.Register();
                Config.ItemConfigs.H.Register();
                Config.ItemConfigs.N.Register();
                Config.ItemConfigs.S.Register();
                Config.ItemConfigs.T.Register();
            });
        }

        private void UnregisterCustomItems()
        {
            Config.ItemConfigs.A.Unregister();
            Config.ItemConfigs.B.Unregister();
            Config.ItemConfigs.D.Unregister();
            Config.ItemConfigs.H.Unregister();
            Config.ItemConfigs.N.Unregister();
            Config.ItemConfigs.S.Unregister();
            Config.ItemConfigs.T.Unregister();
        }
    }
}