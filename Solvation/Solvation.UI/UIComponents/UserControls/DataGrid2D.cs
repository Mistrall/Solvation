using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Solvation.UI.UIComponents.Helpers;

namespace Solvation.UI.UIComponents.UserControls
{
    public class DataGrid2D : DataGrid
    {
        #region Statics

        private static readonly Style SDataGridColumnHeaderStyle;
        private static readonly Style SDataGridCellStyle;
        private static readonly Style SDataGridRowHeaderStyle;
        private static readonly Style SDataGridRowStyle;

        static DataGrid2D()
        {
			var resourceLocator = new Uri("/Solvation.UI;component/Themes/DataGridStyleDictionary.xaml", UriKind.Relative);
            var resourceDictionary = (ResourceDictionary)Application.LoadComponent(resourceLocator);
            SDataGridColumnHeaderStyle = resourceDictionary["DataGridColumnHeaderStyle"] as Style;
            SDataGridCellStyle = resourceDictionary["DataGridCellStyle"] as Style;
            SDataGridRowHeaderStyle = resourceDictionary["DataGridRowHeaderStyle"] as Style;
            SDataGridRowStyle = resourceDictionary["DataGridRowStyle"] as Style;
        }

        public static readonly DependencyProperty ItemsSource2DProperty =
            DependencyProperty.Register("ItemsSource2D", typeof(IEnumerable), typeof(DataGrid2D), new UIPropertyMetadata(null,
                ItemsSource2DPropertyChanged));

        private static void ItemsSource2DPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
	        var dataGrid2D = source as DataGrid2D;
	        if (dataGrid2D != null)
		        dataGrid2D.OnItemsSource2DChanged(e.OldValue as IEnumerable, e.NewValue as IEnumerable);
        }

	    #endregion //Statics

        #region Constructor

        public DataGrid2D()
        {
            AutoGenerateColumns = true;
            CanUserAddRows = false;
            Background = Brushes.White;
            SelectionUnit = DataGridSelectionUnit.Cell;
            AutoGeneratingColumn += DataGrid2D_AutoGeneratingColumn;
            LoadingRow += DataGrid2D_LoadingRow;
        }

        #endregion //Constructor

        #region Private Methods

        protected virtual void OnItemsSource2DChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            // Multi Dimensional Arrays with more than 2 dimensions
            // crash on iList[0].
            if (newValue != null && newValue is IList && newValue.GetType().Name.IndexOf("[,,", StringComparison.Ordinal) == -1)
            {
                var type = newValue.GetType();
                var elementType = newValue.GetType().GetElementType();

                var iList = newValue as IList;
                bool multiDimensionalArray = type.IsArray && type.GetArrayRank() == 2;
                if (multiDimensionalArray) // 2D MultiDimensional Array
                {
                    var bindingHelper = new BindingHelper();
                    var method = typeof(BindingHelper).GetMethod("GetBindableMultiDimensionalArray");
                    var generic = method.MakeGenericMethod(elementType);
                    ItemsSource = generic.Invoke(bindingHelper, new object[] { newValue }) as DataView;
                }
                else
                {
                    if (iList.Count == 0)
                    {
                        ItemsSource = null;
                        return;
                    }
                    if (iList[0] is IList) // 2D List
                    {
                        var iListRow1 = iList[0] as IList;
                        if (iListRow1.Count == 0)
                        {
                            ItemsSource = null;
                            return;
                        }
                        var listType = iListRow1[0].GetType();
                        var bindingHelper = new BindingHelper();
                        var method = typeof(BindingHelper).GetMethod("GetBindable2DViewFromIList");
                        var generic = method.MakeGenericMethod(listType);
                        ItemsSource = generic.Invoke(bindingHelper, new object[] { iList }) as DataView;
                    }
                    else // 1D List
                    {
                        var listType = iList[0].GetType();
                        var bindingHelper = new BindingHelper();
                        var method = typeof(BindingHelper).GetMethod("GetBindable1DViewFromIList");
                        var generic = method.MakeGenericMethod(listType);
                        ItemsSource = generic.Invoke(bindingHelper, new object[] { iList }) as DataView;
                    }
                }
            }
            else
            {
                ItemsSource = null;
            }
        }

        #endregion // Private Methods

        #region EventHandlers

        void DataGrid2D_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex()).ToString(CultureInfo.InvariantCulture); 
        }

        void DataGrid2D_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var column = e.Column as DataGridTextColumn;
	        if (column == null) return;
	        var binding = column.Binding as Binding;
	        if (binding != null) binding.Path = new PropertyPath(binding.Path.Path + ".Value");
        }

        #endregion //EventHandlers

        #region Properties

        public IEnumerable ItemsSource2D
        {
            get { return (IEnumerable)GetValue(ItemsSource2DProperty); }
            set { SetValue(ItemsSource2DProperty, value); }
        }

        private bool useModifiedDataGridStyle;
        public bool UseModifiedDataGridStyle
        {
            get
            {
                return useModifiedDataGridStyle;
            }
            set
            {
                useModifiedDataGridStyle = value;
                if (useModifiedDataGridStyle)
                {
                    RowHeaderStyle = SDataGridRowHeaderStyle;
                    CellStyle = SDataGridCellStyle;
                    ColumnHeaderStyle = SDataGridColumnHeaderStyle;
                    RowStyle = SDataGridRowStyle;
                    GridLinesVisibility = DataGridGridLinesVisibility.None;
                }
                else
                {
                    RowHeaderStyle = null;
                    CellStyle = null;
                    ColumnHeaderStyle = null;
                    RowStyle = null;
                    GridLinesVisibility = DataGridGridLinesVisibility.All;
                }
            }
        }

        #endregion //Properties
    }
}
