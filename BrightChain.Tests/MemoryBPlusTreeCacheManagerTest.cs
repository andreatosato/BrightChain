using BrightChain.Services;
using CSharpTest.Net.Interfaces;
using CSharpTest.Net.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BrightChain.Tests
{
    /// <summary>
    /// test object for the cache
    /// </summary>
    public class MemoryCacheTestObject : object, ITransactable, IComparable<MemoryCacheTestObject>
    {
        public string id;

        public MemoryCacheTestObject(string id) => this.id = id;

        public MemoryCacheTestObject() => this.id = MemoryBPlusTreeCacheManagerTest.GenerateTestKey();

        public void Commit() => throw new NotImplementedException();

        public void Rollback() => throw new NotImplementedException();

        public void Dispose() => this.id = null;

        public int CompareTo(MemoryCacheTestObject other) => this.id.CompareTo(other.id);
    }

    /// <summary>
    /// test object serializer/deserializer
    /// </summary>
    public class MemoryCacheTestObjectSerializer : PrimitiveSerializer, ISerializer<MemoryCacheTestObject>
    {
        public MemoryCacheTestObject ReadFrom(Stream stream)
        {
            List<byte> streamdata = new List<byte>();
            do
            {
                int b = stream.ReadByte();
                if (b == -1)
                {
                    break;
                }

                if (b == 0)
                {
                    break; // end of string
                }

                streamdata.Add(streamdata[0]);
            } while (true);

            return new MemoryCacheTestObject(
                id: System.Text.ASCIIEncoding.ASCII.GetString(streamdata.ToArray()));
        }

        public void WriteTo(MemoryCacheTestObject value, Stream stream) => stream.Write(System.Text.ASCIIEncoding.ASCII.GetBytes(value.id));
    }

    /// <summary>
    /// Tests the MemoryBPlusTreeCacheManager using the BPlusTreeCacheManagerTest
    /// </summary>
    [TestClass]
    public class MemoryBPlusTreeCacheManagerTest : BPlusTreeCacheManagerTest<MemoryBPlusTreeCacheManager<string, MemoryCacheTestObject, PrimitiveSerializer, MemoryCacheTestObjectSerializer>, string, MemoryCacheTestObject, PrimitiveSerializer, MemoryCacheTestObjectSerializer>
    {
        private static int TestKeyLength { get; } = 11;
        public static string GenerateTestKey()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var random = new Random(Guid.NewGuid().GetHashCode());
            var randomString = new string(Enumerable.Repeat(chars, TestKeyLength)
                                                    .Select(s => s[random.Next(s.Length)]).ToArray());
            return randomString;
        }

        internal override MemoryBPlusTreeCacheManager<string, MemoryCacheTestObject, PrimitiveSerializer, MemoryCacheTestObjectSerializer> NewCacheManager(ILogger logger) => new MemoryBPlusTreeCacheManager<string, MemoryCacheTestObject, PrimitiveSerializer, MemoryCacheTestObjectSerializer>(logger: logger);

        internal override KeyValuePair<string, MemoryCacheTestObject> NewKeyValue()
        {
            var testKey = GenerateTestKey();
            var testValue = new MemoryCacheTestObject(testKey);
            return new KeyValuePair<string, MemoryCacheTestObject>(testKey, testValue);
        }

        internal override MemoryCacheTestObject NewNullData() => null;

        [TestMethod]
        public void TestSetGetIntegrity()
        {
            // Arrange
            var expectation = this.testPair.Value;
            this.cacheManager.Set(this.testPair.Key, expectation);
            Assert.IsTrue(this.cacheManager.Contains(this.testPair.Key));

            // Act
            object result = this.cacheManager.Get(this.testPair.Key);

            // Assert
            Assert.IsNotNull(expectation);
            Assert.AreEqual(expectation, result);
            Assert.AreSame(expectation, result);
            Assert.AreEqual(this.testPair.Key, expectation.id);
        }
    }
}
