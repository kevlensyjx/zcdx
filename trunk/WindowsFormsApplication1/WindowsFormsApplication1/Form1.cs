
using org.apache.pdfbox.pdfparser;
using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.pdmodel.common;
using org.apache.pdfbox.util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private static DataTable dt = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                MessageBox.Show("未选择文件类型");
                return;
            }
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("未选择解析文件");
                return;
            }
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("未选择保存文件位置");
                return;
            }
            PDDocument doc = PDDocument.load(textBox1.Text);
            //  PDFTextStripper stripper = new PDFTextStripper();
            //  string s = stripper.getText(doc);
            int pages = doc.getNumberOfPages();
            PDFTextStripper stripper = new PDFTextStripper();
            stripper.setSortByPosition(true);

            string content = stripper.getText(doc);
            content = content.Replace(comboBox1.Text + "兑现清单", "^");
            string[] cc = content.Split('^');

            for (int i = 1; i < cc.Length; i++)
            {
                var res = cc[i].TrimEnd();
                int page = int.Parse(res.Remove(0, res.Length - 2));
                string name = res.Replace("\r\n", "$").Split('$')[1].Split(' ')[1];
                var code = dt.Select("ITEM_TYPE = '" + comboBox1.Text + "' and ITEM_NAME = '" + name + "'");
                string codename = code[0]["ITEM_CODE"].ToString();
                string savepath = textBox2.Text + "\\" + codename + ".pdf";
                generateSubPdf(doc, savepath, pages, "L", page, 1);
                //  newdoc.addPage(doc.pa)
            }

        }
        public bool generateSubPdf(PDDocument document, string depositPath, int totalPage, string pgStartPos, int startPage, int pageNum)
        {
            bool flag = false;

            int stopPage = startPage + pageNum - 1;
            //处理结束页  
            String pgStopPos = null;
            if (stopPage % 2 == 0)
            {
                pgStopPos = "L";
            }
            else if (stopPage % 2 == 1)
            {
                pgStopPos = "R";
            }
            
            //截取PDF  
            try
            {
                PDPageNode pdpt = document.getDocumentCatalog().getPages();
                var it = pdpt.getKids().iterator();
                PDDocument doc = new PDDocument();
                PDPage pdp = null;
                int i = 1;
                for (; it.hasNext();)
                {
                    if (i <= stopPage && i >= startPage)
                    {
                        pdp = (PDPage)it.next();
                        PDRectangle r = pdp.getBleedBox();
                       
                            if (pageNum == 1)
                            {
                                if (pgStartPos.Equals("R"))
                                {
                                    r.setLowerLeftX(r.getUpperRightX() / 2);
                                    r.setLowerLeftY(0);
                                }
                                if (pgStartPos.Equals("L"))
                                {
                                    r.setUpperRightX(r.getUpperRightX() / 2);
                                    r.setUpperRightY(r.getUpperRightY());
                                }
                            }
                            else
                            {
                                if (i == startPage)
                                {
                                    if (pgStartPos.Equals("R"))
                                    {
                                        r.setLowerLeftX(r.getUpperRightX() / 2);
                                        r.setLowerLeftY(0);
                                    }
                                }
                                else if (i == stopPage)
                                {
                                    if (pgStopPos.Equals("L"))
                                    {
                                        r.setUpperRightX(r.getUpperRightX() / 2);
                                        r.setUpperRightY(r.getUpperRightY());
                                    }
                                }
                            }
                        
                        pdp.setBleedBox(r);
                        doc.addPage(pdp);
                    }
                    else
                    {
                        it.next();
                    }
                    i++;
                }
                doc.save(depositPath);
                doc.close();
            }
            catch (Exception ex)
            {

            }

            return flag;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            sq_SolutionDBDataSetTableAdapters.BASE_PROJECT_INFOTableAdapter adapter = new sq_SolutionDBDataSetTableAdapters.BASE_PROJECT_INFOTableAdapter();
            dt = adapter.GetData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
