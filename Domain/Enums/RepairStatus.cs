using System;
using System.Collections.Generic;
using System.Text;

namespace HardwareTech.Domain.Enums;

public enum RepairStatus
{
    Received,
    Quoted,
    AwaitingAcceptance,
    InProgress,
    Completed,
    Rejected
}

