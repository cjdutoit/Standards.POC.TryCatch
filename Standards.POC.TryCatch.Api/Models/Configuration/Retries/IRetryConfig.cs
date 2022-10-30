// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;

namespace Standards.POC.TryCatch.Api.Models.Configuration.Retries
{
    public interface IRetryConfig
    {
        int MaxRetryAttempts { get; }
        TimeSpan DelayBetweenFailures { get; }
    }
}
