//using System;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Serilog;

//namespace Piedpiper.Host.Modules.ClassifiedAds
//{

//    [Route("/ads")]
//    public class InvestorsQueriesApi : Controller
//    {
//        static readonly ILogger Log = Serilog.Log.ForContext<InvestorsQueriesApi>();

//        InvestorsQueryService Service { get; }

//        public InvestorsQueriesApi(InvestorsQueryService service) => Service = service;

//        [HttpGet, Route("available")]
//        public Task<IActionResult> When([FromQuery] Piedpiper.Contracts.Investors.V1.GetInvestor qry)
//            => RunQuery(qry, () => Service.GetInvestor(qry, HttpContext.RequestAborted));

//        async Task<IActionResult> RunQuery<T, TResult>(T query, Func<Task<TResult>> runQuery)
//        {
//            Log.Information(query.ToString());
//            var result = await runQuery();
//            return Ok(result);
//        }
//    }
//}
