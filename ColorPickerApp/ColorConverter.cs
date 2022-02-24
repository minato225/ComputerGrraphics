using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorPickerApp
{
    public class ColorConverter
    {
        public static (double R, double G, double B) HSV_2_RGB(double H, double S, double V)
        {
            S /= 100; V /= 100;
            if (S == 0)
                return (V, V, V);

            (double R, double G, double B) result;

            long i;
            double f, p, q, t;

            H = H == 360 ? 0 : H / 60;

            i = (long)H;
            f = H - i;

            p = V * (1.0 - S);
            q = V * (1.0 - S * f);
            t = V * (1.0 - S * (1.0 - f));

            switch (i)
            {
                case 0: { result = (V, t, p); break; }
                case 1: { result = (q, V, p); break; }
                case 2: { result = (p, V, t); break; }
                case 3: { result = (p, q, V); break; }
                case 4: { result = (t, p, V); break; }
                default: { result = (V, p, q); break; }
            };

            return (result.R * 255, result.G * 255, result.B * 255);
        }

        public static (double H, double S, double V) RGB_2_HSV(double R, double G, double B)
        {
            R /= 255; G /= 255; B /= 255;
            double delta, min, max, H, S, V;

            min = Math.Min(Math.Min(R, G), B);
            max = Math.Max(Math.Max(R, G), B);
            V = max;
            delta = max - min;

            if (delta < 0.00001) return (0, 0, V);
            if (max > 0.0) S = delta / max;
            else return (0, 0, V);

            if (R >= max) H = (G - B) / delta;
            else if (G >= max) H = 2.0 + (B - R) / delta;
            else H = 4.0 + (R - G) / delta;

            H *= 60;

            if (H < 0.0) H += 360;

            return (H, S * 100, V * 100);
        }

        public static (double X, double Y, double Z) RGB_2_XYZ(double R, double G, double B)
        {
            double F(double x) => x >= 0.04045 ? Math.Pow((x + 0.055) / 1.055, 2.4) : x / 12.92;

            var X = 100 * (0.412453 * F(R / 255) + 0.357580 * F(G / 255) + 0.180423 * F(B / 255));
            var Y = 100 * (0.212671 * F(R / 255) + 0.715160 * F(G / 255) + 0.072169 * F(B / 255));
            var Z = 100 * (0.019334 * F(R / 255) + 0.119193 * F(G / 255) + 0.950227 * F(B / 255));

            return (X, Y, Z);
        }

        public static (double R, double G, double B) XYZ_2_RGB(double X, double Y, double Z)
        {
            double F(double x) => x >= 0.0031308 ? 1.055 * Math.Pow(x, 1 / 2.4) - 0.055 : x * 12.92;

            var R = F((3.2406 * X + -1.5372 * Y + -0.4986 * Z) / 100) * 255;
            var G = F((0.9689 * X + +1.8758 * Y + +0.0415 * Z) / 100) * 255;
            var B = F((0.0557 * Y + -0.2040 * Y + +1.0570 * Z) / 100) * 255;

            return (R, G, B);
        }
    }
}
