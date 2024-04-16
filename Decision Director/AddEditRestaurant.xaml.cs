using Decision_Director.Models;

namespace Decision_Director;

public partial class AddEditRestaurant : ContentPage
{
    bool IsBeingEdited = false;
    Restaurant restaurantToBeEdited = new Restaurant();
	public AddEditRestaurant()
	{
		InitializeComponent();
	}

    public AddEditRestaurant(Restaurant restaurant)
    {
        InitializeComponent();
        restaurantToBeEdited = restaurant;
        IsBeingEdited = true;
        RestaurantGrid.BindingContext = restaurant;
    }

    private void ContentPage_Appearing(object sender, EventArgs e)
    {
        
    }

    private async void save_changes_button_Clicked(object sender, EventArgs e)
    {
        if (restaurant_name_Entry.Text == null || restaurant_name_Entry.Text.Replace(" ", "") == string.Empty)
        {
            await DisplayAlert("Error", "Restaurant name cannot be blank", "Okay");
            return;
        }
        restaurantToBeEdited.Name = restaurant_name_Entry.Text;

        if(restaurant_genre_picker.SelectedItem == null)
        {
            await DisplayAlert("Error", "Restaurant genre cannot be blank", "Okay");
            return;
        }
        restaurantToBeEdited.Genre = restaurant_genre_picker.SelectedItem.ToString();

        if (restaurant_type_picker.SelectedItem == null)
        {
            await DisplayAlert("Error", "Restaurant type cannot be blank", "Okay");
            return;
        }
        restaurantToBeEdited.Type = restaurant_type_picker.SelectedItem.ToString();

        if (IsBeingEdited)
        {
            await DBHandler.UpdateRestaurant(restaurantToBeEdited);
            RestaurantList.RefreshPlayerList();
            await Navigation.PopAsync();
            return;
        }
        else
        {
            await DBHandler.AddRestaurant(restaurantToBeEdited);
            RestaurantList.RefreshPlayerList();
            await Navigation.PopAsync();
            return;
        }
    }
}