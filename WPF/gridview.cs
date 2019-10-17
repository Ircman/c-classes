
// <summary>
        /// Метод получает строку 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dg_datagrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

            Class1 s;
            var cellInfo = dg_datagrid.SelectedCells[0];

            s = (Class1)cellInfo.Item;
            var tmp = s.test2;
            

            var content = cellInfo.Column.GetCellContent(cellInfo.Item);
           
            ;
        }
