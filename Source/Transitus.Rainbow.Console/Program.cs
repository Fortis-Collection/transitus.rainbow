using System.Text;

namespace Transitus.Rainbow.Console
{
	using System.Collections.Generic;
	using System.Linq;

	class Program
	{
		const string BaseRenderingParametersTemplateId = "{E13DB450-F493-42D5-B1C1-536A57AED2A6}";

		static void Main(string[] args)
		{
			System.Console.SetWindowSize(150, 40);

			var folderDeserializer = TransitusProvider.FolderDeserializer;
			var templateFactory = TransitusProvider.TemplateFactory;

			var myFolder = @"C:\Projects\crtv-pokercentral.com\Unicorn\Default Configuration";

			var folders = new List<string>
			{
				myFolder + @"..\Ignite",
				myFolder + @"..\User Defined",
				myFolder + @"..\Media",
				myFolder + @"..\MediaFramework"
			};

	var items = new List<IItem>();

	foreach (var folder in folders)
	{
		var deserializedItems = Transitus.Rainbow.TransitusProvider.FolderDeserializer.Deserialize(folder);

		items.AddRange(deserializedItems);
	}

			var templates = templateFactory.Create(items);

			foreach (var template in templates.Where(t => t.Name.Contains("Settings")))
			{
				var templateOutput = new StringBuilder();

				templateOutput.AppendLine($"{template.Name.PadRight(20, ' ')} | {template.Path}");
				templateOutput.AppendLine($"Type: {(HasRenderingOptionsBase(template.BaseTemplateIds, template.Id) ? "Rendering Parameters Template" : "Data Template") }");

				templateOutput.AppendLine("Base Templates:");
				foreach (var t in template.BaseTemplates)
				{
					templateOutput.AppendLine($"{string.Empty.PadRight(20, ' ')} | {t.Name.PadRight(30, ' ')}");
				}

				templateOutput.AppendLine("Base Template Ids:");
				foreach (var t in template.BaseTemplateIds)
				{
					templateOutput.AppendLine($"{string.Empty.PadRight(20, ' ')} | {t.PadRight(30, ' ')}");
				}


				templateOutput.AppendLine("Fields:");

				foreach (var field in template.LocalFields)
				{
					templateOutput.AppendLine($"{string.Empty.PadRight(20, ' ')} | {field.Name.PadRight(30, ' ')} | {field.TypeName}");
				}

				templateOutput.AppendLine(string.Empty.PadRight(140, '-'));

				System.Console.Write(templateOutput);
			}

			System.Console.ReadLine();
		}

		public static bool HasRenderingOptionsBase(IEnumerable<string> templateItemIds, string templateId)
		{
			var renderingParameterTemplateId = BaseRenderingParametersTemplateId.ToLower();
			return templateId.ToLower() == renderingParameterTemplateId || templateItemIds.Any(t => t.ToLower() == renderingParameterTemplateId);
		}
	}
}
