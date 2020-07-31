using System;
namespace Domain.Entities
{
    public class JoelTest : Entity
    {
        public bool HasSourceControl { get; set; }

        public bool HasOneStepBuilds { get; set; }

        public bool HasDailyBuilds { get; set; }

        public bool HasBugDatabase { get; set; }

        public bool HasBusFixedBeforeProceding { get; set; }

        public bool HasUpToDateSchedule { get; set; }

        public bool HasSpec { get; set; }

        public bool HasQuiteEnvironment { get; set; }

        public bool HasBestTools { get; set; }

        public bool HasTesters { get; set; }

        public bool HasWrittenTest { get; set; }

        public bool HasHallwayTests { get; set; }
    }
}
