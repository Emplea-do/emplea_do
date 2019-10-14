using System;
using NUnit.Framework;

namespace UnitTests.Framework
{
    public abstract class BaseTest<T>
    {
        protected T _sut;

        [SetUp]
        public abstract void Setup();

    }
}
