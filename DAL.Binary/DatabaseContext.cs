using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Common.Settings;
using DAL.Binary.Model;

namespace DAL.Binary
{
    internal class DatabaseContext
    {
        [DllImport("msvcrt.dll", SetLastError = false)]
        static extern IntPtr memcpy(IntPtr dest, IntPtr src, int count);
        
        private readonly DatabaseOptions _dbOptions;
        
        internal LocationInfo LocationsInfo { get; private set; }
        internal IpRange[] IpRanges { get; private set; }
        
        public DatabaseContext(DatabaseOptions dbOptions)
        {
            _dbOptions = dbOptions;
        }

        
        internal async Task Initialize()
        {
            var directory = Path.Combine(Directory.GetCurrentDirectory(), "database");
            var fullPath = Path.Combine(directory, _dbOptions.File);
            if (!File.Exists(fullPath))
                throw new FileNotFoundException("Database file not found", fullPath);

            var sw = new Stopwatch();
            sw.Start();
            await using (var fileStream = File.Open(fullPath, FileMode.Open))
            {
                var header = await ByteToTypeAsync<Header>(fileStream);
                fileStream.Seek(header.offset_ranges, SeekOrigin.Begin);
                IpRanges = await ByteToTypeArrayAsync<IpRange>(fileStream, header.records);

                fileStream.Seek(header.offset_locations, SeekOrigin.Begin);
                var locationsArray = await ByteToTypeArrayAsync<Location>(fileStream, header.records);

                fileStream.Seek(header.offset_cities, SeekOrigin.Begin);
                var cityIndexArray = await ByteToTypeArrayAsync<uint>(fileStream, header.records);
                LocationsInfo = new LocationInfo(cityIndexArray, locationsArray);
                
                sw.Stop();
                Console.WriteLine($"Database loading take:'{sw.Elapsed}'");

                var size = Unsafe.SizeOf<Location>();
                for (var i = 0; i < LocationsInfo.SortedIndex.Length; i++)
                {
                    // convert index from DB to local
                    LocationsInfo.SortedIndex[i] = (uint) (LocationsInfo.SortedIndex[i] / size);
                }
            }
        }

        private async Task<T> ByteToTypeAsync<T>(FileStream reader) where T : new()
        {
            var bytes = new byte[Marshal.SizeOf(typeof(T))];
            await reader.ReadAsync(bytes, 0, Marshal.SizeOf(typeof(T)));

            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            T theStructure = Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject());
            handle.Free();

            return theStructure;
        }

        private async Task<T[]> ByteToTypeArrayAsync<T>(FileStream reader, int count) where T : struct
        {
            var size = Unsafe.SizeOf<T>();
            var bytes = new byte[size * count];
            await reader.ReadAsync(bytes.AsMemory(0, size * count));

            //var result = MorphFast<byte, T>(bytes, count);
            var result = MorphSlow<byte, T>(bytes, count);
            return result;
        }

        private TTo[] MorphSlow<TFrom, TTo>(TFrom[] array, int count) where TTo : struct
        {
            if (array.Length == 0)
                return Array.Empty<TTo>();

            var result = new TTo[count];
            var sourceHandle = Marshal.UnsafeAddrOfPinnedArrayElement(array, 0);
            var destHandle = Marshal.UnsafeAddrOfPinnedArrayElement(result, 0);

            memcpy(destHandle, sourceHandle, Unsafe.SizeOf<TTo>() * count);
            return result;
        }

        // Dirty hack
        private TTo[] MorphFast<TFrom, TTo>(TFrom[] array, int count) where TTo : struct
        {
            if (array.Length == 0)
                return Array.Empty<TTo>();

            var dummy = new TTo[1];

            ref var r0 = ref Unsafe.As<TFrom, int>(ref array[0]);
            ref var r1 = ref Unsafe.As<TTo, int>(ref dummy[0]);

            // Overwrite method table and length
            Unsafe.Add(ref r0, -4) = Unsafe.Add(ref r1, -4);
            Unsafe.Add(ref r0, -2) = count;

            return (TTo[]) (object) array;
        }
    }
}