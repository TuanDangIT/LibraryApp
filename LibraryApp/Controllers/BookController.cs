using Microsoft.AspNetCore.Mvc;
using Biblioteka.Models;
using Biblioteka.DAL;
using Biblioteka.Models.DBEntities;
using Biblioteka.Models.DBEntities.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Biblioteka.Controllers
{
    public class BookController : Controller
    {
        private readonly BibliotekaDbContext _context;
        public BookController(BibliotekaDbContext context)
        {
            _context = context;
        }
        [Authorize]
        public IActionResult Index(string SearchString)
        {
            ViewData["CurrentFiltler"] = SearchString;
            var books = _context.Books.Where(b => SearchString == null || b.Title.ToLower().Contains(SearchString.ToLower()) ||
                b.Author.ToLower().Contains(SearchString.ToLower())).ToList();
            List<BookViewModel> bookList = new List<BookViewModel>();
            if (books != null)
            {
                
                foreach(var book in books)
                {
                    var BookViewModel = new BookViewModel()
                    {
                        Id = book.Id,
                        Category = book.Category.ToString(),
                        Author = book.Author,
                        Title = book.Title,
                        RealeaseDate = book.RealeaseDate.ToLocalTime(),
                        Keywords = string.Join(", ", book.Keywords),
                        Quantity = book.Quantity,
                    };
                    bookList.Add(BookViewModel);
                }
                return View(bookList);
            }
            return View(bookList);
        }
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(BookViewModel bookData)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var keywords = bookData.Keywords.Trim(' ');
                    var book = new Book()
                    {
                        Author = bookData.Author,
                        Category = (Category)Enum.Parse(typeof(Category), bookData.Category),
                        Title = bookData.Title,
                        RealeaseDate = bookData.RealeaseDate.ToUniversalTime(),
                        Keywords = bookData.Keywords.Split(",", StringSplitOptions.None).ToList(),
                        Quantity = bookData.Quantity,
                    };
                    _context.Books.Add(book);
                    _context.SaveChanges();
                    TempData["successMessage"] = "Task completed";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["errorMessage"] = "Not valid";
                    return View();
                }

            }
            catch (Exception ex)
            {

                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }
        [Authorize]
        public IActionResult Edit(int Id)
        {
            var book = _context.Books.SingleOrDefault(x => x.Id == Id);
            try
            {
                if (book != null)
                {
                    var bookView = new BookViewModel()
                    {
                        Id = book.Id,
                        Author = book.Author,
                        Title = book.Title,
                        RealeaseDate = book.RealeaseDate.ToLocalTime(),
                        Keywords = string.Join(", ", book.Keywords),
                        Quantity = book.Quantity
                    };
                    return View(bookView);
                }
                else
                {
                    TempData["errorMessage"] = $"Not available with that Id {Id}";
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult Edit(BookViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var book = new Book()
                    {
                        Id = model.Id,
                        Category = (Category)Enum.Parse(typeof(Category), model.Category),
                        Author = model.Author,
                        Title = model.Title,
                        RealeaseDate = model.RealeaseDate.ToUniversalTime(),
                        Keywords = model.Keywords.Split(",", StringSplitOptions.None).ToList(),
                        Quantity = model.Quantity
                    };
                    _context.Books.Update(book);
                    _context.SaveChanges();
                    TempData["successMessage"] = "Updated!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["errorMessage"] = "Model data is invalid";
                    return View();
                }
            }
            catch (Exception ex)
            {

                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }
        [Authorize]
        public IActionResult Delete(int Id)
        {
            var book = _context.Books.SingleOrDefault(x => x.Id == Id);
            try
            {
                if (book != null)
                {
                    var bookView = new BookViewModel()
                    {
                        Id = book.Id,
                        Category = book.Category.ToString(),
                        Author = book.Author,
                        Title = book.Title,
                        RealeaseDate = book.RealeaseDate.ToLocalTime(),
                        Keywords = string.Join(", ", book.Keywords),
                        Quantity = book.Quantity
                    };
                    return View(bookView);
                }
                else
                {
                    TempData["errorMessage"] = $"Not available with that Id {Id}";
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
        [HttpPost]

        public IActionResult Delete(BookViewModel model)
        {
            var book = _context.Books.SingleOrDefault(x => x.Id == model.Id);
            try
            {
                if (book != null)
                {
                    _context.Books.Remove(book);
                    _context.SaveChanges();
                    TempData["successMessage"] = "Deleted!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["errorMessage"] = $"Not available with that Id {model.Id}";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {

                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }
    }
}
