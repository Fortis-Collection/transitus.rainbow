namespace Transitus.Rainbow
{
	public interface IFileDeserializer
	{
		string ItemFileExtension { get; }
		IItem Deserialize(string filePath);
	}
}
