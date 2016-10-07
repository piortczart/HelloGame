using System;

namespace HelloGame.Common.Model
{
    public class ThingToRespawn
    {
        public ThingBase Thing { get; }
        public TimeSpan WhenToRespawn { get; }

        public ThingToRespawn(TimeSpan whenToRespawn, ThingBase thing)
        {
            WhenToRespawn = whenToRespawn;
            Thing = thing;
        }
    }
}