using App5S5S6.Datos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App5S5S6
{
    public partial class MainPage : ContentPage
    {
        public HttpClient Client = new HttpClient();
        List<Datos.Estudiante> resultado = new List<Datos.Estudiante>();
        public MainPage()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Select();
        }
        public async void Select()
        {
            try
            {
                var Uri = "http://192.168.0.106:80/Uisrael2021/postEstudiante.php";
                HttpResponseMessage response = await Client.GetAsync(Uri);
                if (response.IsSuccessStatusCode)
                {
                    string PlacesJson = response.Content.ReadAsStringAsync().Result;
                    if (PlacesJson.Length > 0)
                    {
                        resultado = JsonConvert.DeserializeObject<List<Datos.Estudiante>>(PlacesJson);
                    }
                }
                else
                {
                    //error en el servicio
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            lvDat.ItemsSource = null;
            lvDat.ItemsSource = resultado;
        }
        private void btnGet_Clicked(object sender, EventArgs e)
        {
            Select();
        }

        private async void btnPost_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Estudiante(new Datos.Estudiante()));
        }

        private async void btnPut_Clicked(object sender, EventArgs e)
        {
            try
            {
                var estudiante = lvDat.SelectedItem as Datos.Estudiante;
                await Navigation.PushAsync(new Estudiante(estudiante));
            }
            catch
            {
                await DisplayAlert("Error", "Seleccione un estudiante", "Aceptar");
            }
        }

        private async Task btnDelete_ClickedAsync(object sender, EventArgs e)
        {
            try
            {
                var estudiante = lvDat.SelectedItem as Datos.Estudiante;
                bool answer = await DisplayAlert("Consulta?", "Desea eliminar el estudiante=" + estudiante.codigo.ToString(), "Yes", "No");
                if (answer)
                {
                    //eliminar
                    Eliminar(estudiante.codigo);
                    //cargar de nuevo
                    Select();
                }

            }
            catch
            {
                await DisplayAlert("Error", "Seleccione un estudiante", "Aceptar");
            }
        }
        public bool Eliminar(int codigo)
        {
            bool resultado = false;
            try
            {
                var Client = new HttpClient();
                var response = Client.DeleteAsync("http://192.168.0.106:80/Uisrael2021/postEstudiante.php?codigo=" + codigo).Result;
                if (response.IsSuccessStatusCode)
                {
                    resultado = true;
                }
                else
                {
                    // resultado diferente de 200 o succesfull
                }
            }
            catch (Exception ex)
            {
                //error
            }
            return resultado;
        }
    }
}
