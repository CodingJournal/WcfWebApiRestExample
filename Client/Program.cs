using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;

namespace Client
{
    [ServiceContract]
    public interface IRestService
    {
        [OperationContract]
        [WebGet]
        int Multiply(int a, int b);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped)]
        bool CreateNewUser(string username, string password);
    }


    class Program
    {
        private const string RestUri = "http://localhost:8080";

        static void Main(string[] args)
        {
            try
            {
                using (var cf = new ChannelFactory<IRestService>(new WebHttpBinding(), RestUri))
                {
                    cf.Endpoint.Behaviors.Add(new WebHttpBehavior());
                    var channel = cf.CreateChannel();

                    Console.WriteLine("Calling Multiply: ");
                    var result = channel.Multiply(6, 23);
                    Console.WriteLine("6*23 = {0}", result);

                    Console.WriteLine("");

                    Console.WriteLine("Calling CreateNewUser (POST): ");
                    var result1 = channel.CreateNewUser("foobar", "verystrongpasswortnot");
                    Console.WriteLine("User created? {0}", result1);
                    Console.WriteLine("");
                }

                Console.WriteLine("Press <ENTER> to terminate");
                Console.ReadLine();
            }
            catch (CommunicationException e)
            {
                Console.WriteLine("An exception occurred: {0}", e.Message);
            }
        }
    }
}
