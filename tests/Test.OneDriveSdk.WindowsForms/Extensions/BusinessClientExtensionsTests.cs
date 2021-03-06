// ------------------------------------------------------------------------------
//  Copyright (c) 2015 Microsoft Corporation
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
// ------------------------------------------------------------------------------

namespace Test.OneDriveSdk.WindowsForms.Authentication
{
    using System.Net.Http;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;

    using Microsoft.OneDrive.Sdk;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mocks;
    using Moq;
    using OneDriveSdk.Mocks;

    [TestClass]
    public class BusinessClientExtensionsTests
    {
        protected const string serviceEndpointUri = "https://localhost";
        protected const string serviceResourceId = "https://localhost/resource/";

        protected MockAuthenticationProvider authenticationProvider;
        protected MockAdalCredentialCache credentialCache;
        protected MockHttpProvider httpProvider;
        protected HttpResponseMessage httpResponseMessage;
        protected MockSerializer serializer;

        [TestInitialize]
        public void Setup()
        {
            this.credentialCache = new MockAdalCredentialCache();
            this.httpResponseMessage = new HttpResponseMessage();
            this.serializer = new MockSerializer();
            this.httpProvider = new MockHttpProvider(this.httpResponseMessage, this.serializer.Object);

            this.authenticationProvider = new MockAuthenticationProvider();
            this.authenticationProvider.Setup(provider => provider.AuthenticateAsync()).Returns(Task.FromResult(new AccountSession()));
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.httpResponseMessage.Dispose();
        }

