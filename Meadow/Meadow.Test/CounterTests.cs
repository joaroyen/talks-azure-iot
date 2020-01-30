using FluentAssertions;
using Xunit;

namespace Meadow.Test
{
    public class CounterTests
    {
        private readonly Counter _counter;

        public CounterTests()
        {
            _counter = new Counter();
        }

        [Fact]
        public void Is_initialized_to_zero()
        {
            _counter.Direction.Should().Be(CounterDirection.Unknown);
            _counter.Value.Should().Be(0);
            _counter.DeviceName.Should().Be("");
        }

        [Fact]
        public void Can_be_incremented()
        {
            _counter.Up("UnitTest");

            _counter.Direction.Should().Be(CounterDirection.Up);
            _counter.Value.Should().Be(1);
            _counter.DeviceName.Should().Be("UnitTest");
        }

        [Fact]
        public void Can_be_decremented()
        {
            _counter.Down("UnitTest");

            _counter.Direction.Should().Be(CounterDirection.Down);
            _counter.Value.Should().Be(-1);
            _counter.DeviceName.Should().Be("UnitTest");
        }
    }
}
