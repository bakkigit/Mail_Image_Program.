using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using MailKit;
using MailKit.Net.Imap;
using MimeKit;


namespace MailimageProgram
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<byte[]> imagesToScreen = new List<byte[]>();
        List<byte[]> imagesCheck = new List<byte[]>();
        private DispatcherTimer imageTimer = new DispatcherTimer();
        private int currentImageIndex = 0;

        public MainWindow()
        {
            InitializeComponent();
            InitializeTimer();

            // Set up the timer to change images every 3 seconds
            imageTimer.Interval = TimeSpan.FromSeconds(10);
            imageTimer.Tick += (sender, e) => ChangeImage();
            imageTimer.Start();
        }

        private void InitializeTimer()
        {
            // Create a timer with a 10-second interval (10000 milliseconds).
            Timer timer = new Timer(50000);
            timer.Elapsed += OnTimerElapsed;
            timer.AutoReset = true; // Set to true for repeated execution.
            timer.Enabled = true;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            ReadImagesFromMail();
            InsertImagesInWPF();
            ReadImagesFromDB();

        }

      
        public void ReadImagesFromMail() {
            using (var client = new ImapClient())
            {
                client.Connect("outlook.office365.com", 993, true); // IMAP server and port
                client.Authenticate("Mail", "Password");

                using (var dbContext = new ImageContext())
                {
                    var inbox = client.Inbox;
                    inbox.Open(FolderAccess.ReadWrite);

                    for (int i = 0; i < inbox.Count; i++)
                    {
                        var message = inbox.GetMessage(i);

                        foreach (var attachment in message.Attachments)
                        {
                            if (attachment is MimePart mimePart && mimePart.FileName != null)
                            {
                                if (mimePart.ContentType.MediaType == "image" &&
                                    (mimePart.ContentType.MediaSubtype == "jpeg" ||
                                     mimePart.ContentType.MediaSubtype == "png"))
                                {
                                    using (var memoryStream = new MemoryStream())
                                    {
                                        mimePart.Content.DecodeTo(memoryStream);
                                        var imageBytes = memoryStream.ToArray();

                                        var imageEntity = new ImageEntity
                                        {
                                            ImageData = imageBytes
                                        };

                                        dbContext.ImageTable.Add(imageEntity);
                                        
                                        dbContext.SaveChanges();


                                        try
                                        {
                                            inbox.MoveTo(new[] { i }, client.GetFolder("arkiv"));
                                        }
                                        catch (Exception ex)
                                        {
                                            // Handle the error gracefully (e.g., log it as an informational message)
                                            Console.WriteLine("Error moving message: " + ex.Message);
                                        }



                                    }
                                }
                            }
                        }
                    }
                }
                client.Disconnect(true);
            }
        }

        private void OnImageTimerTick(object sender, EventArgs e)
        {
            ChangeImage();
        }
        public void ChangeImage()
        {
            if (imagesToScreen.Count > 0)
            {
                // Create a BitmapImage from the current byte array
                BitmapImage image = new BitmapImage();
                using (var stream = new MemoryStream(imagesToScreen[currentImageIndex]))
                {
                    image.BeginInit();
                    image.StreamSource = stream;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit();
                }

                // Display the image in the Image control
                billedeContainer.Source = image;

                // Move to the next image (circular)
                currentImageIndex = (currentImageIndex + 1) % imagesToScreen.Count;
            }
            else
            {
                Console.WriteLine("No images to display.");
            }
        }



        public void InsertImagesInWPF()
        {
            if (imagesCheck.Count != imagesToScreen.Count)
            {
                imagesToScreen.Clear(); // Clear the existing list to match the length of imagesCheck
                imagesToScreen.AddRange(imagesCheck); // Add elements from imagesCheck to imagesToScreen
                
            }
            ChangeImage();
        }

        public void ReadImagesFromDB()
        {
            string connectionString = "Server=DESKTOP-IGG583J\\SQLEXPRESS;User Id=sa;Password=daoudi123;Database=billede;TrustServerCertificate=True;";
          

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT ImageData FROM ImageTable";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["ImageData"] != DBNull.Value)
                            {
                                byte[] imageByteArray = (byte[])reader["ImageData"];
                                imagesToScreen.Add(imageByteArray);
                            }
                        }
                    }
                }
            }
        }
    }
}