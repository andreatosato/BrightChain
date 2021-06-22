﻿using BrightChain.Models.Blocks;
using BrightChain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BrightChain.Tests
{
    [TestClass]
    public abstract class TransactableBlockCacheManagerTest : CacheManagerTest<BPlusTreeCacheManager<BlockHash, TransactableBlock>, BlockHash, TransactableBlock>
    {
    }
}