using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZ_ANN_4_Letter_Recognition;

namespace Activation_funcs
{
    public enum ActivationFunctionType
    {
        LINEAR,
        THRESHOLD,
        LOGISTIC,
        HYPERBOLIC_TANGENT
    }

    public class ActivationFunction
    {
        public ActivationFunction(ActivationFunctionType f_type)
        {
            switch (f_type)
            {
                case ActivationFunctionType.LINEAR:
                    calculate = Methods.calculate_linear;
                    break;
                case ActivationFunctionType.THRESHOLD:
                    calculate = Methods.calculate_threshold;
                    break;
                case ActivationFunctionType.LOGISTIC:
                    calculate = Methods.calculate_logistic;
                    break;
                case ActivationFunctionType.HYPERBOLIC_TANGENT:
                    calculate = Methods.calculate_hyperbolic_tangent;
                    break;
                default:
                    break;
            }
        }
        public delegate double CalculateFunc(Range sensitivity, double weight);
        public CalculateFunc calculate;

        private static class Methods
        {
            public static double calculate_linear(Range sensitivity, double weight)
            {
                double formula;

                if (weight > sensitivity.max)
                    formula = sensitivity.max;
                else if (weight < sensitivity.min)
                    formula = sensitivity.min;
                else
                    formula = weight;

                return formula;
            }

            public static double calculate_threshold(Range sensitivity, double weight)
            {
                double formula = weight >= sensitivity.max ? 1 : 0;

                return formula;
            }

            public static double calculate_logistic(Range sensitivity, double weight)
            {
                double formula;

                double t;

                if ((sensitivity.min >= -1.0f) && (sensitivity.max <= 1.0f))
                    t = 3.0f;
                else if ((sensitivity.min >= -2.0f) && (sensitivity.max <= 2.0f))
                    t = 1.5f;
                else if ((sensitivity.min >= -3.0f) && (sensitivity.max <= 3.0f))
                    t = 1.0f;
                else
                    t = 4.0f;

                formula = 1 / (1 + Math.Exp((t * (-1.0f)) * weight));

                return formula;
            }

            public static double calculate_hyperbolic_tangent(Range sensitivity, double weight)
            {
                double formula;

                double a;

                if ((sensitivity.min >= -1.0f) && (sensitivity.max <= 1.0f))
                    a = 2.0f;
                else if ((sensitivity.min >= -2.0f) && (sensitivity.max <= 2.0f))
                    a = 1.0f;
                else if ((sensitivity.min >= -3.0f) && (sensitivity.max <= 3.0f))
                    a = 0.5f;
                else
                    a = 1.5f;

                formula = Math.Exp(a * weight) - Math.Exp(a * weight * (-1.0f))
                                                   /
                          Math.Exp(a * weight) + Math.Exp(a * weight * (-1.0f));

                return formula;
            }
        }
    }
}
