using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpMonitoring.MessagingObjects
{
    public class HeartbeatObject : IMessage
    {
        public string HeartbeatData { get; set; }
    }
}
