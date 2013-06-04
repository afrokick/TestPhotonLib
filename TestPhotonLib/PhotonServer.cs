using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using Photon.SocketServer;
using log4net.Config;

namespace TestPhotonLib
{
    public class PhotonServer:ApplicationBase
    {
        private readonly ILogger Log = LogManager.GetCurrentClassLogger();

        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            Log.Debug("connect new client...");

            return new UnityClient(initRequest.Protocol, initRequest.PhotonPeer);
        }

        protected override void Setup()
        {
            var file = new FileInfo(Path.Combine(BinaryPath, "log4net.config"));
            if (file.Exists)
            {
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
                XmlConfigurator.ConfigureAndWatch(file);
            }

            Log.Debug("Server is setup!");
        }

        protected override void TearDown()
        {
            Log.Debug("Server is closed!");
        }
    }
}
