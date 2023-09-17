namespace TollFeeCalculator.Utilities.JsonSerializer
{
    public interface IJsonSerializerWrapper
    {
        T Deserialize<T>(Stream jsonStream);
    }

}
