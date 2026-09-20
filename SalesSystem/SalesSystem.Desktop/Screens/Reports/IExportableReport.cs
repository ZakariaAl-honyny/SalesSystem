namespace SalesSystem.Desktop.Screens.Reports;

public interface IExportableReport
{
    DataGridView GetDataGridView();
    string GetReportName();
}



