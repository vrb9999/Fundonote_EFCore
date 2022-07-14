// <copyright file="ILoggerManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NLogger.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface ILoggerManager
    {
        void LogInfo(string message);

        void LogWarn(string message);

        void LogDebug(string message);

        void LogError(string message);
    }
}
