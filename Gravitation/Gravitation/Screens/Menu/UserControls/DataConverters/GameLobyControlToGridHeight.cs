using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.ComponentModel;
using System.Windows.Forms;

namespace Gravitation.Screens.Menu.UserControls.DataConverters
{
    class GameLobyControlToGridHeight : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            return value;
            //int ivalue = (int)value;
            //return ivalue - 22;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;

            //int ivalue = (int)value;
            //return ivalue + 22;
        }
    }
}
