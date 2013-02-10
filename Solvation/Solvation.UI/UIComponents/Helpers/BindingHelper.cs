using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace Solvation.UI.UIComponents.Helpers
{
    public class BindingHelper
    {
        public DataView GetBindableMultiDimensionalArray<T>(T[,] array)
        {
            var dataTable = new DataTable();
            for (int i = 0; i < array.GetLength(1); i++)
            {
                dataTable.Columns.Add(i.ToString(CultureInfo.InvariantCulture), typeof(Ref<T>));
            }
            for (int i = 0; i < array.GetLength(0); i++)
            {
                var dataRow = dataTable.NewRow();
                dataTable.Rows.Add(dataRow);
            }
            var dataView = new DataView(dataTable);
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    int a = i;
                    int b = j;
                    var refT = new Ref<T>(() => array[a, b], z => { array[a, b] = z; });
                    dataView[i][j] = refT;
                }
            }
            return dataView;
        }

        public DataView GetBindable1DViewFromIList<T>(IList<T> list1D)
        {
            var dataTable = new DataTable();
            for (int i = 0; i < list1D.Count; i++)
            {
                dataTable.Columns.Add(i.ToString(CultureInfo.InvariantCulture), typeof(Ref<T>));
            }
            var dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);
            var dataView = new DataView(dataTable);
            for (int i = 0; i < list1D.Count; i++)
            {
                int a = i;
                var refT = new Ref<T>(() => list1D[a], z => { list1D[a] = z; });
                dataView[0][i] = refT;
            }
            return dataView;
        }

        public DataView GetBindable2DViewFromIList<T>(IList list2D)
        {
            var dataTable = new DataTable();
            for (int i = 0; i < ((IList)list2D[0]).Count; i++)
            {
                dataTable.Columns.Add((i+1).ToString(CultureInfo.InvariantCulture), typeof(Ref<T>));
            }
            for (int i = 0; i < list2D.Count; i++)
            {
                var dataRow = dataTable.NewRow();
                dataTable.Rows.Add(dataRow);
            }
            var dataView = new DataView(dataTable);
            for (int i = 0; i < list2D.Count; i++)
            {
                for (int j = 0; j < ((IList)list2D[i]).Count; j++)
                {
                    int a = i;
                    int b = j;
                    var refT = new Ref<T>(() => (list2D[a] as IList<T>)[b], z => { (list2D[a] as IList<T>)[b] = z; });                    
                    dataView[i][j] = refT;
                }
            }
            return dataView;
        }
    }
}
