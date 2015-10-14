namespace Transitus.Rainbow
{
	using System.Collections.Generic;

	public class Template : ITemplate
	{
		public string Path { get; set; }
		public string Name { get; set; }
		public string Id { get; set; }
		public string ParentId { get; set; }
		public IEnumerable<string> BaseTemplateIds { get; set; }
		public IEnumerable<ITemplate> BaseTemplates { get; set; }
		public IEnumerable<ITemplateField> CombinedFields { get; set; }
		public IEnumerable<ITemplateField> LocalFields { get; set; }
	}
}
