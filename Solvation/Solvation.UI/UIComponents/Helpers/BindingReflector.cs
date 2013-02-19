﻿using System.Windows;
using System.Windows.Data;

namespace Solvation.UI.UIComponents.Helpers
{
	public class BindingReflector : FrameworkElement
	{
		public static DependencyProperty SourceProperty =
			DependencyProperty.Register("Source", typeof(object), typeof(BindingReflector),
			new FrameworkPropertyMetadata()
			{
				DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
				PropertyChangedCallback = OnSourceChanged
			});

		public object Source
		{
			get { return GetValue(SourceProperty); }
			set { SetValue(SourceProperty, value); }
		}

		public static DependencyProperty TargetProperty =
			DependencyProperty.Register("Target", typeof(object), typeof(BindingReflector),
			new FrameworkPropertyMetadata() { DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

		public object Target
		{
			get { return GetValue(TargetProperty); }
			set { SetValue(TargetProperty, value); }
		}

		private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var reflector = (BindingReflector)d;
			if (reflector.Source != reflector.Target)
			{
				reflector.Target = reflector.Source;
			}
		}
	}
}
