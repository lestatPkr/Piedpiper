using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Piedpiper.Host.Modules.Investors
{

    [Route("/investors")]
    public class InvestorsQueriesApi : Controller
    {
        static readonly ILogger Log = Serilog.Log.ForContext<InvestorsQueriesApi>();

        InvestorsQueryService Service { get; }

        public InvestorsQueriesApi(InvestorsQueryService service) => Service = service;

        [HttpGet, Route("getinvestor")]
        public Task<IActionResult> When([FromQuery] Contracts.Investors.V1.GetDashboard qry)
            => RunQuery(qry, () => Service.GetDashboard(qry, HttpContext.RequestAborted));

        async Task<IActionResult> RunQuery<T, TResult>(T query, Func<Task<TResult>> runQuery)
        {
            Log.Information(query.ToString());
            var result = await runQuery();
            return Ok(result);
        }
    }
}
