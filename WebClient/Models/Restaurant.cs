using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebClient.Models
{
    public class Restaurant
    {
        [Key]
        public string RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantAddress { get; set; }
        public string RestaurantPopular { get; set; }
        public string RestaurantPhone { get; set; }
        public string RestaurantRating { get; set; }
    }
}
