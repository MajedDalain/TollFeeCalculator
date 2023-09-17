namespace TollFeeCalculator.Utilities.JsonSerializer
{
    public class JsonSerializerWrapper : IJsonSerializerWrapper
    {
        public T Deserialize<T>(Stream jsonStream)
        {
            var result = System.Text.Json.JsonSerializer.Deserialize<T>(jsonStream);
            if (result == null)
            {
                throw new InvalidOperationException("Deserialization returned null");
            }
            return result;
        }
    }

}
