using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Oshw3
{
    class simulator
    {
        public static void Turtle()
        {
            string url = "https://youtu.be/jQK6iUj9Uts?t=20";
            try
            {
                System.Diagnostics.Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    System.Diagnostics.Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    System.Diagnostics.Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    System.Diagnostics.Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
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

        public class Process
        {
            public int arrival { get; set; }
            public int prio { get; set; }
            public int bursttime { get; set; }
            public string processname { get; set; }
            public int runtime { get; set; } = 0;
            public bool activated { get; set; } = false;
            public int endtime { get; set; }
            public int tempcount { get; set; } = 0;

        }
        //loop to read input file and assgn each item to the list
        static List<Process> processes = new List<Process>() {
            new Process
            {
               //processname = generatestring(),
                processname = "a",
                arrival = 0,
                prio = 1,
                bursttime = 5
            },
            new Process
            {
               //processname = generatestring(),
                processname = "b",
                arrival = 2,
                prio = 2,
                bursttime = 5
            },

            new Process
            {
                processname = "c",
               //processname = generatestring(),
                arrival = 3,
                prio = 1,
                bursttime = 3
            },

            new Process
            {
            processname = "d",
            //processname = generatestring(),
            arrival = 3,
            prio = 6,
            bursttime = 3
            }

        };



        private static Process temp = new Process();
        public static int numprocesses = 4;
        public static int time = 0;
        public static int maxtime = 0;
        public static int quantum = 2;
        public static int count = 0;
        public static float turnaround;
        public static int intermediate = 0;

        static void Main(string[] args)
        {
            //sorts the list based on priority
            processes.Sort((x, y) => x.prio.CompareTo(y.prio));



            //finds the total amount of processing time needed
            foreach (Process name in processes)
            {
                maxtime = maxtime + name.bursttime;
            }

            //while there is still time remaining, continue running
            while(time != maxtime)
            {
                foreach (Process name in processes)
                {
                    //process has arrived
                    if (time == name.arrival)
                    {
                        name.activated = true;
                        Console.WriteLine($"{name.processname} has arrived at time {time}");
                    }
                }
                //loop through list of processes in the array
                for (int i = 0; i < numprocesses; i++)
                {
                    //if process at location i is ready to run
                    if (processes[i].activated == true)
                    {
                        while (processes[i].runtime != processes[i].bursttime)
                        {
                            processes[i].runtime++; 
                            processes[i].tempcount += 1;
                            Console.WriteLine($"Process {processes[i].processname} has run at time slot {time}");
                            //Console.WriteLine($"Process {processes[i].processname} has been running for {processes[i].tempcount}");

                            for (int c = i + 1; c < numprocesses; c++)
                            {
                                int temp = c;
                                while (processes[i].prio == processes[temp].prio)
                                {
                                    processes[c].tempcount = 0;
                                    temp++;
                                }
                            }

                            break;
                        }
                        //process has finished
                        if (processes[i].runtime == processes[i].bursttime && processes[i].activated == true)
                        {
                            Console.WriteLine($"{processes[i].processname} has left the building at time {time+1}");
                            processes[i].activated = false;
                            processes[i].endtime = time + 1; //sets the time process completed to calculate turn around
                        }

                        if (i + 1 < numprocesses) // there is nothing next in line
                        {
                            if (processes[i].tempcount == quantum && processes[i].prio == processes[i + 1].prio) //time for round robin change order of queue
                            {
                                temp = processes[i];
                                int c = i;
                                while (processes[c].prio == processes[c + 1].prio)
                                {
                                    processes[c] = processes[c + 1];
                                    //temp.tempcount = 0;
                                    c++;
                                }
                                processes[c] = temp;
                                temp = null;
                            }
                        }

                        break;
                    }
                }
                time++;
            }

            for (int i = 0; i < numprocesses; i++)
            {
                intermediate += processes[i].endtime - processes[i].arrival;
                Console.WriteLine($"{processes[i].processname} ran for a total time of {processes[i].endtime - processes[i].arrival}");
            }

            turnaround = intermediate / numprocesses;
            Console.WriteLine($"turn around time is {turnaround} cycles");

           // Turtle();

        }
    }
}