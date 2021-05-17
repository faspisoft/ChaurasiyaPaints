using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace faspi
{
    class DOSReport
    {
        DOSPrint dmprnt = new DOSPrint();
        DataTable tdt = new DataTable();
       
        private String formatting(String st)
        {
            StringBuilder read = new StringBuilder(st);
            read.Replace("<b>", dmprnt.BoldOn);
            read.Replace("</b>", dmprnt.BoldOff);
            read.Replace("<u>", dmprnt.UnderlineOn);
            read.Replace("</u>", dmprnt.UnderlineOff);
            read.Replace("<h1>", dmprnt.HeadingOn);
            read.Replace("</h1>", dmprnt.HeadingOff);
            read.Replace("<sfont>", dmprnt.SmallFont);
            read.Replace("</sfont>", dmprnt.Normal);
            read.Replace("<lfont>", dmprnt.LargeFont);
            read.Replace("</lfont>", dmprnt.Normal);
            read.Replace("<condensed>", dmprnt.CondensedOn);
            read.Replace("</condensed>", dmprnt.Normal);
            read.Replace("<expand>", dmprnt.Expandido);
            read.Replace("</expand>", dmprnt.ExpandidoNormal);
            return read.ToString();
        }

        public void Journal(String fileName, DateTime DateFrom, DateTime DateTo)
        {
            if (fileName == "LPT1")
            {
                dmprnt.Inicio(fileName);
            }
            else
            {
                dmprnt.Inicio(System.Windows.Forms.Application.StartupPath + "/" + fileName + ".txt");
            }

            string str = "";
            DataTable dtCompany = new DataTable();
            Database.GetSqlData("Select * from Company", dtCompany);
            if (dtCompany.Rows.Count > 0)
            {
                str = "<h1>" + dmprnt.GetCenterdFormatedText(dtCompany.Rows[0]["Name"].ToString(), 40) + Environment.NewLine;
                str += "<b>" + dmprnt.GetCenterdFormatedText(dtCompany.Rows[0]["Address1"].ToString(), 40) + Environment.NewLine;
                str += dmprnt.GetCenterdFormatedText(dtCompany.Rows[0]["Address2"].ToString(), 40) + "</h1></b>" + Environment.NewLine + Environment.NewLine;
            }

            str += dmprnt.GetCenterdFormatedText("Journal, for the period " + DateFrom.ToString("dd MMM, yyyy") + " To " + DateTo.ToString("dd MMM, yyyy"), 75) + Environment.NewLine;
            str += dmprnt.Line();
            str += Environment.NewLine;
            str += dmprnt.GetFormatedText("Doc. No.", 25);
            str += dmprnt.GetFormatedText("Account", 32);
            str += dmprnt.GetFormatedText("Narration", 41);
            str += dmprnt.GetFormatedText("Debit", 15);
            str += dmprnt.GetFormatedText("Credit", 15) + Environment.NewLine;
            str += dmprnt.Line();
            str += Environment.NewLine;
            DataTable dt = new DataTable();
            string sql = "";

            if (Database.IsKacha == false)
            {
                sql = "SELECT Vdate, DocNumber, Name, Expr1, Dr, Cr FROM QryJournal WHERE (((A)=" + access_sql.Singlequote + "True" + access_sql.Singlequote + ")) ORDER BY Vdate, Short, Vnumber, Cr, Dr;";
            }
            else
            {
                sql = "SELECT QryJournal.Vdate, QryJournal.DocNumber, QryJournal.Name, QryJournal.Expr1, QryJournal.Dr, QryJournal.Cr FROM QryJournal WHERE (((QryJournal.B)=True)) ORDER BY QryJournal.JOURNAL.Vdate, QryJournal.VOUCHERTYPE.Short, QryJournal.VOUCHERINFO.Vnumber, QryJournal.JOURNAL.Cr, QryJournal.JOURNAL.Dr;";
            }
            Database.GetSqlData(sql, dt);

            DataRow[] drow = dt.Select("Vdate>=#" + DateFrom.ToString(Database.dformat) + "# and Vdate<=#" + DateTo.ToString(Database.dformat) + "#");
            tdt.Clear();
            tdt = drow.CopyToDataTable();
            tdt.DefaultView.Sort = "Vdate";

            double totdr = 0;
            double totcr = 0;

            for (int i = 0; i < tdt.Rows.Count; i++)
            {
                if (tdt.Rows[i]["Name"].ToString().Length > 18)
                {
                    str += dmprnt.GetFormatedText(funs.GetFixedLengthString(tdt.Rows[i]["DocNumber"].ToString(), 17), 17);
                    str += dmprnt.GetFormatedText(funs.GetFixedLengthString(tdt.Rows[i]["Name"].ToString(), 18), 18);
                    str += dmprnt.GetFormatedText(funs.GetFixedLengthString(tdt.Rows[i]["Expr1"].ToString(), 24), 24);

                    if (double.Parse(tdt.Rows[i]["Dr"].ToString()) == 0)
                    {
                        str += dmprnt.GetRightFormatedText("", 6);
                    }
                    else
                    {
                        str += dmprnt.GetRightFormatedText(funs.DecimalPoint(double.Parse(tdt.Rows[i]["Dr"].ToString()), 2), 6);
                    }

                    if (double.Parse(tdt.Rows[i]["Cr"].ToString()) == 0)
                    {
                        str += dmprnt.GetRightFormatedText("", 9);
                    }
                    else
                    {
                        str += dmprnt.GetRightFormatedText(funs.DecimalPoint(double.Parse(tdt.Rows[i]["Cr"].ToString()), 2), 9);
                    }

                    str += Environment.NewLine;
                    string strName = tdt.Rows[i]["Name"].ToString();
                    int strNamelastindex = strName.Length - 19;
                    string finalstrName = strName.Substring(18, strNamelastindex + 1);
                    string strNarr = tdt.Rows[i]["Expr1"].ToString();
                    int strNarrlastindex = strNarr.Length - 25;
                    string finalstrNarr = "";
                    if (tdt.Rows[i]["Expr1"].ToString().Length > 24)
                    {
                        finalstrNarr = strNarr.Substring(24, strNarrlastindex + 1);
                    }
                    if (finalstrName != "" && finalstrNarr != "")
                    {
                        str += dmprnt.GetFormatedText(funs.GetFixedLengthString("", 17), 17);
                        str += dmprnt.GetFormatedText(funs.GetFixedLengthString(finalstrName.Trim(), 18), 18);
                        str += dmprnt.GetFormatedText(funs.GetFixedLengthString(finalstrNarr.Trim(), 24), 24);
                    }
                    else if (finalstrName != "")
                    {
                        str += dmprnt.GetFormatedText(funs.GetFixedLengthString("", 17), 17);
                        str += dmprnt.GetFormatedText(funs.GetFixedLengthString(finalstrName.Trim(), 18), 18);
                    }
                    else if (finalstrNarr != "")
                    {
                        str += dmprnt.GetFormatedText(funs.GetFixedLengthString("", 17), 17);
                        str += dmprnt.GetFormatedText(funs.GetFixedLengthString("", 18), 18);
                        str += dmprnt.GetFormatedText(funs.GetFixedLengthString(finalstrNarr.Trim(), 24), 24);
                    }
                    str += Environment.NewLine;
                }
                else
                {
                    str += dmprnt.GetFormatedText(funs.GetFixedLengthString(tdt.Rows[i]["DocNumber"].ToString(), 17), 17);
                    str += dmprnt.GetFormatedText(funs.GetFixedLengthString(tdt.Rows[i]["Name"].ToString(), 18), 18);
                    str += dmprnt.GetFormatedText(funs.GetFixedLengthString(tdt.Rows[i]["Expr1"].ToString(), 24), 24);

                    if (double.Parse(tdt.Rows[i]["Dr"].ToString()) == 0)
                    {
                        str += dmprnt.GetRightFormatedText("", 6);
                    }
                    else
                    {
                        str += dmprnt.GetRightFormatedText(funs.DecimalPoint(double.Parse(tdt.Rows[i]["Dr"].ToString()), 2), 6);
                    }

                    if (double.Parse(tdt.Rows[i]["Cr"].ToString()) == 0)
                    {
                        str += dmprnt.GetRightFormatedText("", 9);
                    }
                    else
                    {
                        str += dmprnt.GetRightFormatedText(funs.DecimalPoint(double.Parse(tdt.Rows[i]["Cr"].ToString()), 2), 9);
                    }
                    str += Environment.NewLine;

                    string strNarr = tdt.Rows[i]["Expr1"].ToString();
                    int strNarrlastindex = strNarr.Length - 25;
                    string finalstrNarr = strNarr.Substring(24, strNarrlastindex + 1);

                    if (finalstrNarr != "")
                    {
                        str += dmprnt.GetFormatedText(funs.GetFixedLengthString("", 17), 17);
                        str += dmprnt.GetFormatedText(funs.GetFixedLengthString("", 18), 18);
                        str += dmprnt.GetFormatedText(funs.GetFixedLengthString(finalstrNarr.Trim(), 24), 24);
                        str += Environment.NewLine;
                    }
                }

                totdr += double.Parse(tdt.Rows[i]["Dr"].ToString());
                totcr += double.Parse(tdt.Rows[i]["Cr"].ToString());
            }

            str += dmprnt.Line();
            str += Environment.NewLine;
            str += dmprnt.GetFormatedText(funs.GetFixedLengthString("Total", 17), 17);
            str += dmprnt.GetFormatedText(funs.GetFixedLengthString("", 18), 18);

            str += dmprnt.GetFormatedText(funs.GetFixedLengthString("", 22), 22);
            str += dmprnt.GetRightFormatedText(funs.DecimalPoint(totdr, 2), 6);
            str += dmprnt.GetRightFormatedText(funs.DecimalPoint(totcr, 2), 9);
            str += Environment.NewLine;
            str += dmprnt.Line();
            str = formatting(str);
            dmprnt.Imp(str);

            dmprnt.Eject();
            dmprnt.Fim();
        }
    }
}
