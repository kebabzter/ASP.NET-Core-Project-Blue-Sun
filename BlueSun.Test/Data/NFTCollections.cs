namespace BlueSun.Test.Data
{
    using BlueSun.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class NFTCollections
    {
        public static IEnumerable<NFTCollection> TenPublicNFTCollections()
           => Enumerable
               .Range(0, 10)
               .Select(i => new NFTCollection { Description = "", ImageUrl = "", Name = "" });
    }
}
