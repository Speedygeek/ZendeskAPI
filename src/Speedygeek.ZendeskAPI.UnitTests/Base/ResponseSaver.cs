using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Speedygeek.ZendeskAPI.UnitTests.Base
{
    public class ResponseSaver : DelegatingHandler
    {
        private const string DEFAULTFILENAME = "originalData.json";

        public string FileName { get; set; }

        [ExcludeFromCodeCoverage]
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var resp = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            var filePath = Path.Combine(TestContext.CurrentContext.GetDataDirectoryPath(), FileName);

            if (!File.Exists(filePath) | FileName == DEFAULTFILENAME)
            {
                var content = await resp.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                content = Regex.Replace(content, @"\s+", " ");
                await File.WriteAllTextAsync(filePath, content, cancellationToken);
            }
            return resp;
        }
    }
}
