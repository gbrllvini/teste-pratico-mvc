using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using TestePraticoMvc.BLL;
using TestePraticoMvc.Models;

namespace TestePraticoMvc.Controllers
{
    [RoutePrefix("pessoas")]
    public class PessoasController : Controller
    {
        private readonly PessoasBLL _service;

        public PessoasController(PessoasBLL service)
        {
            _service = service;
        }

        public async Task<ActionResult> Index()
        {
            List<Pessoa> resposta = await _service.Get();
            return View(resposta);
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
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome,Sobrenome,DataNascimento,EstadoCivil,Cpf,Rg")] Pessoa pessoa)
        {
            if (ModelState.IsValid)
            {
                var resposta = _service.Create(pessoa);

                if(!resposta.Sucesso) return Json(resposta);

                return RedirectToAction("Index");
            }

            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors) });
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
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome,Sobrenome,DataNascimento,EstadoCivil,Cpf,Rg")] Pessoa pessoa)
        {
            if (ModelState.IsValid)
            {
                var resposta = _service.Edit(pessoa);

                if (!resposta.Sucesso) return Json(resposta);

                return RedirectToAction("Index");
            }
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors) });
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
        public ActionResult DeleteConfirmed(Guid id)
        {
            //Pessoa pessoa = db.Pessoas.Find(id);
           // db.Pessoas.Remove(pessoa);
           // db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            //    db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
