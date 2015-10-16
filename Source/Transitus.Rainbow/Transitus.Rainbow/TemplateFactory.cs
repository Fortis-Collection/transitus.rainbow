namespace Transitus.Rainbow
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Sitecore;
	using Sitecore.Data;

	public class TemplateFactory : ITemplateFactory
	{
		protected string[] StringSeparators = { "\r\n" };

		public IEnumerable<ITemplate> Create(IEnumerable<IItem> items)
		{
			var templateItems = this.GetTemplates(items);
			var templates = new List<Template>();

			foreach (var templateItem in templateItems)
			{
				var combinedTemplateItems = this.GetCombinedBaseTemplates(templateItem, items);
				var combinedSections = this.GetCombinedSections(combinedTemplateItems, items);
				var combinedFields = this.GetFields(combinedSections, items);
				var localSections = this.GetLocalSections(templateItem, items);
				var localFields = this.GetFields(localSections, items);
				var baseTemplateIds = this.GetBaseTemplateIds(templateItem, combinedTemplateItems);
				var template = new Template
				{
					Id = templateItem.Id,
					Name = templateItem.Name,
					ParentId = templateItem.ParentId,
					Path = templateItem.ItemPath,
					CombinedFields = combinedFields,
					LocalFields = localFields,
					BaseTemplateIds = baseTemplateIds
				};

				templates.Add(template);
			}

			foreach (var template in templates)
			{
				template.BaseTemplates = templates.Where(t => !IsIdEqual(t.Id, template.Id) && template.BaseTemplateIds.Contains(t.Id));
			}

			return templates;
		}

		public IEnumerable<IItem> GetCombinedBaseTemplates(IItem item, IEnumerable<IItem> items)
		{
			var baseTemplates = new List<IItem>();

			this.GetCombinedBaseTemplates(item, baseTemplates, items);

			return baseTemplates;
		}

		public void GetCombinedBaseTemplates(IItem item, IList<IItem> baseTemplates, IEnumerable<IItem> items)
		{
			if (item != null && baseTemplates.All(i => i.Id != item.Id))
			{
				baseTemplates.Add(item);

				var baseTemplateField = item.SharedFields.FirstOrDefault(i => IsBaseTemplateField(i.Id));

				if (baseTemplateField != null)
				{
					foreach (var value in baseTemplateField.Value.Split(StringSeparators, StringSplitOptions.None))
					{
						var baseTemplateItem = items.FirstOrDefault(i => IsIdEqual(i.Id, value));

						this.GetCombinedBaseTemplates(baseTemplateItem, baseTemplates, items);
					}
				}
			}
		}

		public IEnumerable<string> GetBaseTemplateIds(IItem item, IEnumerable<IItem> items)
		{
			var baseTemplateIds = new List<string>();

			this.GetBaseTemplatesIds(item, item.Id, baseTemplateIds, items);

			return baseTemplateIds;
		}

		public IEnumerable<string> GetBaseTemplatesIds(IItem item, string itemId, IList<string> baseTemplateIds, IEnumerable<IItem> items)
		{
			if (string.IsNullOrWhiteSpace(itemId) == false && baseTemplateIds.All(i => IsIdEqual(i, itemId) == false))
			{
				baseTemplateIds.Add(itemId);

				var baseTemplateField = item?.SharedFields.FirstOrDefault(i => this.IsBaseTemplateField(i.Id));

				if (baseTemplateField != null)
				{
					foreach (var value in baseTemplateField.Value.Split(this.StringSeparators, StringSplitOptions.None))
					{
						var baseTemplateItem = items.FirstOrDefault(i => this.IsIdEqual(i.Id, value));

						this.GetBaseTemplatesIds(baseTemplateItem, value, baseTemplateIds, items);
					}
				}
			}

			return baseTemplateIds;
		}

		public IEnumerable<ITemplateField> GetFields(IEnumerable<IItem> sections, IEnumerable<IItem> items)
		{
			var fieldItems = items.Where(field => sections.Select(i => i.Id).Contains(field.ParentId)).ToList();
			var fields = new List<ITemplateField>();

			foreach (var fieldItem in fieldItems)
			{
				if (fieldItem.SharedFields.Any() == false)
				{
					continue;
				}

				var typeName = fieldItem.SharedFields
										.Where(f => IsIdEqual(f.Id, "{ab162cc0-dc80-4abf-8871-998ee5d7ba32}"))
										.Select(i => i.Value)
										.FirstOrDefault();

				var field = new TemplateField
				{
					Id = fieldItem.Id,
					Name = fieldItem.Name,
					Key = fieldItem.Name.ToLower(),
					TypeName = typeName,
					TypeKey = typeName.ToLower()
				};

				fields.Add(field);
			}

			return fields.OrderBy(field => field.Name).ToList();
		}

		public IEnumerable<IItem> GetCombinedSections(IEnumerable<IItem> combinedTemplateItems, IEnumerable<IItem> items)
		{
			return items.Where(item => combinedTemplateItems.Select(i => i.Id).Contains(item.ParentId)).ToList();
		}

		public IEnumerable<IItem> GetLocalSections(IItem templateItem, IEnumerable<IItem> items)
		{
			return items.Where(item => item.ParentId == templateItem.Id).ToList();
		}

		public IEnumerable<IItem> GetTemplates(IEnumerable<IItem> items)
		{
			return items.Where(i => this.IsTemplate(i.TemplateId));
		}

		public bool IsBaseTemplateField(string id)
		{
			return new ID(id) == FieldIDs.BaseTemplate;
		}

		public bool IsTemplate(string id)
		{
			return new ID(id) == TemplateIDs.Template;
		}

		public bool IsIdEqual(string id, string comparedToId)
		{
			return id.Equals(comparedToId, StringComparison.InvariantCultureIgnoreCase);
		}
	}
}
