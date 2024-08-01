using Assignment.Models;

namespace Assignment.ViewsModel
{
    public class OrderNewModel
    {
       
            public Customer Customer { get; set; } = default!;
            public OrderItem? OrderItem { get; set; }
        
    }
}