        [TestMethod]
        [ExpectedException(typeof(OneDriveException))]
        public async Task GetAuthenticatedClientAsync_AppIdRequired()
        {
            try
            {
                var client = await BusinessClientExtensions.GetAuthenticatedClientAsync(
                    new BusinessAppConfig
                    {
                        ActiveDirectoryReturnUrl = "https://return"
                    },
                    /* userId */ null,
                    this.credentialCache.Object,
                    this.httpProvider.Object);
            }
            catch (OneDriveException exception)
            {
                Assert.AreEqual(OneDriveErrorCode.AuthenticationFailure.ToString(), exception.Error.Code, "Unexpected error thrown.");
                Assert.AreEqual("ActiveDirectoryAppId is required for authentication.", exception.Error.Message, "Unexpected error thrown.");

                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(OneDriveException))]
        public async Task GetAuthenticatedClientAsync_ReturnUrlRequired()
        {
            try
            {
                var client = await BusinessClientExtensions.GetAuthenticatedClientAsync(
                    new BusinessAppConfig(),
                    /* userId */ null,
                    this.credentialCache.Object,
                    this.httpProvider.Object);
            }
            catch (OneDriveException exception)
            {
                Assert.AreEqual(OneDriveErrorCode.AuthenticationFailure.ToString(), exception.Error.Code, "Unexpected error thrown.");
                Assert.AreEqual("ActiveDirectoryReturnUrl is required for authenticating a business client.", exception.Error.Message, "Unexpected error thrown.");

                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(OneDriveException))]
        public async Task GetAuthenticatedWebClientAsync_ClientCertificateOrSecretRequired()
        {
            var appId = "appId";

            try
            {
                var client = await BusinessClientExtensions.GetAuthenticatedWebClientAsync(
                    new BusinessAppConfig
                    {
                        ActiveDirectoryAppId = appId,
                    },
                    /* userId */ null,
                    this.credentialCache.Object,
                    this.httpProvider.Object);
            }
            catch (OneDriveException exception)
            {
                Assert.AreEqual(OneDriveErrorCode.AuthenticationFailure.ToString(), exception.Error.Code, "Unexpected error thrown.");
                Assert.AreEqual("Client certificate or client secret is required for authenticating a business web client.", exception.Error.Message, "Unexpected error thrown.");

                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(OneDriveException))]
        public async Task GetAuthenticatedWebClientUsingAppOnlyAuthenticationAsync_ClientCertificateRequired()
        {
            var appId = "appId";
            var siteId = "site_id";
            var tenant = "tenant";
            
            try
            {
                var client = await BusinessClientExtensions.GetAuthenticatedWebClientUsingAppOnlyAuthenticationAsync(
                new BusinessAppConfig
                {
                    ActiveDirectoryAppId = appId,
                    ActiveDirectoryServiceResource = serviceResourceId,
                },
                siteId,
                tenant,
                this.credentialCache.Object,
                this.httpProvider.Object);
            }
            catch (OneDriveException exception)
            {
                Assert.AreEqual(OneDriveErrorCode.AuthenticationFailure.ToString(), exception.Error.Code, "Unexpected error thrown.");
                Assert.AreEqual("ActiveDirectoryClientCertificate is required for app-only authentication.", exception.Error.Message, "Unexpected error thrown.");

                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(OneDriveException))]
        public async Task GetAuthenticatedWebClientUsingAppOnlyAuthenticationAsync_ServiceEndpointBaseUrlRequired()
        {
            var appId = "appId";
            var tenant = "tenant";

            var clientCertificate = new X509Certificate2(@"Certs\testwebapplication.pfx", "password");

            try
            {
                var client = await BusinessClientExtensions.GetAuthenticatedWebClientUsingAppOnlyAuthenticationAsync(
                    new BusinessAppConfig
                    {
                        ActiveDirectoryAppId = appId,
                        ActiveDirectoryClientCertificate = clientCertificate,
                        ActiveDirectoryServiceResource = serviceResourceId,
                    },
                    /* serviceEndpointBaseUrl */ null,
                    tenant,
                    this.credentialCache.Object,
                    this.httpProvider.Object);
            }
            catch (OneDriveException exception)
            {
                Assert.AreEqual(OneDriveErrorCode.AuthenticationFailure.ToString(), exception.Error.Code, "Unexpected error thrown.");
                Assert.AreEqual("Service endpoint base URL is required for app-only authentication.", exception.Error.Message, "Unexpected error thrown.");

                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(OneDriveException))]
        public async Task GetAuthenticatedWebClientUsingAppOnlyAuthenticationAsync_ServiceResourceRequired()
        {
            var appId = "appId";
            var tenant = "tenant";

            var clientCertificate = new X509Certificate2(@"Certs\testwebapplication.pfx", "password");

            try
            {
                var client = await BusinessClientExtensions.GetAuthenticatedWebClientUsingAppOnlyAuthenticationAsync(
                    new BusinessAppConfig
                    {
                        ActiveDirectoryAppId = appId,
                        ActiveDirectoryClientCertificate = clientCertificate,
                    },
                    serviceResourceId,
                    tenant,
                    this.credentialCache.Object,
                    this.httpProvider.Object);
            }
            catch (OneDriveException exception)
            {
                Assert.AreEqual(OneDriveErrorCode.AuthenticationFailure.ToString(), exception.Error.Code, "Unexpected error thrown.");
                Assert.AreEqual("ActiveDirectoryServiceResource is required for app-only authentication.", exception.Error.Message, "Unexpected error thrown.");

                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(OneDriveException))]
        public async Task GetAuthenticatedWebClientUsingAppOnlyAuthenticationAsync_TenantIdRequired()
        {
            var appId = "appId";

            var clientCertificate = new X509Certificate2(@"Certs\testwebapplication.pfx", "password");

            try
            {
                var client = await BusinessClientExtensions.GetAuthenticatedWebClientUsingAppOnlyAuthenticationAsync(
                    new BusinessAppConfig
                    {
                        ActiveDirectoryAppId = appId,
                        ActiveDirectoryClientCertificate = clientCertificate,
                        ActiveDirectoryServiceResource = serviceResourceId,
                    },
                    serviceResourceId,
                    /* tenantId */ null,
                    this.credentialCache.Object,
                    this.httpProvider.Object);
            }
            catch (OneDriveException exception)
            {
                Assert.AreEqual(OneDriveErrorCode.AuthenticationFailure.ToString(), exception.Error.Code, "Unexpected error thrown.");
                Assert.AreEqual("Tenant ID is required for app-only authentication.", exception.Error.Message, "Unexpected error thrown.");

                throw;
            }
        }

