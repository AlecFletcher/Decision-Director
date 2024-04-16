using SQLite;

namespace Decision_Director.Models
{
    [Table("Players")]
    public class Player
    {

        [PrimaryKey, AutoIncrement] public int Id { get; set; }

        public string Name { get; set; }

        public bool IsPresent { get; set; }
    }
}
