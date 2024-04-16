using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SQLite;

namespace Decision_Director.Models
{
    public static class DBHandler
    {
        public static SQLiteAsyncConnection db;
        public static SQLiteConnection dbConnection;

        public static async Task Init()
        {

            if (db != null)
            {
                return;
            }

            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Data.db");
            db = new SQLiteAsyncConnection(databasePath);
            dbConnection = new SQLiteConnection(databasePath);
            await db.CreateTableAsync<Game>();
            await db.CreateTableAsync<Restaurant>();
            await db.CreateTableAsync<Player>();
            await db.CreateTableAsync<PossibleGameList>();

        }

        public static async void ClearTables()
        {
            await Init();
        }

        #region Game Handlers

        public static async Task AddGame(Game game)
        {
            await Init();
            await db.InsertAsync(game);
            await RefreshGameList();
            return;
        }

        public static async Task RefreshGameList()
        {
            await Init();
            Game.GameList.Clear();
            var games = await db.Table<Game>().ToListAsync();
            foreach (var game in games)
            {
                Game.GameList.Add(game);
            }
        }

        public static async Task<Game> GetGame(int id)
        {
            await Init();
            Game game = await db.Table<Game>()
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();
            return game;
        }

        public static async Task<IEnumerable<Game>> GetGames()
        {
            await Init();
            var games = await db.Table<Game>().ToListAsync();
            return games;
        }

        public static async void DropTables()
        {
            await Init();
            await db.DropTableAsync<Game>();
        }

        public static async Task UpdateGame(int gameID, string gameName, int minPlayer, int maxPlayer)
        {
            await Init();
            Game game = await GetGame(gameID);
            game.GameName = gameName;
            game.MinPlayers = minPlayer;
            game.MaxPlayers = maxPlayer;

            await db.UpdateAsync(game);
            await RefreshGameList();
        }

        public static async Task DeleteGame(Game game)
        {
            await Init();
            await db.DeleteAsync(game);
            await RefreshGameList();
        }

        #endregion

        #region Possible Game Handlers

        public static async Task<IEnumerable<PossibleGameList>> GetCurrentPossibleList()
        {
            await Init();
            var list = await db.Table<PossibleGameList>().ToListAsync();
            return list;
        }

        public static async Task ClearPossibleGameList()
        {
            await Init();
            await db.DeleteAllAsync<PossibleGameList>();
        }

        public static async Task AddPossibleGame(PossibleGameList possibleGame)
        {
            await Init();
            await db.InsertAsync(possibleGame);
        }

        public static async Task DeletePossibleGame(int possibleGameID)
        {
            await Init();
            PossibleGameList game = await db.Table<PossibleGameList>().Where(i => i.Id == possibleGameID).FirstAsync();
            await db.DeleteAsync(game);
        }

        public static async Task<IEnumerable<PossibleGameList>> GetPossibleGames(int players)
        {
            await Init();
            await ClearPossibleGameList();
            var gameList = await db.Table<Game>()
                .Where(i => i.MinPlayers <= players && i.MaxPlayers >= players)
                .ToListAsync();

            foreach(var game in gameList)
            {
                PossibleGameList possibleGame = new PossibleGameList();
                possibleGame.Id = game.Id;
                possibleGame.Name = game.GameName;
                possibleGame.MinPlayers = game.MinPlayers;
                possibleGame.MaxPlayers = game.MaxPlayers;

                await AddPossibleGame(possibleGame);
            }

            var possibleGameList = await db.Table<PossibleGameList>().ToListAsync();
            return possibleGameList;
        }

        #endregion

        #region Player Handlers

        public static async Task AddPlayer(Player player)
        {
            await Init();
            await db.InsertAsync(player);
        }

        public static async Task<IEnumerable<Player>> GetPlayers()
        {
            await Init();
            var players = await db.Table<Player>().ToListAsync();
            return players;
        }

        public static async Task UpdatePlayer(int playerId, string name, bool isPresent)
        {
            await Init();
            Player player = await db.Table<Player>().Where(i=> i.Id == playerId).FirstAsync();
            player.Name = name;
            player.IsPresent = isPresent;
            await db.UpdateAsync(player);
        }

        public static async Task UpdatePlayerStatus(int playerId, bool isPresent)
        {
            await Init();
            Player player = await db.Table<Player>().Where(i => i.Id == playerId).FirstAsync();
            player.IsPresent = isPresent;
            await db.UpdateAsync(player);
        }

        public static async Task RemovePlayer(int playerId)
        {
            await Init();
            Player player = await db.Table<Player>().Where(i => i.Id == playerId).FirstAsync();
            await db.DeleteAsync(player);
        }

        public static async Task<IEnumerable<Player>> GetPresentPlayers()
        {
            await Init();
            var presentPlayers = await db.Table<Player>()
                .Where(i=> i.IsPresent == true)
                .ToListAsync();
            return presentPlayers;
        }

        #endregion

        #region Restaurant Handlers

        public static async Task AddRestaurant(Restaurant restaurant)
        {
            await Init();
            await db.InsertAsync(restaurant);
        }

        public static async Task<IEnumerable<Restaurant>> GetAllRestaurants()
        {
            await Init();
            var restaurants = await db.Table<Restaurant>().ToListAsync();
            return restaurants;
        }

        public static async Task<IEnumerable<Restaurant>> GetRestaurantWithConstraint(string genre, string type)
        {
            await Init();
            if(genre == "Any" && type == "Any")
            {
                var allRestaurants = await db.Table<Restaurant>().ToListAsync();
                return allRestaurants;
            }
            if(genre == "Any" && type != "Any")
            {
                var allRestaurants = await db.Table<Restaurant>().Where(i => i.Type == type).ToListAsync();
                return allRestaurants;
            }
            if(genre != "Any" && type == "Any")
            {
                var allRestaurants = await db.Table<Restaurant>().Where(i => i.Genre == genre).ToListAsync();
                return allRestaurants;
            }
            if(genre != "Any" && type != "Any")
            {
                var allRestaurants = await db.Table<Restaurant>().Where(i => i.Genre == genre && i.Type == type).ToListAsync();
                return allRestaurants;
            }
            return null;
        }

        public static async Task UpdateRestaurant(Restaurant restaurant)
        {
            await Init();
            var restaurantList = await db.Table<Restaurant>().Where(i => i.Id == restaurant.Id).FirstAsync();
            restaurantList.Name = restaurant.Name;
            restaurantList.Genre = restaurant.Genre;
            restaurantList.Type = restaurant.Type;
            await db.UpdateAsync(restaurantList);
        }

        public static async Task RemoveRestaurant(Restaurant restaurant)
        {
            await Init();
            await db.DeleteAsync(restaurant);
        }

        #endregion
    }
}
