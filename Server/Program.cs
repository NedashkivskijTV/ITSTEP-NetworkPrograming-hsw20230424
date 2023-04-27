using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    internal class Program
    {

        static void Main(string[] args)
        {
            int portNumber = 1024;

            IPAddress address = Dns.GetHostAddresses(Dns.GetHostName())[2];
            IPEndPoint endPoint = new IPEndPoint(address, portNumber);
            Socket pass_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP); 
            pass_socket.Bind(endPoint); 
            pass_socket.Listen(10);

            Console.WriteLine($"Server was started at {DateTime.Now} port {portNumber}, address {address}");

            //// Отримання списку IP-адрес microsoft.com
            //// їх виведення у консоль на боці сервера та
            //// передача їх у повідомленні клієнту
            //IPAddress[] addresses = Dns.GetHostAddresses("microsoft.com");
            //string str = "";
            //foreach (var adr in addresses)
            //{
            //    Console.WriteLine(adr);
            //    str += adr + "\r\n";
            //}

            try
            {
                // Прослуховування - постійне підключення в пасивному режимі
                while (true)
                {
                    // Створення сокету, який буде підключати клієнта та отримувати відправлені ним дані
                    Socket ns = pass_socket.Accept(); // створення сокета 

                    // Інформаційне повідомлення про підключення клієнта
                    //Console.WriteLine($"Client #{ns.LocalEndPoint} connected"); // виведення 
                    Console.WriteLine($"Client #{ns.RemoteEndPoint} connected"); // виведення IP та порта клієнта

                    // Отримання повідомлення та його виведення
                    String clientMessage = "";
                    byte[] buffer = new byte[1024];
                    int len;
                    do
                    {
                        len = ns.Receive(buffer);
                        clientMessage += Encoding.Default.GetString(buffer, 0, len);
                    } while (ns.Available > 0);

                    Console.WriteLine(clientMessage);


                    // Відправка даних
                    // - метод Send приймає параметр, переведений у байти
                    // - використовується клас Encoding,
                    // - спосіб кодування Default(за замовчуванням UTF8)
                    // - метод GetBytes
                    ns.Send(Encoding.Default.GetBytes($"Server {ns.LocalEndPoint} ansver : client data was received at {DateTime.Now}"));

                    // Закриття сокета - зазвичай розташовується у блоці finally
                    // - закриття комунікації між клієнтом і сервером
                    // - закриття сокета
                    ns.Shutdown(SocketShutdown.Both);
                    ns.Close();
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}