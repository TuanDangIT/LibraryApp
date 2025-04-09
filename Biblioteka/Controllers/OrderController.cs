using Microsoft.AspNetCore.Mvc;
using Biblioteka.Models;
using Biblioteka.DAL;
using Biblioteka.Models.DBEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Biblioteka.Controllers
{
    public class OrderController : Controller
    {
        private readonly BibliotekaDbContext _context;
        public OrderController(BibliotekaDbContext context)
        {
            this._context = context;
        }
        [Authorize]
        public IActionResult Index(string SearchString)
        {
            ViewData["CurrentFiltler"] = SearchString;
            var orders = _context.Orders
                .Include(o => o.Client)
                .OrderBy(o => o.RentalDate)
                .Where(o => SearchString == null || o.Client.FirstName.ToLower().Contains(SearchString.ToLower()) ||
                    o.Client.LastName.ToLower().Contains(SearchString.ToLower()))
                .ToList();
            List<OrderViewModel> orderList = new List<OrderViewModel>();
            if (orders != null)
            {

                foreach (var order in orders)
                {
                    var OrderViewModel = new OrderViewModel()
                    {
                        Id = order.Id,
                        IsReturned = order.IsReturned,
                        RentalDate = order.RentalDate.ToLocalTime(),
                        ExpiryDate = order.RentalExpireDate.ToLocalTime(),
                        FirstName = order.Client.FirstName,
                        LastName = order.Client.LastName,
                    };
                    orderList.Add(OrderViewModel);
                }
                return View(orderList);
            }
            return View(orderList);
        }
        [Authorize]
        [Route("Order/{orderId}/Details")]
        public IActionResult Details([FromRoute]int orderId)
        {
            var order = _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Items)
                .ThenInclude(i => i.Book)
                .SingleOrDefault(o => o.Id == orderId) ?? throw new Exception();
            var orderView = new OrderDetailsViewModel()
            {
                Id = order.Id,
                IsReturned = order.IsReturned,
                RentalDate = order.RentalDate,
                ExpiryDate = order.RentalExpireDate,
                ClientShortened = new ClientShortenedViewModel()
                {
                    FirstName = order.Client.FirstName,
                    LastName = order.Client.LastName,
                    PhoneNumber = order.Client.PhoneNumber
                },
                Books = order.Items.Select(i =>
                {
                    var bookView = new BookShortenedViewModel()
                    {
                        Category = i.Book.Category.ToString(),
                        Author = i.Book.Author,
                        RealeaseDate = i.Book.RealeaseDate,
                        Title = i.Book.Title,
                        Keywords = string.Join(", ", i.Book.Keywords.OrderBy(k => k))
                    };
                    return bookView;
                }).OrderBy(b => b.Author).ThenBy(b => b.Title).ToList()
            };
            return View(orderView);
        }
        [Authorize]
        public IActionResult Create()
        {
            var books = _context.Books.Where(b => b.Quantity > 0).ToList().Select(b => new OrderBookCreateViewModel()
            {
                Author = b.Author,
                BookId = b.Id,
                Title = b.Title
            }).ToList();
            var clients = _context.Clients.ToList().Select(c => new OrderClientCreateViewModel()
            {
                ClientId = c.Id,
                FullName = c.FirstName + " " + c.LastName
            }).ToList();
            var orderCreateView = new OrderCreateViewModel()
            {
                Clients = clients,
                Books = books
            };
            return View(orderCreateView);
        }

        [HttpPost]
        public IActionResult Create(OrderCreateViewModel orderData)
        {
            try
            {
                if (ModelState.IsValid && orderData.ExpiryDate<=DateTime.Now)
                {
                    var a = orderData.BookIds;
                    var books = _context.Books.Where(b => orderData.BookIds.Contains(b.Id)).ToList();
                    var items = new List<OrderItem>();
                    foreach (var book in books)
                    {
                        if(book.Quantity == 0)
                        {
                            throw new Exception("Quantity is equal 0.");
                        }
                        items.Add(new OrderItem()
                        {
                            Book = book
                        });
                        book.Quantity -= 1;
                    }
                    var client = _context.Clients.SingleOrDefault(c => c.Id == orderData.ClientId) 
                        ?? throw new Exception();
                    var order = new Order()
                    {
                        IsReturned = false,
                        RentalDate = DateTime.Now.ToUniversalTime(),
                        RentalExpireDate = orderData.ExpiryDate.ToUniversalTime(),
                        Client = client,
                        Items = items 
                    };
                    _context.Orders.Add(order);
                    _context.SaveChanges();
                    TempData["successMessage"] = "Task completed";
                    return RedirectToAction("Index");
                }
                else
                {
                    var books = _context.Books.ToList().Select(b => new OrderBookCreateViewModel()
                    {
                        Author = b.Author,
                        BookId = b.Id,
                        Title = b.Title
                    }).ToList();
                    var clients = _context.Clients.ToList().Select(c => new OrderClientCreateViewModel()
                    {
                        ClientId = c.Id,
                        FullName = c.FirstName + " " + c.LastName
                    }).ToList();
                    var orderCreateView = new OrderCreateViewModel()
                    {
                        Clients = clients,
                        Books = books
                    };
                    TempData["errorMessage"] = "Not valid";
                    return View(orderCreateView);
                }

            }
            catch (Exception ex)
            {
                var books = _context.Books.ToList().Select(b => new OrderBookCreateViewModel()
                {
                    Author = b.Author,
                    BookId = b.Id,
                    Title = b.Title
                }).ToList();
                var clients = _context.Clients.ToList().Select(c => new OrderClientCreateViewModel()
                {
                    ClientId = c.Id,
                    FullName = c.FirstName + " " + c.LastName
                }).ToList();
                var orderCreateView = new OrderCreateViewModel()
                {
                    Clients = clients,
                    Books = books
                };
                TempData["errorMessage"] = ex.Message;
                return View(orderCreateView);
            }
        }
        //public IActionResult Edit(int Id)
        //{
        //    var order = _context.Orders.SingleOrDefault(x => x.Id == Id);
        //    try
        //    {
        //        if (order != null)
        //        {
        //            var orderView = new OrderViewModel()
        //            {
        //                Id = order.Id,
        //                RentalDate = order.RentalDate.ToLocalTime(),
        //                ExpiryDate = order.RentalExpireDate.ToLocalTime(),
        //                IsReturned = order.IsReturned
        //            };
        //            return View(orderView);
        //        }
        //        else
        //        {
        //            TempData["errorMessage"] = $"Not available with that Id {Id}";
        //            return RedirectToAction("Index");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["errorMessage"] = ex.Message;
        //        return RedirectToAction("Index");
        //    }
        //}
        //[HttpPost]
        //public IActionResult Edit(OrderViewModel model)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid && model.RentalDate <= DateTime.Now && model.RentalDate.Year >= 2000)
        //        {
        //            var order = new Order()
        //            {
        //                Id = model.Id,
        //                RentalDate = model.RentalDate,
        //                RentalExpireDate = model.ExpiryDate.ToUniversalTime(),
        //                IsReturned = model.IsReturned
        //            };
        //            _context.Orders.Update(order);
        //            _context.SaveChanges();
        //            TempData["successMessage"] = "Updated!";
        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            TempData["errorMessage"] = "Model data is invalid";
        //            return View();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        TempData["errorMessage"] = ex.Message;
        //        return View();
        //    }
        //}
        [HttpPost("Order/{orderId}/Return")]
        public IActionResult SetReturnStatusToTrue([FromRoute]int orderId)
        {
            try
            {
                var order = _context.Orders
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Book)
                    .SingleOrDefault(o => o.Id == orderId) ?? throw new Exception();
                order.IsReturned = true;
                foreach(var item in order.Items)
                {
                    item.Book.Quantity += 1;
                }
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception)
            {
                TempData["errorMessage"] = "Order not found";
                return View();
            }
        }
        [Authorize]
        public IActionResult Delete(int Id)
        {
            var order = _context.Orders
                .Include(o => o.Client)
                .SingleOrDefault(x => x.Id == Id);
            try
            {
                if (order != null)
                {
                    var orderView = new OrderViewModel()
                    {
                        Id = order.Id,
                        FirstName = order.Client.FirstName,
                        LastName = order.Client.LastName,
                        RentalDate = order.RentalDate,
                        ExpiryDate = order.RentalExpireDate
                        
                    };
                    return View(orderView);
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

        public IActionResult Delete(OrderViewModel model)
        {
            var order = _context.Orders
                .Include(o => o.Items)
                    .ThenInclude(i => i.Book)
                .SingleOrDefault(x => x.Id == model.Id);
            try
            {
                if (order != null)
                {
                    if(order.IsReturned == false)
                    {
                        foreach (var item in order.Items)
                        {
                            item.Book.Quantity += 1;
                        }
                    }
                    _context.Orders.Remove(order);
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
        //[HttpGet]
        //public IActionResult Sort()
        //{
        //    var orders = _context.Orders.OrderByDescending(x => x.RentalDate).ToList();
        //    List<OrderViewModel> orderList = new List<OrderViewModel>();
        //    if (orders != null)
        //    {

        //        foreach (var order in orders)
        //        {
        //            var OrderViewModel = new OrderViewModel()
        //            {
        //                Id = order.Id,
        //                RentalDate = order.RentalDate,


        //            };
        //            orderList.Add(OrderViewModel);
        //        }
        //        return View(orderList);
        //    }
        //    return View(orderList);
        //}
    }
}
