using Nancy.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RestClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static HttpClient client = new HttpClient();
        People p = new People();
        public MainWindow()
        {
            InitializeComponent();
            client.BaseAddress = new Uri("https://localhost:44362/");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        static async Task<Uri> Post(People p)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "api/listitems/", p);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }

        private async void btnadd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                p.name = tbname.Text;
                p.city = tbcity.Text;
                p.year = int.Parse(tbyear.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            await Post(p);

            tbcity.Clear();
            tbname.Clear();
            tbyear.Clear();
        }

        static async Task<People> Put(People p)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(
                $"api/listitems/{p.id}", p);
            response.EnsureSuccessStatusCode();

            // Deserialize the updated product from the response body.
            p = await response.Content.ReadAsAsync<People>();
            return p;
        }

        private async void btnedit_Click(object sender, RoutedEventArgs e)
        {
            int selectedid;
            p = list.SelectedItem as People;
            selectedid = p.id;

            p.name = tbname.Text;
            p.city = tbcity.Text;
            p.year = int.Parse(tbyear.Text);
            p.id = selectedid;

            await Put(p);
        }

        static async Task<HttpStatusCode> Delete(string id)
        {
            HttpResponseMessage response = await client.DeleteAsync(
                $"api/listitems/{id}");
            return response.StatusCode;
        }

        private async void btndelete_Click(object sender, RoutedEventArgs e)
        {
            int selectedid;
            p = list.SelectedItem as People;
            selectedid = p.id;

            await Delete(selectedid.ToString());

        }

        static async Task<List<People>> Get()
        {
            List<People> p = null;

            HttpResponseMessage response = await client.GetAsync("api/listitems/");
            if (response.IsSuccessStatusCode)
            {
                p = await response.Content.ReadAsAsync<List<People>>();
            }
            return p;
        }

        private async void btnget_Click(object sender, RoutedEventArgs e)
        {
            List<People> p = await Get();
            list.ItemsSource = p;
        }

        static async Task<People> GetOne(string path)
        {
            People p = null;

            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                p = await response.Content.ReadAsAsync<People>();
            }
            return p;
        }

        private async void btngetone_Click(object sender, RoutedEventArgs e)
        {
            int selectedid;
            //p = list.SelectedItem as People;
            selectedid = int.Parse(tbid.Text);

            string path = "api/listitems/" + selectedid.ToString();
            People p = await GetOne(path);
            tbname.Text = p.name;
        }


        private void btnsearch_Click(object sender, RoutedEventArgs e)
        {
           
        }

        
    }
}
