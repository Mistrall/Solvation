using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Solvation.UI.Controls;
using WPF.MDI;

namespace Solvation.UI
{
	/// <summary>
	/// MainWindow.xaml startup and logic
	/// </summary>
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();
			originalTitle = Title;
			Container.Children.CollectionChanged += (o, e) => RefreshOpenWindowsMenu();
			Container.MdiChildTitleChanged += Container_MdiChildTitleChanged;
			RefreshOpenWindowsMenu();
		}

		#region Mdi-like title

		readonly string originalTitle;

		void Container_MdiChildTitleChanged(object sender, RoutedEventArgs e)
		{
			if (Container.ActiveMdiChild != null && Container.ActiveMdiChild.WindowState == WindowState.Maximized)
				Title = originalTitle + " - [" + Container.ActiveMdiChild.Title + "]";
			else
				Title = originalTitle;
		}

		#endregion

		#region File menu events
		private void StartNewProblem_Click(object sender, RoutedEventArgs e)
		{
			var problemNum = (1 + Container.Children.Count).ToString(CultureInfo.InvariantCulture);
			var problemFrame = new MdiChild
					{
						Title = "New problem " + problemNum,
						Content = new CreateNewProblem(Container),
						Width = 1100,
						Height = 800,
						Position = new Point(100, 100)
					};
			Container.Children.Add(problemFrame);
		}

		private void OpenProblem_Click(object sender, RoutedEventArgs e)
		{

		}

		private void SaveProblem_Click(object sender, RoutedEventArgs e)
		{

		}

		private void SaveAsProblem_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ProgramExit_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown(0);
		}
		#endregion

		#region Theme Menu Events

		/// <summary>
		/// Handles the Click event of the Generic control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		private void Generic_Click(object sender, RoutedEventArgs e)
		{
			Generic.IsChecked = true;
			Luna.IsChecked = false;
			Aero.IsChecked = false;

			Container.Theme = ThemeType.Generic;
		}

		/// <summary>
		/// Handles the Click event of the Luna control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		private void Luna_Click(object sender, RoutedEventArgs e)
		{
			Generic.IsChecked = false;
			Luna.IsChecked = true;
			Aero.IsChecked = false;

			Container.Theme = ThemeType.Luna;
		}

		/// <summary>
		/// Handles the Click event of the Aero control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		private void Aero_Click(object sender, RoutedEventArgs e)
		{
			Generic.IsChecked = false;
			Luna.IsChecked = false;
			Aero.IsChecked = true;

			Container.Theme = ThemeType.Aero;
		}

		#endregion

		#region New window menu Events

		int ooo = 1;

		/// <summary>
		/// Handles the Click event of the 'Normal window' menu item.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		private void AddWindow_Click(object sender, RoutedEventArgs e)
		{
			Container.Children.Add(new MdiChild { Content = new Label { Content = "Normal window" }, Title = "Window " + ooo++ });
		}

		/// <summary>
		/// Handles the Click event of the 'Fixed window' menu item.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		private void AddFixedWindow_Click(object sender, RoutedEventArgs e)
		{
			Container.Children.Add(new MdiChild { Content = new Label { Content = "Fixed width window" }, Title = "Window " + ooo++, Resizable = false });
		}

		/// <summary>
		/// Handles the Click event of the 'Scroll window' menu item.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		private void AddScrollWindow_Click(object sender, RoutedEventArgs e)
		{
			StackPanel sp = new StackPanel { Orientation = Orientation.Vertical };
			sp.Children.Add(new TextBlock { Text = "Window with scroll", Margin = new Thickness(5) });
			sp.Children.Add(new ComboBox { Margin = new Thickness(20), Width = 300 });
			ScrollViewer sv = new ScrollViewer { Content = sp, HorizontalScrollBarVisibility = ScrollBarVisibility.Auto };

			Container.Children.Add(new MdiChild { Content = sv, Title = "Window " + ooo++ });
		}

		#endregion

		#region Windows menu events
		/// <summary>
		/// Refresh windows list
		/// </summary>
		void RefreshOpenWindowsMenu()
		{
			WindowsSubMenu.Items.Clear();
			MenuItem mi;
			foreach (MdiChild child in Container.Children)
			{
				mi = new MenuItem { Header = child.Title };
				MdiChild child1 = child;
				mi.Click += (o, e) => child1.Focus();
				WindowsSubMenu.Items.Add(mi);
			}
		}

		private void WindowsCascade_Click(object sender, RoutedEventArgs e)
		{
			Container.MdiLayout = MdiLayout.Cascade;
		}

		private void WindowsHorizontally_Click(object sender, RoutedEventArgs e)
		{
			Container.MdiLayout = MdiLayout.TileHorizontal;
		}

		private void WindowsVertically_Click(object sender, RoutedEventArgs e)
		{
			Container.MdiLayout = MdiLayout.TileVertical;
		}

		private void WindowsCloseAll_Click(object sender, RoutedEventArgs e)
		{
			Container.Children.Clear();
		}

		#endregion
	}
}
