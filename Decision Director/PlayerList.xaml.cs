using Decision_Director.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Decision_Director;

public partial class PlayerList : ContentPage
{
    public static ObservableCollection<Player> _players = new ObservableCollection<Player>();
	public PlayerList()
	{
		InitializeComponent();
	}

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        RefreshPlayerList();
        players_collectionView.ItemsSource = _players;
    }

    private async void edit_player_button_clicked(object sender, EventArgs e)
    {
        ImageButton imageButton = (ImageButton)sender;
        Player player = (Player)imageButton.BindingContext;
        AddEditPlayer editPlayer = new AddEditPlayer(player);
        await Navigation.PushAsync(editPlayer);
    }

    private async void delete_player_button_clicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Delete?", "Are you sure you want to delete this player?", "Yes", "No");
        if (answer)
        {
            ImageButton imageButton = (ImageButton)sender;
            Player player = (Player)imageButton.BindingContext;
            await DBHandler.RemovePlayer(player.Id);
            _players.Remove(player);
        }
    }

    private async void add_player_Clicked(object sender, EventArgs e)
    {
        AddEditPlayer addPlayer = new AddEditPlayer();
        await Navigation.PushAsync(addPlayer);
    }

    public static async void RefreshPlayerList()
    {
        _players.Clear();
        var playerList = await DBHandler.GetPlayers();
        foreach (var player in playerList)
        {
            _players.Add(player);
        }
    }
}