namespace Domain.ValueObjects
{
	/// <summary>
	/// Su finalidad es brindad funcionalidad de validación sin depender de librerías de terceros
	/// </summary>
	public partial record TextNotNull
	{
		public string Value { get; set; }
		private TextNotNull(string value) => Value = value;

		public static TextNotNull? Validate(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return null;
			}

			return new TextNotNull(value);
		}
	}

}
