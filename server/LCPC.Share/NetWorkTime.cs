using System.ComponentModel;

namespace LCPC.Share;

public class NetWorkTime:  IComparable<NetWorkTime>
{
    public override string ToString()
    {
        return base.ToString();
    }
    
    
    public int CompareTo(NetWorkTime other)
    {
        return 1;
    }
}