using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;

namespace test
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            string nom = nomText.Text;
            string prenom = prenomText.Text;
            string email = emailText.Text;
            string password = mdpText.Text;

            if (string.IsNullOrWhiteSpace(nom) || string.IsNullOrWhiteSpace(prenom) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Veuillez remplir tous les champs.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string apiUrl = "http://127.0.0.1/phpcsharp/register.php";
                    var formData = new Dictionary<string, string>
                    {
                        { "nom", nom },
                        { "prenom", prenom },
                        { "email", email },
                        { "password", password }
                    };

                    var content = new FormUrlEncodedContent(formData);
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(responseContent);
                        MessageBox.Show("Le compte a bien été créé, redirection vers la page de connexion...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        Login newForm = new Login();
                        newForm.Show();
                        Hide();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de l'inscription", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Erreur lors de l'inscription", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Login newForm = new Login();
            newForm.Show();
            Hide();
        }
    }
}
