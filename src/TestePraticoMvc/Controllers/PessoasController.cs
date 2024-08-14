using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using TestePraticoMvc.BLL;
using TestePraticoMvc.DAL;
using TestePraticoMvc.Models;

namespace TestePraticoMvc.Controllers
{
    [RoutePrefix("pessoas")]
    public class PessoasController : Controller
    {
        private readonly PessoasContext db = new PessoasContext();
        private readonly PessoasBLL _service;

        public PessoasController(PessoasContext db, PessoasBLL service)
        {
            this.db = db;
            _service = service;
        }

        public ActionResult Index()
        {
            return View(db.Pessoas.ToList());
        }
        [Route("detalhes/{id:Guid}")]
        public ActionResult Details(Guid id)
        {
            Pessoa pessoa = db.Pessoas.Find(id);
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
        public async Task<ActionResult> Create([Bind(Include = "Id,Nome,Sobrenome,DataNascimento,EstadoCivil,Cpf,Rg")] Pessoa pessoa)
        {
            if (ModelState.IsValid)
            {

                var resposta = await _service.Create(pessoa);
                return resposta;
            }

            return View(pessoa);
        }

        [Route("editar/{id:Guid}")]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pessoa pessoa = db.Pessoas.Find(id);
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
                db.Entry(pessoa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pessoa);
        }
        [Route("excluir/{id:Guid}")]
        public ActionResult Delete(Guid id)
        {
            Pessoa pessoa = db.Pessoas.Find(id);
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
            Pessoa pessoa = db.Pessoas.Find(id);
            db.Pessoas.Remove(pessoa);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
