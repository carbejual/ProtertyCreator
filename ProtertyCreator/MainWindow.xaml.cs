using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProtertyCreator
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SwitchButton_Click(object sender, RoutedEventArgs e)
        {
            OutTXT.Document.Blocks.Clear();
            TextRange range = new TextRange(InputTXT.Document.ContentStart, InputTXT.Document.ContentEnd);
            string fields = range.Text;
            if (!string.IsNullOrEmpty(fields))
            {
                string result = GetResults(fields);
                OutTXT.Document = new FlowDocument(new Paragraph(new Run(result)));
            }
        }
        public string GetResults(String items)
        {
            string[] result = items.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            var sb = new StringBuilder();

            for (int i = 0; i < result.Length; i++)
            {

                string item = CreateProperties(result[i].Trim());
                sb.Append(item).Append("\r\n");
            }
            return sb.ToString();
        }

       

        public string CreateProperties(string items)
        {
            string sep = "\r\n";
            items = items.Replace(";", "");
            string[] arrItems = items.Split(' ');
            string typeName = arrItems[1];
            string fieldName = arrItems[2];
            string PropertyName = _doCutHead(fieldName);
          


            StringBuilder sb = new StringBuilder();
            sb.Append("public ").Append(typeName).Append(" ").
                Append(PropertyName);
            
            if (Toggle.IsChecked ==true)
            {

                sb.Append("{ get=>").Append(fieldName).Append(";");
            }
            else
            {
                sb.Append(sep);
                sb.Append("{").Append(sep);
                sb.Append("get").Append(sep);
                sb.Append("{").Append(sep);
                sb.Append("return ").Append(fieldName).Append(";").Append(sep);
                sb.Append("}").Append(sep);
                sb.Append("set").Append(sep);
                sb.Append("{").Append(sep);
                if (typeName.Equals("string"))
                {
                    sb.Append("if (!string.Equals(value,").Append(fieldName).Append("))").Append(sep);
                }
                else
                {
                    sb.Append("if (value!=").Append(fieldName);
                }
                sb.Append(") ").Append(sep);

                sb.Append("{").Append(sep);
                sb.Append(fieldName).Append("= value;").Append(sep);
                sb.Append("}").Append(sep);
                sb.Append("}").Append(sep);
            }

            sb.Append("}").Append(sep);
            return sb.ToString();
        }
        private string _doCutHead(string fieldName)
        {
            if (fieldName[0].Equals('m') && fieldName[1].Equals('_'))
            {
                fieldName = fieldName.Substring(2);
            }
            string[] arrItems = fieldName.Split('_');
            if (arrItems != null)
            {
                var resultSt = "";
                var length = arrItems.Length;
                for (int i = 0; i < length; i++)
                {
                    if (string.IsNullOrEmpty(arrItems[i]))
                    {
                        continue;
                    }
                    var tempString=arrItems[i].Trim();
                    if (tempString.Length==1)
                    {
                        tempString = tempString.ToUpper();
                    }
                    else
                    {
                        tempString = tempString.Substring(0, 1).ToUpper() + tempString.Substring(1);
                    }
                    resultSt += tempString;
                }
                return resultSt;
            }
            else
            {
                return fieldName.Substring(0, 1).ToUpper() + fieldName.Substring(1);

            }
        }

       
    }
}
