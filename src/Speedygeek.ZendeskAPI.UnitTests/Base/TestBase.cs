using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace Speedygeek.ZendeskAPI.UnitTests.Base
{
    [ExcludeFromCodeCoverage]
    public class TestBase
    {
        protected IZendeskClient Client { get; set; }
        protected FakeHttpMessageHandlerBuilder Builder { get; set; }

        [OneTimeSetUp]
        public void Setup()
        {
            var collection = new ServiceCollection();
            collection.AddZendeskClient(Settings.SubDomain, Settings.AdminUserName, Settings.AdminPassword);

            collection.Replace(new ServiceDescriptor(typeof(HttpMessageHandlerBuilder), new FakeHttpMessageHandlerBuilder { SaveRespose = false }));

            var serviceProvider = collection.BuildServiceProvider();
            Client = serviceProvider.GetService<IZendeskClient>();
            Builder = serviceProvider.GetService<HttpMessageHandlerBuilder>() as FakeHttpMessageHandlerBuilder;
        }

        protected void BuildResponse(string pathAndQuery, string fileName, HttpMethod httpMethod = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            httpMethod ??= HttpMethod.Get;

            Builder.MockHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync((HttpRequestMessage requestMessage, CancellationToken token) =>
               {
                   var result = new HttpResponseMessage();

                   if (requestMessage.RequestUri.PathAndQuery == pathAndQuery && requestMessage.Method == httpMethod)
                   {
                       HttpContent content = null;
                       if (!string.IsNullOrWhiteSpace(fileName))
                       {
                           var file = File.ReadAllText(Path.Combine(TestContext.CurrentContext.GetDataDirectoryPath(), fileName));
                           content = new StringContent(file, Encoding.UTF8, "application/json");
                       }

                       result = new HttpResponseMessage { StatusCode = statusCode };
                       if (content != null)
                       {
                           result.Content = content;
                       }
                   }

                   return result;
               }).Verifiable();
        }
    }
}
