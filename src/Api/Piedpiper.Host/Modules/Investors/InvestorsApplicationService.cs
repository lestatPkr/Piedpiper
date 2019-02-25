using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Piedpiper.Domain.Inverstors;
using Piedpiper.Domain.Screening;
using Piedpiper.Framework;

namespace Piedpiper.Host.Modules.Investors
{
    public class InvestorsApplicationService
    {
        readonly IAggregateStore _store;
        readonly Func<DateTimeOffset> _getUtcNow;

        public InvestorsApplicationService(
            IAggregateStore store, Func<DateTimeOffset> getUtcNow)
        {
            _store = store;
            _getUtcNow = getUtcNow;
        }

        public Task Handle<T>(T cmd) where T : class
        {
            switch (cmd)
            {
                  case Contracts.Investors.V1.Register x:
                     return Execute(x.InvestorId, async inv =>
                     { 
                        inv.Register(x.InvestorId, x.Name, _getUtcNow);
                     });

                  case Contracts.Investors.V1.ChangeName x:
                      return Execute(x.InvestorId, async inv =>
                      {
                          inv.ChangeName(x.InvestorId, x.Name, _getUtcNow);
                      });
                  case Contracts.Investors.V1.ChangeScreeningCriteria x:
                      return Execute(x.InvestorId, async inv =>
                      {
                          var sc = new ScreeningCriteria
                          {
                              MustHave = x.MustHave.Select(c=> (KPI)c)?.ToList(),
                              NiceToHave = x.NiceToHave.Select(c => (KPI)c)?.ToList(),
                              SuperNiceToHave = x.SuperNiceToHave.Select(c => (KPI)c)?.ToList()
                          };
                          inv.ChangeSreeningCriteria(x.InvestorId, sc, _getUtcNow);
                      });
                  case Contracts.Investors.V1.RegisterCompany x:
                      return Execute(x.InvestorId, async inv =>
                      {
                          var name = x.CompanyName;
                          var screeningData = x.ScreeningData.Select(d => new ScreeningData
                          {
                              Status = d.Status,
                              Kpi = (KPI) d.KPI
                          }).ToList();
                          inv.RegisterCompany(x.InvestorId, x.CompanyId, name, screeningData, _getUtcNow);
                      });


                default:
                     return Task.CompletedTask;
            }
        }

        async Task Execute(InvestorId id, Func<Investor, Task> update)
        {
            var ad = await _store.Load<Investor>(id);
            await update(ad);
            await _store.Save(ad);
        }

        Task Execute(InvestorId id, Action<Investor> update)
        {
            return Execute(id, ad => { update(ad); return Task.CompletedTask; });
        }
    }
}
