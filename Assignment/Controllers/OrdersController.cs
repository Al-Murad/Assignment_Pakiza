using Assignment.Models;
using Assignment.ViewsModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Assignment.Controllers
{
    public class OrdersController : Controller
    {
        private readonly AssignmentDbContext db;
        public OrdersController(AssignmentDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            var data = db.Orders.Include(x => x.Customer).Include(x => x.OrderItems).ThenInclude(y => y.Product).ToList();
            return View(data);
        }
        public ActionResult Create()
        {
            ViewBag.CustomerNames = db.Customers.Select(X => X.CustomerName).ToList();
            ViewBag.ProductNames = db.Products.Select(X => X.ProductName).ToList();
            return View();
        }
        [HttpPost]
        public JsonResult Create(Customer customer)
        {
           
            if (ModelState.IsValid)
            {
                var order = new Order();
                var existing = db.Customers.AsEnumerable().FirstOrDefault(x => string.Equals(customer.CustomerName, x.CustomerName, StringComparison.InvariantCultureIgnoreCase));
                if(existing == null)
                {
                    db.Customers.Add(customer);
                    order.Customer = customer;
                }
                else
                {
                    existing.Phone = customer.Phone;
                    existing.Address = customer.Address;
                    order.Customer = existing;
                }
               

                var Dataitem = HttpContext.Session.GetString("orderitems");
                if (Dataitem !=null)
                {
                    var orderItems = JsonConvert.DeserializeObject<List<OrderModel>>(Dataitem);
                    if (orderItems != null)
                    {
                        foreach (var item in orderItems)
                        {
                            var prod = db.Products.FirstOrDefault(x => x.ProductName.ToLower()==item.ProductName.ToLower());
                            if (prod == null) { 
                                var newP = new Product { ProductName=item.ProductName, UnitPrice=item.UnitPrice};
                                db.Products.Add(newP);
                                order.OrderItems.Add(new OrderItem { Product=newP, Quantity = item.Quantity, UnitPrice=item.UnitPrice });
                            }
                            else
                            {
                                order.OrderItems.Add(new OrderItem { Product = prod, Quantity = item.Quantity });
                            }
                           
                        }
                    }
                    
                }
                db.Orders.Add(order);
                db.SaveChanges();
                HttpContext.Session.Remove("orderitems");
                return Json(new {success= true});

            }
            return Json(new {success=false});
        }
        [HttpPost]
        public PartialViewResult AddItem(OrderModel? item = null)
        {
            var Dataitem = HttpContext.Session.GetString("orderitems");
            List<OrderModel> items = new List<OrderModel>();
            if (Dataitem != null)
            {
                var x = JsonConvert.DeserializeObject<List<OrderModel>>(Dataitem);
                if (x != null) { items = x; }

            }
            if (item != null && !string.IsNullOrEmpty(item.ProductName))
            {
                items.Add(item);
                HttpContext.Session.SetString("orderitems", JsonConvert.SerializeObject(items));
                return PartialView("_OrderItemView", items);
            }
            return PartialView("_OrderItemView", null);
        }

        public JsonResult GetCustomerDetail(string customerName)
        {
            var customer = db.Customers.FirstOrDefault(x => x.CustomerName.ToLower() == customerName.ToLower());
            return Json(customer);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var order = new Order { OrderId = id };
            db.Entry(order).State = EntityState.Deleted;
            db.Orders.Remove(order);
            await db.SaveChangesAsync();
            return Json(new { success = true, msg = "Data has Deleted" });
        }
        public IActionResult Print(int id)
        {
            var data = db.Orders.Include(y => y.Customer).Include(x => x.OrderItems).ThenInclude(z => z.Product).FirstOrDefault(o => o.OrderId == id);
            return View(data);
        }
    }
}
