using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PaymentAPI.src.Models;
using PaymentAPI.src.Persistence;

namespace PaymentAPI.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendaController : ControllerBase
    {
        private DatabaseContext _context { get; set; }

        public VendaController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public ActionResult<List<Venda>> Get([FromRoute] int id)
        {
            var result = _context.Vendas.Include(p => p.Produtos).Where(x => x.IdPedido == id);
            if (result is null)
                return NoContent();
            
            return Ok(result);
                     
        }

        [HttpPost]
        public ActionResult<Venda> Post([FromBody]Venda venda)
        {
            try
            {
                _context.Vendas.Add(venda);
                _context.SaveChanges();
            }
            catch(System.Exception)
            {
                return BadRequest();
            }

            return Created("Cadastrada", venda);
        }       

        [HttpPut("{id}")]
        public ActionResult Update([FromRoute]int id, [FromBody]Venda venda)
        {
            _context.Vendas.Update(venda);
            _context.SaveChanges();

            return Ok($"Dados da Venda {id} atualizado com sucesso!");
        }

        [HttpPatch("{id}")]
        public ActionResult UpdateStatus(int id)
        {
            return Ok($"Status da Venda {id} atualizado com sucesso!");
        }

        [HttpDelete("{id}")]
        public ActionResult<Venda> Delete([FromRoute]int id)
        {
            var vendaBanco = _context.Vendas.Find(id);

            if(vendaBanco == null)
                return BadRequest( new
                {
                    msg = "Venda Inexistente, solicitção inválida.", status = 400
                });
            
            _context.Vendas.Remove(vendaBanco);
            _context.SaveChanges();
            return Ok(new { msg = $"Venda {vendaBanco} excluída com sucesso!", status = 200 });
        }
    }
}
