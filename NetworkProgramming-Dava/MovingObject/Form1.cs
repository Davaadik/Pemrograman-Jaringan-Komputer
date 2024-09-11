using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MovingObject
{
    public partial class Form1 : Form
    {
        Pen red = new Pen(Color.Red);
        Rectangle rect = new Rectangle(20, 20, 30, 30);
        SolidBrush fillBlue = new SolidBrush(Color.Blue);
        int slide = 10;
        TcpListener server;
        Thread listenerThread;

        public Form1()
        {
            InitializeComponent();
            timer1.Interval = 50;
            timer1.Enabled = true;

            // Start the server
            listenerThread = new Thread(StartServer);
            listenerThread.Start();
        }

        private void StartServer()
        {
            server = new TcpListener(IPAddress.Any, 8888);
            server.Start();

            while (true)
            {
                try
                {
                    TcpClient client = server.AcceptTcpClient();
                    Thread clientThread = new Thread(() => HandleClient(client));
                    clientThread.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            while (true)
            {
                try
                {
                    string message = $"{rect.X},{rect.Y}";
                    byte[] data = Encoding.ASCII.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                    Thread.Sleep(50); // Sync with the timer
                }
                catch (Exception)
                {
                    client.Close();
                    break;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            MoveRectangle();
            Invalidate();
        }

        private void MoveRectangle()
        {
            if (rect.X >= this.Width - rect.Width * 2)
                slide = -10;
            else if (rect.X <= rect.Width / 2)
                slide = 10;

            rect.X += slide;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawRectangle(red, rect);
            g.FillRectangle(fillBlue, rect);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            server.Stop();
            listenerThread.Abort();
            base.OnFormClosing(e);
        }
    }
}
