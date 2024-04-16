using Decision_Director.Models;
using Plugin.LocalNotification;

namespace Decision_Director
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void restaurant_button_Clicked(object sender, EventArgs e)
        {
            RestaurantPicker restaurantPicker = new RestaurantPicker();
            await Navigation.PushAsync(restaurantPicker);
        }

        private void board_game_button_Clicked(object sender, EventArgs e)
        {
            BoardGameView boardGameView = new BoardGameView();
            Navigation.PushAsync(boardGameView);
        }

        private async void person_button_Clicked(object sender, EventArgs e)
        {
            PlayerPicker picker = new PlayerPicker();
            await Navigation.PushAsync(picker);
        }

        private async void ContentPage_Loaded(object sender, EventArgs e)
        {
            if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
            {
                await LocalNotificationCenter.Current.RequestNotificationPermission();
            }


            var notification = new NotificationRequest
            {
                NotificationId = 100,
                Title = "Hello?",
                Description = "It's been 30 days. If you're decisive, think about removing me to clear space!",
                ReturningData = "", // Returning data when tapped on notification.
                Schedule =
    {
        NotifyTime = DateTime.Now.AddDays(30) // Used for Scheduling local notification, if not specified notification will show immediately.
    }
            };
            await LocalNotificationCenter.Current.Show(notification);

            await DBHandler.Init();
        }
    }

}
