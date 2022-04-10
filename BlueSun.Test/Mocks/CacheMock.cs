namespace BlueSun.Test.Mocks
{
    using Microsoft.Extensions.Caching.Memory;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class CacheMock
    {
        public static IMemoryCache Instance()
        {
            Mock<IMemoryCache> mockMemoryCache = new Mock<IMemoryCache>();
            mockMemoryCache.Setup(x => x.Get<string>(It.IsAny<string>())).Returns("");
            return mockMemoryCache.Object;
        }
    }
}
