﻿namespace AestheticWordle.Api.Dtos;

public class AccountRegistrationDto
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}

