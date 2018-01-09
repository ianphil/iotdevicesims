namespace csvdevicesim
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Azure.Devices.Client;
    using Microsoft.Azure.Devices.Client.Transport.Mqtt;

    class Program
    {
        static async Task<int> Main(string[] args)
        {
            string connectionString = Environment.GetEnvironmentVariable("EdgeHubConnectionString");
            await Init(connectionString);

            return 0;
        }

        static async Task Init(string connectionString)
        {
            Console.WriteLine("Connection String {0}", connectionString);

            MqttTransportSettings mqttSetting = new MqttTransportSettings(TransportType.Mqtt_Tcp_Only);
            ITransportSettings[] settings = { mqttSetting };
            DeviceClient ioTHubModuleClient = DeviceClient.CreateFromConnectionString(connectionString, settings);

            await ioTHubModuleClient.OpenAsync();
            Console.WriteLine("IoT Hub module client initialized.");

            await SendEvent(ioTHubModuleClient);
        }

        static async Task SendEvent(DeviceClient deviceClient)
        {
            foreach(var data in Generator.Generate())
            {
                var cloudMessage = new Message(Encoding.UTF8.GetBytes(data));
                Console.WriteLine($"\t{DateTime.Now.ToLocalTime()}> Sending message: {data}");
                await deviceClient.SendEventAsync("output1", cloudMessage);
            }
        }
    }
}
