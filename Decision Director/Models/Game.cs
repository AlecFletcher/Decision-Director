using SQLite;
using System.Collections.ObjectModel;

namespace Decision_Director.Models
{
    [Table("Games")]
    public class Game
    {
        [PrimaryKey, AutoIncrement] public int Id { get; set; }

        public string? GameName { get; set; }

        public int MinPlayers { get; set; }

        public int MaxPlayers { get; set; }

        public static ObservableCollection<Game> GameList { get; set; } = [];

        public Game() { }

        public Game(string gameName, int minPlayers, int maxPlayers)
        {
            this.GameName = gameName;
            this.MinPlayers = minPlayers;
            this.MaxPlayers = maxPlayers;
        }
    }


}
