// ---------------------------------------------------------------
// Copyright (c) Christo du Toit. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Standards.POC.TryCatch.Api.Models.Configuration.TryCatches;
using Standards.POC.TryCatch.Api.Models.Students;

namespace Standards.POC.TryCatch.Api.Services.Foundations.Students
{
    public partial class StudentService
    {
        static readonly ActivitySource source = new ActivitySource("Standards.POC.TryCatch.Api");

        private async ValueTask<Student> WithTracing(
            ReturningStudentFunction returningStudentFunction,
            Operation<ValueTask<Student>> operation)
        {
            var withTracing = operation.WithTracing;

            if (withTracing != null)
            {
                using (var activity = source.StartActivity(withTracing.ActivityName, ActivityKind.Internal)!)
                {
                    SetupActivity(activity, withTracing.ActivityName, withTracing.Tags, withTracing.Baggage);
                    var result = await returningStudentFunction();

                    return result;
                }
            }

            return await returningStudentFunction();
        }

        private static void SetupActivity(
            Activity activity,
            string activityName,
            Dictionary<string, string> tags = null,
            Dictionary<string, string> baggage = null)
        {
            if (activity == null)
            {
                activity = new Activity(activityName);
            }

            if (tags != null)
            {
                foreach (var tag in tags)
                {
                    activity.AddTag(tag.Key, tag.Value);
                }
            }

            if (baggage != null)
            {
                foreach (var baggageItem in baggage)
                {
                    activity.AddBaggage(baggageItem.Key, baggageItem.Value);
                }
            }
        }

        private static string FormatTraceMessage(string message)
        {
            StringBuilder traceMessage = new StringBuilder();
            traceMessage.Append(message);
            traceMessage.AppendLine($"ParentSpanId: {Activity.Current.ParentSpanId}");
            traceMessage.AppendLine($"ParentId: {Activity.Current.ParentId}");
            traceMessage.AppendLine($"SpanId: {Activity.Current.SpanId}");
            traceMessage.AppendLine($"Id: {Activity.Current.Id}");

            return traceMessage.ToString();
        }
    }
}