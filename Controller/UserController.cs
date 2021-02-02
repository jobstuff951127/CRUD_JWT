using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        public UserController(TokaContext context)
        {
            _context = context;
        }

        // [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CostumerDTO>>> GetCostumers()
        {
            try
            {
                var costumers = await _context.Costumers
                .Select(x => new CostumerDTO
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Cellphone = x.Cellphone,
                    Adress = x.Adress,
                    BirthDate = x.BirthDate

                }).ToListAsync();

                if (costumers.Count() == 0)
                {
                    return NotFound();
                }

                return Ok(costumers);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        //Este metodo es el endpoint "get one", es asincrono y devuelve un registro
        [HttpGet("{id}")]
        public async Task<ActionResult<Costumer>> GetCostumer(int id)
        {
            try
            {
                var costumerObj = await _context.Costumers.FindAsync(id);
                if (costumerObj is null)
                {
                    return NotFound();

                }

                return Ok(costumerObj);
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
                var costumerObj = await _context.Costumers.FindAsync(id);

                costumerObj.FirstName = costumerDTO.FirstName;
                costumerObj.LastName = costumerDTO.LastName;
                costumerObj.Cellphone = costumerDTO.Cellphone;
                costumerObj.Adress = costumerDTO.Adress;
                costumerObj.BirthDate = costumerDTO.BirthDate;

                await _context.SaveChangesAsync();

                if (costumerObj is null)
                    return NotFound();

                return NoContent();
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
        public async Task<ActionResult<Costumer>> CreateCostumer(Costumer costumerObj)
        {

            try
            {
                _context.Costumers.Add(costumerObj);
                await _context.SaveChangesAsync();

                //El NoContent regresa el 204
                return NoContent();
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
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        #endregion
    }
}