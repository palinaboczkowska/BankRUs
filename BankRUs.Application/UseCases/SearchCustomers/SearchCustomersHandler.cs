using BankRUs.Api.Configuration;
using BankRUs.Application.Abstractions;
using BankRUs.Application.Common;
using BankRUs.Application.UseCases.GetUsers;
using Microsoft.Extensions.Options;

namespace BankRUs.Application.UseCases.SearchCustomers
{
    public sealed class SearchCustomersHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IOptions<QueryParamsOptions> _options;

        public SearchCustomersHandler(
            IUserRepository userRepository,
            IOptions<QueryParamsOptions> options)
        {
            _userRepository = userRepository;
            _options = options;
        }

        public async Task<PagedResult<UserResult>> Handle(SearchCustomersQuery query)
        {
            var maxPageSize = _options.Value.MaxPageSize;

            var page = query.Page < 1 ? 1 : query.Page;
            var pageSize = query.PageSize > maxPageSize
                ? maxPageSize
                : query.PageSize;

            return await _userRepository.SearchAsync(
                page: page,
                pageSize: pageSize,
                ssn: query.Ssn
            );
        }
    }

}
