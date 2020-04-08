using System;
using System.IO;
using System.Data;
using System.ComponentModel;
using System.Collections.Generic;
using ExcelLibrary;
using UnityEngine;


public static class LogExporter
{
    /// <summary>
    /// Export multiple test to an Excel sheet, each test gets its own worksheet
    /// </summary>
    public static void ExportTestDataToExcel(List<TestData> pLog)
    {
        DataSet dataSet = new DataSet("Test Results");

        foreach (TestData data in pLog)
        {
            DataTable dataTable = logToTable(data);
            dataSet.Tables.Add(dataTable);
        }

        string fileName = "TestResults_" + DateTime.Now.ToString("dd-MM-yyyy-h-mm").ToString() + ".xls";
        DataSetHelper.CreateWorkbook(fileName, dataSet);
    }


    private static DataTable logToTable(TestData pData)
    {
        DataTable table = new DataTable(pData.testName);
        table.Columns.Add("Frame Number", typeof(int));
        table.Columns.Add("FPS", typeof(decimal));
        table.Columns.Add("Frame Time(ms)", typeof(decimal));
        table.Columns.Add("Additonal data", typeof(string));
        table.Columns.Add("values", typeof(string));

        foreach (FrameInformation frame in pData.frameData)
        {
            DataRow row = table.NewRow();
            row["Frame Number"] = frame.frameNumber;
            row["FPS"] = frame.frameFPS;
            row["Frame Time(ms)"] = frame.frameTime * 1000;
            row["Additonal data"] = "";
            row["values"] = "";
            table.Rows.Add(row);
        }

        addDescriptionRow(table, "Test: ", pData.testDescription);
        addDescriptionRow(table, "Duration: ", pData.testDurationSeconds.ToString());
        addDescriptionRow(table, "Object types: ", pData.type.ToString());
        addDescriptionRow(table, "Grid size: ", pData.gridSize.ToString());
        addDescriptionRow(table, "Triangle count: ", pData.trisCount.ToString());
        addDescriptionRow(table, "Object count: ", pData.objectCount.ToString());
        addDescriptionRow(table, "Tris / Object: ", (pData.trisCount / pData.objectCount).ToString());

        return table;
    }


     /* Sorry for this lazy hack... 
     * The Excel Library doesn't support DBNull so I've added default values. */
    private static void addDescriptionRow(DataTable Table, string col1, string col2)
    {
        DataRow row = Table.NewRow();
        row["Frame Number"] = 0;
        row["FPS"] = 0;
        row["Frame Time(ms)"] = 0;
        row["Additonal data"] = col1;
        row["values"] = col2;
        Table.Rows.Add(row);
    }
}