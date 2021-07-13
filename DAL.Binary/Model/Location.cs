using System.Runtime.InteropServices;

namespace DAL.Binary.Model
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct Location
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        internal fixed sbyte country[8];       // название страны (случайная строка с префиксом "cou_")
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        internal fixed sbyte region[12];       // название области (случайная строка с префиксом "reg_")
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        internal fixed sbyte postal[12];       // почтовый индекс (случайная строка с префиксом "pos_")
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
        internal fixed sbyte city[24];         // название города (случайная строка с префиксом "cit_")
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        internal fixed sbyte organization[32]; // название организации (случайная строка с префиксом "org_")
        [MarshalAs(UnmanagedType.R4)] 
        internal float latitude;       // широта
        [MarshalAs(UnmanagedType.R4)] 
        internal float longitude;      // долго
    }

    internal class LocationInfo
    {
        internal uint[] SortedIndex { get; }
        internal Location[] Locations { get; }

        internal LocationInfo(uint[] sortedIndex, Location[] locations)
        {
            SortedIndex = sortedIndex;
            Locations = locations;
        }
    }
}