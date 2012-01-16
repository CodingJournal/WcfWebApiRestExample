using System;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Server
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


    public class RestService : IRestService
    {
        public int Multiply(int a, int b)
        {
            Console.WriteLine("Multiply received");
            return a * b;
        }

        public bool CreateNewUser(string username, string password)
        {
            Console.WriteLine("CreateNewUser received");
            if (username.Length < 3 || password.Length < 8)
            {
                return false;
            }

            // [ Do lots of stuff ]

            return true;
        }
    }


    class Program
    {
        private const string RestUri = "http://localhost:8080/";

        static void Main(string[] args)
        {
            var host = new WebServiceHost(typeof(RestService), new Uri(RestUri));
            try
            {
                host.AddServiceEndpoint(typeof(IRestService), new WebHttpBinding(), "");
                host.Open();

                Console.WriteLine("Press <ENTER> to terminate");
                Console.ReadLine();

                host.Close();
            }
            catch (CommunicationException e)
            {
                Console.WriteLine("An exception occurred: {0}", e.Message);
                host.Abort();
            }
        }
    }
}
