using System.Net.WebSockets;
using System.Text;

class SimpleChat
{
    static async Task ConnectAsync()
    {
        using (ClientWebSocket client = new ClientWebSocket())
        {
            try
            {
                //ввод uri адреса
                string uri;
                Console.Write("Введите URI: ");
                uri = Console.ReadLine();

                //подключение к серверу
                await client.ConnectAsync(new Uri(uri), CancellationToken.None);
                Console.WriteLine("Соединение с сервером успешно установлено");

                Console.Write("Введите сообщение: ");
                string message = Console.ReadLine();
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                Console.WriteLine($"Отправлено: {message}");

                byte[] receiveBuffer = new byte[1024];
                WebSocketReceiveResult result = await client.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                string response = Encoding.UTF8.GetString(receiveBuffer);
                Console.WriteLine($"Ответ от сервера: {response}");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Возникла ошибка: {ex.Message}");
            }
            finally
            {
                if(client.State == WebSocketState.Open)
                {
                    await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Закрытие клиента", CancellationToken.None);
                }
                Console.WriteLine("Соединение успешно закрыто!");
            }
        }
    }

    static async Task Main()
    {
        await ConnectAsync();
    }
}