using UnityEngine;
using System.Collections;
/*
 * This Code was derived from a redit post at https://www.reddit.com/r/Unity3D/comments/2r7jfo/in_game_24hr_clock_script/
 */
public class ClockScript : MonoBehaviour
{

    public class Clock
    {
        private System.DateTime now;
        private System.TimeSpan timeNow;
        private System.TimeSpan gameTime;
        private int minutesPerDay; //Realtime minutes per game-day (1440 would be realtime) 24 is 60 ingame sec for one real sec

        public Clock(int minPerDay)
        {
            minutesPerDay = minPerDay;
        }

        public System.TimeSpan GetTime()
        {
            now = System.DateTime.Now;
            timeNow = now.TimeOfDay;
            double hours = timeNow.TotalMinutes % minutesPerDay;
            double minutes = (hours % 1) * 60;
            double seconds = (minutes % 1) * 60;
            gameTime = new System.TimeSpan((int)hours, (int)minutes, (int)seconds);

            return gameTime;
        }
    }

    public Clock clock;
    
    void Start()
    {
        clock = new Clock(48);
    }

    void FixedUpdate()
    {
        //Debug.Log(clock.GetTime().ToString());
    }
}