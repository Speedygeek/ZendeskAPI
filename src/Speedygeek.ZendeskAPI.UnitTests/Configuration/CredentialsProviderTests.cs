using System;
using System.Collections.Generic;
using System.Linq;
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
            Assert.That(() => { _ = new BasicCredentials(null, Settings.AdminPassword); },
                Throws.ArgumentException.With.Message.EqualTo("Parameter can not be null, empty or whitespace\r\nParameter name: userName"));
        }

        [Test]
        public void BasicAuthPasswordNull()
        {
            Assert.That(() => { _ = new BasicCredentials(Settings.AdminUserName, null); },
                Throws.ArgumentException.With.Message.EqualTo("Parameter can not be null, empty or whitespace\r\nParameter name: password"));
        }

        [Test]
        public void ApiTokenAuthUsernameNull()
        {
            Assert.That(() => { _ = new APITokenCredentials(null, Settings.ApiToken); },
                Throws.ArgumentException.With.Message.EqualTo("Parameter can not be null, empty or whitespace\r\nParameter name: userName"));
        }

        [Test]
        public void ApiTokenAuthApiTokenNull()
        {
            Assert.That(() => { _ = new APITokenCredentials(Settings.AdminUserName, null); },
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
        public void BasicAuthNullClient()
        {
            Assert.That(() => { new BasicCredentials(Settings.AdminUserName, Settings.AdminPassword).ConfigureHttpClient(null).ConfigureAwait(false); },
                Throws.ArgumentNullException);
        }

        [Test]
        public void OAuthTokenAuthNullToken()
        {
            Assert.That(() => { _ = new OAuthAccessTokenCredentials(null); },
                Throws.ArgumentException.With.Message.EqualTo("Parameter can not be null, empty or whitespace\r\nParameter name: accessToken"));
        }

        [Test]
        public void OAuthTokenAuthNullToken2()
        {
            Assert.That(() => { _ = new OAuthAccessTokenCredentials(null, "endUser@test.com"); },
                Throws.ArgumentException.With.Message.EqualTo("Parameter can not be null, empty or whitespace\r\nParameter name: accessToken"));
        }

        [Test]
        public void OAuthTokenAuthNullEndUserId()
        {
            Assert.That(() => { _ = new OAuthAccessTokenCredentials(Settings.AdminOAuthToken, null); },
                Throws.ArgumentException.With.Message.EqualTo("Parameter can not be null, empty or whitespace\r\nParameter name: endUserId"));
        }

        [Test]
        public async Task ApiTokenAuthBuildHeader()
        {
            using var client = new HttpClient();
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
            using var client = new HttpClient();
            var cred = new BasicCredentials(Settings.AdminUserName, Settings.AdminPassword);

            await cred.ConfigureHttpClient(client).ConfigureAwait(false);

            var headerScheme = client.DefaultRequestHeaders.Authorization.Scheme;
            var headerParameter = client.DefaultRequestHeaders.Authorization.Parameter;

            Assert.That(headerScheme, Is.EqualTo("Basic"));
            Assert.That(headerParameter, Is.EqualTo("Y3NoYXJwemVuZGVza2FwaTEyMzRAZ21haWwuY29tOiZIM24hMHFeM09qRExkbQ=="));
        }

        [Test]
        public async Task OAuthOnBehalfOfAuthBuildHeader()
        {
            using var client = new HttpClient();
            var endUserId = "enduser@test.com";
            var cred = new OAuthAccessTokenCredentials(Settings.AdminOAuthToken, endUserId);

            await cred.ConfigureHttpClient(client).ConfigureAwait(false);

            var headerValue = client.DefaultRequestHeaders.GetValues("X-On-Behalf-Of").FirstOrDefault();

            Assert.That(headerValue, Is.EqualTo(endUserId));
        }
    }
}
