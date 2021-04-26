using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Raid_AFK_Manager.Tools
{
    internal static class KeyBoardHandler
    {
        internal static void SendKey(string key)        
        {
            SendKeys.SendWait(key);
            Thread.Sleep(1000);
        }
    }
}
