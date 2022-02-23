using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oshw3
{
    class simulator
    {
        public class Process
        {
            public int arrival { get; set; }
            public int prio { get; set; }
            public int bursttime { get; set; }
            public string processname { get; set; }
            public int runtime { get; set; } = 0;
            public bool activated { get; set; } = false;
            public int endtime { get; set; }

        }



        public static int numprocesses = 3;
        public static int time = 0;
        public static bool stop = false;
        public static int maxtime = 0;
        public static int quantum = 2;
        public static int highestprio = 1000000;
        public static string currentrunning;
        public static int runtime;

        //loop to read input file and assgn each item to the list
        static List<Process> processes = new List<Process>() {
            new Process
            {
               // processname = generatestring(),
               processname = "a",
                arrival = 0,
                prio = 3,
                bursttime = 5
            },
            new Process
            {
               // processname = generatestring(),
                processname = "b",
                arrival = 2,
                prio = 2,
                bursttime = 5
            },

            new Process
            {
                processname = "c",
               // processname = generatestring(),
                arrival = 3,
                prio = 1,
                bursttime = 3
            }
        };

        static Process temp = new Process();

        //used to create the new process name
        public static string generatestring()
        {
            int length = 2;

            // creating a StringBuilder object()
            StringBuilder str_build = new StringBuilder();
            Random random = new Random();

            char letter;

            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);
            }
            return str_build.ToString();
            System.Console.WriteLine(str_build.ToString());
        }


        static void Main(string[] args)
        {
            Console.WriteLine($"{processes[0].processname}");
            Console.WriteLine($"{processes[2].processname}");
            temp = processes[0];
            Console.WriteLine($"{temp.processname}");
            processes[0] = processes[2];
            processes[2] = temp;
            Console.WriteLine($"{processes[0].processname}");
            Console.WriteLine($"{processes[2].processname}");

            //finds the amount of time needed total
            foreach (Process name in processes)
            {
                maxtime = maxtime + name.bursttime;
            }

            //while there is still time remaining, continue running
            while(time != maxtime)
            {
                //checks if process is arriving or finishing
                foreach(Process name in processes)
                {
                    //item has entered the queue
                    if(time == name.arrival)
                    {
                        name.activated = true;
                        Console.WriteLine($"{name.processname} has arrived.");
                    }
                    //item has finished running
                    if(name.bursttime == name.runtime)
                    {
                        name.activated = false;
                        Console.WriteLine($"{name.processname} has finished.");
                    }
                }

                for(int i = 0; i <= numprocesses; i++)
                {

                }


            }
        }
    }
}


