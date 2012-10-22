using System;

namespace Server.Xcap
{
	enum XcapErrors
	{
		NotWellFormed,
		NotXmlFrag,
		NoParent,
		SchemaValidationError,
		NotXmlAttValue,
		CannotInsert,
		CannotDelete,
		UniquenessFailure,
		ConstraintFailure,
		Extension,
		NotUtf8,
	}

	static class XcapErrorsConverter
	{
		static XcapErrorsConverter()
		{
			if (Enum.GetValues(typeof(XcapErrors)).Length != values.Length)
				throw new InvalidProgramException(@"@XcapErrorsConverter");
		}

		private static string[] values = new[]
		{
			"not-well-formed",
			"not-xml-frag",
			"no-parent",
			"schema-validation-error",
			"not-xml-att-value",
			"cannot-insert",
			"cannot-delete",
			"uniqueness-failure",
			"constraint-failure",
			"extension",
			"not-utf-8",
		};

		public static string Convert(this XcapErrors error)
		{
			return values[(int)error];
		}
	}
}
