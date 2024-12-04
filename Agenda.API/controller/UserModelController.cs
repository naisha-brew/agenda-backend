using System.Text.Json;
using Agenda.API.dto;
using Agenda.API.models;
using Agenda.API.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Agenda.API.controller;
[Route("api/[controller]")]
[ApiController]
[Authorize]
[EnableCors("AllowAll")]
public class UserModelController : ControllerBase
{
    private readonly UserService _userService;
    private readonly PasswordHasher<UserModel> _passwordHasher;
    private readonly IConfiguration _configuration;

    public UserModelController(UserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _passwordHasher = new PasswordHasher<UserModel>();
        _configuration = configuration;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login([FromBody] ULoginRequest request)
    {
        var user = _userService.GetByIdAsync(request.Email).Result;
        if (user == null) return NotFound("Invalid username or password");
        var result = _passwordHasher.VerifyHashedPassword(user,user.Userpwd ,request.Password);
        if (result == PasswordVerificationResult.Success)
        {
            return Ok(new UserResponse(user.Email, user.Username, _userService.GeneretaToken(user.Email)));
        }

        return NotFound("Invalid username or password");
    }

    [HttpPost]
    [Route("register")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUser(UserModel model)
    {
        try
        {
            // Hash the user's password
            model.Userpwd = _passwordHasher.HashPassword(model, model.Userpwd);

            // Call the service to create the user
            var result = await _userService.CreateAsync(model);

            // Return appropriate response
            if (result)
            {
                return Ok(true); // Or some success message
            }

            return BadRequest("User creation failed.");
        }
        catch (Exception ex)
        {
            // Log the exception (consider using ILogger for logging)
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpGet]
    [Route("getUser")]
    public IActionResult GetUser(string cpf)
    {
        var user = _userService.GetByIdAsync(cpf);
        if (user.Result is not null)
        {
            return Ok(user.Result);
        }
        return BadRequest("User not found");
    }

    [HttpPut]
    [Route("updateUser")]
    public IActionResult UpdateUser(string cpf, string password)
    {
        try
        {
            var result = _userService.UpdatePasswordAsync(password, cpf);
            if (result.Result)
            {
                return Ok("User updated");
            }
            return NotFound("User not found");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    [HttpDelete]
    [Route("deleteUser")]
    public IActionResult DeleteUser(string cpf)
    {
        try
        {
            var result = _userService.DeleteByIdAsync(cpf);
            if (result.Result)
            {
                return Ok("User deleted");
            }
            return NotFound("User not found");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}