﻿#region License
/*
Copyright (c) 2013 Daniil Rodin of Buhgalteria.Kontur team of SKB Kontur

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
#endregion

using System;
using NUnit.Framework;
using SharpRpc.Codecs;

namespace SharpRpc.Tests.Codecs
{
    [TestFixture]
    public class ReferenceArrayCodecTests : CodecTestsBase
    {
        #region Custom element types
        private struct StructWithReferences : IEquatable<StructWithReferences>
        {
            public double A;
            public string B;

            public bool Equals(StructWithReferences other) { return A == other.A && B == other.B; }
            public override bool Equals(object obj) { return obj is StructWithReferences && Equals((StructWithReferences)obj); }
            public override int GetHashCode() { return 0; }
        }
        #endregion

        private ICodecContainer codecContainer;

        [SetUp]
        public void Setup()
        {
            codecContainer = new CodecContainer();
        }

        private void DoTest<T>(T[] array)
        {
            DoTest(new ReferenceArrayCodec(typeof(T), codecContainer), array, (b, a) => Assert.That(b, Is.EqualTo(a)));
        }

        private void DoTestBasic<T>() where T : class
        {
            DoTest((T[])null);
            DoTest(new T[0]);
            DoTest(new T[] { null });
            DoTest(new T[] { null, null, null, null });
        }

        [Test]
        [Ignore("until StructWithReferencesCodec is created")]
        public void StructsWithReferences()
        {
            DoTest((StructWithReferences[])null);
            DoTest(new StructWithReferences[0]);
            DoTest(new[] { new StructWithReferences { A = 123.456, B = null } });
            DoTest(new[] { new StructWithReferences { A = 123.456, B = "ASdasd sdh qiwu diqwh d" } });
        }

        [Test]
        public void Strings()
        {
            DoTestBasic<string>();
            DoTest(new[] { "asdasd" });
            DoTest(new[] { "For", "the", "Horde", "!!!" });
            DoTest(new[] { "For", null, "the", null, "Horde", null, "!!!" });
        }

        [Test]
        public void NestedArrays()
        {
            DoTestBasic<string[]>();
            DoTest(new[] {new[] {"asdasd", null}, null, new string[0]});
        }

        [Test]
        public void Experimental()
        {
            var a1 = new[] { 123, 345 };
            var a2 = new[] { 123, 345 };
            Assert.That(a1, Is.EqualTo(a2));

            var a3 = new[] {new[] {123, 234}};
            var a4 = new[] {new[] {123, 234}};
            Assert.That(a3, Is.EqualTo(a4));
        }
    }
}
