using System.Runtime.InteropServices;

namespace DAL.Binary.Model
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct Header
    {
        [MarshalAs(UnmanagedType.I4)] 
        internal int version;              // версия база данных
        internal unsafe fixed sbyte name[32];             // название/префикс для базы данных
        [MarshalAs(UnmanagedType.U8)] 
        internal ulong timestamp;          // время создания базы данных
        [MarshalAs(UnmanagedType.I4)] 
        internal int records;              // общее количество записей
        [MarshalAs(UnmanagedType.U4)] 
        internal uint offset_ranges;       // смещение относительно начала файла до начала списка записей с геоинформацией
        [MarshalAs(UnmanagedType.U4)] 
        internal uint offset_cities;       // смещение относительно начала файла до начала индекса с сортировкой по названию городов
        [MarshalAs(UnmanagedType.U4)] 
        internal uint offset_locations;    // смещение относительно начала файла до начала списка записей о местоположении
    }
}