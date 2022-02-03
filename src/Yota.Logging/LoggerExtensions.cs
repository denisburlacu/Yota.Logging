using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Yota.Logging
{
    public static class LoggerExtensions
    {
        public static TResponse Verbose<TRequest, TResponse>(this ILogger logger, string key, TRequest request, Func<TResponse> response)
        {
            var serializedRequest = JsonConvert.SerializeObject(request);
            logger.LogInformation($"Request: {key}, {serializedRequest}");
            
            var watch = Stopwatch.StartNew();
            var resp = response();
            watch.Stop();
            
            var serializedResponse = JsonConvert.SerializeObject(resp);
            logger.LogInformation($"Finished Request (elapsed: {watch.ElapsedMilliseconds}): {key}, {serializedResponse}");

            return resp;
        }
    }
}