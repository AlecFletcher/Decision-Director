using Decision_Director.Models;
using System.Collections.ObjectModel;

namespace Decision_Director;

public partial class RestaurantList : ContentPage
{
    public static ObservableCollection<Restaurant> _restaurants = new ObservableCollection<Restaurant>();
    public RestaurantList()
	{
		InitializeComponent();
	}

    private async void add_restaurant_clicked(object sender, EventArgs e)
    {
		AddEditRestaurant addEditRestaurant = new AddEditRestaurant();
		await Navigation.PushAsync(addEditRestaurant);
    }

    private async void edit_restaurant_button_clicked(object sender, EventArgs e)
    {
        ImageButton imageButton = (ImageButton)sender;
        Restaurant restaurant = (Restaurant)imageButton.BindingContext;
        AddEditRestaurant editRestaurant = new AddEditRestaurant(restaurant);
        await Navigation.PushAsync(editRestaurant);
    }

    private async void delete_restaurant_button_clicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Delete?", "Are you sure you want to delete this restaurant?", "Yes", "No");
        if (answer)
        {
            ImageButton imageButton = (ImageButton)sender;
            Restaurant restaurant = (Restaurant)imageButton.BindingContext;
            await DBHandler.RemoveRestaurant(restaurant);
            RefreshPlayerList();
        }

    }

    private void ContentPage_Appearing(object sender, EventArgs e)
    {
        RefreshPlayerList();
        restaurant_collectionView.ItemsSource = _restaurants;
    }

    public static async void RefreshPlayerList()
    {
        _restaurants.Clear();
        var restaurantList = await DBHandler.GetAllRestaurants();
        foreach (var restaurant in restaurantList)
        {
            _restaurants.Add(restaurant);
        }
    }
}