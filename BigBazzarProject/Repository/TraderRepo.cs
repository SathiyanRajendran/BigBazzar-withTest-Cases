using BigBazzar.Data;
using BigBazzar.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BigBazzar.Repository
{
    public class TraderRepo : ITraderRepo
    {
        private readonly BigBazzarContext _context;
        private readonly IConfiguration _configuration;
       
        public TraderRepo(BigBazzarContext context,IConfiguration configuration)
        {
            _configuration = configuration;

            _context = context;
        }

        public TraderRepo(BigBazzarContext context)
        {
            _context = context;
        }

        public async Task<Traders> AddNewTraders(Traders T)
        {
            _context.Traders.Add(T);
            foreach (var tracker in _context.ChangeTracker.Entries<Traders>())
            {
                //Console.WriteLine(tracker.State);
                Console.WriteLine(_context.ChangeTracker.DebugView.ShortView);
            }
            await _context.SaveChangesAsync();
            return T;
        }

        public async Task<bool> DeleteTraders(int TraderId)
        {
            try
            {
                Traders T = _context.Traders.Find(TraderId);
                _context.Traders.Remove(T);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;   
            }
        }

        public async Task<List<Traders>> GetAllTraders()
        {
            try
            {
                return await  _context.Traders.ToListAsync();
            }
            catch
            {
                throw new NotImplementedException();

            }
        }

        public async Task<List<Products>> GetProductByTraderId(int TraderId)
        {
            List<Products> products = await (from i in _context.Products.Include(x => x.Categories).Include(y => y.Traders) 
                                             where i.TraderId ==TraderId            //Here we show the products owned by the respective traders
                                             select i).ToListAsync();
            return products;
        }

        public async Task<Traders> GetTraderbyId(int TraderId)
        {
            return await  _context.Traders.FindAsync(TraderId);
        }
        //-------------------------------------------------------------------------------------------------

        public async Task<Traders> TraderLogin(Traders T)
        {
            //TraderToken Tt = new TraderToken();
            Traders trader = await  (from i in _context.Traders where i.TraderEmail == T.TraderEmail && i.Password == T.Password select i).FirstOrDefaultAsync();
            if (trader != null)
            {
                return trader;
                //var authClaims = new List<Claim>
                //{
                //    new Claim(ClaimTypes.Name,trader.TraderEmail),
                //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                //};




                //var token = GetToken(authClaims);
                //string s = new JwtSecurityTokenHandler().WriteToken(token);
                //Tt.Token = s;
                //Tt.traders = trader;
                //return Tt;



            }
            return null;
        }
        //private JwtSecurityToken GetToken(List<Claim> authClaims)
        //{
        //    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));



        //    var token = new JwtSecurityToken(
        //         issuer: _configuration["JWT:ValidIssuer"],
        //         audience: _configuration["JWT:ValidAudience"],
        //         expires: DateTime.Now.AddMinutes(30),
        //         claims: authClaims,
        //         signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        //         ); ;



        //    return token;
        //}
        //--------------------------------------------------------------------------------------------------------

        public async Task<Traders> UpdateTraders(int TraderId, Traders Trader)
        {
            _context.Update(Trader);
            //Console.WriteLine(tracker.State);
            foreach (var tracker in _context.ChangeTracker.Entries<Traders>())
            {
                Console.WriteLine(tracker.State);
            }
            _context.SaveChanges();
            return Trader;
        }
        private bool IsTrader(int id)
        {
            var traders = _context.Traders.Find(id);
            if(traders != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
