using System;
using System.Data;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;

namespace Scorpid.Helpers
{
    public class GridHandler
    {
        #region "Constructors"

        public GridHandler(BaseView prm_objBaseView)
        {
            _objGV = prm_objBaseView as GridView;
        }//constructor

        #endregion

        #region "Constants"

        private const int GRIDCOLUMN_MINWIDTH = 50;

        #endregion

        #region "Variables"

        private GridView _objGV = null;

        #endregion

        #region "Properties"

        public GridView GV
        {
            get { return _objGV; }
        }//property

        #endregion

        #region "Methods"

        public void PrepareDG()
        {
            _objGV.OptionsBehavior.Editable = false;
            _objGV.OptionsBehavior.FocusLeaveOnTab = true;
            _objGV.OptionsBehavior.KeepFocusedRowOnUpdate = false;

            _objGV.OptionsCustomization.AllowColumnMoving = false;

            _objGV.OptionsSelection.EnableAppearanceFocusedCell = false;
            _objGV.OptionsSelection.EnableAppearanceFocusedRow = true;
            _objGV.OptionsSelection.MultiSelect = false;

            _objGV.OptionsView.ColumnAutoWidth = false;
            _objGV.OptionsView.ShowGroupPanel = false;
            _objGV.OptionsView.ShowAutoFilterRow = true;

            _objGV.EndSorting += new EventHandler(_objGV_EndSorting);
        }//void

        public void AddHeader(string prm_strFieldName, string prm_strCaption, Type prm_objType, int prm_intWidth, bool prm_boolVisible, DataTable prm_objDataTable)
        {
            GridColumn objGridColumn = _objGV.Columns.AddField(prm_strFieldName);

            objGridColumn.Caption = prm_strCaption;
            objGridColumn.Width = prm_intWidth;
            objGridColumn.MinWidth = GRIDCOLUMN_MINWIDTH;
            objGridColumn.Visible = prm_boolVisible;

            if (prm_objDataTable != null)
                prm_objDataTable.Columns.Add(new DataColumn(prm_strFieldName, prm_objType));
        }//void

        public void BeginUpdate()
        {
            _objGV.BeginUpdate();
            _objGV.BeginDataUpdate();
        }//void

        public void EndUpdate()
        {
            _objGV.EndDataUpdate();
            _objGV.EndUpdate();
        }//void

        #endregion

        #region "Events"

        private void _objGV_EndSorting(object sender, EventArgs e)
        {
            _objGV.FocusedRowHandle = 0;
        }//void

        #endregion
    }//class
}//namespace
