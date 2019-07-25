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
        public override string Name { get; set; } = "test";
        public override HttpMessageHandler PrimaryHandler { get; set; }
        public override IList<DelegatingHandler> AdditionalHandlers { get; } = new List<DelegatingHandler> { new ResponseSaver() };

        public override HttpMessageHandler Build()
        {
            if (!SaveRespose)
            {
                PrimaryHandler = MockHandler.Object;
                return MockHandler.Object;
            }
            else
            {
                PrimaryHandler = new HttpClientHandler();
                return CreateHandlerPipeline(PrimaryHandler, AdditionalHandlers);
            }
        }
    }
}
