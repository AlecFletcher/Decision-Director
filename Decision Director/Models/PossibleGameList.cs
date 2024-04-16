using SQLite;

namespace Decision_Director.Models
{
    [Table("PossibleGames")]
    public class PossibleGameList
    {
        [PrimaryKey] public int Id { get; set; }

        public string Name { get; set; }

        public int MinPlayers { get; set; }

        public int MaxPlayers { get; set; }
    }
}
