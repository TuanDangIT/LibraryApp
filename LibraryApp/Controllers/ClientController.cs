using Microsoft.AspNetCore.Mvc;
using Biblioteka.Models;
using Biblioteka.DAL;
using Biblioteka.Models.DBEntities;
using Microsoft.AspNetCore.Authorization;

namespace Biblioteka.Controllers
{
    public class ClientController : Controller
    {
        private readonly BibliotekaDbContext _context;
        public ClientController(BibliotekaDbContext context)
        {
            this._context = context;
        }
        [Authorize]
        public IActionResult Index(string SearchString)
        {
            ViewData["CurrentFiltler"] = SearchString;
            var clients = _context.Clients.Where(b => SearchString == null || b.FirstName.ToLower().Contains(SearchString.ToLower()) || b.LastName.ToLower().Contains(SearchString.ToLower()) 
            || b.PhoneNumber.ToLower().Contains(SearchString.ToLower())).ToList();
            List<ClientViewModel> clientList = new List<ClientViewModel>();
            if (clients != null)
            {

                foreach (var client in clients)
                {
                    var ClientViewModel = new ClientViewModel()
                    {
                        Id = client.Id,
                        FirstName = client.FirstName,
                        LastName = client.LastName,
                        PhoneNumber = client.PhoneNumber,

                    };
                    clientList.Add(ClientViewModel);
                }
                return View(clientList);
            }
            return View(clientList);
        }
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ClientViewModel clientData)
        {
            try
            {
                if (ModelState.IsValid && clientData.PhoneNumber.Length == 9)
                {
                    var client = new Client()
                    {
                        FirstName = clientData.FirstName,
                        LastName = clientData.LastName,
                        PhoneNumber = clientData.PhoneNumber,
                    };
                    _context.Clients.Add(client);
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
            var client = _context.Clients.SingleOrDefault(x => x.Id == Id);
            try
            {
                if (client != null)
                {
                    var clientView = new ClientViewModel()
                    {
                        Id = client.Id,
                        FirstName = client.FirstName,
                        LastName= client.LastName,
                        PhoneNumber= client.PhoneNumber,
                    };
                    return View(clientView);
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
        public IActionResult Edit(ClientViewModel model)
        {
            try
            {
                if (ModelState.IsValid && model.PhoneNumber.Length == 9)
                {
                    var client = new Client()
                    {
                        Id = model.Id,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber,
                    };
                    _context.Clients.Update(client);
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
            var client = _context.Clients.SingleOrDefault(x => x.Id == Id);
            try
            {
                if (client != null)
                {
                    var clientView = new ClientViewModel()
                    {
                        Id = client.Id,
                        FirstName = client.FirstName,
                        LastName = client.LastName,
                        PhoneNumber = client.PhoneNumber,
                    };
                    return View(clientView);
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

        public IActionResult Delete(ClientViewModel model)
        {
            var client = _context.Clients.SingleOrDefault(x => x.Id == model.Id);
            try
            {
                if (client != null)
                {
                    _context.Clients.Remove(client);
                    _context.SaveChanges();
                    TempData["successMessage"] = "Deleted";
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
