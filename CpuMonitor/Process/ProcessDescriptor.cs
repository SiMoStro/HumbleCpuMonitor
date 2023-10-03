namespace HumbleCpuMonitor.Process
{
    internal class ProcessDescriptor
    {
        public uint Pid { get; set; }

        public string Name { get; set; }

        public ProcessSnapshot Snapshot { get; set; }

        public ProcessDescriptor()
        {
            Snapshot = new ProcessSnapshot();
        }
    }
}
