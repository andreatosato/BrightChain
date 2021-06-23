﻿namespace BrightChain.Services
{
    /// <summary>
    /// Cache system focused on grouping expiring blocks into cache keys by second and containing a list of expiring block hashes
    /// </summary>
    /// <typeparam name="Tkey"></typeparam>
    /// <typeparam name="Tvalue"></typeparam>
    public class BlockCacheExpirationCache<Tkey, Tvalue>
    {
        private BlockCacheManager expirationCache;
        private BPlusTreeCacheManager<Tkey, Tvalue> sourceCache;

        public BlockCacheExpirationCache(BPlusTreeCacheManager<Tkey, Tvalue> sourceCache)
        {
            /* TODO:
             * for each entry in the cache, create or add to the list of blocks expiring that second
             * whenever new blocks are stored, make sure to update this cache
             * this cache should be written to disk with DiskBlockCache
             */
        }
    }
}
