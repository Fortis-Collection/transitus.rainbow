namespace Transitus.Rainbow.Sync
{
	using System.Collections.Generic;
	using System.Linq;

	using global::Rainbow.Model;

	public class RainbowItem : IItem
	{
		public RainbowItem(IItemData itemData)
		{
			this.Name = itemData.Name;
			this.DatabaseName = itemData.DatabaseName;
			this.Id = itemData.Id.ToString("B");
			this.BranchId = itemData.BranchId.ToString("B");
			this.ItemPath = itemData.Path;
			this.ParentId = itemData.ParentId.ToString("B");
			this.TemplateId = itemData.TemplateId.ToString("B");
			this.SharedFields = itemData.SharedFields.Select(x => new RainbowField(x)).Cast<IField>().ToList().ToList();
		}

		public string BranchId { get; }
		public string DatabaseName { get; }
		public string Id { get; }
		public string ItemPath { get; }
		public string MasterId { get; }
		public string Name { get; }
		public string ParentId { get; }
		public string TemplateId { get; }
		public string TemplateName { get; }
		public IList<IField> SharedFields { get; }
	}
}
