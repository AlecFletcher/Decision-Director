using Decision_Director.Models;
using static System.Random;

namespace Decision_Director;

public partial class BoardGameView : ContentPage
{
	public BoardGameView()
	{
		InitializeComponent();
	}

    private void minus_button_clicked(object sender, EventArgs e)
    {
		int numPlayers = Convert.ToInt32(Num_players_label.Text);
	    if(numPlayers == 0 )
        {
            return;
        }
        numPlayers--;
		Num_players_label.Text = numPlayers.ToString();
    }

    private void plus_button_clicked(object sender, EventArgs e)
    {
        int numPlayers = Convert.ToInt32(Num_players_label.Text);
        numPlayers++;
        Num_players_label.Text = numPlayers.ToString();
    }

    private void edit_game_button_clicked(object sender, EventArgs e)
    {
        GameListView gameListView = new GameListView();
        Navigation.PushAsync(gameListView);
    }

    private async void FindGameButton_Clicked(object sender, EventArgs e)
    {
        //See if a query has been made to avoid duplicates
        var currentGames = await DBHandler.GetCurrentPossibleList();
        
        //If the list has been cleared or there are no more games in the list, perform the query
        if (currentGames.Count() == 0)
        {
            //See if the number is valid
            var possibleGames = await DBHandler.GetPossibleGames(Convert.ToInt32(Num_players_label.Text));
            List<PossibleGameList> games = possibleGames.ToList();

            if (games.Count == 0)
            {
                selected_game_Label.Text = "No games found with that many players.";
                return;
            }
            Random rand = new Random();
            int index = rand.Next(0, games.Count() - 1);
            selected_game_Label.Text = games[index].Name;

            await DBHandler.DeletePossibleGame(games[index].Id);
            return;
        }
        // if the list has games in it, pull from that list
        List<PossibleGameList> validGames = currentGames.ToList();

        Random random = new Random();
        int randIndex = random.Next(0, validGames.Count() - 1);
        selected_game_Label.Text = validGames[randIndex].Name;

        await DBHandler.DeletePossibleGame(validGames[randIndex].Id);
        return;
    }
}