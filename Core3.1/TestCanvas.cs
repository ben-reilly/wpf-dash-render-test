using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace Core_3_1
{
    class TestCanvas : FrameworkElement
    {
        const double LineLength = 1500.0;

        const double SpaceBetweenLineGroups = 60.0;
        const double SpaceBetweenScaleGroups = 60.0;

        const double MagicDashLimit = 0.05;

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            var scales = new List<double> { 1, 10, 100, 1000 };

            // Eliminate blurring caused by rounding
            dc.PushTransform(new TranslateTransform(0.5, 0.5));

            dc.PushTransform(new TranslateTransform(30, 0));

            // Vertical references lines; 10px apart
            dc.PushTransform(new TranslateTransform(40, 0));
            var gridPen = new Pen(Brushes.LightGray, 1);
            for (int x = 0; x <= LineLength; x += 10)
            {
                dc.DrawGeometry(null, gridPen, new LineGeometry(new Point(x, 0), new Point(x, this.Height)));
            }
            dc.Pop();

            dc.PushTransform(new TranslateTransform(0, 30));
            dc.DrawText(
                new FormattedText(
                    "Dash length\nbefore scaling",
                    CultureInfo.GetCultureInfo("en-us"),
                    FlowDirection.LeftToRight,
                    new Typeface("Verdana"),
                    8.0,
                    Brushes.Black,
                    1.0),
                new Point(-10, -5));

            dc.PushTransform(new TranslateTransform(0, 30));

            double cursorY = 0.0;

            void DrawLineGroup(double dashValue)
            {
                var dashStyle = dashValue == 0 ? null : new DashStyle(new List<double> { dashValue, dashValue }, 0);

                dc.PushTransform(new TranslateTransform(0, cursorY));

                for (int row = 0; row < scales.Count; ++row)
                {
                    var scale = scales[row];
                    var dashLengthBeforeScale = dashValue / scale;
                    var brush = dashLengthBeforeScale < MagicDashLimit ? Brushes.Red : Brushes.DarkGreen;

                    dc.PushTransform(new TranslateTransform(0, 10 * row));

                    dc.DrawText(
                        new FormattedText(
                            $"{dashLengthBeforeScale:F5}",
                            CultureInfo.GetCultureInfo("en-us"),
                            FlowDirection.LeftToRight,
                            new Typeface("Verdana"),
                            8.0,
                            brush,
                            1.0),
                        new Point(-10, -5));

                    dc.PushTransform(new TranslateTransform(40, 0));
                    dc.PushTransform(new ScaleTransform(scale, scale));

                    var pen = new Pen(brush, 1 / scale);
                    pen.DashStyle = dashStyle;
                    dc.DrawGeometry(null, pen, new LineGeometry(new Point(0, 0), new Point(LineLength / scale, 0)));

                    dc.Pop();
                    dc.Pop();
                    dc.Pop();
                }

                dc.Pop();

                cursorY += SpaceBetweenLineGroups;
            }

            DrawLineGroup(0.1);
            DrawLineGroup(0.2);
            DrawLineGroup(0.45);
            DrawLineGroup(0.5);

            cursorY += SpaceBetweenScaleGroups;

            DrawLineGroup(1);
            DrawLineGroup(2);
            DrawLineGroup(4.5);
            DrawLineGroup(5);

            cursorY += SpaceBetweenScaleGroups;

            DrawLineGroup(10);
            DrawLineGroup(20);
            DrawLineGroup(45);
            DrawLineGroup(50);

            cursorY += SpaceBetweenScaleGroups;

            DrawLineGroup(100);
            DrawLineGroup(200);
            DrawLineGroup(450);
            DrawLineGroup(500);

            dc.Pop();
            dc.Pop();
            dc.Pop();

            this.Height = cursorY + 100;
            this.Width = LineLength + 100;
        }
    }
}
