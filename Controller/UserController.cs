using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Controller.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly TokaContext _context;
        private readonly IMapper _mapper;
        public UserController(TokaContext context, IMapper mapper, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, JwtGenerator jwtGenerator)
        {
            _mapper = mapper;
            _context = context;
            _jwtGenerator = jwtGenerator;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(UserDTO userObj)
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
        //This is an asynchronous GET endpoint that maps the result into a DTO to send the proper response in a ternary 
        // operator     
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CostumerDTO>>> GetCostumers()
        {
            try
            {
                var costumers = await _context.Costumers.OrderByDescending(c => c.Id)
                .Take(50).Where(c => c.Status)
                .Select(c => _mapper.Map<CostumerDTO>(c)).ToListAsync();

                return costumers.Any() ? Ok(costumers) : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
        //This is an asynchronous GET by id endpoint that maps the result into a DTO in a ternary operator       
        //[Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<CostumerDTO>> GetCostumer(int id)
        {
            try
            {
                var costumers = await _context.Costumers.FirstOrDefaultAsync(c => c.Status && c.Id == id);

                return costumers is null ? NotFound() : Ok(_mapper.Map<CostumerDTO>(costumers));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }


        #region UPDATE

        //This is an asynchronous PUT endpoint that maps a DTO into Costumers

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCostumer(int id, CostumerDTO costumerDTO)
        {
            try
            {
                var costumers = await _context.Costumers
                .FirstOrDefaultAsync(c => c.Status && c.Id == id);

                if (costumers is null)
                    return NotFound();

                _mapper.Map(costumerDTO, costumers);

                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        #endregion

        #region CREATE
        //This is an asynchronous POST endpoint that maps a DTO into Costumers (DataContext Object)

        [HttpPost]
        public async Task<ActionResult<Costumer>> CreateCostumer(CostumerDTO costumerDTO)
        {
            try
            {
                _context.Costumers.Add(_mapper.Map<Costumer>(costumerDTO));
                await _context.SaveChangesAsync();
                //El NoContent regresa el 204
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
        #endregion

        #region DELETE

        //This is an asynchronous DEL endpoint that LOGICALLY delete a row from the DB

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCostumer(int id)
        {
            try
            {
                var costumers = await _context.Costumers
                .FirstOrDefaultAsync(c => c.Status && c.Id == id);

                if (costumers is null)
                    return NotFound();

                costumers.Status = false;
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        #endregion
    }
}