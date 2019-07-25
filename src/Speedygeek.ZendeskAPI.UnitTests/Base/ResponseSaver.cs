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
        [ExcludeFromCodeCoverage]
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var resp = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            var content = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);



            var path = Path.Combine(TestContext.CurrentContext.GetDataDirectoryPath(), "originalData.json");

            content = Regex.Replace(content, @"\s+", " ");

            File.WriteAllText(path, content);

            return resp;
        }
    }
}
