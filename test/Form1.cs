using System;
using System.Net.Http;
using System.Windows.Forms;

namespace test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string apiUrl = "http://127.0.0.1/phpcsharp/addmembers.php";
            string prenom = textBox1.Text;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var postData = new System.Collections.Generic.Dictionary<string, string>
                    {
                        { "prenom", prenom }
                    };

                    var content = new FormUrlEncodedContent(postData);
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseBody);
                    MessageBox.Show("Le prénom a bien été envoyé", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception)
                {
                    MessageBox.Show("Erreur lors l'ajout du membre", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            string apiUrl = "http://127.0.0.1/phpcsharp/getmembers.php";
            listView1.Clear();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    string[] prenoms = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(responseBody);
                    listView1.Columns.Add("Prénom", 100);
                    listView1.View = View.Details;
                    listView1.FullRowSelect = true;
                    foreach (var prenom in prenoms)
                    {
                        listView1.Items.Add(new ListViewItem(prenom));
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Erreur lors la récupération des membres", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

        }

        private async void supprimerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string prenom = listView1.SelectedItems[0].Text;
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        string apiUrl = "http://127.0.0.1/phpcsharp/deletemembers.php?prenom=" + prenom;
                        HttpResponseMessage response = await client.PostAsync(apiUrl, null);
                        response.EnsureSuccessStatusCode();
                        listView1.Items.Remove(listView1.SelectedItems[0]);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Erreur lors la suppression du membre", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private async void modifierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string prenom = listView1.SelectedItems[0].Text;

                string nouveauPrenom = InputDialog.Show("Nouveau prénom :", "Modifier le prénom", prenom);

                if (!string.IsNullOrEmpty(nouveauPrenom))
                {
                    listView1.SelectedItems[0].Text = nouveauPrenom;

                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            string apiUrl = "http://127.0.0.1/phpcsharp/updatemembers.php";

                            var formData = new System.Collections.Generic.Dictionary<string, string>
                            {
                                { "ancien_prenom", prenom },
                                { "nouveau_prenom", nouveauPrenom }
                            };

                            var content = new FormUrlEncodedContent(formData);
                            HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                            response.EnsureSuccessStatusCode();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Erreur lors la mise à jour du membre", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
        }

        public class InputDialog
        {
            public static string Show(string title, string promptText, string defaultValue = "")
            {
                Form form = new Form();
                form.Text = title;
                Label label = new Label() { Left = 50, Top = 20, Text = promptText };
                TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 200, Text = defaultValue };
                Button okButton = new Button() { Text = "Envoyer", Left = 50, Width = 100, Top = 80 };
                Button cancelButton = new Button() { Text = "Annuler", Left = 150, Width = 100, Top = 80 };

                okButton.Click += (sender, e) => { form.DialogResult = DialogResult.OK; };
                cancelButton.Click += (sender, e) => { form.DialogResult = DialogResult.Cancel; };

                form.Controls.Add(label);
                form.Controls.Add(textBox);
                form.Controls.Add(okButton);
                form.Controls.Add(cancelButton);

                form.AcceptButton = okButton;
                form.CancelButton = cancelButton;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    return textBox.Text;
                }

                return defaultValue;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
