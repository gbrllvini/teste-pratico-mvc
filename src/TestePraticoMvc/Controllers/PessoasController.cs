using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using TestePraticoMvc.BLL;
using TestePraticoMvc.Models;
using TestePraticoMvc.Utils;

namespace TestePraticoMvc.Controllers
{
    [RoutePrefix("pessoas")]
    public class PessoasController : Controller
    {
        private readonly PessoasBLL _service = new PessoasBLL();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            List<Pessoa> resposta = await _service.Get();
            return Json(resposta, JsonRequestBehavior.AllowGet);
        }

        [Route("detalhes/{id:Guid}")]
        public async Task<ActionResult> Details(Guid id)
        {
            Pessoa pessoa = await _service.Exists(id);
            if (pessoa == null)
            {
                return HttpNotFound();
            }
            return View(pessoa);
        }

        [Route("nova")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("nova")]
        public async Task<ActionResult> Create([Bind(Include = "Id,Nome,Sobrenome,DataNascimento,EstadoCivil,Cpf,Rg")] Pessoa pessoa)
        {
            if (ModelState.IsValid)
            {
                var resposta = await _service.Create(pessoa);

                return Json(new { resposta.Sucesso, resposta.Mensagem });
            }

            return Json(new { Sucesso = false, Mensagem = "Formato de dados inválido." });
        }

        [Route("editar/{id:Guid}")]
        public async Task<ActionResult> Edit(Guid id)
        {
            Pessoa pessoa = await _service.Exists(id);
            if (pessoa == null)
            {
                return HttpNotFound();
            }
            return View(pessoa);
        }

        [HttpPost]
        [Route("editar/{id:Guid}")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nome,Sobrenome,DataNascimento,EstadoCivil,Cpf,Rg")] Pessoa pessoa)
        {
            if (ModelState.IsValid)
            {
                var resposta = await _service.Edit(pessoa);

                return Json(new { resposta.Sucesso, resposta.Mensagem });
            }

            return Json(new { Sucesso = false, Mensagem = "Formato de dados inválido." });
        }

        [Route("excluir/{id:Guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            Pessoa pessoa = await _service.Exists(id);
            if (pessoa == null)
            {
                return HttpNotFound();
            }
            return View(pessoa);
        }

        [HttpPost, ActionName("Delete")]
        [Route("excluir/{id:Guid}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            var resposta = await _service.Delete(id);

            return Json(new { resposta.Sucesso, resposta.Mensagem });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _service.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
