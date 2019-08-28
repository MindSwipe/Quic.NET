using System.Collections.Generic;

namespace QuicNet.Infrastructure.Settings
{
    public class QuicVersion
    {
        public const int CurrentVersion = 16;

        public static readonly List<uint> SupportedVersions = new List<uint>() { 15, 16 };
    }
}
