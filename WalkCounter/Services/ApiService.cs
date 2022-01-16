using System.Threading.Tasks;
using Refit;
using WalkCounterClient.Models;

namespace WalkCounterClient.Services
{
    public interface IApiService
    {
        [Post("/walks")]
        Task<SaveWalkResponse> SaveWalk(
            [Body] Walk request,
            [Header("x-cassandra-token")] string token);

        [Get("/walks/rows")]
        Task<GetWalksResponse> GetAllWalks(
            [Header("x-cassandra-token")] string token);

        [Get("/walks/{year}")]
        Task<GetWalksResponse> GetYearWalks(
            int year,
            [Header("x-cassandra-token")] string token);
    }
}
