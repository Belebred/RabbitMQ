using imap.consumer.Core;
using imap.library;
using imap.rabbitmq.Extensions;
using RabbitMQ.Client;

namespace imap.service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            
            builder.Services.UseDataBus();
            builder.Services.AddPublisher("MailMessagePublisher");
            builder.Services.AddConsumer("MailMessageConsumer");

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            //var factory = new ConnectionFactory() { HostName = "localhost" };
            //var pers = new PersistentConnectionFactory(factory, "a");
            //var pool = new ChannelsPool(pers);
            //var pub = new Publisher(pool);

            //var cons = new Consumer(pool);
            //cons.AddDirectConsumer();
            //for (var i = 0; i < 10; i++)
            //{
            //    Thread.Sleep(1500);
            //    pub.Publish($"Erotishna {i}", "booking");
            //}

            app.UseAuthorization();

            app.MapControllers();
            
            app.Run();
        }
    }
}