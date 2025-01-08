using System.Text.Json;
using System.Text.Json.Serialization;

namespace NekoRay.Data;

[JsonSourceGenerationOptions(JsonSerializerDefaults.Web)]
[JsonSerializable(typeof(SpriteFile))]
[JsonSerializable(typeof(SpriteAnimationFile))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}