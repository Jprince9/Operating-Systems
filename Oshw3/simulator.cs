using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
                    Console.WriteLine("TURTLE POWER WAS TOO STRONG FOR YOUR PC!");
                }
            }
        }



        //used to create the new process name
        public static string generatestring()
        {
            int length = 2;
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

        //list or array of my processes
        private static List<Process> processes = new List<Process>();


        public static void classcreation(int x, int y, int z)
        {
            processes.Add(new Process
            {
                processname = generatestring(),
                arrival = x,
                prio = y,
                bursttime = z
            });
        }


        private static Process temp = new Process();
        public static int numprocesses;
        public static int time = 0;
        public static int maxtime = 0;
        public static int quantum = 2;
        public static int count = 0;
        public static float turnaround;
        public static int intermediate = 0;
        public static int lines = 0;

        static void Main(string[] args)
        {
            //reads the input file
            using (StreamReader sr = new StreamReader("inputs.txt"))
            {
                string line;
                //while there is another line in the file
                while ((line = sr.ReadLine()) != null)
                {
                    lines += 1;
                    //replace all spaces with commas
                    string mynewstring = line.Replace(" ", ",");
                    while (mynewstring.Contains(" "))
                    {
                        mynewstring = line.Replace(" ", ",");
                    }

                    if (lines == 1) //first line marks how many processes we have
                    {
                        numprocesses = Int32.Parse(mynewstring);
                    }
                    else // every additional line will create an object of type processes
                    {
                        string[] ints = mynewstring.Split(",");
                        int x = Int32.Parse(ints[0]);
                        int y = Int32.Parse(ints[1]);
                        int z = Int32.Parse(ints[2]);
                        classcreation(x, y, z);
                    }
                }
            }

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
                        Console.WriteLine($"{name.processname} has arrived at time {time} with priority of {name.prio}");
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


            //calculate turnaround time here
            for (int i = 0; i < numprocesses; i++)
            {
                intermediate += processes[i].endtime - processes[i].arrival;
                Console.WriteLine($"{processes[i].processname} ran for a total time of {processes[i].endtime - processes[i].arrival}");
            }
            turnaround = intermediate / numprocesses;
            Console.WriteLine($"turn around time is {turnaround} cycles");

            //because we had fun

            //DONT FORGET TO PRESS PLAY AND ENJOY THE TURTLES
            Turtle();

        }
    }
}