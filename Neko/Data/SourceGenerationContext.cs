using System.Text.Json;
using System.Text.Json.Serialization;

namespace Neko.Data;

[JsonSourceGenerationOptions(JsonSerializerDefaults.Web, IncludeFields = true)]
[JsonSerializable(typeof(SpriteFile))]
[JsonSerializable(typeof(SpriteAnimationFile))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}