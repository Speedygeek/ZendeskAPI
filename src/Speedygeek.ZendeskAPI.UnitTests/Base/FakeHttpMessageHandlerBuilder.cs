using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using Microsoft.Extensions.Http;
using Moq;

namespace Speedygeek.ZendeskAPI.UnitTests.Base
{
    [ExcludeFromCodeCoverage]
    public class FakeHttpMessageHandlerBuilder : HttpMessageHandlerBuilder
    {
        public Mock<HttpMessageHandler> MockHandler { get; set; } = new Mock<HttpMessageHandler>();

        public bool SaveRespose { get; set; } = false;
        public string FileName { get; set; } = "originalData.json";
        public override string Name { get; set; } = "DataCaptureHandler";
        public override HttpMessageHandler PrimaryHandler { get; set; }
        public override IList<DelegatingHandler> AdditionalHandlers { get; } = new List<DelegatingHandler>();

        public override HttpMessageHandler Build()
        {
            if (!SaveRespose)
            {
                PrimaryHandler = MockHandler.Object;
            }
            else
            {
                PrimaryHandler = new HttpClientHandler();

                AdditionalHandlers.Clear();
                AdditionalHandlers.Add(new ResponseSaver { FileName = FileName });
            }

            return CreateHandlerPipeline(PrimaryHandler, AdditionalHandlers);
        }
    }
}
