namespace Transitus.Rainbow
{
	using System.Collections.Generic;

	public interface IFolderDeserializer
	{
		IEnumerable<IItem> Deserialize(string folderPath, bool recursive = true);
	}
}
