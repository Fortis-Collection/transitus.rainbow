namespace Transitus.Rainbow
{
	using System.Collections.Generic;
	using System.IO;

	public class FolderDeserializer : IFolderDeserializer
	{
		protected readonly IFileDeserializer Deserializer;

		public FolderDeserializer(IFileDeserializer deserializer)
		{
			this.Deserializer = deserializer;
		}

		public IEnumerable<IItem> Deserialize(string folderPath, bool recursive = true)
		{
			var items = new List<IItem>();

			this.ReadItems(folderPath, recursive, items);

			return items;
		}

		public void ReadItems(string folderPath, bool recursive, IList<IItem> items)
		{
			var files = Directory.GetFiles(folderPath);

			foreach (var file in files)
			{
				if (file.EndsWith(this.Deserializer.ItemFileExtension))
				{
					var item = this.Deserializer.Deserialize(file);

					if (item != null)
					{
						items.Add(item);

						if (recursive)
						{
							var folderName = file.Substring(0, file.Length - this.Deserializer.ItemFileExtension.Length);

							if (this.IsValidDirectory(folderName))
							{
								this.ReadItems(folderName, true, items);
							}
						}
					}
				}
			}
		}

		public bool IsValidDirectory(string folderPath)
		{
			return Directory.Exists(folderPath) && !this.IsDirectoryHidden(folderPath);
		}

		public bool IsDirectoryHidden(string folderPath)
		{
			return this.IsHidden(new DirectoryInfo(folderPath));
		}

		public bool IsHidden(FileSystemInfo info)
		{
			return (info.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
		}
	}
}
