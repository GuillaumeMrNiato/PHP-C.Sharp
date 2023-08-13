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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string email = emailText.Text;
            string password = passwordText.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Veuillez remplir tous les champs.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string apiUrl = "http://127.0.0.1/phpcsharp/login.php"; 

                    var formData = new Dictionary<string, string>
                    {
                        { "username", email },
                        { "password", password }
                    };

                    var content = new FormUrlEncodedContent(formData);
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(responseContent);
                        MessageBox.Show("Bienvenue !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Form1 newForm = new Form1();
                        newForm.Show();
                        Hide();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la connexion", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Erreur lors de la connexion", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Register newForm = new Register();
            newForm.Show();
            Hide();
        }
    }
}
