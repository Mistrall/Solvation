using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Solvation.UI.UIComponents.Helpers.Selectors
{
	[ContentProperty("TypeStyleMappings")]
	public class ByTypeStyleSelector : StyleSelector
	{
		private List<TypeStyleMapping> typeStyleMappings = new List<TypeStyleMapping>();

		public List<TypeStyleMapping> TypeStyleMappings
		{
			get { return typeStyleMappings; }
			set { typeStyleMappings = value; }
		}

		public override Style SelectStyle(object item, DependencyObject container)
		{
			return (from mapping in TypeStyleMappings where mapping.Type.IsInstanceOfType(item) select mapping.Style).FirstOrDefault();
		}
	}

	public class TypeStyleMapping
	{
		public Type Type { get; set; }
		public Style Style { get; set; }
	}
}