        [TestMethod]
        public async Task GetAuthenticatedClientUsingCustomAuthenticationAsync()
        {
            var baseEndpointUrl = "https://resource/";

            var client = await BusinessClientExtensions.GetAuthenticatedClientUsingCustomAuthenticationAsync(
                baseEndpointUrl,
                this.authenticationProvider.Object,
                this.httpProvider.Object) as OneDriveClient;

            var clientServiceInfoProvider = client.serviceInfoProvider as ServiceInfoProvider;

            Assert.IsNotNull(clientServiceInfoProvider, "Unexpected service info provider initialized for client.");
            Assert.AreEqual(this.authenticationProvider.Object, clientServiceInfoProvider.AuthenticationProvider, "Unexpected authentication provider set.");
            Assert.AreEqual(this.httpProvider.Object, client.HttpProvider, "Unexpected HTTP provider set.");
            Assert.IsNull(client.credentialCache, "Unexpected credential cache set.");

            Assert.AreEqual(
                string.Format(
                    Constants.Authentication.OneDriveBusinessBaseUrlFormatString,
                    baseEndpointUrl.TrimEnd('/'),
                    "v2.0"),
                client.BaseUrl,
                "Unexpected base service URL initialized.");

            this.authenticationProvider.Verify(provider => provider.AuthenticateAsync(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(OneDriveException))]
        public async Task GetAuthenticatedClientUsingCustomAuthenticationAsync_AuthenticationProviderRequired()
        {
            var baseEndpointUrl = "https://resource/";

            try
            {
                var client = await BusinessClientExtensions.GetAuthenticatedClientUsingCustomAuthenticationAsync(
                    baseEndpointUrl,
                    /* authenticationProvider */ null,
                    this.httpProvider.Object);
            }
            catch (OneDriveException exception)
            {
                Assert.AreEqual(OneDriveErrorCode.AuthenticationFailure.ToString(), exception.Error.Code, "Unexpected error thrown.");
                Assert.AreEqual("An authentication provider is required for a client using custom authentication.", exception.Error.Message, "Unexpected error thrown.");

                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(OneDriveException))]
        public async Task GetAuthenticatedClientUsingCustomAuthenticationAsync_ServiceEndpointBaseUrlRequired()
        {
            try
            {
                var client = await BusinessClientExtensions.GetAuthenticatedClientUsingCustomAuthenticationAsync(
                    /* serviceEndpointBaseUrl */ null,
                    this.authenticationProvider.Object,
                    this.httpProvider.Object);
            }
            catch (OneDriveException exception)
            {
                Assert.AreEqual(OneDriveErrorCode.AuthenticationFailure.ToString(), exception.Error.Code, "Unexpected error thrown.");
                Assert.AreEqual("Service endpoint base URL is required when using custom authentication.", exception.Error.Message, "Unexpected error thrown.");

                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(OneDriveException))]
        public async Task GetAuthenticatedWebClientUsingAuthenticationByCodeAsync_AppIdRequired()
        {
            try
            {
                var client = await BusinessClientExtensions.GetAuthenticatedWebClientUsingAuthenticationByCodeAsync(
                    new BusinessAppConfig
                    {
                        ActiveDirectoryReturnUrl = "https://return",
                        ActiveDirectoryServiceResource = "https://resource/",
                    },
                    "code",
                    this.credentialCache.Object,
                    this.httpProvider.Object);
            }
            catch (OneDriveException exception)
            {
                Assert.AreEqual(OneDriveErrorCode.AuthenticationFailure.ToString(), exception.Error.Code, "Unexpected error thrown.");
                Assert.AreEqual("ActiveDirectoryAppId is required for authentication.", exception.Error.Message, "Unexpected error thrown.");

                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(OneDriveException))]
        public async Task GetAuthenticatedWebClientUsingAuthenticationByCodeAsync_CodeRequired()
        {
            try
            {
                var client = await BusinessClientExtensions.GetAuthenticatedWebClientUsingAuthenticationByCodeAsync(
                    new BusinessAppConfig
                    {
                        ActiveDirectoryAppId = "appId",
                        ActiveDirectoryReturnUrl = "https://return",
                        ActiveDirectoryServiceResource = "https://resource/",
                    },
                    /* code */ null,
                    this.credentialCache.Object,
                    this.httpProvider.Object);
            }
            catch (OneDriveException exception)
            {
                Assert.AreEqual(OneDriveErrorCode.AuthenticationFailure.ToString(), exception.Error.Code, "Unexpected error thrown.");
                Assert.AreEqual("Authorization code is required for authentication by code.", exception.Error.Message, "Unexpected error thrown.");

                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(OneDriveException))]
        public async Task GetAuthenticatedWebClientUsingAuthenticationByCodeAsync_ReturnUrlRequired()
        {
            try
            {
                var client = await BusinessClientExtensions.GetAuthenticatedClientAsync(
                    new BusinessAppConfig(),
                    "code",
                    this.credentialCache.Object,
                    this.httpProvider.Object);
            }
            catch (OneDriveException exception)
            {
                Assert.AreEqual(OneDriveErrorCode.AuthenticationFailure.ToString(), exception.Error.Code, "Unexpected error thrown.");
                Assert.AreEqual("ActiveDirectoryReturnUrl is required for authenticating a business client.", exception.Error.Message, "Unexpected error thrown.");

                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(OneDriveException))]
        public async Task GetAuthenticatedWebClientUsingAuthenticationByCodeAsync_ServiceResourceRequired()
        {
            try
            {
                var client = await BusinessClientExtensions.GetAuthenticatedWebClientUsingAuthenticationByCodeAsync(
                    new BusinessAppConfig
                    {
                        ActiveDirectoryAppId = "appId",
                        ActiveDirectoryReturnUrl = "https://return",
                    },
                    "code",
                    this.credentialCache.Object,
                    this.httpProvider.Object);
            }
            catch (OneDriveException exception)
            {
                Assert.AreEqual(OneDriveErrorCode.AuthenticationFailure.ToString(), exception.Error.Code, "Unexpected error thrown.");
                Assert.AreEqual("Service resource ID is required for authentication by code.", exception.Error.Message, "Unexpected error thrown.");

                throw;
            }
        }

        [TestMethod]
        public void GetClient()
        {
            var appId = "appId";
            var returnUrl = "returnUrl";
            var userId = "userId";
            
            var client = BusinessClientExtensions.GetClient(
                new BusinessAppConfig
                {
                    ActiveDirectoryAppId = appId,
                    ActiveDirectoryReturnUrl = returnUrl,
                    ActiveDirectoryServiceResource = serviceResourceId,
                },
                userId,
                this.credentialCache.Object,
                this.httpProvider.Object) as OneDriveClient;

            var clientServiceInfoProvider = client.serviceInfoProvider as ServiceInfoProvider;

            Assert.IsNotNull(clientServiceInfoProvider, "Unexpected service info provider initialized for client.");
            Assert.AreEqual(userId, clientServiceInfoProvider.UserSignInName, "Unexpected user sign-in name set.");
            Assert.AreEqual(this.httpProvider.Object, client.HttpProvider, "Unexpected HTTP provider set.");
            Assert.AreEqual(this.credentialCache.Object, client.credentialCache, "Unexpected credential cache set.");
        }

        [TestMethod]
        public void GetClient_InitializeDefaults()
        {
            var appId = "appId";

            var client = BusinessClientExtensions.GetClientInternal(
                new BusinessAppConfig
                {
                    ActiveDirectoryAppId = appId,
                },
                serviceInfoProvider: null,
                credentialCache: null,
                httpProvider: null) as OneDriveClient;

            var adalAppConfig = client.appConfig as BusinessAppConfig;

            Assert.IsNotNull(adalAppConfig, "Unexpected app configuration initialized.");

            Assert.IsNotNull(client.credentialCache, "Credential cache not initialized.");
            Assert.IsInstanceOfType(client.credentialCache, typeof(AdalCredentialCache), "Unexpected credential cache initialized.");

            Assert.IsNotNull(client.HttpProvider, "HTTP provider not initialized.");
            Assert.IsInstanceOfType(client.HttpProvider, typeof(HttpProvider), "Unexpected HTTP provider initialized.");

            Assert.IsNotNull(client.serviceInfoProvider, "Service info provider not initialized.");
            Assert.IsInstanceOfType(client.serviceInfoProvider, typeof(AdalServiceInfoProvider), "Unexpected service info provider initialized.");

            Assert.AreEqual(ClientType.Business, client.ClientType, "Unexpected client type set.");
        }

        [TestMethod]
        public void GetClientUsingCustomAuthentication_InitializeDefaults()
        {
            var baseEndpointUrl = "https://resource/";

            var client = BusinessClientExtensions.GetClientUsingCustomAuthentication(
                baseEndpointUrl,
                this.authenticationProvider.Object) as OneDriveClient;

            var clientServiceInfoProvider = client.serviceInfoProvider as ServiceInfoProvider;

            Assert.IsNotNull(clientServiceInfoProvider, "Unexpected service info provider initialized for client.");
            Assert.AreEqual(this.authenticationProvider.Object, clientServiceInfoProvider.AuthenticationProvider, "Unexpected authentication provider set.");
            Assert.IsInstanceOfType(client.HttpProvider, typeof(HttpProvider), "Unexpected HTTP provider set.");
            Assert.IsNull(client.credentialCache, "Unexpected credential cache set.");
            Assert.AreEqual(ClientType.Business, client.ClientType, "Unexpected client type set.");
        }

        [TestMethod]
        [ExpectedException(typeof(OneDriveException))]
        public async Task GetSilentlyAuthenticatedClientAsync_RefreshTokenRequired()
        {
            try
            {
                var client = await BusinessClientExtensions.GetSilentlyAuthenticatedClientAsync(
                    new BusinessAppConfig
                    {
                        ActiveDirectoryAppId = "appId",
                        ActiveDirectoryServiceResource = "https://localhost/resource/"
                    },
                    /* refreshToken */ null,
                    this.credentialCache.Object,
                    this.httpProvider.Object);
            }
            catch (OneDriveException exception)
            {
                Assert.AreEqual(OneDriveErrorCode.AuthenticationFailure.ToString(), exception.Error.Code, "Unexpected error thrown.");
                Assert.AreEqual("Refresh token is required for silently authenticating a business client.", exception.Error.Message, "Unexpected error thrown.");

                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(OneDriveException))]
        public async Task GetSilentlyAuthenticatedClientAsync_ServiceResourceRequired()
        {
            try
            {
                var client = await BusinessClientExtensions.GetSilentlyAuthenticatedClientAsync(
                    new BusinessAppConfig
                    {
                        ActiveDirectoryAppId = "appId",
                    },
                    "refreshToken",
                    this.credentialCache.Object,
                    this.httpProvider.Object);
            }
            catch (OneDriveException exception)
            {
                Assert.AreEqual(OneDriveErrorCode.AuthenticationFailure.ToString(), exception.Error.Code, "Unexpected error thrown.");
                Assert.AreEqual("ActiveDirectoryServiceResource is required for silently authenticating a business client.", exception.Error.Message, "Unexpected error thrown.");

                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(OneDriveException))]
        public async Task GetSilentlyAuthenticatedWebClientAsync_ClientCertificateOrSecretRequired()
        {
            try
            {
                var client = await BusinessClientExtensions.GetSilentlyAuthenticatedWebClientAsync(
                    new BusinessAppConfig
                    {
                        ActiveDirectoryAppId = "appId",
                    },
                    "refresh",
                    this.credentialCache.Object,
                    this.httpProvider.Object);
            }
            catch (OneDriveException exception)
            {
                Assert.AreEqual(OneDriveErrorCode.AuthenticationFailure.ToString(), exception.Error.Code, "Unexpected error thrown.");
                Assert.AreEqual("Client certificate or client secret is required for authenticating a business web client.", exception.Error.Message, "Unexpected error thrown.");

                throw;
            }
        }

        [TestMethod]
        public void GetWebClientUsingAppOnlyAuthentication()
        {
            var appId = "appId";
            var tenant = "tenant";

            var clientCertificate = new X509Certificate2(@"Certs\testwebapplication.pfx", "password");

            var client = BusinessClientExtensions.GetWebClientUsingAppOnlyAuthentication(
                new BusinessAppConfig
                {
                    ActiveDirectoryAppId = appId,
                    ActiveDirectoryClientCertificate = clientCertificate,
                    ActiveDirectoryServiceResource = serviceResourceId,
                },
                serviceResourceId,
                tenant,
                this.credentialCache.Object,
                this.httpProvider.Object) as OneDriveClient;

            Assert.AreEqual(
                string.Format(
                Constants.Authentication.OneDriveBusinessBaseUrlFormatString,
                serviceResourceId.TrimEnd('/'),
                "v2.0"),
                client.appConfig.ActiveDirectoryServiceEndpointUrl,
                "Unexpected service endpoint URL initialized for app config.");

            Assert.AreEqual(
                string.Format(Constants.Authentication.ActiveDirectoryAuthenticationServiceUrlFormatString, tenant),
                client.appConfig.ActiveDirectoryAuthenticationServiceUrl,
                "Unexpected authentication service URL.");

            Assert.IsInstanceOfType(client.serviceInfoProvider, typeof(AdalAppOnlyServiceInfoProvider), "Unexpected authentication provider.");
        }
    }
}
