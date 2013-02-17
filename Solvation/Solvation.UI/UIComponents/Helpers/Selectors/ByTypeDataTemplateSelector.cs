using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Solvation.UI.UIComponents.Helpers.Selectors
{
	[ContentProperty("TypeTemplateMappings")]
	public class ByTypeDataTemplateSelector : DataTemplateSelector
	{
		private List<TypeTemplateMapping> typeTemplateMappings = new List<TypeTemplateMapping>();

		public List<TypeTemplateMapping> TypeTemplateMappings
		{
			get { return typeTemplateMappings; }
			set { typeTemplateMappings = value; }
		}

		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			return (from mapping in TypeTemplateMappings 
					where mapping.Type.IsInstanceOfType(item) 
					select mapping.Template).FirstOrDefault();
		}
	}

	public class TypeTemplateMapping
	{
		public Type Type { get; set; }
		public DataTemplate Template { get; set; }
	}
}
