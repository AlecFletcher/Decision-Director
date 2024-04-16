using Decision_Director.Models;
using System.Diagnostics;

namespace Decision_Director;

public partial class AddEditGame : ContentPage
{
    Game gameBeingEdited;
    private bool IsBeingEdited = false;
	public AddEditGame()
	{
		InitializeComponent();
	}

	public AddEditGame(Game game)
	{
		InitializeComponent();
		GameGrid.BindingContext = game;
        IsBeingEdited = true;
        gameBeingEdited = game;
	}

    private async void max_players_add_button(object sender, EventArgs e)
    {
        int num = Convert.ToInt32(max_players_Entry.Text);
        num++;
        max_players_Entry.Text = num.ToString();
        await DBHandler.ClearPossibleGameList();

    }

    private async void min_players_subtract_button(object sender, EventArgs e)
    {
        int num = Convert.ToInt32(min_players_Entry.Text);
        if(num <= 1) { return; }
        num--;
        min_players_Entry.Text = num.ToString();
        await DBHandler.ClearPossibleGameList();

    }

    private async void min_players_add_button(object sender, EventArgs e)
    {
        int num = Convert.ToInt32(min_players_Entry.Text);
        if(num >= Convert.ToInt32(max_players_Entry.Text)) { return; }
        num++;
        min_players_Entry.Text = num.ToString();
        await DBHandler.ClearPossibleGameList();

    }

    private async void max_players_subtract_button(object sender, EventArgs e)
    {
        int num = Convert.ToInt32(max_players_Entry.Text);
        if(num <= Convert.ToInt32(min_players_Entry.Text)) { return; }
        num--;
        max_players_Entry.Text = num.ToString();
        await DBHandler.ClearPossibleGameList();

    }

    private async void save_button_Clicked(object sender, EventArgs e)
    {
        try {
            int minNum = Convert.ToInt32(min_players_Entry.Text);
            int maxNum = Convert.ToInt32(max_players_Entry.Text);
        }
        catch {
            await DisplayAlert("Error", "Minimum and Maximum player count MUST be numbers", "Okay");
            return; }

        int MinNum = Convert.ToInt32(min_players_Entry.Text);
        int MaxNum = Convert.ToInt32(max_players_Entry.Text);

        if (MinNum > MaxNum)
        {
            await DisplayAlert("Error", "Minimum player count is greater than maximum player count", "Okay");
        }

        if (MinNum < 1)
        {
            await DisplayAlert("Error", "Minimum player count cannot be less than 1", "Okay");
        }

        if(IsBeingEdited)
        {
            Debug.WriteLine(gameBeingEdited.Id);
            await DBHandler.UpdateGame(gameBeingEdited.Id, game_name_Entry.Text, Convert.ToInt32(min_players_Entry.Text), Convert.ToInt32(max_players_Entry.Text));
            await DBHandler.ClearPossibleGameList();
            await Navigation.PopAsync();
            return;
        }

        Game newGame = new Game(game_name_Entry.Text, Convert.ToInt32(min_players_Entry.Text), Convert.ToInt32(max_players_Entry.Text));

        await DBHandler.AddGame(newGame);
        await DBHandler.ClearPossibleGameList();
        await Navigation.PopAsync();

    }
}