using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace charity_website_backend.Common.Services
{
    public class SessionService:ISessionService
    {
        IHttpContextAccessor _httpContextAccessor;
        IConfiguration _config;
        List<Claim> claims;
        public CharitySession CharitySession = new CharitySession();

        public ISessionService Session => this;

        public SessionService(IHttpContextAccessor httpContextAccessor, IConfiguration config)
        {
            _httpContextAccessor = httpContextAccessor;
            _config = config;
            GetPrincipalFromExpiredToken(GetBearerToken());
        }
        private List<Claim> GetPrincipalFromExpiredToken(string token)
        {
            var Key = Encoding.UTF8.GetBytes(_config["JWT:Key"]);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Key),
                
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken securityToken;

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }
            claims = principal.Claims.ToList();

            ParseSession();
            return claims;
        }

        private string GetBearerToken()
        {
            var token = _httpContextAccessor
                            .HttpContext.Request
                                .Headers["Authorization"]
                                .ToString()
                                .Substring("Bearer ".Length);
            return token;

        }
        private void ParseSession()
        {
            int.TryParse(claims.Where(x => x.Type == "Id").First().Value, out int i);
            CharitySession.Id = i;
            int.TryParse(claims.Where(x => x.Type == "UserType").First().Value, out int u);
            CharitySession.UserType = u;
        }

        public CharitySession GetSession()
        {
            return CharitySession;
        }
    }
}
