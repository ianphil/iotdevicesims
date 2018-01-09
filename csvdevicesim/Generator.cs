namespace csvdevicesim
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;
    using System.Threading;

    public class Motor
    {
        public string liftOperation { get; set; }
        public string eventTime { get; set; }
        public string speed { get; set; }
        public string torque { get; set; }
        public string volts { get; set; }
    }

    public class Generator
    {
        static List<Motor> CreatePayload()
        {
            string filename = "motor.csv";

            var motorReadingList = new List<Motor>();

            var lines = File.ReadAllLines(filename);
            foreach(var line in lines)
            {
                if (line == "time,running_speed,torque,voltage") continue;

                var data = line.Split(',').ToArray();
                var motorReading = new Motor
                {
                    liftOperation = Guid.NewGuid().ToString(),
                    eventTime = DateTime.Now.ToUniversalTime().ToString("o"),
                    speed = data[1],
                    torque = data[2],
                    volts = data[3]
                };

                motorReadingList.Add(motorReading);
            }

            return motorReadingList;
        }

        public static IEnumerable<string> Generate()
        {
            var payload = CreatePayload();

            while(true)
            {
                foreach(var reading in payload)
                {
                    var data = JsonConvert.SerializeObject(reading);
                    yield return data;
                    Thread.Sleep(1000);
                }

                Thread.Sleep(30000);
            }
        }
    }
}
