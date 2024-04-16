using Decision_Director.Models;

namespace Decision_Director;

public partial class GameListView : ContentPage
{
	public GameListView()
	{
		InitializeComponent();
	}

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
		await DBHandler.RefreshGameList();
		GameCollectionView.ItemsSource = Game.GameList;
    }

    private void EditButton_Clicked(object sender, EventArgs e)
    {
        ImageButton imageButton = (ImageButton)sender;
        Game game = imageButton.BindingContext as Game;
        AddEditGame editGame = new AddEditGame(game);
        Navigation.PushAsync(editGame);
    }

    private async void DeleteButton_Clicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Delete?", "Are you sure you want to delete this term?", "Yes", "No");
        if(answer)
        {
            ImageButton imageButton = (ImageButton)sender;
            Game game = imageButton.BindingContext as Game;
            await DBHandler.DeleteGame(game);
        }


    }

    private async void AddGame_Clicked(object sender, EventArgs e)
    {
        AddEditGame addGame = new AddEditGame();
        await Navigation.PushAsync(addGame);
    }
}