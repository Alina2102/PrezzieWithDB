using System;

namespace PrezzieWithDB.ViewModels
{
    internal class RangeAttribute : Attribute
    {
        private int v1;
        private int v2;
        private double v;
        private double v3;
        private string errorMessage;

        public RangeAttribute(int v1, int v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }

        public RangeAttribute(double v, double v3, string ErrorMessage)
        {
            this.v = v;
            this.v3 = v3;
            errorMessage = ErrorMessage;
        }
    }
}