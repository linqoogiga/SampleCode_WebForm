using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using PublicTool;
/// <summary>
/// CExcel 的摘要描述
/// </summary>
public class CExcelBase
{
    private Microsoft.Office.Interop.Excel.Application app = null;
    private Microsoft.Office.Interop.Excel.Workbook workbook = null;
    private Microsoft.Office.Interop.Excel.Worksheet worksheet = null;
    private Microsoft.Office.Interop.Excel.Range workSheet_range = null;

    public CExcelBase()
    {
        createDoc();
    }

    private void InitailExcel()
    {
        //檢查PC有無Excel在執行
        bool flag = false;
        foreach (var item in Process.GetProcesses())
        {
            if (item.ProcessName == "EXCEL")
            {
                flag = true;
                break;
            }
        }
        if (!flag)
        {
            app = new Microsoft.Office.Interop.Excel.Application();
        }
        else
        {
            object obj = Marshal.GetActiveObject("Excel.Application");//引用已在執行的Excel
            app = obj as Microsoft.Office.Interop.Excel.Application;
        }

    }

    public void createDoc()
    {
        try
        {
            InitailExcel();
            //app = new Microsoft.Office.Interop.Excel.Application(); 
            //app.Visible = true;
            if (app == null)
            {
                Console.WriteLine("EXCEL could not be started. Check that your office installation and project references are correct.");
                return;
            }
            app.Visible = true;//設false效能會比較好
            workbook = app.Workbooks.Add(1);
            worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1]; //指定活頁簿,代表Sheet1 
        }
        catch (Exception ex)
        {
            
        }
    }

    public void InsertData(CExcelBE be)
    {
        worksheet.Cells[be.Row, be.Col] = be.Text;
        workSheet_range = worksheet.get_Range(be.StartCell, be.EndCell);
        workSheet_range.Interior.Color = GetColorValue(be.InteriorColor);
        workSheet_range.MergeCells = be.IsMerge;
        workSheet_range.Borders.Color = System.Drawing.Color.Black.ToArgb();
        workSheet_range.ColumnWidth = be.Size;
        workSheet_range.Font.Color = string.IsNullOrEmpty(be.FontColor) ?
        System.Drawing.Color.White.ToArgb() : System.Drawing.Color.Black.ToArgb();
        workSheet_range.NumberFormat = be.Formart;
    }

    private int GetColorValue(string interiorColor)
    {
        switch (interiorColor)
        {
            case "YELLOW":
                return System.Drawing.Color.Yellow.ToArgb();
            case "GRAY":
                return System.Drawing.Color.Gray.ToArgb();
            case "GAINSBORO":
                return System.Drawing.Color.Gainsboro.ToArgb();
            case "Turquoise":
                return System.Drawing.Color.Turquoise.ToArgb();
            case "PeachPuff":
                return System.Drawing.Color.PeachPuff.ToArgb();

            default:
                return System.Drawing.Color.White.ToArgb();
        }
    }
}