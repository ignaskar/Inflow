﻿using Inflow.Shared.Abstractions.Time;

namespace Inflow.Shared.Infrastructure.Time;

public class UtcClock : IClock
{
    public DateTime CurrentDateTime() => DateTime.UtcNow;
}