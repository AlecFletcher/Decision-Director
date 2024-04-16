namespace Decision_Director;
using Models;
using System.Diagnostics;

public partial class PlayerPicker : ContentPage
{
	public PlayerPicker()
	{
		InitializeComponent();
	}

    private void edit_player_clicked(object sender, EventArgs e)
    {
        PlayerList playerList = new PlayerList();
        Navigation.PushAsync(playerList);
    }

    private async void find_player_button_clicked(object sender, EventArgs e)
    {
        var players = await DBHandler.GetPresentPlayers();
        List<Player> playerList = players.ToList();
        Random rand = new Random();
        try
        {
            int index = rand.Next(0, playerList.Count);
            int random = rand.Next(0, PlayerExtraText.List.Count());

            Player selectedPlayer = playerList[index];
            selected_player_label.Text = selectedPlayer.Name + PlayerExtraText.List[random];
            selected_player_label.HorizontalTextAlignment = TextAlignment.Start;
            selected_player_label.HorizontalTextAlignment = TextAlignment.Center;
        }
        catch
        {
            await DisplayAlert("Error", "No players are set to present", "Okay");
            return;
        }

    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        player_collection_view.ItemsSource = await DBHandler.GetPlayers();
    }

    private async void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        CheckBox checkBox = (CheckBox)sender;
        Player player = checkBox.BindingContext as Player;
        await DBHandler.UpdatePlayerStatus(player.Id, checkBox.IsChecked);
        return;
        }

    private async void ContentPage_Appearing(object sender, EventArgs e)
    {
        player_collection_view.ItemsSource = await DBHandler.GetPlayers();
    }
}