using Parquet.Serialization;
namespace CommonCrawl.Readers;

public interface IParquetReader
{
    IAsyncEnumerable<TModel> ReadAsAsyncEnumerable<TModel>(string filePath, Func<TModel, bool>? predicate = null)
        where TModel : class, new();
    IAsyncEnumerable<TModel> ReadAsAsyncEnumerable<TModel>(Stream stream, Func<TModel, bool>? predicate = null)
        where TModel : class, new();
}
public class ParquetReader : IParquetReader
{
    public static ParquetReader Instance { get; } = new();

    private ParquetReader() { }
    public async IAsyncEnumerable<TModel> ReadAsAsyncEnumerable<TModel>(string filePath, Func<TModel, bool>? predicate=null)
    where TModel : class, new()
    {
        predicate ??= _ => true;
        await using var stream = File.OpenRead(filePath);
        await foreach (var item in ParquetSerializer.DeserializeAllByGroupsAsync<TModel>(stream))
        {
            foreach (var t in item.Where(predicate))
                yield return t;
        }
    }

    public async IAsyncEnumerable<TModel> ReadAsAsyncEnumerable<TModel>(Stream stream, Func<TModel, bool>? predicate = null)
        where TModel : class, new()
    {
        predicate ??= _ => true;
        if(stream.CanSeek)stream.Seek(0, SeekOrigin.Begin);
        await foreach (var item in ParquetSerializer.DeserializeAllByGroupsAsync<TModel>(stream))
        {
            foreach (var t in item.Where(predicate))
                yield return t;
        }
    }
}
