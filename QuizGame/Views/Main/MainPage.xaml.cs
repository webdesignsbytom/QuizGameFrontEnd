using Npgsql;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using System;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Data;
using System.Diagnostics;
using Newtonsoft.Json;

namespace QuizGame.Views.Main
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            OnAppearing();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CallApiAsync();
        }

        static async Task CallApiAsync()
        {
            string apiUrl = "https://quiz-5cpoinrp2-webdesignbytom.vercel.app/users/all-users";

            using var httpClient = new HttpClient();

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                response.EnsureSuccessStatusCode(); // Ensure the request was successful

                string responseBody = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(responseBody);

                // Parse the JSON response and extract the user data
                var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseBody);


                var users = jsonResponse.data.users.ToObject<List<User>>();

                // Process the user data
                foreach (var user in users)
                {
                    Debug.WriteLine($"User ID: {user.id}, Username: {user.username}, Score: {user.score}");
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
            }
        }

        public class User
        {
            public string id { get; set; }
            public string username { get; set; }
            public int score { get; set; }
        }

        private async void AddScoreBtn_Clicked(object sender, EventArgs e)
        {
            var username = "NewUser";
            var score = 100;

            var ApiUrlPost = "https://quiz-5cpoinrp2-webdesignbytom.vercel.app/users/post-score";
            using var httpClient = new HttpClient();

            var requestBody = new
            {
                username,
                score
            };

            try
            {
                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(ApiUrlPost, content);

                response.EnsureSuccessStatusCode(); // Ensure the request was successful

                string responseBody = await response.Content.ReadAsStringAsync();

                // Process the response body here
                Debug.WriteLine(responseBody);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}