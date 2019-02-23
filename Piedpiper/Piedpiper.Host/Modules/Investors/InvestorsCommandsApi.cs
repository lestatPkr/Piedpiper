using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Piedpiper.Host.Modules.Investors
{
    [Route("/investors")]
    public class InvestorsCommandsApi : Controller
    {
        static readonly ILogger Log = Serilog.Log.ForContext<InvestorsCommandsApi>();

        readonly InvestorsApplicationService _service;

        public InvestorsCommandsApi(InvestorsApplicationService service) => _service = service;

        [HttpPost, Route("register")]
        public Task<IActionResult> When([FromBody] Piedpiper.Contracts.Investors.V1.Register cmd) => Handle(cmd);

        [HttpPost, Route("changename")]
        public Task<IActionResult> When([FromBody] Piedpiper.Contracts.Investors.V1.ChangeName cmd) => Handle(cmd);

        [HttpPost, Route("changescreeningcriteria")]
        public Task<IActionResult> When([FromBody] Piedpiper.Contracts.Investors.V1.ChangeScreeningCriteria cmd) => Handle(cmd);

        [HttpPost, Route("registercompany")]
        public Task<IActionResult> When([FromBody] Piedpiper.Contracts.Investors.V1.RegisterCompany cmd) => Handle(cmd);

        async Task<IActionResult> Handle<T>(T cmd) where T : class
        {
            Log.Information(cmd.ToString());
            await _service.Handle(cmd);
            return Ok();
        }
    }
}
