using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace charity_website_backend.Common.Services
{
    public class SessionService:ISessionService
    {
    
        public HttpContext? HttpContext { get; }
        public IConfiguration Configuration { get; }
        public int Id { get; }
        public int UserType{ get; }
 
        public SessionService(IHttpContextAccessor accessor, IConfiguration config)
        {
            this.HttpContext = accessor.HttpContext ?? throw new ArgumentNullException(nameof(accessor.HttpContext));
            this.Configuration = config;


            var Key = Encoding.UTF8.GetBytes(Configuration["JWT:Key"]);
            var token = this.HttpContext.Request
                                .Headers["Authorization"]
                                .ToString()
                                .Substring("Bearer ".Length);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Key),
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken securityToken;

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }
            var claims = principal.Claims.ToList();


            //claims.Where(x => x.Type == "branchId").First().Value, out int b);
            int.TryParse(claims.Where(x => x.Type == "Id").First().Value, out int b);
            this.Id = b;
            int.TryParse(claims.Where(x => x.Type == "UserType").First().Value, out int u);
            this.UserType = u;
        }
        public HttpContext GetCurrentContext()
        {
            return this.HttpContext;
        }
    }
}
