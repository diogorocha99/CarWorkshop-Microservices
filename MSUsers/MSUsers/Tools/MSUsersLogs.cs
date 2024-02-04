using Microsoft.AspNetCore.Connections;
using MSUsers.Models;
using RabbitMQ.Client;
using RestSharp;
using System.Text;
using System.Text.Json;

namespace MSUsers.Tools
{
    public class MSUsersLogs
    {
        public bool InsertLog(Logs x)
        {

            try
            {

                bool IsRunningInContainer = bool.TryParse(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), out var inDocker) && inDocker;
                var host = IsRunningInContainer ? "rabbitmq-service" : "localhost";
                var factory = new ConnectionFactory() { HostName = host };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {

                    channel.QueueDeclare(queue: "logsqueue",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    string message = JsonSerializer.Serialize(x);
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                         routingKey: "logsqueue",
                                         basicProperties: null,
                                         body: body);
                    Console.WriteLine(" [x] Sent {0}", message);

                }

                return true;

            }
            catch (Exception)
            {

                return false;

            }
        }


        public void CallLogsApi()
        {
            string url = "http://host.docker.internal:7292/api/Logs/InsertLogs";
            var client = new RestClient(url);
            var request = new RestRequest(url, Method.Post);
            request.AddHeader("Content-Type", "application/json");
            client.Execute(request);

        }
    }
}
