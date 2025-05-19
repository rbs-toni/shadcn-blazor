using System;
using System.Linq;
using System.Text;

namespace ShadcnBlazor.Tests.Common;
public class StringBuilderCacheTests
{
    [Fact]
    public void Acquire_ShouldReturnNewInstance_WhenCacheIsEmpty()
    {
        var builder = StringBuilderCache.Acquire();

        Assert.NotNull(builder);
        Assert.Equal(0, builder.Length);
    }

    [Fact]
    public void GetStringAndRelease_ShouldReturnString_AndStoreInstanceInCache()
    {
        var builder = StringBuilderCache.Acquire();
        builder.Append("test");

        var result = StringBuilderCache.GetStringAndRelease(builder);

        Assert.Equal("test", result);

        // Next Acquire should reuse the same instance
        var reused = StringBuilderCache.Acquire();
        Assert.Same(builder, reused);
        Assert.Equal(0, reused.Length);
    }

    [Fact]
    public void Acquire_ShouldNotReuseBuilder_UntilReleased()
    {
        var first = StringBuilderCache.Acquire();

        // Not released, should not be reused
        var second = StringBuilderCache.Acquire();

        Assert.NotSame(first, second);
    }

    [Fact]
    public void Cache_ShouldBeThreadLocal()
    {
        StringBuilder? mainThreadBuilder = null;
        StringBuilder? otherThreadBuilder = null;

        // Acquire and release in main thread
        var builder = StringBuilderCache.Acquire();
        builder.Append("main");
        StringBuilderCache.GetStringAndRelease(builder);
        mainThreadBuilder = builder;

        // Start new thread and acquire
        var thread = new Thread(() =>
        {
            var other = StringBuilderCache.Acquire();
            otherThreadBuilder = other;
        });

        thread.Start();
        thread.Join();

        Assert.NotNull(mainThreadBuilder);
        Assert.NotNull(otherThreadBuilder);
        Assert.NotSame(mainThreadBuilder, otherThreadBuilder);
    }
}
