using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            btnSendMessage.Enabled = false;

            tbServerMessage.Text += $"Client was started at {DateTime.Now}\r\n";
            tbServerMessage.Text += $"Enter a message to send to the server:\r\n";
        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            // ��������� ����� � ������� 
            //IPAddress address = IPAddress.Parse(textBoxIP.Text);
            IPAddress address = Dns.GetHostAddresses(Dns.GetHostName())[2];
            IPEndPoint endPoint = new IPEndPoint(address, 1024);

            // ��������� ��������� ������ �� ������ �볺���
            Socket client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            try
            {
                // ����� ��������� �'�������
                // ϳ��������� ������ - � ��������� ���������� ������ �����
                client_socket.Connect(endPoint);

                // �������� ����� �������� �� ���� �� ����� ���������� �볺��
                if (client_socket.Connected)
                {
                    // ³������� ����� � ��� ������� ����������
                    string message = tbClientMessage.Text;
                    client_socket.Send(Encoding.Default.GetBytes(message));
                    tbServerMessage.Text += message + "\r\n";
                    tbClientMessage.Text = "";

                    // ��������� ��������� ������, �� ��������������������� ��� �������� ��� �� �������
                    byte[] buffer = new byte[1024];

                    // �����, �� ���������� ����� - ������� ��������� �����
                    int len;

                    // ���� ��� ��������� ����� - ������������ �� �������� ��������� �����
                    // - ���� ���������� ������ Available > 0 - ��� ������� � �� ����� ��������
                    do
                    {
                        // ��������� �����
                        len = client_socket.Receive(buffer);
                        // �������� ���������� ����������� (�������) �� ���������� ���������� ���� � ��������� ������������(���������� ������������� ��������� �� ������)
                        tbServerMessage.Text += Encoding.Default.GetString(buffer, 0, len) + "\r\n";

                    } while (client_socket.Available > 0);
                    //tbServerMessage.Text += "----------------------------\r\n";
                    tbServerMessage.Text += $"Enter a message to send to the server:\r\n\r\n";
                }
                else
                {
                    MessageBox.Show("Error connection !");
                }

            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                client_socket.Shutdown(SocketShutdown.Both);
                client_socket.Close();
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Client";

            // �� ����'������ - ������ ������� ���� ������� ����� (��� ��������) - ������ Server �� ���� ������ �� �����������
            Process.Start("Server.exe");
        }

        private void tbClientMessage_TextChanged(object sender, EventArgs e)
        {
            if(tbClientMessage.Text.Length == 0)
            {
                btnSendMessage.Enabled = false;
            }else
            {
                btnSendMessage.Enabled = true;
            }
        }
    }
}