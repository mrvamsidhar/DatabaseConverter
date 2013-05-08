using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConverter.Common
{
    public class ReportConverter
    {
        public ReportConverter() { }
        
        public void Convert(ObservableCollection<FeatureListItem> featureitems,bool bAll=false)
        {
            // Creating a new document.
            WordDocument document = new WordDocument();
            foreach (FeatureListItem item in featureitems)
            {
                if (item.Name == "All")
                    continue;
                // Adding a new section to the document.
                IWSection section = document.AddSection();
                IWParagraph paragraph = section.AddParagraph();
                paragraph.ParagraphFormat.BeforeSpacing = 20f;

                //Format the heading.
                IWTextRange text = paragraph.AppendText(item.Name);
                text.CharacterFormat.Bold = true;
                text.CharacterFormat.FontName = "Cambria";
                text.CharacterFormat.FontSize = 14.0f;
                text.CharacterFormat.TextColor = System.Drawing.Color.DarkBlue;
                paragraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;

                paragraph = section.AddParagraph();
                paragraph.ParagraphFormat.BeforeSpacing = 18f;

                DataSet ds = new DataSet();
                DataTable table = null;
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = App.Result.DatabaseConnectionString();
                   //string.Format("Server={0}; user id={1}; Pwd={2}", App.Result.Server,App.Result.UserName,App.Result.Password);// "AIS-163\\TestServer", "sa", "ais@2012");// AIS-163\\TestServer,sa,ais@2012);
                   // con.ConnectionString = con.ConnectionString + ";Database=TestDatabase;";
                    con.Open();
                    //SqlDataAdapter adapter = new SqlDataAdapter("SELECT * from Company", con);
                    SqlDataAdapter adapter = new SqlDataAdapter(item.Query, con);
                    adapter.Fill(ds);
                    ds.Tables[0].TableName = item.Name;
                    table = ds.Tables[0];
                    adapter.Dispose();
                    con.Close();

                }

                //Create a new table
                WTextBody textBody = section.Body;
                IWTable docTable = textBody.AddTable();

                //Set the format for rows
                RowFormat format = new RowFormat();
                format.Borders.BorderType = Syncfusion.DocIO.DLS.BorderStyle.Single;
                format.Borders.LineWidth = 1.0F;
                format.Borders.Color = System.Drawing.Color.Black;

                //Initialize number of rows and cloumns.
                docTable.ResetCells(table.Rows.Count + 1, table.Columns.Count, format, 84);

                //Repeat the header.
                docTable.Rows[0].IsHeader = true;

                string colName;

                //Format the header rows
                for (int c = 0; c <= table.Columns.Count - 1; c++)
                {
                    string[] Cols = table.Columns[c].ColumnName.Split('|');
                    colName = Cols[Cols.Length - 1];
                    IWTextRange theadertext = docTable.Rows[0].Cells[c].AddParagraph().AppendText(colName);
                    theadertext.CharacterFormat.FontSize = 12f;
                    theadertext.CharacterFormat.Bold = true;
                    theadertext.CharacterFormat.TextColor = System.Drawing.Color.White;
                    docTable.Rows[0].Cells[c].CellFormat.BackColor = System.Drawing.Color.FromArgb(33, 67, 126);
                    docTable.Rows[0].Cells[c].CellFormat.Borders.Color = System.Drawing.Color.Black;
                    docTable.Rows[0].Cells[c].CellFormat.Borders.BorderType = Syncfusion.DocIO.DLS.BorderStyle.Single;
                    docTable.Rows[0].Cells[c].CellFormat.Borders.LineWidth = 1.0f;

                    docTable.Rows[0].Cells[c].CellFormat.VerticalAlignment = Syncfusion.DocIO.DLS.VerticalAlignment.Middle;
                }

                //Format the table body rows
                for (int r = 0; r <= table.Rows.Count - 1; r++)
                {
                    for (int c = 0; c <= table.Columns.Count - 1; c++)
                    {
                        string Value = table.Rows[r][c].ToString();
                        IWTextRange theadertext = docTable.Rows[r + 1].Cells[c].AddParagraph().AppendText(Value);
                        theadertext.CharacterFormat.FontSize = 10;

                        docTable.Rows[r + 1].Cells[c].CellFormat.BackColor = ((r & 1) == 0) ? System.Drawing.Color.FromArgb(237, 240, 246) : System.Drawing.Color.FromArgb(192, 201, 219);

                        docTable.Rows[r + 1].Cells[c].CellFormat.Borders.Color = System.Drawing.Color.Black;
                        docTable.Rows[r + 1].Cells[c].CellFormat.Borders.BorderType = Syncfusion.DocIO.DLS.BorderStyle.Single;
                        docTable.Rows[r + 1].Cells[c].CellFormat.Borders.LineWidth = 0.5f;
                        docTable.Rows[r + 1].Cells[c].CellFormat.VerticalAlignment = Syncfusion.DocIO.DLS.VerticalAlignment.Middle;
                    }
                }

                // Add a footer paragraph text to the document.
                WParagraph footerPar = new WParagraph(document);
                // Add text.
                footerPar.AppendText("Copyright Syncfusion Inc. 2001 - 2012");
                // Add page and Number of pages field to the document.
                footerPar.AppendText("			Page ");
                footerPar.AppendField("Page", FieldType.FieldPage);

                section.HeadersFooters.Footer.Paragraphs.Add(footerPar);
            }
            
            document.Save("Sample.doc");



            // Execute Mail Merge with groups.
            //  document.MailMerge.ExecuteGroup(ds.Tables[0]);





            return;// View();
        }
    }
}
