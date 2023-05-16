
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

class HelloWorld
{


    class GFG : IComparer<KeyValuePair<int, bool>>
    {
        public int Compare(KeyValuePair<int, bool> x, KeyValuePair<int, bool> y)
        {
            return x.Key.CompareTo(y.Key);
        }
    }

    public static List<KeyValuePair<int, bool>> segmentUnion(List<KeyValuePair<int, int>> seg)
    {
        int n = seg.Count;
        List<KeyValuePair<int, bool>> points = new List<KeyValuePair<int, bool>>();
        for (int i = 0; i < 2 * n; i++)
        {
            points.Add(new KeyValuePair<int, bool>(0, true));
        }


        for (int i = 0; i < n; i++)
        {
            points[i * 2] = new KeyValuePair<int, bool>(seg[i].Key, false);
            points[i * 2 + 1] = new KeyValuePair<int, bool>(seg[i].Value, true);
        }

        GFG gg = new GFG();
        points.Sort(gg);

        List<KeyValuePair<int, bool>> rPoints = new List<KeyValuePair<int, bool>>();
        bool firstItem = true;

        // Traverse through all points
        for (int i = 0; i < n * 2; i++)
        {
            //Start packing Solution   
            if (!points[i].Value && firstItem) {
                rPoints.Add(new KeyValuePair<int, bool>(points[i].Key, points[i].Value));
                firstItem = false;
            }
            //End Point
            if (i + 1 == n * 2)
            {
                rPoints.Add(new KeyValuePair<int, bool>(points[i].Key, points[i].Value));
            }
            else {
                //Gaps:  Avoid evaluating end
                if (points[i].Value && !firstItem && !points[i + 1].Value)
                {
                    //Check if not overlap
                    if (points[i].Key != points[i + 1].Key)
                    {
                        rPoints.Add(new KeyValuePair<int, bool>(points[i].Key, points[i].Value));
                        rPoints.Add(new KeyValuePair<int, bool>(points[i + 1].Key, points[i + 1].Value));

                    }
                }
            }

        }
        return rPoints;
    }

    public static List<KeyValuePair<int, bool>> segmentUnionInvert(List<KeyValuePair<int, bool>> seg, int lowerLimit, int upperLimit)
    {
        List<KeyValuePair<int, bool>> seginvert = new List<KeyValuePair<int, bool>>();
        //Check Start
        bool addUpperLimit = false;
        if (seg[0].Key != 0) { seginvert.Add(new KeyValuePair<int, bool>(0, false)); }
        for (int i = 0; i < seg.Count; i++)
        {
            seginvert.Add(new KeyValuePair<int, bool>(seg[i].Key, !seg[i].Value));

            //Check End
            if ((i == (seg.Count) - 1) && seg[i].Key != upperLimit)
            {
                addUpperLimit = true;
            }
        }
        if (addUpperLimit)
        {
            seginvert.Add(new KeyValuePair<int, bool>(upperLimit, true));
        }
        return seginvert;
    }

    public static List<int[]> kleenToEvent(List<KeyValuePair<int, bool>> kleenPoints) {

        List<int[]> scheduleList = new List<int[]>();
        for (int i = 0; i < kleenPoints.Count; i = i + 2)
        {
            scheduleList.Add(new int[2] { kleenPoints[i].Key, kleenPoints[i + 1].Key });
        }
        return scheduleList;
    }

    public static List<KeyValuePair<int, int>> eventsToSgments(List<List<string>> inputEvents){
        List<KeyValuePair<int, int>> segments = new List<KeyValuePair<int, int>>();
        foreach(List<string> inputEvent in inputEvents) { 
        segments.Add(new KeyValuePair<int, int>(
            hourStringToInt(inputEvent[0]) ,
            hourStringToInt(inputEvent[1])
            ));
        }
        return segments;
    }

    public static List<List<string>> segmentsToEvents(List<int[]> inputSegments)
    {
        List<List<string>> rEvents = new List<List<string>>();
        foreach (int[] inputSegment in inputSegments)
        {
            rEvents.Add(new List<string>() { hourIntToString(inputSegment[0]), hourIntToString(inputSegment[1]) });
              
        }
        return rEvents;
    }

    public static int hourStringToInt(string hourString) {
        int actualMinute = Int32.Parse(hourString.Substring(hourString.Length - 2));
        int actualHoursToMinutes = 0;
        if (hourString.Length > 3)
        {
            actualHoursToMinutes = Int32.Parse(hourString.Substring(0,2)) * 60;
        }
        else {
            actualHoursToMinutes = Int32.Parse(hourString.Substring(0,1)) * 60;
        }
        return actualMinute + actualHoursToMinutes;
    }

    public static string hourIntToString(int intMinutes)
    {
        string minutes = (intMinutes % 60).ToString();
        string hours = ((intMinutes - (intMinutes % 60))/60).ToString();
        if (minutes.Length == 1) { minutes = "0" + minutes; }
        return hours + minutes;
    }

    static void Main()
    {

        //Prepare Events

        List<List<string>> events = new List<List<string>>();
        events.Add(new List<string>() { "600", "700", "Event" });
        events.Add(new List<string>() { "700", "800", "Event" });
        events.Add(new List<string>() { "900", "1030", "Event" });
        events.Add(new List<string>() { "1300", "1345", "Event" });
        events.Add(new List<string>() { "1400", "1800", "Event" });
        events.Add(new List<string>() { "1800", "1900", "Event" });

      
        List<KeyValuePair<int, bool>> schedule = segmentUnion(eventsToSgments(events));
        List<KeyValuePair<int, bool>> freeSchedule = segmentUnionInvert(schedule,0,1440);
        List<int[]> freeScheduleSegments= kleenToEvent(freeSchedule);
        List<List<string>> freeScheduleEvent = segmentsToEvents(freeScheduleSegments);

        foreach(List<string> fse in freeScheduleEvent){
            Console.WriteLine("[" + fse[0] + "," + fse[1] + "]");
        }
        Console.ReadLine();
    }
}
