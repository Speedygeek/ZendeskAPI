using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Speedygeek.ZendeskAPI.Configuration;

namespace Speedygeek.ZendeskAPI.UnitTests.Configuration
{
    [TestFixture]
    public class CredentialsProviderTests
    {
        [Test]
        public void BasicAuthUsernameNull()
        {
            Assert.That(() => { new BasicCredentials(null, Settings.AdminPassword); },
                Throws.ArgumentException.With.Message.EqualTo("Parameter can not be null, empty or whitespace\r\nParameter name: userName"));
        }

        [Test]
        public void BasicAuthPasswordNull()
        {
            Assert.That(() => { new BasicCredentials(Settings.AdminUserName, null); },
                Throws.ArgumentException.With.Message.EqualTo("Parameter can not be null, empty or whitespace\r\nParameter name: password"));
        }

        [Test]
        public void ApiTokenAuthUsernameNull()
        {
            Assert.That(() => { new APITokenCredentials(null, Settings.ApiToken); },
                Throws.ArgumentException.With.Message.EqualTo("Parameter can not be null, empty or whitespace\r\nParameter name: userName"));
        }

        [Test]
        public void ApiTokenAuthApiTokenNull()
        {
            Assert.That(() => { new APITokenCredentials(Settings.AdminUserName, null); },
                Throws.ArgumentException.With.Message.EqualTo("Parameter can not be null, empty or whitespace\r\nParameter name: apiToken"));
        }

        [Test]
        public void ApiTokenAuthNullClient()
        {
            Assert.That(() => { new APITokenCredentials(Settings.AdminUserName, Settings.ApiToken).ConfigureHttpClient(null).ConfigureAwait(false); },
                Throws.ArgumentNullException);
        }

        [Test]
        public void OAuthTokenAuthNullClient()
        {
            Assert.That(() => { new OAuthAccessTokenCredentials(Settings.AdminOAuthToken).ConfigureHttpClient(null).ConfigureAwait(false); },
                Throws.ArgumentNullException);
        }

        [Test]
        public void OAuthTokenAuthNullToken()
        {
            Assert.That(() => { new OAuthAccessTokenCredentials(null); },
                Throws.ArgumentException.With.Message.EqualTo("Parameter can not be null, empty or whitespace\r\nParameter name: accessToken"));
        }

        [Test]
        public async Task ApiTokenAuthBuildHeader()
        {
            var client = new HttpClient();
            var cred = new APITokenCredentials(Settings.AdminUserName, Settings.ApiToken);

            await cred.ConfigureHttpClient(client).ConfigureAwait(false);

            var headerScheme = client.DefaultRequestHeaders.Authorization.Scheme;
            var headerParameter = client.DefaultRequestHeaders.Authorization.Parameter;

            Assert.That(headerScheme, Is.EqualTo("Basic"));
            Assert.That(headerParameter, Is.EqualTo("Y3NoYXJwemVuZGVza2FwaTEyMzRAZ21haWwuY29tL3Rva2VuOlI1UFhSaDVoRXdUOVJ5M2hmU3pHRjJXS3N4UGYzU2NTQ3Ewc3V4aWk="));
        }

        [Test]
        public async Task BasicAuthBuildHeader()
        {
            var client = new HttpClient();
            var cred = new BasicCredentials(Settings.AdminUserName, Settings.AdminPassword);

            await cred.ConfigureHttpClient(client).ConfigureAwait(false);

            var headerScheme = client.DefaultRequestHeaders.Authorization.Scheme;
            var headerParameter = client.DefaultRequestHeaders.Authorization.Parameter;

            Assert.That(headerScheme, Is.EqualTo("Basic"));
            Assert.That(headerParameter, Is.EqualTo("Y3NoYXJwemVuZGVza2FwaTEyMzRAZ21haWwuY29tOiZIM24hMHFeM09qRExkbQ=="));
        }
    }
}
