using System;

namespace SingleCS.Models
{
    [Flags]
    public enum CombineOptions
    {
        None = 0,
        Refactor = 1,
        NameSpaceMerge = 2,
        PartialMerge = 4,
    }
}
