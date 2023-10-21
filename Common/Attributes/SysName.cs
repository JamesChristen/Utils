namespace Common.Attributes
{
    public sealed class SysName : Attribute
    {
        public string Name { get; set; }

        public SysName(string sysName)
        {
            Name = sysName;
        }
    }
}
