﻿using System;
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
        private ServiceCollection _collection;
        protected FakeHttpMessageHandlerBuilder Builder { get; set; }

        [OneTimeSetUp]
        public void Setup()
        {
            _collection = new ServiceCollection();
            _collection.AddZendeskClientWithBasicAuth(Settings.SubDomain, Settings.AdminUserName, Settings.AdminPassword);

            _collection.Replace(new ServiceDescriptor(typeof(HttpMessageHandlerBuilder), new FakeHttpMessageHandlerBuilder { SaveRespose = false }));

            var serviceProvider = _collection.BuildServiceProvider();
            Client = serviceProvider.GetService<IZendeskClient>();
            Builder = serviceProvider.GetService<HttpMessageHandlerBuilder>() as FakeHttpMessageHandlerBuilder;
        }

        public void SaveResponse(string responseFileName = "originalData.json")
        {
#if DEBUG
            _collection.Replace(new ServiceDescriptor(typeof(HttpMessageHandlerBuilder), new FakeHttpMessageHandlerBuilder { SaveRespose = true, FileName = responseFileName }));
            var serviceProvider = _collection.BuildServiceProvider();
            Client = serviceProvider.GetService<IZendeskClient>();
            Builder = serviceProvider.GetService<HttpMessageHandlerBuilder>() as FakeHttpMessageHandlerBuilder;
#endif
        }


        protected void BuildResponse(string pathAndQuery, string fileName, HttpMethod httpMethod = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            httpMethod ??= HttpMethod.Get;

            Builder.MockHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync((HttpRequestMessage requestMessage, CancellationToken token) =>
               {
                   var result = new HttpResponseMessage();

                   var prefix = "/";
                   if (requestMessage.RequestUri.PathAndQuery.Contains("/api/v2/",StringComparison.InvariantCulture))
                   {
                       prefix = "/api/v2/";
                   }

                   var encodedURL = new Uri($"{requestMessage.RequestUri.Scheme}://{requestMessage.RequestUri.Host}{prefix}{pathAndQuery.TrimStart('/')}");

                   if (requestMessage.RequestUri.PathAndQuery == encodedURL.PathAndQuery && requestMessage.Method == httpMethod)
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
