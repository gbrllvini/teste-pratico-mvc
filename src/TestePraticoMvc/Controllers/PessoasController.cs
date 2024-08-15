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
            return View("Index");
        }

        [Route("detalhes/{id:Guid}")]
        public async Task<ActionResult> Details(Guid id)
        {
            Pessoa pessoa = await _service.Exists(id, "detalhes");
            if (pessoa == null)
            {
                return HttpNotFound();
            }
            return View("Details");
        }

        [Route("nova")]
        public ActionResult Create()
        {
            return View("Create");
        }

        [Route("editar/{id:Guid}")]
        public async Task<ActionResult> Edit(Guid id)
        {
            Pessoa pessoa = await _service.Exists(id, "editar");
            if (pessoa == null)
            {
                return HttpNotFound();
            }
            return View("Edit");
        }

        [Route("excluir/{id:Guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            Pessoa pessoa = await _service.Exists(id, "excluir");
            if (pessoa == null)
            {
                return HttpNotFound();
            }
            return View(pessoa);
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            List<Pessoa> resposta = await _service.Get();
            return Json(resposta, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("pessoa/{id:Guid}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            Pessoa pessoa = await _service.Exists(id, "pessoa");
            if (pessoa == null)
            {
                return HttpNotFound();
            }

            return Json(pessoa, JsonRequestBehavior.AllowGet);
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

            return Json(new { Sucesso = false, Mensagem = "Verifique os campos e tente novamente." });
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
