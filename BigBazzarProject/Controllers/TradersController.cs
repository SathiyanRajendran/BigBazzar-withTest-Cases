using BigBazzar.Helper;
using BigBazzar.Models;
using BigBazzar.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BigBazzar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TradersController : ControllerBase
    {
        private readonly ITraderRepo _repository;
        private readonly IConfiguration _configuration;
        public TradersController(ITraderRepo repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<ActionResult<List<Traders>>> GetTraders()
        {
            return await _repository.GetAllTraders();
        }
        [Authorize]
        [HttpGet("{TraderId}")]
        public async Task<ActionResult<List<Products>>> GetProductByTraderId(int id)//It can shows the products details added by the Traders
        {
            return await _repository.GetProductByTraderId(id);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Traders>> GetTrader(int id) //It can shows the trader Details
        {
            return Ok(await _repository.GetTraderbyId(id));
            //var trader= await _repository.GetTraderbyId(id);
            //return Ok(trader);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Traders>> PutTrader(int id,Traders T)
        {
            Traders c = T;
            var PasswordHash = EncodePassword.GetMd5Hash(c.Password);
            T.Password = PasswordHash;
            T.ConfirmPassword = PasswordHash;
            return await _repository.UpdateTraders(id,T);

        }
        [HttpPost]
        public async Task<ActionResult<Traders>> PostTrader(Traders T)
        {
            Traders c = T;
            var PasswordHash = EncodePassword.GetMd5Hash(c.Password);
            T.Password = PasswordHash;
            T.ConfirmPassword = PasswordHash;
            return await _repository.AddNewTraders(T);
        }
        [HttpPost("Login")]
        public async Task<ActionResult<TraderToken>> TraderLogin(Traders T)
        {
            Traders c = T;
            var PasswordHash = EncodePassword.GetMd5Hash(c.Password);
            T.Password = PasswordHash;
            T.ConfirmPassword = PasswordHash;
            Traders traders = await _repository.TraderLogin(T);
            TraderToken Tt = new TraderToken();
            if (traders != null)
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,traders.TraderEmail),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                var token = GetToken(authClaims);
                string s = new JwtSecurityTokenHandler().WriteToken(token);
                Tt.Token = s;
                Tt.traders = traders;
                return Tt;

            }
            return Ok(Tt);
        

            //Traders c = T;
            //var PasswordHash = EncodePassword.GetMd5Hash(c.Password);
            //T.Password = PasswordHash;
            //T.ConfirmPassword = PasswordHash;
           
            //return await _repository.TraderLogin(T);
            //-------------------------------------------------
            //TraderToken traderlogin = await _repository.TraderLogin(T);
            //if (string.IsNullOrEmpty(traderlogin.Token))
            //{
            //    return Unauthorized();
            //}
            //return Ok(traderlogin);
            //--------------------------------------------------
            //var traderlogin = await _repository.TraderLogin(T);
            //if (traderlogin == null)
            //{
            //    return BadRequest("Invalid Credentials");
            //}
            //return traderlogin;
        }
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));



            var token = new JwtSecurityToken(
                 issuer: _configuration["JWT:ValidIssuer"],
                 audience: _configuration["JWT:ValidAudience"],
                 expires: DateTime.Now.AddMinutes(30),
                 claims: authClaims,
                 signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                 ); ;



            return token;
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrader(int id)
        {
            bool ans= await _repository.DeleteTraders(id);

            if(ans)
                return Ok();
            else
                return BadRequest();
        }

    }
}
