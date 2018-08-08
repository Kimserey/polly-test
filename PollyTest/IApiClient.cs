using System.Net.Http;
using System.Threading.Tasks;

namespace PollyTest
{
    public interface IApiClient
    {
        Task<HttpResponseMessage> GetNotFound();
        Task<HttpResponseMessage> GetException();
        Task<HttpResponseMessage> GetTimeout();
    }
}
