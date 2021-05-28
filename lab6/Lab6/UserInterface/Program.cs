using System;
using Terminal.Gui;

namespace lab6
{
    class Program
    {
        static void Main(string[] args)
        {
            ProductRepository productRepo = new ProductRepository("/home/alex/projects/progbase2/lab6/Lab6/ProductRepository/products");

            Application.Init();
            Toplevel top = Application.Top;

            MainWindow win = new MainWindow();
            // ProductsMenuBar menu = new ProductsMenuBar();

            win.SetRepository(productRepo);
            // menu.SetRepository(productRepo);
            top.Add(win);

            Application.Run();
        }
    }
}
