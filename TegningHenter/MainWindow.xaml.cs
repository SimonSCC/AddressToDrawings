using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Size = System.Windows.Size;
using Point = System.Windows.Point;
using System.Windows.Markup;
using System;

namespace TegningHenter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        //private void test(object sender, RoutedEventArgs e)
        //{

        //    PrintDialog print = new();
        //    print.ShowDialog();
        //    print.Print()

        //    PageContent pageContent = new PageContent();
        //    FixedPage fixedPage = new FixedPage();
        //    UIElement visual = CreateVisual(); // Create the visual element for the image

        //    FixedPage.SetLeft(visual, 0);
        //    FixedPage.SetTop(visual, 0);

        //    double pageWidth = 96 * 8.5; // The size of the A4 page
        //    double pageHeight = 96 * 11;

        //    fixedPage.Width = pageWidth;
        //    fixedPage.Height = pageHeight;

        //    fixedPage.Children.Add((UIElement)visual);

        //    Size sz = new Size(pageWidth, pageHeight);
        //    fixedPage.Measure(sz);
        //    fixedPage.Arrange(new Rect(new Point(), sz));
        //    fixedPage.UpdateLayout();

        //    ((IAddChild)pageContent).AddChild(fixedPage);
        //}

        //private UIElement CreateVisual()
        //{
        //    return new UIElement();
        //}
    }
}
