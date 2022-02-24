using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace ColorPickerApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private bool _lockRGB, _lockXYZ, _lockHSV;
        public MainPage() => InitializeComponent();

        private void BtnShowColorPicker_Click(object sender, RoutedEventArgs e) => Nav_Pop.IsOpen = true;

        private void MyColorPicker_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
        {
            var color = args.NewColor;
            TargetViewBorder.Background = new SolidColorBrush(color);
            (Rs.Value, Gs.Value, Bs.Value) = (color.R, color.G, color.B);
        }

        private void RGB_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (_lockRGB) return;

            (_lockXYZ, _lockHSV) = (true, true);
            (Xs.Value, Ys.Value, Zs.Value) = ColorConverter.RGB_2_XYZ(Rs.Value, Gs.Value, Bs.Value);
            (Hs.Value, Ss.Value, Vs.Value) = ColorConverter.RGB_2_HSV(Rs.Value, Gs.Value, Bs.Value);
            (_lockXYZ, _lockHSV) = (false, false);

            ChangeMainColor();
        }

        private void HSV_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (_lockHSV) return;
            var (R, G, B) = ColorConverter.HSV_2_RGB(Hs.Value, Ss.Value, Vs.Value);

            (_lockXYZ, _lockRGB) = (true, true);
            (Rs.Value, Gs.Value, Bs.Value) = (R, G, B);
            (Xs.Value, Ys.Value, Zs.Value) = ColorConverter.RGB_2_XYZ(R, G, B);
            (_lockXYZ, _lockRGB) = (false, false);

            ChangeMainColor();
        }

        private void XYZ_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (_lockXYZ) return;
            var (R, G, B) = ColorConverter.XYZ_2_RGB(Xs.Value, Ys.Value, Zs.Value);

            (_lockHSV, _lockRGB) = (true, true);
            (Rs.Value, Gs.Value, Bs.Value) = (R, G, B);
            (Hs.Value, Ss.Value, Vs.Value) = ColorConverter.RGB_2_HSV(R, G, B);
            (_lockHSV, _lockRGB) = (false, false);

            ChangeMainColor();
        }

        private void ChangeMainColor()
        {
            RGB.Text = $"#{(int)Rs.Value:X2}{(int)Gs.Value:X2}{(int)Bs.Value:X2}";
            HSV.Text = $"{Hs.Value:0.##}-{Ss.Value:0.##}-{Vs.Value:0.##}";
            XYZ.Text = $"{Xs.Value:0.##}-{Ys.Value:0.##}-{Zs.Value:0.##}";

            var color = Color.FromArgb(myColorPicker.Color.A, (byte)Rs.Value, (byte)Gs.Value, (byte)Bs.Value);
            TargetViewBorder.Background = new SolidColorBrush(color);
            myColorPicker.Color = color;
        }
    }
}
