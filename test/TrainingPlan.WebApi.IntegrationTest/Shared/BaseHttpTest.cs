using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace TrainingPlan.WebApi.IntegrationTest.Shared
{
    public abstract class BaseHttpTest : IDisposable
    {
        protected BaseHttpTest()
        {
            var builder = new WebHostBuilder()
                .UseEnvironment(Environment)
                .ConfigureServices(ConfigureServices)
                .UseStartup<Startup>();

            Server = new TestServer(builder);
            Client = Server.CreateClient();
            Client.BaseAddress = BaseAddress;
        }

        protected TestServer Server { get; }
        protected HttpClient Client { get; }

        private static Uri BaseAddress => new Uri("http://localhost");
        private static string Environment => "Testing";

        protected virtual void ConfigureServices(IServiceCollection services)
        {
        }

        #region IDisposable Support

        private bool _disposedValue;

        private void Dispose(bool disposing)
        {
            if (_disposedValue) return;
            if (disposing)
            {
                Client.Dispose();
                Server.Dispose();
            }

            _disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}