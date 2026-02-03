using System;
using System.Collections.Generic;
using System.Text;

namespace BankRUs.Application.Common
{
    public record PagedResult<T>(
    IReadOnlyList<T> Data,
    int Page,
    int PageSize,
    int TotalItems,
    int TotalPages
);

}
