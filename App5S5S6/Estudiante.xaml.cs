using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App5S5S6
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Estudiante : ContentPage
    {
        public HttpClient Client = new HttpClient();
        Datos.Estudiante estudiante = new Datos.Estudiante();
        public Estudiante(Datos.Estudiante _estudiante)
        {
            InitializeComponent();
            estudiante = _estudiante;
            if (estudiante.codigo == 0)
            {
                this.Title = "Nuevo estudiante";
                txtCod.Text = string.Empty;
                txtCod.IsEnabled = true;
                txtNom.Text = estudiante.nombre;
                txtApel.Text = estudiante.apellido;
                txtEdad.Text = estudiante.edad.ToString();
                this.btnGrabar.Text = "Grabar";
            }
            else
            {
                this.Title = "Editar estudiante";
                txtCod.Text = estudiante.codigo.ToString();
                txtCod.IsEnabled = false;
                txtNom.Text = estudiante.nombre;
                txtApel.Text = estudiante.apellido;
                txtEdad.Text = estudiante.edad.ToString();
                this.btnGrabar.Text = "Actualizar";
            }
        }

        private async void btnGrabar_Clicked(object sender, EventArgs e)
        {
            if (estudiante.codigo.Equals(0))//nuevo
            {
                estudiante.codigo = Convert.ToInt32(txtCod.Text);
                estudiante.nombre = txtNom.Text;
                estudiante.apellido = txtApel.Text;
                estudiante.edad = Convert.ToInt32(txtEdad.Text);
                if (Insertar(estudiante))
                {
                    await DisplayAlert("Mensaje", "Estudiante insertado", "Aceptar");
                }

            }
            else//actualizar
            {
                estudiante.nombre = txtNom.Text;
                estudiante.apellido = txtApel.Text;
                estudiante.edad = Convert.ToInt32(txtEdad.Text);
                if (ActualizarUsuario(estudiante))
                {
                    await DisplayAlert("Mensaje", "Estudiante actualizado", "Aceptar");
                }


            }
        }
        public bool Insertar(Datos.Estudiante estudiante)
        {
            bool resultado = false;
            try
            {
                var Client = new HttpClient();
                var parametros = new Dictionary<string, string>();
                parametros.Add("codigo", estudiante.codigo.ToString());
                parametros.Add("nombre", estudiante.nombre);
                parametros.Add("apellido", estudiante.apellido);
                parametros.Add("edad", estudiante.edad.ToString());

                Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var response = Client.PostAsync("http://192.168.0.106:80/Uisrael2021/postEstudiante.php", new FormUrlEncodedContent(parametros)).Result;
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
        public bool ActualizarUsuario(Datos.Estudiante estudiante)
        {
            bool resultado = false;
            try
            {
                var Client = new HttpClient();
                var response = Client.PutAsync("http://192.168.0.106:80/Uisrael2021/postEstudiante.php?codigo=" + estudiante.codigo + "&nombre=" + estudiante.nombre + "&apellido=" + estudiante.apellido + "&edad=" + estudiante.edad + "", null).Result;
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

        private async void btnRegresar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}