using ProtoBuf;
using System;
using System.Collections.Generic;

namespace ControllerServer
{
    [ProtoContract]
    public sealed class Report
    {
        [ProtoMember(1)]
        public List<string> URLs;
        [ProtoMember(2)]
        public int VirtualUsers;
        [ProtoMember(3)]
        public int Timeout;
        [ProtoMember(4)]
        public int RequestDuration;
        [ProtoMember(5)]
        public int Duration;
        [ProtoMember(6)]
        public TestingStrategy Strategy;
        [ProtoMember(7)]
        public List<Point> Points;
        [ProtoMember(8)]
        public DateTime Time;
        [ProtoMember(9)]
        public int Id;
    }

    [ProtoContract]
    public sealed class Reports
    {
        [ProtoMember(1)]
        public List<Report> List;
    }
}
