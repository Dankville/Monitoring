﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpMonitoring.MessagingObjects
{
    public interface IMessage
    {
        string Data { get; set; }
    }
}