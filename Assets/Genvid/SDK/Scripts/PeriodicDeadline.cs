using System;
using UnityEngine;

namespace Genvid
{
    /// <summary>
    /// A class that maintains a periodic deadline
    /// against the frame time.
    /// </summary>
    public class PeriodicDeadline
    {
        /// <summary>
        /// Clamps a value between two other values.
        /// </summary>
        static private double ClampDouble(double value, double min, double max)
        {
            return Math.Min(Math.Max(value, min), max);
        }

        /// <summary>
        /// Time of the next trigger.
        /// </summary>
        private double m_Deadline = 0;

        /// <summary>
        /// Time between triggers.
        /// </summary>
        public double Period = 0.001;

        /// <summary>
        /// By default, use frame time instead of realtime for stability.
        /// On: The real time in seconds since the game started.
        /// Off: The timeScale-independent time for this frame.
        /// </summary>
        public bool UseRealtime = false;

        /// <summary>
        /// Calculates the framerate based on the period.
        /// </summary>
        public double Framerate
        {
            get
            {
                return 1.0 / Period;
            }
            set
            {
                Period = ClampDouble(1.0 / value, 0.001, 1000);
            }
        }

        /// <summary>
        /// Returns the current time since the start of the game.
        /// </summary>
        public double Now
        {
            get
            {
                /// Use frame time for a more stability.
                /// 
                if (UseRealtime)
                {
#if UNITY_2021_OR_NEWER
                return Time.readtimeSinceStartupAsDouble;
#else
                    return (double)Time.realtimeSinceStartup;
#endif
                }
                else
                {
#if UNITY_2021_OR_NEWER
                return Time.unscaledTimeAsDouble;
#else
                    return (double)Time.unscaledTime;
#endif
                }
            }
        }

        /// <summary>
        /// Checks if the trigger is due.
        /// </summary>
        public bool IsPassed()
        {
            return Now >= m_Deadline;
        }

        /// <summary>
        /// Get the next deadline, which is a multiple
        /// of the period. The next deadline is guaranteed
        /// to be within the next period.
        /// </summary>
        public void Next()
        {
            double now = Now;
            double period = Period;
            double deadline = m_Deadline;

            double offset = now % period;
            double lowerBound = now - offset;
            double upperBound = lowerBound + period;

            m_Deadline = ClampDouble(deadline + period, lowerBound, upperBound);
        }
    }
}