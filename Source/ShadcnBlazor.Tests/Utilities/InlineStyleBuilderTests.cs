namespace ShadcnBlazor.Tests.Utilities;
public class InlineStyleBuilderTests : TestBase
{
    [Fact]
    public void InlineStyleBuilder_Build()
    {
        // Assert
        var styleBuilder = new InlineStyleBuilder();

        // Act
        styleBuilder.AddStyle("test1", "test", string.Empty, true);
        styleBuilder.AddStyle("test2", string.Empty, string.Empty, false);

        // Assert 
        Assert.Null(styleBuilder.Build());
    }
    [Fact]
    public void InlineStyleBuilder_Default()
    {
        // Assert
        var styleBuilder = new InlineStyleBuilder();

        // Act
        styleBuilder.AddStyle("div", "color", "red");
        styleBuilder.AddStyle("div", "background-color", "blue");
        styleBuilder.AddStyle("span", "color", "yellow");

        // Assert - Values are sorted
        Assert.Equal(@"<style> div { color: red; background-color: blue; } span { color: yellow; } </style>", styleBuilder.Build(newLineSeparator: false));
    }
    [Fact]
    public void InlineStyleBuilder_NoStyle()
    {
        // Assert
        var styleBuilder = new InlineStyleBuilder();

        // Act
        styleBuilder.AddStyle("div", "color", string.Empty);

        // Assert - Values are sorted
        Assert.Null(styleBuilder.Build(newLineSeparator: false));
        Assert.Equal(string.Empty, styleBuilder.BuildMarkupString().Value);
    }
    [Fact]
    public void InlineStyleBuilder_SingleStyle()
    {
        // Assert
        var styleBuilder = new InlineStyleBuilder();

        // Act
        styleBuilder.AddStyle("div", "color", "red");

        // Assert - Values are sorted
        Assert.Equal(@"<style> div { color: red; } </style>", styleBuilder.Build(newLineSeparator: false));
    }
    [Fact]
    public void InlineStyleBuilder_When()
    {
        // Assert
        var styleBuilder = new InlineStyleBuilder();

        // Act
        styleBuilder.AddStyle("div", "color", "red", true);
        styleBuilder.AddStyle("div", "color", "green", false);

        // Assert 
        Assert.Equal("<style>\r\ndiv { color: red; }\r\n</style>", styleBuilder.BuildMarkupString().Value);
        Assert.Equal(@"<style> div { color: red; } </style>", styleBuilder.Build(newLineSeparator: false));
    }
    [Fact]
    public void InlineStyleBuilder_WhenFunc()
    {
        // Assert
        var styleBuilder = new InlineStyleBuilder();

        // Act
        styleBuilder.AddStyle("div", "color", "red", () => true);

        // Assert - Values are sorted
        Assert.Equal(@"<style> div { color: red; } </style>", styleBuilder.Build(newLineSeparator: false));
    }
    [Fact]
    public void InlineStyleBuilder_WhenFuncFalse()
    {
        // Assert
        var styleBuilder = new InlineStyleBuilder();

        // Act
        styleBuilder.AddStyle("div", "color", "red", () => false);

        // Assert - Values are sorted
        Assert.Null(styleBuilder.Build(newLineSeparator: false));
    }
}
