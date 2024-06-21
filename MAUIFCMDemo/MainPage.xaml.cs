
// using FirebaseAdmin;
// using FirebaseAdmin.Messaging;
// using FirebaseAdmin;
// using FirebaseAdmin.Messaging;
// using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using System.Text;
using CommunityToolkit.Mvvm.Messaging;
using MAUIFCMDemo.Models;

namespace MAUIFCMDemo;
public partial class MainPage : ContentPage
{
    int count = 0;
    private string _deviceToken = "";

    public MainPage()
    {
        InitializeComponent();
        WeakReferenceMessenger.Default.Register<PushNotificationReceived>(this, (r, m) =>
        {
            string msg = m.Value;
            NavigateToPage();
        });

        if (Preferences.ContainsKey("DeviceToken"))
        {
            _deviceToken = Preferences.Get("DeviceToken", "");
        }
        
        Console.WriteLine("Token: " + _deviceToken);
        NavigateToPage();

    }

    private void NavigateToPage()
    {
        if (Preferences.ContainsKey("NavigationID"))
        {
            string id = Preferences.Get("NavigationID", "");
            if (id == "1")
            {
                AppShell.Current.GoToAsync(nameof(NewPage1));
            }
            if (id == "2")
            {
                AppShell.Current.GoToAsync(nameof(NewPage2));
            }
            Preferences.Remove("NavigationID");
        }
    }

    private async void OnCounterClicked(object sender, EventArgs e)
    {

        var androidNotificationObject = new Dictionary<string, string>();
        androidNotificationObject.Add("NavigationID", "2");
        
        var pushnotificationRequest = new PushNotificationRequest
        {
            notification = new NotificationMessageBody
            {
                title = "Sample Title",
                body = "FCMTesting sample body"
            },
            data = androidNotificationObject,
            registration_ids = new List<string>{_deviceToken}
        };

        string url = "https://fcm.googleapis.com/fcm/send";
        
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("key", "=" + "AAAAGEAY6RM:APA91bFzUOqvlTaEub3Oc0h-sFhxP78KZH7DoXFHJHUYX6eTnLyZOWN9-2-pnDdY9qD5IiwySnohHyM5xYvRfRjzbXLXChnXayFEsSDjKUnJ-HNsIQKL-7nwDxGeOpZh8IJcRaFS-z82");

            string serilizedRequest = JsonConvert.SerializeObject(pushnotificationRequest);
            HttpResponseMessage response = await client.PostAsync(url, new StringContent(serilizedRequest, Encoding.UTF8, "application/json"));
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                await DisplayAlert("Success", "Notification sent successfully", "OK");   // Pragnesh uses slightly different message
            }
            else
            {
                await DisplayAlert("Error", "Error sending Notification", "OK");
            }
        }
        
    }
}