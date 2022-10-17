using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _db;
        public CategoryController(ICategoryRepository db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _db.GetAll();
            return View(objCategoryList);
        }
        //CREATE
        //GET
        public IActionResult Create()
        {
            return View();
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (!ModelState.IsValid)
            {
                return View(obj);
            }
            for (int i = 0; i < obj.Name.Length; i++)
            {
                if (obj.Name[i] > 'Z' || obj.Name[i] < 'A')
                {
                    if (obj.Name[i] < 'a' || obj.Name[i] > 'z')
                    {
                        ModelState.AddModelError("Name", "The characters is invalid, please enter only alphabet.");
                        break;
                    }
                }
            }
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The DisplayOrder cannot exactly match the Name.");
            }
            _db.Add(obj);
            _db.Save();
            TempData["success"] = "Add object success!!!";
            return RedirectToAction("Index");
        }
        //EDIT
        //GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //var CategoryFromDb = _db.Categories.Find(id);
            var CategoryFromDbFirst = _db.GetFirstOrDefault(u => u.Id == id);
            //var CategoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);
            if (CategoryFromDbFirst == null)
            {
                return NotFound();
            }
            return View(CategoryFromDbFirst);
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Add(obj);
                _db.Update(obj);
                _db.Save();
                TempData["success"] = "Edit object success!!!";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //var CategoryFromDb = _db.Categories.Find(id);
            //var CategoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);
            var CategoryFromDbFirst = _db.GetFirstOrDefault(u => u.Id == id);
            if (CategoryFromDbFirst == null)
            {
                return NotFound();
            }
            return View(CategoryFromDbFirst);
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _db.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Remove(obj);
            _db.Save();
            TempData["success"] = "Delete object success!!!";
            return RedirectToAction("Index");
        }
    }
}
