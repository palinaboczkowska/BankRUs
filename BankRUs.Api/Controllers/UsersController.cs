using BankRUs.Api.Configuration;
using BankRUs.Api.Dtos.BankAccounts;
using BankRUs.Api.Dtos.Users;
using BankRUs.Application.Abstractions;
using BankRUs.Application.UseCases.GetSingleUser;
using BankRUs.Application.UseCases.GetUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BankRUs.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "CustomerService")]
public class CustomersController : ControllerBase
{
    private readonly IUserRepository _repo;
    private readonly IOptions<QueryParamsOptions> _options;

    public CustomersController(
        IUserRepository repo,
        IOptions<QueryParamsOptions> options)
    {
        _repo = repo;
        _options = options;
    }

    [HttpGet]
    public async Task<IActionResult> Get(int page = 1, int pageSize = 20)
    {
        var handler = new GetUsersHandler(_repo, _options);
        var result = await handler.Handle(new GetUsersQuery(page, pageSize));

        var dto = result.Data
            .Select(c => new UserDto(c.Id, c.FirstName, c.LastName, c.Email))
            .ToList();

        return Ok(new PagedResponse<UserDto>(
            Data: dto,
            Page: result.Page,
            PageSize: result.PageSize,
            TotalItems: result.TotalItems,
            TotalPages: result.TotalPages
        ));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var handler = new GetUserByIdHandler(_repo);
        var result = await handler.Handle(new GetUserByIdQuery(id));

        if (result is null)
            return NotFound();

        var dto = new CustomerDetailsDto(
            Id: result.Id,
            FirstName: result.FirstName,
            LastName: result.LastName,
            Email: result.Email,
            BankAccounts: result.BankAccounts
                .Select(a => new BankAccountResponseDto(
                    Name: a.Name,
                    AccountId: a.Id,
                    AccountNumber: a.AccountNumber,
                    Balance: a.Balance
                ))
                .ToList()
        );

        return Ok(dto);
    }

}