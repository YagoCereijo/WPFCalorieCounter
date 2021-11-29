using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace Conversor
{
    public class NumericTextBox : TextBox
    {
        public int IntValue
        {
            get
            {
                return Int32.Parse(this.Text);
            }
        }
        public double DoubleValue
        {
            get
            {
                return Double.Parse(this.Text);
            }
        }


        public NumericTextBox()
        {
            this.PreviewTextInput += new TextCompositionEventHandler(NumericTextBox_PreviewTextInput);
        }

        void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            NumberFormatInfo numberFormatInfo = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
            string decimalSeparator = numberFormatInfo.NumberDecimalSeparator;
            string negativeSign = numberFormatInfo.NegativeSign;
            string caracter = e.Text;
            if (Char.IsDigit(e.Text[0]))
            {
                // No hacemos nada porque aceptamos los dígitos
            }
            else if (caracter.Equals(decimalSeparator) || caracter.Equals(negativeSign))
            { // No hacemos nada porque aceptamos el punto y el signo

            }
            else if (caracter == "\b")
            {
                // No hacemos nada porque aceptamos el Backspace
            }
            else
            {
                // Nos saltamos el carácter deteniendo el enrutamiento
                e.Handled = true;
            }
        }
    }
}
