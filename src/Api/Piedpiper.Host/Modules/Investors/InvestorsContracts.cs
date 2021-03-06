using System;
using System.Collections.Generic;
using Piedpiper.Domain.Screening;

// ReSharper disable CheckNamespace

namespace Piedpiper.Contracts
{
    public static partial class Investors
    {
        public static partial class V1
        {
            public class Register
            {
                public Guid InvestorId { get; set; }
                public string Name { get; set; }

                public override string ToString() => $"Registering Investor '{InvestorId}'...";
            }
            public class ChangeName
            {
                public Guid InvestorId { get; set; }
                public string Name { get; set; }

                public override string ToString() => $"Changing investor name '{InvestorId}'...";
            }

            public class ChangeScreeningCriteria
            {
                public Guid InvestorId { get; set; }
                public List<string> MustHave { get; set; }
                public List<string> NiceToHave { get; set; }
                public List<string> SuperNiceToHave { get; set; }

                public override string ToString() => $"Changing investor screening criteria '{InvestorId}'...";
            }

            public class RegisterCompany
            {
                public class CompanyScreeningData
                {
                    public int KPI { get; set; }
                    public KPIStatus Status { get; set; }

                }
                public Guid InvestorId { get; set; }
                public Guid CompanyId { get; set; }
                public string CompanyName { get; set; }
                public List<CompanyScreeningData> ScreeningData { get; set; }

                public override string ToString() => $"Registering company {CompanyId} in investor '{InvestorId}'...";


            }

            public class GetDashboard
            {
                public class Company
                {
                    public Guid CompanyId { get; set; }
                    public string Name { get; set; }
                    public double Score { get; set; }
                    public bool MustHavesMissing { get; set; }
                    public int NiceToHavePercentage { get; set; }
                    public int SuperNiceToHavePercentage { get; set; }
                    public int MissingKpis { get; set; }
                    public int NoMetKpis { get; set; }
                    public int MatchStatus { get; set; }
                }
                public Guid InvestorId { get; set; }

                public class Result
                {
                    public Guid InvestorId { get; set; }
                    public string Name { get; set; }
                    public List<string> MustHave { get; set; }
                    public List<string> NiceToHave { get; set; }
                    public List<string> SuperNiceToHave { get; set; }
                    public List<Company> Companies { get; set; }
                    
                }
            }
        }
    }

    public static class Shared
    {
        public static class V1
        {
            public class ItemsPage<T>
            {
                public ItemsPage() { }

                public ItemsPage(int page, int pageSize, int totalPages, int totalItems, params T[] items) {
                    Page       = page;
                    PageSize   = pageSize;
                    TotalPages = totalPages;
                    TotalItems = totalItems;
                    Items      = items;
                }

                public int Page { get; set; }
                public int PageSize { get; set; }
                public int TotalPages { get; set; }
                public int TotalItems { get; set; }
                public T[] Items { get; set; }
            }

            public enum ClassifiedAdStatus { Registered, Published, Removed, Sold }
        }
    }
}
