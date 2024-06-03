using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace backend.Services
{

    public class CustomHeaderHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("Client-Identifier", "oppgave-oscarRomeo");
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
