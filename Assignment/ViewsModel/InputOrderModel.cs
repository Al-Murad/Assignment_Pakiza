using Assignment.Models;
using System.ComponentModel.DataAnnotations;

namespace Assignment.ViewsModel
{
    public class InputOrderModel
    {
        public int OrderId { get; set; }
        [Required]
        public int CustomerId { get; set; }

        [Required, StringLength(50)]
        public string CustomerName { get; set; } = default!;
        [Required, StringLength(20)]
        public string Phone { get; set; } = default!;
        [Required, StringLength(50)]
        public string Address { get; set; } = default!;

        public virtual List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
