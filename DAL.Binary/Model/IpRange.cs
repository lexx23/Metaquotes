using System.Runtime.InteropServices;

namespace DAL.Binary.Model
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct IpRange
    {
        [MarshalAs(UnmanagedType.U4)] 
        internal uint ip_from;        // начало диапазона IP адресов
        [MarshalAs(UnmanagedType.U4)] 
        internal uint ip_to;          // конец диапазона IP адресов
        [MarshalAs(UnmanagedType.U4)] 
        internal uint location_index; // индекс записи о местоположении
    }
}