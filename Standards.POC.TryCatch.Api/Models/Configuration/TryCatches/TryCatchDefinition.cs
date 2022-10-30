// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Standards.POC.TryCatch.Api.Models.Configuration.TryCatches
{
    public class TryCatchDefinition<T>
    {
        public Func<T> Execution { get; set; }
        public List<string> WithSecurityRoles { get; set; } = new List<string>();
        public List<Type> WithRetryOn { get; set; } = new List<Type>();
        public List<Type> WithRollbackOn { get; set; } = new List<Type>();
        public dynamic WithTracing { get; set; } = null;
    }
}
