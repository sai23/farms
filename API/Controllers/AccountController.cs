using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController :BaseApiController
    {
        private readonly DataContext _context;

        public AccountController(DataContext context)
        {
            this._context = context;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<AppUser>> Regiser(RegisterDto registerdto)
        {
          using var hmac = new HMACSHA512();
          var user  = new AppUser{
                    UserName = registerdto.Username,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerdto.Password)),
                    PasswordSalt = hmac.Key
          };
          _context.Users.Add(user);
          await _context.SaveChangesAsync();
          return user;
        }

       
    } 
}