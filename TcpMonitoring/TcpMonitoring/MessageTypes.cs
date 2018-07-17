using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpMonitoring
{
    // In XIMEx implementation, i should make a interface per messagetype.
    public enum MessageType
    {
        // Management Messages
        HeartBeat = 0,
        Interface = 1,
        Object = 2,

        // Monitoring Messages
        MonitorMessage = 10,
        ErrorMessage = 11,

        // Message Data
        MessageData = 100,
        InterfaceName,
        InterfaceMethods,
        InterfaceProperties
    }
}
