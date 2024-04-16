using Decision_Director.Models;

namespace Decision_Director;

public partial class RestaurantPicker : ContentPage
{
	public RestaurantPicker()
	{
		InitializeComponent();
	}

    private async void edit_restaurantList_clicked(object sender, EventArgs e)
    {
        RestaurantList restaurant = new RestaurantList();
        await Navigation.PushAsync(restaurant);
    }

    private async void find_restaurant_button_clicked(object sender, EventArgs e)
    {
        var selectedRestaurantList = await DBHandler.GetRestaurantWithConstraint(restaurant_genre_picker.SelectedItem.ToString(), restaurant_type_picker.SelectedItem.ToString());

        if (selectedRestaurantList.Count() == 0)
        {
            await DisplayAlert("Error", "No restaurants with that criteria", "Okay");
            return;
        }
        List<Restaurant> restaurantList = selectedRestaurantList.ToList();
        Random rand = new Random();
        int index = rand.Next(0, restaurantList.Count());

        selected_restaurant_label.Text = restaurantList[index].Name;
    }

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        restaurant_genre_picker.SelectedIndex = 0;
        restaurant_type_picker.SelectedIndex = 0;
    }
}