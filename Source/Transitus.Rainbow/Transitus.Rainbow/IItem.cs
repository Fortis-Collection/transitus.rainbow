namespace Transitus.Rainbow
{
	using System.Collections.Generic;

	public interface IItem
	{
		string BranchId { get; }
		string DatabaseName { get; }
		string Id { get; }
		string ItemPath { get; }
		string MasterId { get; }
		string Name { get; }
		string ParentId { get; }
		string TemplateId { get; }
		string TemplateName { get; }
		IList<IField> SharedFields { get; }
	}
}
