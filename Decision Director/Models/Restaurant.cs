using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Decision_Director.Models
{
    [Table("Restaurants")]
    public class Restaurant
    {
        [PrimaryKey, AutoIncrement] public int Id { get; set; }

        public string Name { get; set; }

        public string Genre { get; set; }

        public string Type { get; set; }
    }
}
