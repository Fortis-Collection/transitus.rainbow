namespace Transitus.Rainbow.Sync
{
	using System.IO;
	using System.Xml;

	using global::Rainbow.Filtering;
	using global::Rainbow.Formatting;
	using global::Rainbow.Storage.Yaml;

	using Sitecore.IO;

	public class RainbowItemDeserializer : IFileDeserializer
	{
		protected readonly ISerializationFormatter SerializationFormatter;

		public RainbowItemDeserializer()
		{
			// TODO: See if we can move this to a config file, rather than hard coding it here.
			const string xmlNodeString = @"
				<serializationFormatter type=""Rainbow.Storage.Yaml.YamlSerializationFormatter, Rainbow.Storage.Yaml"" singleInstance=""true"">
					<fieldFormatter type=""Rainbow.Formatting.FieldFormatters.MultilistFormatter, Rainbow"" />
                    <fieldFormatter type=""Rainbow.Formatting.FieldFormatters.XmlFieldFormatter, Rainbow"" />
                  </serializationFormatter >
			";
			var doc = new XmlDocument();
			doc.LoadXml(xmlNodeString);

			// TODO: See if we can move this to a config file, rather than hard coding it here.
			const string fieldFilterXmlString = @"
				<fieldFilter type=""Rainbow.Filtering.ConfigurationFieldFilter, Rainbow"" singleInstance=""true"">
					<exclude fieldID=""{B1E16562-F3F9-4DDD-84CA-6E099950ECC0}"" note=""'Last run' field on Schedule template (used to register tasks)"" />
					<exclude fieldID=""{52807595-0F8F-4B20-8D2A-CB71D28C6103}"" note=""'__Owner' field on Standard Template"" />
					<exclude fieldID=""{F6D8A61C-2F84-4401-BD24-52D2068172BC}"" note=""'__Originator' field on Standard Template"" />
					<exclude fieldID=""{8CDC337E-A112-42FB-BBB4-4143751E123F}"" note=""'__Revision' field on Standard Template"" />
					<exclude fieldID=""{D9CF14B1-FA16-4BA6-9288-E8A174D4D522}"" note=""'__Updated' field on Standard Template"" />
					<exclude fieldID=""{BADD9CF9-53E0-4D0C-BCC0-2D784C282F6A}"" note=""'__Updated by' field on Standard Template"" />
					<exclude fieldID=""{001DD393-96C5-490B-924A-B0F25CD9EFD8}"" note=""'__Lock' field on Standard Template"" />
				</fieldFilter>
			";
			var fieldFilterXml = new XmlDocument();
			fieldFilterXml.LoadXml(fieldFilterXmlString);

			this.SerializationFormatter = new YamlSerializationFormatter(doc, new ConfigurationFieldFilter(fieldFilterXml));
		}

		public IItem Deserialize(string filePath)
		{
			var syncItem = this.ReadItem(filePath);

			return syncItem;
		}

		public string ItemFileExtension => ".yml";

        public IItem ReadItem(string filePath)
        {
			var file = new FileInfo(filePath);
	
			lock (FileUtil.GetFileLock(file.FullName))
			{
				using (var reader = file.OpenRead())
				{
					var readItem = this.SerializationFormatter.ReadSerializedItem(reader, filePath);
					readItem.DatabaseName = "master";
					return new RainbowItem(readItem);					
				}
			}
        }
	}
}
