using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace SemanticKernel.NativeFunction;

public class GeneralPlugin
{
    [KernelFunction]
    [Description("현재 시간을 구합니다.")]
    public TimeSpan GetTime()
    {
        return TimeProvider.System.GetLocalNow().TimeOfDay;
    }

    [KernelFunction]
    [Description("현재 날짜를 구합니다.")]
    public DateTime GetDate()
    {
        return TimeProvider.System.GetLocalNow().Date;
    }

    //[KernelFunction]
    //[Description("사용자 디스크의 전체 용량을 기가 단위로 반환한다")]
    //public int GetDiskSize(string DiskName)
    //{
    //    if(DiskName == "C")
    //    {
    //        return 100;
    //    }
    //    else if (DiskName == "D")
    //    {
    //        return 200;
    //    }
    //    else if (DiskName == "E")
    //    {
    //        return 300;
    //    }
    //    else
    //    {
    //        return 0;
    //    }
    //}
}