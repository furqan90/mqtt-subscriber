﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTTSubscribeClient
{
    class Program
    {
        static string Host = "207.180.236.206";
        static string Username = "mqtt_client";
        static string Password = "nopass";
        public static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                Console.WriteLine("Please provide topic to Subscribe");
                return;
            }

            var factory = new ConnectionFactory() { HostName = Host, UserName = Username, Password = Password };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var queueName = "client1";
                //channel.ExchangeDeclare(exchange: "", type: ExchangeType.Topic, durable : true);
                channel.QueueDeclare(queueName, true);
                foreach (var arg in args)
                {
                    channel.QueueBind(
                        queue: queueName,
                        exchange: "amq.topic",
                        routingKey: arg.Replace('/', '.')
                                      );
                }

                Console.WriteLine(" [*] Waiting for Message.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    
                    var message = Encoding.UTF8.GetString(body.ToArray());
                    Console.WriteLine(" [x] {0} - {1}", message, ea.RoutingKey);
                };
                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
