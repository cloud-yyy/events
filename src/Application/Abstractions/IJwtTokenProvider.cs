using System;
using Domain.Entities;

namespace Application.Abstractions;

public interface IJwtTokenProvider
{
    public string GenerateToken(User user);
}
