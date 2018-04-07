using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestHarness
{
    using DLLLibrary;
    using System.Threading;

    public class Program
    {

        #region Customization
        /// <summary>
        /// How many messages should each thread send before shutting down?
        /// </summary>
        private static int NUMBER_OF_MESSAGES_PER_THREAD = 5;

        /// <summary>
        /// How many threads should be sending messages concurrently?
        /// </summary>
        private static int NUMBER_OF_THREADS_SENDING_MESSAGES = 10;

        /// <summary>
        /// Minimum log level supported by your logger
        /// </summary>
        private static int MIN_LOG_LEVEL = 1;

        /// <summary>
        /// Maximum log level supported by your logger
        /// </summary>
        private static int MAX_LOG_LEVEL = 5;
        /// <summary>
        /// How often should an exception be sent into the logger - every X number of messages
        /// </summary>
        private static int HOW_OFTEN_AN_EXCEPTION = 3;

        #endregion

        static void Main(string[] args)
        {
            ErrorLogger logger = new ErrorLogger(7);
            List<Thread> threads = new List<Thread>();

            Console.WriteLine(" Test Harness Started ...... ");
            // Setup threads
            for (int i = 1; i <= NUMBER_OF_THREADS_SENDING_MESSAGES; i++)
            {
                int threadNumber = i;

                Thread newThread = new Thread(x =>
                {
                    Random rand = new Random();

                    string errorMessage = "Error Message, ApplicationID = 4, Message Number: {0}, Log Level: {1}";
                    Exception exception = new Exception();
                    int logLevel;

                    for (int messageNumber = 1; messageNumber <= NUMBER_OF_MESSAGES_PER_THREAD; messageNumber++)
                    {
                        logLevel = rand.Next(MIN_LOG_LEVEL, MAX_LOG_LEVEL);

                        logger.log(string.Format(errorMessage, messageNumber, logLevel), logLevel,
                        (messageNumber % HOW_OFTEN_AN_EXCEPTION == 0) ? exception : null);
                    }

                });
                threads.Add(newThread);
            }


            // Start threads
            foreach (Thread aThread in threads)
            {
                aThread.Start();
            }

            // Join threads
            foreach (Thread aThread in threads)
            {
                aThread.Join();
            }


            Console.WriteLine("All Threads Finished");
            Console.ReadLine();

        }
    }
}