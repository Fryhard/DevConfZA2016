using EasyNetQ;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;

namespace Fryhard.DevConfZA2016.Common
{
    public static class BusHost
    {
        private static ILog _Log = LogManager.GetLogger(typeof(BusHost));
        private static IBus _Bus;

        public static string ConnectionString
        {
            get
            {
                string conStr = ConfigurationManager.ConnectionStrings["RabbitMQ"].ConnectionString;
                if (String.IsNullOrWhiteSpace(conStr))
                {
                    conStr = "host=localhost;timeout=120;requestedHeartbeat=120";
                    _Log.Warn ("Could not read connection string from config file; using default.");
                }
                return conStr;
            }
        }

        public static IBus Bus
        {
            get
            {
                if (_Bus == null || !_Bus.IsConnected)
                {
                    _Bus = null;
                    Register();
                }
                return _Bus;
            }
            set
            {
                //Since we are setting the bus value make sure that we dispose the old instance first.
                if (_Bus != null && _Bus != value)
                {
                    _Log.Warn("Bus value has been altered. Disposing existing bus object.");
                    Dispose();
                }

                _Bus = value;
            }
        }

        /// <summary>
        /// This will attempt to publish to the bus twice. Should the second publish fail it will still throw an exception.
        /// </summary>
        public static void SafePublish<T>(T message, BusTopic topic) where T : class
        {
            try
            {
                Publish(message, topic);
            }
            catch (Exception ex)
            {
                _Log.Error("An error occurred while publishing message to bus. " + ex.Message, ex);
                Publish(message, topic);
            }
        }

        public static void Publish<T>(T message, BusTopic topic) where T : class
        {
            Bus.Publish(message, topic.ToString());
        }

        public static void PublishAsync<T>(T message, BusTopic topic) where T : class
        {
            Bus.PublishAsync(message, topic.ToString());
        }

        public static IDisposable Subscribe<T>(BusSubscription subscription, BusTopic topic, Action<T> onMessage) where T : class
        {
            return Bus.Subscribe(subscription.ToString(), onMessage, x => x.WithTopic(topic.ToString()));
        }

        public static IDisposable SubscribeAsync<T>(BusSubscription subscription, BusTopic topic, Func<T, Task> onMessage) where T : class
        {
            return Bus.SubscribeAsync(subscription.ToString(), onMessage, x => x.WithTopic(topic.ToString()));
        }

        public static IDisposable SubscribeAsync<T>(BusSubscription subscription, BusTopic topic, Func<T, Task<T>> onMessage) where T : class
        {
            return Bus.SubscribeAsync(subscription.ToString(), onMessage, x => x.WithTopic(topic.ToString()));
        }

        public static IDisposable SubscribeAsync<T>(BusSubscription subscription, Func<T, Task<T>> onMessage) where T : class
        {
            return Bus.SubscribeAsync(subscription.ToString(), onMessage);
        }

        public static void Register(string connectionString = null)
        {
            string connString = ConnectionString;
            if (!String.IsNullOrWhiteSpace(connectionString))
            {
                connString = connectionString;
            }

            _Bus = RabbitHutch.CreateBus(connString);
        }

        public static void Dispose ()
        {
            try
            {
                if (_Bus != null)
                {
                    _Bus.Dispose();
                    _Bus = null;
                }
            }
            catch (Exception ex)
            {
                _Log.Error("An error occurred while disposing bus. " + ex.Message, ex);
            }
        }
    }
}
