using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PlantGenius.GUI.Views
{
    /// <summary>
    /// This class will be responsible for converting multiple input values into a single output value and vice versa.
    /// </summary>
    public class MultiParameterValueConverter : IMultiValueConverter
    {
        /// <summary>
        /// The convert method is required when using the IMultiValueConverter.
        /// It takes an array of two values und combines them to a single tuple.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Here, you can handle how to combine or process your parameters
            return (values[0], values[1]); // Creating a tuple
        }

        /// <summary>
        /// The ConvertBack method is required when using the IMultiValueConverter.
        /// We do not implement it as we do not want to reverse the conversion. 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetTypes"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
