namespace Transitus.Rainbow
{
	using System.Collections.Generic;

	public interface ITemplateFactory
	{
		IEnumerable<ITemplate> Create(IEnumerable<IItem> items);
	}
}
