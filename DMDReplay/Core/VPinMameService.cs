namespace DMDReplay.Core
{
    public interface IVPinMameService
    {
        void Shutdown();
        void RunROM(RomConfiguration config);
        VPinMAMELib.Controller GetController();
    }
    public class VPinMameService : IVPinMameService
    {
        private VPinMAMELib.Controller _controller = new VPinMAMELib.Controller();

        public VPinMameService()
        {

        }

        public void Shutdown()
        {
            if (_controller.Running)
                _controller.Stop();
        }

        public void RunROM(RomConfiguration config)
        {
            _controller.GameName = config.RomName;
            _controller.ShowFrame = false;
            _controller.Hidden = false;
            _controller.ShowDMDOnly = true;
            _controller.ShowTitle = false;
            _controller.HandleKeyboard = false;

            _controller.ShowPinDMD = config.ShowNativePinmameDmd;
            _controller.ShowWinDMD = config.ShowExternalDmd;

   
            //if (config.DipSwitches.Count > 0)
            //{

            //}

            foreach (var sw in config.SwitchInitialization)
            {
                _controller.Switch[sw.Number] = sw.On;
            }

            _controller.Run();

            foreach (var sw in config.SwitchInitialization)
            {
                _controller.Switch[sw.Number] = sw.On;
            }
        }

        public VPinMAMELib.Controller GetController()
        {
            return _controller;
        }
    }
}
