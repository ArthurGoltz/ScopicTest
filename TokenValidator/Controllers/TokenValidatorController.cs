using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TokenValidator.Data;
using TokenValidator.Models;

namespace TokenValidator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenValidatorController : ControllerBase
    {
        private readonly TokenValidatorContext _context;

        public TokenValidatorController(TokenValidatorContext context)
        {
            _context = context;
        }

        public static void Main(string[] args)
        {
            try
            {
                ListenForIntegrationEvents();
                CreateHostBuilder(args).Build().Run();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
      
        }

        private static void ListenForIntegrationEvents()
        {

            var factory = new ConnectionFactory() { HostName = "localhost"};
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "Card.Add",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
            {
                string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
                var ConnectionString = new ConfigurationBuilder()
                    .SetBasePath(projectPath)
                    .AddJsonFile("appsettings.json")
                    .Build().GetConnectionString("CardTest");
                var contextOptions = new DbContextOptionsBuilder<TokenValidatorContext>()
                    .UseSqlServer(ConnectionString)
                    .Options;
                var dbContext = new TokenValidatorContext(contextOptions);

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);
            };
                channel.BasicConsume(queue: "Card.Add",
                                         autoAck: true,
                                         consumer: consumer);
            }
        }

        [HttpPost]
        public async Task<string> PostCardInfo(CustomerData cardRequest)
        {
            try
            {
                var cardBD = _context.CustomerCard.SingleOrDefault(x => x.CardID == cardRequest.CardID);
                Console.WriteLine(cardBD.CardNumber);
                bool Validated = Util.ValidateToken(cardRequest, cardBD);
                return JsonConvert.SerializeObject(Validated);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
         
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                });
    }




}
