// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;

namespace Standards.POC.TryCatch.Api.Models.Configuration.Retries
{
    public class RetryConfig : IRetryConfig
    {
        public RetryConfig()
        {
            MaxRetryAttempts = 3;
            DelayBetweenFailures = TimeSpan.FromSeconds(2);
        }

        public RetryConfig(int maxRetryAttempts, TimeSpan delayBetweenFailures)
        {
            MaxRetryAttempts = maxRetryAttempts;
            DelayBetweenFailures = delayBetweenFailures;
        }

        public int MaxRetryAttempts { get; private set; }
        public TimeSpan DelayBetweenFailures { get; private set; }
    }
}
