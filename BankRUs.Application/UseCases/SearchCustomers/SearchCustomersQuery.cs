using System;
using System.Collections.Generic;
using System.Text;

namespace BankRUs.Application.UseCases.SearchCustomers
{
    public sealed record SearchCustomersQuery(
    int Page,
    int PageSize,
    string Ssn);

}
