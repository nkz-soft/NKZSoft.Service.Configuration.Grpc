namespace NKZSoft.Service.Configuration.Grpc.Converters;

internal sealed class ExceptionConverter : JsonConverter<Exception>
{
    public override bool CanConvert(Type typeToConvert) => typeof(Exception).IsAssignableFrom(typeToConvert);

    public override Exception? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        var exeExceptionInfo = new ExceptionInfo();
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return new JsonException(exeExceptionInfo.Message);
            }

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propName = (reader.GetString() ?? string.Empty).ToLower(CultureInfo.InvariantCulture);
                reader.Read();

                switch (propName)
                {
                    case var _ when propName.Equals(nameof(ExceptionInfo.Message).ToLower(CultureInfo.InvariantCulture), StringComparison.Ordinal):
                        exeExceptionInfo.Message = reader.GetString();
                        break;
                }
            }
        }
        throw new JsonException();
    }


    public override void Write(Utf8JsonWriter writer, Exception value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteString(nameof(value.Message), value.Message);

        writer.WriteEndObject();
    }
}
