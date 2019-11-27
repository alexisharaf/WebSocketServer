using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Security.Permissions;

namespace WebSocketServer
{
    class Program
    {
        static void Main(string[] args)
        {

            // Устанавливаем для сокета адрес и порт
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11111);

            // Создаем сокет Tcp/Ip
            Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Назначаем сокет локальной конечной точке и слушаем входящие сокеты
            try
            {
                sListener.Bind(ipEndPoint);
                sListener.Listen(10);

                Console.WriteLine("Ожидаем соединение через порт {0}", ipEndPoint);

                // Программа приостанавливается, ожидая входящее соединение
                Socket handler = sListener.Accept();

                // Начинаем слушать соединения
                while (true)
                {
                   
                    string data = null;

                    // Мы дождались клиента, пытающегося с нами соединиться

                    byte[] bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);

                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);

                    Console.Write("Полученный текст: " + data + "\n\n");

                    // Отправляем ответ клиенту
                    string reply = "Получено символов: " + data.Length.ToString();

                    byte[] msg = Encoding.UTF8.GetBytes(reply);
                    
                    handler.Send(msg);

                    if (data.IndexOf("#END#") > -1)
                    {
                        Console.WriteLine("Клиент отключился.");
                        break;
                    }

                    
                }

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadLine();
            }
        }
    





    }
}
