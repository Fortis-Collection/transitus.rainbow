namespace Transitus.Rainbow.Sync
{
	using global::Rainbow.Model;

	public class RainbowField : IField
	{
		public RainbowField(IItemFieldValue itemFieldValue)
		{
			this.Id = itemFieldValue.FieldId.ToString("B");
			this.Name = itemFieldValue.NameHint;
			this.Key = itemFieldValue.FieldType;
			this.Value = itemFieldValue.Value;
		}

		public string Id { get; }
		public string Key { get; }
		public string Name { get; }
		public string Value { get; }
	}
}
