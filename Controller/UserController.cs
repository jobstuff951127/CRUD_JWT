using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Controller.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.DTO;

namespace Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //DI at its finest
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly JwtGenerator _jwtGenerator;
        public UserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, JwtGenerator jwtGenerator)
        {
            _jwtGenerator = jwtGenerator;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(UserDTO userObj)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(userObj.Email);

                if (user is null)
                    return NotFound();

                var result = await _signInManager
                    .CheckPasswordSignInAsync(user, userObj.Password, false);

                if (result.Succeeded)
                {
                    return new UserDTO
                    {
                        Token = _jwtGenerator.CreateToken(user),
                        UserName = user.UserName
                    };
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

        }
    }
}