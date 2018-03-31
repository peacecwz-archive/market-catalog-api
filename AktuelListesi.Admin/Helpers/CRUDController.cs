using AktuelListesi.Data;
using AktuelListesi.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AktuelListesi.Admin
{
    public class CRUDController<T, TDto, TProperty> : Controller where T : class where TDto : class where TProperty : struct
    {
        private readonly IRepository<T, TDto> _repository;

        protected CRUDController(IRepository<T, TDto> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            return View(_repository.All().ToList());
        }

        [HttpGet]
        public virtual ActionResult Details(TProperty id)
        {
            var model = _repository.GetById(id);
            if (model == null) return RedirectToAction(nameof(Index));
            return View(model);
        }

        [HttpGet]
        public virtual ActionResult Create()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public virtual ActionResult Create(TDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            if (_repository.Add(dto) != null) return RedirectToAction(nameof(Index));
            else ModelState.AddModelError("", "Eklenirken hata oluştu");
            return View(dto);
        }

        [HttpGet]
        public virtual ActionResult Edit(TProperty id)
        {
            var dto = _repository.GetById(id);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(TDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            if (_repository.Update(dto) != null) return RedirectToAction(nameof(Index));
            else ModelState.AddModelError("", "Güncellerken hata oluştu");
            return View(dto);
        }

        [HttpGet]
        public virtual ActionResult Delete(TProperty id)
        {
            var dto = _repository.GetById(id);
            if (_repository.Delete<TProperty>(dto) == null) return Json("Silerken bir hata oluştu");
            return Json("Başarılı bir şekilde silindi");
        }

        [HttpGet]
        public virtual ActionResult ChangeActiveStatus(TProperty id)
        {
            var dto = _repository.GetById(id);
            (dto as BaseDto<TProperty>).IsActive = !(dto as BaseDto<TProperty>).IsActive;
            if (_repository.Update(dto) == null)
                return Json("GÜncellerken bir hata oluştu");
            return Json("Başarılı bir şekilde güncellendi");
        }
    }
}
