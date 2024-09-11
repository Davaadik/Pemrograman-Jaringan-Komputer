using System;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MovingObjectClient
{
    public partial class Form1 : Form
    {
        private TcpClient client;
        private NetworkStream stream;
        private Thread receiveThread;
        private Rectangle rect;
        private Pen red = new Pen(Color.Red);
        private SolidBrush fillBlue = new SolidBrush(Color.Blue);

        public Form1()
        {
            InitializeComponent();
            rect = new Rectangle(20, 20, 30, 30); // Inisialisasi ukuran kotak
            ConnectToServer();
        }

        // Method untuk koneksi ke server
        private void ConnectToServer()
        {
            try
            {
                client = new TcpClient("127.0.0.1", 8888); // Port should match the server's port
                stream = client.GetStream();

                // Membuat thread untuk menerima data dari server
                receiveThread = new Thread(new ThreadStart(ReceiveData));
                receiveThread.IsBackground = true;
                receiveThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to server: " + ex.Message);
            }
        }

        // Method untuk menerima data dari server
        private void ReceiveData()
        {
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    // Parsing posisi dari string yang diterima
                    string[] position = data.Split(',');
                    if (position.Length == 2)
                    {
                        int x = int.Parse(position[0]);
                        int y = int.Parse(position[1]);

                        // Update posisi kotak
                        rect.X = x;
                        rect.Y = y;

                        // Force refresh form untuk menggambar ulang
                        this.Invalidate();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error receiving data: " + ex.Message);
            }
        }

        // Method untuk menggambar kotak
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawRectangle(red, rect);
            g.FillRectangle(fillBlue, rect);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Menutup koneksi saat form ditutup
            if (client != null)
            {
                client.Close();
            }
            if (receiveThread != null)
            {
                receiveThread.Abort();
            }
        }
    }
}
