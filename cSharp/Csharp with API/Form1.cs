using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Csharp_with_API
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void CreatePictureBoxFromUrl(string url, int x, int y)
        {
            PictureBox pictureBox = new PictureBox();
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.Width = 30;
            pictureBox.Height = 30;
            pictureBox.Location = new Point(x, y);
            var request = WebRequest.Create(url);
            //using (var response = request.GetResponse())
            //using (var stream = response.GetResponseStream())
            //{
            //    pictureBox.Image = Bitmap.FromStream(stream);
            //}
            pictureBox.LoadAsync(@url);

            panel2.Controls.Add(pictureBox);
        }




        private async void ViewBtn_Click_1(object sender, EventArgs e)
        {

            var url = "http://localhost:4050/info"; // Set the URL of your API

            using (var httpClient = new HttpClient())
            {
                try
                {
                    var apiResponse = await httpClient.GetAsync(url);

                    if (apiResponse.IsSuccessStatusCode)
                    {
                        var response = await apiResponse.Content.ReadAsStringAsync();
                        List<Record> records = JsonConvert.DeserializeObject<List<Record>>(response);

                        int x = 0, y = 0;

                        foreach (Record record in records)
                        {
                            PictureBox pictureBox = new PictureBox();
                            pictureBox.Location = new Point(x + 20, y += 60);
                            pictureBox.Size = new Size(50, 50);
                            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

                            panel2.Controls.Add(pictureBox);

                            if (!string.IsNullOrEmpty(record.profile_pic))
                            {
                                // Concatenate the filename with the path to the image folder
                                string imagePath = "http://localhost:4050/uploads/" + record.profile_pic;

                                // Load the image from the new URL
                                pictureBox.LoadAsync(imagePath);
                            }
                            else
                            {
                                pictureBox.Image = Properties.Resources.default_image;
                            }

                            Label label = new Label();
                            label.Location = new Point(x + 95, y + 10);
                            label.Text = record.name;
                            

                            panel2.Controls.Add(label);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Unable to Fetch Data", "Error");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }

        }
    }
}
