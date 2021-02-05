using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
// using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Controller
{
    //Aqui se asigna la ruta
    [Route("api/[controller]")]
    //Nos permite no tener que esspecificar el [fromBody]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly TokaContext _context;
        private readonly IMapper _mapper;
        public UserController(TokaContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        // [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CostumerDTO>>> GetCostumers()
        {
            try
            {
                var costumers = await _context.Costumers.Take(50)
                .Select(c => _mapper.Map<CostumerDTO>(c)).ToListAsync();

                return costumers.Any() ? Ok(costumers) : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        //Este metodo es el endpoint "get one", es asincrono y devuelve un registro
        [HttpGet("{id}")]
        public async Task<ActionResult<CostumerDTO>> GetCostumer(int id)
        {
            try
            {
                var costumers = await _context.Costumers.FindAsync(id);
                return costumers is null ? NotFound() : Ok(_mapper.Map<CostumerDTO>(costumers));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }


        #region UPDATE

        //Este metodo es el endpoint "update", es asincrono y devuelve un 204 en caso
        //caso de que el registro a editar haya sido encontrado por medio del id y editado
        //o un 404 en caso contrario o un 500 en caso de algun error de conexion u otro

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(int id, CostumerDTO costumerDTO)
        {
            try
            {
                var costumers = await _context.Costumers.FindAsync(id);

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
        //Este metodo es el "endpoint" crear o insertar, 
        //retornando un 204 en caso de que la transaccion haya sido exitosa o un 500 en caso contrario
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
        //Este endpoint es el delete, primero busca el registro a eliminar 
        //luego en caso de que no exista devuelve un 404
        //En caso contrario devuelve un 204, una respuesta sin contenido

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCostumer(int id)
        {
            try
            {
                var contactObj = await _context.Costumers.FindAsync(id);

                if (contactObj is null)
                    return NotFound();

                //Este es un delete en EF Core
                _context.Costumers.Remove(contactObj);
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