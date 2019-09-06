using System;
using System.Collections.Generic;
using System.Net.Http;
using Bogus;
using Demo.Neo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Demos.Neo.Generator
{
    public class EventGeneratorHttpTrigger
    {
        [FunctionName(nameof(EventGeneratorHttpTrigger))]
        public IActionResult Run(
            [HttpTrigger("GET", Route = "generate/neoevents/{numberOfEvents}")] HttpRequestMessage request,
            int numberOfEvents)
        {
            var generatedEvents = NeoEventGenerator.Generate(numberOfEvents);
            
            return new JsonResult(generatedEvents);
        }
    }
}
