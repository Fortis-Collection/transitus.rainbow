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

			var items = folderDeserializer.Deserialize(@"C:\Projects\crtv-pokercentral.com\Unicorn\Default Configuration\Ignite");
			var templates = templateFactory.Create(items);

			foreach (var template in templates)
			{
				var templateOutput = new StringBuilder();

				templateOutput.AppendLine($"{template.Name.PadRight(20, ' ')} | {template.Path}");
				templateOutput.AppendLine($"Type: {(HasRenderingOptionsBase(template.BaseTemplates, template.Id) ? "Rendering Parameters Template" : "Data Template") }");

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

		public static bool HasRenderingOptionsBase(IEnumerable<ITemplate> templateItems, string templateId)
		{
			var renderingParameterTemplateId = BaseRenderingParametersTemplateId.ToLower();
			return templateId.ToLower() == renderingParameterTemplateId || templateItems.Any(t => t.Id.ToLower() == renderingParameterTemplateId);
		}
	}
}
