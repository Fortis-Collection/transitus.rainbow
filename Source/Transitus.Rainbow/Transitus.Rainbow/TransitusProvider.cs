namespace Transitus.Rainbow
{
	using Transitus.Rainbow.Sync;

	public static class TransitusProvider
	{
		public static IFileDeserializer FileDeserializer = new RainbowItemDeserializer();
		public static IFolderDeserializer FolderDeserializer = new FolderDeserializer(FileDeserializer);
		public static ITemplateFactory TemplateFactory = new TemplateFactory();
	}
}
