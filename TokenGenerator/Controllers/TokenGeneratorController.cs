using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using TokenGenerator.Data;
using TokenGenerator.Models;

namespace TokenGenerator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenGeneratorController : ControllerBase
    {

        private readonly ILogger<TokenGeneratorController> _logger;

        public TokenGeneratorController(TokenGeneratorContext context)
        {
            _context = context;
        }

 
        private readonly TokenGeneratorContext _context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerCard>>> GetCard()
        {
            return await _context.CustomerCard.ToListAsync();
        }

        private void PublishToMessageQueue(string integrationEvent, string eventData)
        {

            // TOOO: Reuse and close connections and channel, etc, 
            var factory = new ConnectionFactory() { HostName = "localhost"};

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "Card.Add",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(eventData);

                channel.BasicPublish(exchange: "CustomerCard",
                                     routingKey: "Card.Add",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine(" [x] Sent {0}", eventData);
            }
        }

       

        [HttpPost]
        public async Task<ActionResult<CustomerCard>> PostCardInfo(CustomerCard card)
        {
            try
            {
                card.Token = Util.GenerateToken(card.CVV, card.CardNumber);
                card.RegistationDate = DateTime.Now;
                _context.CustomerCard.Add(card);
                await _context.SaveChangesAsync();
                _context.Entry(card).GetDatabaseValues();
                var integrationEventData = JsonConvert.SerializeObject(new CustomerCard
                {
                    CardID = card.CardID,
                    RegistationDate = card.RegistationDate,
                    Token = card.Token
                });
                PublishToMessageQueue("CardData.Add", integrationEventData);

                return CreatedAtAction("GetCard", new CustomerCard { CustomerID = card.CustomerID }, new CustomerCard { CardID = card.CardID, RegistationDate = card.RegistationDate, Token = card.Token });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
           
        }
    }

}
