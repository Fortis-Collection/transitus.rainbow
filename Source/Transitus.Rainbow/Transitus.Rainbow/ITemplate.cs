namespace Transitus.Rainbow
{
	using System.Collections.Generic;

	public interface ITemplate
	{
		string Path { get; }
		string Name { get; }
		string Id { get; }
		string ParentId { get; }
		IEnumerable<string> BaseTemplateIds { get; }
		IEnumerable<ITemplate> BaseTemplates { get; }
		IEnumerable<ITemplateField> CombinedFields { get; }
		IEnumerable<ITemplateField> LocalFields { get; }
	}
}
