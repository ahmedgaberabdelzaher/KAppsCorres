using System;
using System.ComponentModel;
using System.Reflection;

namespace CorresApp.Helpers
{
	public static class EnumExtensions
	{
		public static string GetDescription(this Enum value)
		{
		
			System.Reflection.FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
			if (fieldInfo == null) return null;
			var attribute = (DescriptionAttribute)fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute));
			return attribute.Description;
		}
	}

}
