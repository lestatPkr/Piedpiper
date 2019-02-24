using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Piedpiper.Domain.Screening;

namespace Piedpiper.Domain.Services
{
    public class ScoreCalculator
    {
        private readonly ScreeningCriteria _screeningCriteria;

        public ScoreCalculator(ScreeningCriteria screeningCriteria)
        {
            _screeningCriteria = screeningCriteria;
        }
        public ScreeningScore CalculateScore(List<ScreeningData> data)
        {
            var score = 0.0;
            data.Where(d=> d.Status == KPIStatus.Yes)?.ToList().ForEach(d =>
            {
                if (IsMustHave(d)) score += _screeningCriteria.MustHaveWeigth;
                if (IsNiceToHave(d)) score += _screeningCriteria.NiceToHaveWeigth;
                if (IsSuperNiceToHave(d)) score += _screeningCriteria.SuperNiceToHaveWeigth;
            });
            return new ScreeningScore(
                score,
                AnyMustHaveMissing(data),
                CalculateNiceToHavePercentage(data), 
                CalculateSuperNiceToHavePercentage(data),
                data.Count(d=> d.Status == KPIStatus.Unknowns),
                    data.Count(d=> d.Status == KPIStatus.No));
        }

        private int CalculateNiceToHavePercentage(List<ScreeningData> data)
        {
            var metValues = data.Where(d => d.Status == KPIStatus.Yes).Select(d => d.Kpi)?.ToList();
            var percentage = _screeningCriteria.NiceToHave.Count(d => metValues.Any()) * 100 / _screeningCriteria.NiceToHave.Count;
            return percentage;
        }
        private int CalculateSuperNiceToHavePercentage(List<ScreeningData> data)
        {
            var metValues = data.Where(d => d.Status == KPIStatus.Yes).Select(d => d.Kpi)?.ToList();
            var percentage = _screeningCriteria.SuperNiceToHave.Count(d => metValues.Any()) * 100 / _screeningCriteria.SuperNiceToHave.Count;
            return percentage;
        }
        private bool AnyMustHaveMissing(List<ScreeningData> data)
        {
            var missingOrNoValues = data.Where(d => d.Status != KPIStatus.Yes);
            return missingOrNoValues.Any(value => IsMustHave(value));
        }
        private bool IsMustHave(ScreeningData data)
        {
           return  _screeningCriteria.MustHave.Contains(data.Kpi);
        }
        private bool IsNiceToHave(ScreeningData data)
        {
            return _screeningCriteria.NiceToHave.Contains(data.Kpi);
        }
        private bool IsSuperNiceToHave(ScreeningData data)
        {
            return _screeningCriteria.SuperNiceToHave.Contains(data.Kpi);
        }
    }
}
