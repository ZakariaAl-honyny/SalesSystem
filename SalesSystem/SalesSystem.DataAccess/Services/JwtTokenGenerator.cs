using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SalesSystem.Business.Interfaces.Services;
using SalesSystem.DTO.Common;
using SalesSystem.Models;

namespace SalesSystem.DataAccess.Services;

/// <summary>
/// JWT token generator implementation.
/// </summary>
public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(JwtSettings jwtSettings)
    {
        _jwtSettings = jwtSettings;
    }

    public string GenerateToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.GivenName, user.UserName),
            new(ClaimTypes.Role, user.RoleId.ToString()),
            new("RoleId", user.RoleId.ToString()),
            new("BranchId", user.BranchId?.ToString() ?? ""),
        };

        if (user.Role != null)
        {
            claims.Add(new("RoleName", user.Role.Name));
            claims.Add(new("SalesPermissions", user.Role.SalesPermissions.ToString()));
            claims.Add(new("InventoryPermissions", user.Role.InventoryPermissions.ToString()));
            claims.Add(new("AccountingPermissions", user.Role.AccountingPermissions.ToString()));
            claims.Add(new("SystemPermissions", user.Role.SystemPermissions.ToString()));
        }

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(_jwtSettings.ExpirationHours),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}