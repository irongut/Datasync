// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json;

namespace CommunityToolkit.Datasync.Common.Test.Json;

[ExcludeFromCodeCoverage]
public class DateTimeConverter_Tests
{
    private readonly JsonSerializerOptions settings = new DatasyncServiceOptions().JsonSerializerOptions;

    [Fact]
    public void Converter_ReadsJson()
    {
        string json = """{"updatedAt":"2021-08-21T12:30:15.123+00:00"}""";
        DateTime value = DateTime.Parse("2021-08-21T12:30:15.123+00:00");

        Entity entity = JsonSerializer.Deserialize<Entity>(json, this.settings);
        entity.UpdatedAt.ToFileTime().Should().Be(value.ToFileTime());
    }

    [Fact]
    public void Converter_WritesJson()
    {
        string json = """{"updatedAt":"2021-08-21T12:30:15.123Z"}""";
        Entity entity = new() { UpdatedAt = DateTime.Parse("2021-08-21T12:30:15.1234567+00:00") };
        string actual = JsonSerializer.Serialize(entity, this.settings);
        Assert.Equal(json, actual);
    }

    [Fact]
    public void Converter_WritesJson_WithTimeZone()
    {
        string json = """{"updatedAt":"2021-08-21T12:30:15.123Z"}""";
        Entity entity = new() { UpdatedAt = DateTime.Parse("2021-08-21T20:30:15.1234567+08:00") };
        string actual = JsonSerializer.Serialize(entity, this.settings);
        Assert.Equal(json, actual);
    }

    [Fact]
    public void Converter_ThrowsOnBadDateInInput()
    {
        string json = """{"updatedAt":"foo"}""";
        Action act = () => _ = JsonSerializer.Deserialize<Entity>(json, this.settings);
        act.Should().Throw<FormatException>();
    }

    [Fact]
    public void Converter_ThrowsOnNullDateInInput()
    {
        string json = """{"updatedAt":null}""";
        Action act = () => _ = JsonSerializer.Deserialize<Entity>(json, this.settings);
        act.Should().Throw<FormatException>();
    }

    #region Models
    public class Entity
    {
        public DateTime UpdatedAt { get; set; }
    }
    #endregion
}