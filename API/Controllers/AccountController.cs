using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AutoMapper;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
        {
            this._tokenService = tokenService;
            this._context = context;
            this._mapper = mapper;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Regiser(RegisterDto registerdto)
        {
            if (await UserExists(registerdto.Username)) return BadRequest("Username is taken");

            var user = _mapper.Map<AppUser>(registerdto);

            using var hmac = new HMACSHA512();
            
            user.UserName = registerdto.Username.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerdto.Password));
            user.PasswordSalt = hmac.Key;
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto 
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                KnownAs = user.KnownAs
                
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto logindto)
        {
            var user = await _context.Users
            .Include(p=> p.Photos)
            .SingleOrDefaultAsync(a => a.UserName == logindto.Username);
            if (user == null) return BadRequest("invalid Username");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(logindto.Password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");

            }
            return new UserDto 
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos?.FirstOrDefault(x=>x.IsMain)?.Url,
                 KnownAs = user.KnownAs
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(a => a.UserName == username.ToLower());
        }

    }
}