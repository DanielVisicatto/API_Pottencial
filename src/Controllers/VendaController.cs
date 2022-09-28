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
        private VendaContext _context { get; set; }

        public VendaController(VendaContext context)
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

            return Created("Cadastrada", StatusCode(201));
        }       

        [HttpPut("{id}")]
        public ActionResult<Venda> Update([FromRoute]int id, [FromBody]Venda venda)
        {
            _context.Vendas.Update(venda);
            _context.SaveChanges();

            return Accepted($"Dados da Venda {id} atualizado com sucesso!", StatusCode(202));
        }        

        [HttpPatch("{WaitingToApproved}")]
        public ActionResult<Venda> UpdateWaitingToApproved([FromRoute]int id, [FromBody]Venda venda)
        {
            var vendaBanco = _context.Vendas.Find(EnumStatusVenda.AguardandoPagamento);
            if (vendaBanco.Status == EnumStatusVenda.AguardandoPagamento)
            {
                vendaBanco.Status = EnumStatusVenda.PagamentoAprovado;
                _context.Vendas.Update(venda);
                _context.SaveChanges();
                return Accepted($"Status da Venda {id} atualizado com sucesso!", StatusCode(202));
            }                
            else
                return Unauthorized(StatusCode(401));
        }

        [HttpPatch("{WaitingToCanceled}")]
        public ActionResult<Venda> UpdateWaitingToCanceled([FromRoute] int id, [FromBody] Venda venda)
        {
            var vendaBanco = _context.Vendas.Find(EnumStatusVenda.AguardandoPagamento);
            if (vendaBanco.Status == EnumStatusVenda.AguardandoPagamento)
            {
                vendaBanco.Status = EnumStatusVenda.Cancelada;
                _context.Vendas.Update(venda);
                _context.SaveChanges();
                return Accepted($"Status da Venda {id} atualizado com sucesso!", StatusCode(202));
            }
            else
                return Unauthorized(StatusCode(401));
        }

        [HttpPatch("{ApprovedToInTransport}")]
        public ActionResult<Venda> UpdateApprovedToInTransport([FromRoute] int id, [FromBody] Venda venda)
        {
            var vendaBanco = _context.Vendas.Find(EnumStatusVenda.PagamentoAprovado);
            if (vendaBanco.Status == EnumStatusVenda.PagamentoAprovado)
            {
                vendaBanco.Status = EnumStatusVenda.EnviadoParaTransportadora;
                _context.Vendas.Update(venda);
                _context.SaveChanges();
                return Accepted($"Status da Venda {id} atualizado com sucesso!", StatusCode(202));
            }
            else
                return Unauthorized(StatusCode(401));
        }

        [HttpPatch("{ApprovedToCancelled}")]
        public ActionResult<Venda> UpdateApprovedToCancelled([FromRoute] int id, [FromBody] Venda venda)
        {
            var vendaBanco = _context.Vendas.Find(EnumStatusVenda.PagamentoAprovado);
            if (vendaBanco.Status == EnumStatusVenda.PagamentoAprovado)
            {
                vendaBanco.Status = EnumStatusVenda.Cancelada;
                _context.Vendas.Update(venda);
                _context.SaveChanges();
                return Accepted($"Status da Venda {id} atualizado com sucesso!", StatusCode(202));
            }
            else
                return Unauthorized(StatusCode(401));
        }

        [HttpPatch("{InTransportToDelivered}")]
        public ActionResult<Venda> UpdateInTransportToDelivered([FromRoute] int id, [FromBody] Venda venda)
        {
            var vendaBanco = _context.Vendas.Find(EnumStatusVenda.EnviadoParaTransportadora);
            if (vendaBanco.Status == EnumStatusVenda.EnviadoParaTransportadora)
            {
                vendaBanco.Status = EnumStatusVenda.Entregue;
                _context.Vendas.Update(venda);
                _context.SaveChanges();
                return Accepted($"Status da Venda {id} atualizado com sucesso!", StatusCode(202));
            }
            else
                return Unauthorized(StatusCode(401));
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
