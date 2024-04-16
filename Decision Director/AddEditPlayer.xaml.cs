namespace Decision_Director;
using Models;

public partial class AddEditPlayer : ContentPage
{
	bool IsBeingEdited = false;
	Player playerBeingEdited = new Player();
	public AddEditPlayer()
	{
		InitializeComponent();
	}

	public AddEditPlayer(Player player)
	{
        InitializeComponent();
        IsBeingEdited = true;
		playerBeingEdited = player;
		PlayerGrid.BindingContext = player;
	}

    private async void save_changes_button_Clicked(object sender, EventArgs e)
    {
		if(player_name_Entry.Text == null || player_name_Entry.Text.Replace(" ", "") == string.Empty)
		{
			await DisplayAlert("Error", "Player name cannot be blank", "Okay");
			return;
		}
		playerBeingEdited.Name = player_name_Entry.Text;
		playerBeingEdited.IsPresent = player_present_Checkbox.IsChecked;

		if (IsBeingEdited)
		{
			await DBHandler.UpdatePlayer(playerBeingEdited.Id, playerBeingEdited.Name, playerBeingEdited.IsPresent);
			PlayerList.RefreshPlayerList();
            await Navigation.PopAsync();
            return;
		}
		else
		{
			await DBHandler.AddPlayer(playerBeingEdited);
            PlayerList.RefreshPlayerList();
            await Navigation.PopAsync();
			return;
		}
	}

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
		if(!IsBeingEdited)
		{
			save_changes_button.Text = "Add Player";
		}
    }
}