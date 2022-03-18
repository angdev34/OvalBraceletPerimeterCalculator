using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Markup;

namespace OvalBracelePerimeterCalculator
{

    public partial class MainWindow : Window, IComponentConnector
    {
        private void UserInputClick(object sender, RoutedEventArgs e)
        {

            if (new Regex("[^0-9]+").IsMatch(UserInput.Text))
            {
                MessageBox.Show("User input is not an integer", "Regex Error");
            }
            else if (int.Parse(UserInput.Text) > 5 || int.Parse(UserInput.Text) < 1)
            {
                MessageBox.Show("User input must be between 1 and 5", "Validation Error");
            }
            else
            {
                HttpWebResponse response = (HttpWebResponse)WebRequest.Create($"http://kutez.com/testapi/get_diameter.php?size={UserInput.Text}").GetResponse();
                if (response.StatusDescription == "OK")
                {
                    MainWindow.APIData apiData = JsonConvert.DeserializeObject<MainWindow.APIData>(new StreamReader(response.GetResponseStream()).ReadToEnd());
                    GetSmallDiameter.Content = (object)apiData.SmallDiameter;
                    GetBigDiameter.Content = (object)apiData.BigDiameter;
                }
                else
                {
                    MessageBox.Show("Server Error", "Error");
                }
            }
        }

        private void CalcBtnClick(object sender, RoutedEventArgs e)
        {
            int BigDiameter = int.Parse(GetBigDiameter.Content.ToString());
            int SmallDiameter = int.Parse(GetSmallDiameter.Content.ToString());
            double Result = (2.0 * Math.PI * Math.Sqrt((double)(((BigDiameter / 2) * (BigDiameter / 2) + (SmallDiameter / 2) * (SmallDiameter / 2)) / 2)));
            GetResult.Content = (object)Result.ToString("F2");
        }

        public class APIData
        {

            public double BigDiameter { get; set; }

            public double SmallDiameter { get; set; }
        }

    }
}
