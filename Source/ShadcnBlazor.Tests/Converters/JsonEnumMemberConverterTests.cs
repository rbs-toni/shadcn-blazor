using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json;

namespace ShadcnBlazor.Tests.Converters;
public class JsonEnumMemberConverterTests
{
    [DataContract]
    public enum TestEnum
    {
        [EnumMember(Value = "value_one")]
        ValueOne,

        [EnumMember(Value = "value_two")]
        ValueTwo,

        [EnumMember]
        ValueThree, // Should default to name

        ValueFour // No EnumMember attribute
    }

    [Fact]
    public void Constructor_InitializesDictionaries()
    {
        // Arrange & Act
        var converter = new JsonEnumMemberConverter<TestEnum>();

        // Assert
        // Just verify the static constructor ran successfully
        Assert.NotNull(converter);
    }
    [Theory]
    [InlineData("value_one", TestEnum.ValueOne)]
    [InlineData("value_two", TestEnum.ValueTwo)]
    [InlineData("ValueThree", TestEnum.ValueThree)] // Defaults to name when no value specified
    [InlineData("ValueFour", TestEnum.ValueFour)] // No EnumMember attribute, uses name
    public void Read_ConvertsStringToEnum(string input, TestEnum expected)
    {
        // Arrange
        var converter = new JsonEnumMemberConverter<TestEnum>();
        var reader = new Utf8JsonReader(JsonSerializer.SerializeToUtf8Bytes(input));

        // Act
        reader.Read(); // Move to the value
        var result = converter.Read(ref reader, typeof(TestEnum), new JsonSerializerOptions());

        // Assert
        Assert.Equal(expected, result);
    }
    [Theory]
    [InlineData(TestEnum.ValueOne, "value_one")]
    [InlineData(TestEnum.ValueTwo, "value_two")]
    [InlineData(TestEnum.ValueThree, "ValueThree")] // Defaults to name when no value specified
    [InlineData(TestEnum.ValueFour, "ValueFour")] // No EnumMember attribute, uses name
    public void Write_ConvertsEnumToString(TestEnum input, string expected)
    {
        // Arrange
        var converter = new JsonEnumMemberConverter<TestEnum>();
        using var memoryStream = new MemoryStream();
        var writer = new Utf8JsonWriter(memoryStream);

        // Act
        converter.Write(writer, input, new JsonSerializerOptions());
        writer.Flush();
        memoryStream.Position = 0;
        var result = JsonSerializer.Deserialize<string>(memoryStream.ToArray());

        // Assert
        Assert.Equal(expected, result);
    }
    [Fact]
    public void Write_ThrowsJsonException_WhenValueNotFound()
    {
        // Arrange
        var converter = new JsonEnumMemberConverter<TestEnum>();
        var invalidValue = (TestEnum)999; // Value not in enum
        using var memoryStream = new MemoryStream();
        var writer = new Utf8JsonWriter(memoryStream);

        // Act & Assert
        var ex = Assert.Throws<JsonException>(() =>
            converter.Write(writer, invalidValue, new JsonSerializerOptions()));

        Assert.Contains(invalidValue.ToString(), ex.Message);
    }
}
