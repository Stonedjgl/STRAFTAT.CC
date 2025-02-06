using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STRAFTSHAT.Features
{
    public class ESP
    {
        public static void OnGUI()
        {
            if (Config.Instance.enable3DBoxESP)
            {

                foreach (var player in Cheat.Instance.Cache.Players)
                    player.Draw(Cheat.Instance.Cache.MainCamera);
            }
        }
    }
}
