using Terminal.Gui;
using System.Collections.Generic;

namespace lab6
{
    public class MainWindow : Window
    {
        private int pageLength = 10;
        private int page = 1;

        private FrameView frameView;
        private Label noProductsLbl;
        private ListView allProductsListView;
        private ProductRepository productRepo;
        private Label currentPageLbl;
        private Label totalPagesLbl;
        private Button prevPageBtn;
        private Button nextPageBtn;
        private Label slash;
        private Button deleteProductBtn;
        private Button editProductBtn;
        // protected Product product;

        public MainWindow()
        {
            this.Title = "Products";

            MenuBar menu = new MenuBar(new MenuBarItem[] {
                new MenuBarItem ("_File", new MenuItem [] {
                    new MenuItem ("_Quit", "Quit the program", OnQuit),
                    new MenuItem ("_New product", "Add new product", OnNewProduct),
                }),
                new MenuBarItem ("_Help", new MenuItem [] {
                    new MenuItem ("_About", "", OnAbout),
                }),
            });

            Rect frame = new Rect(4, 8, 40, 200);
            allProductsListView = new ListView(new List<Product>())
            {
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            allProductsListView.OpenSelectedItem += OnOpenProduct;

            frameView = new FrameView("Products list")
            {
                X = 2,
                Y = 8,
                Width = Dim.Fill() - 4,
                Height = pageLength + 2,
            };
            frameView.Add(allProductsListView);
            this.Add(frameView, menu);

            Button createNewProductBtn = new Button("Add Product")
            {
                X = 2,
                Y = Pos.Top(frameView) - 3,
            };
            createNewProductBtn.Clicked += OnCreateButtonClicked;
            this.Add(createNewProductBtn);

            // Label if there are no products
            noProductsLbl = new Label("There are no products in the database!")
            {
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                Visible = false,
            };
            frameView.Add(noProductsLbl);

            // Previous page btn
            prevPageBtn = new Button("<")
            {
                X = 2,
                Y = Pos.Bottom(frameView) + 1,
            };
            prevPageBtn.Clicked += OnPrevPage;
            this.Add(prevPageBtn);

            // Current page
            currentPageLbl = new Label("?")
            {
                X = Pos.Right(prevPageBtn) + 3,
                Y = Pos.Top(prevPageBtn),
            };
            this.Add(currentPageLbl);

            // Separator
            slash = new Label("/")
            {
                X = Pos.Right(currentPageLbl) + 1,
                Y = Pos.Top(prevPageBtn),
            };
            this.Add(slash);

            // Total pages
            totalPagesLbl = new Label("?")
            {
                X = Pos.Right(prevPageBtn) + 7,
                Y = Pos.Top(prevPageBtn),
            };
            this.Add(totalPagesLbl);

            // Next page btn
            nextPageBtn = new Button(">")
            {
                X = Pos.Right(prevPageBtn) + 11,
                Y = Pos.Bottom(frameView) + 1
            };
            nextPageBtn.Clicked += OnNextPage;
            this.Add(nextPageBtn);

            // Delete product btn
            deleteProductBtn = new Button("Delete")
            {
                X = Pos.Right(frameView) - 10,
                Y = Pos.Bottom(frameView) + 1,
            };
            deleteProductBtn.Clicked += OnDeleteProduct;
            this.Add(deleteProductBtn);

            // Edit product btn
            editProductBtn = new Button("Edit")
            {
                X = Pos.Right(frameView) - 20,
                Y = Pos.Bottom(frameView) + 1,
            };
            editProductBtn.Clicked += OnEditProduct;
            this.Add(editProductBtn);
        }

        public void SetRepository(ProductRepository productRepo)
        {
            this.productRepo = productRepo;
            this.UpdCurrentPage();
        }

        private void OnDeleteProduct()
        {
            int index = MessageBox.Query("Delete product", "Are you sure?", "No", "Yes");
            if (index == 1)
            {
                int productIndex = this.allProductsListView.SelectedItem;
                if (productIndex == -1)
                {
                    MessageBox.ErrorQuery("Delete product", "Select product at first!", "OK");
                    return;
                }

                List<Product> products = (List<Product>)this.allProductsListView.Source.ToList();
                if (productIndex >= products.Count)
                {
                    return;
                }

                Product selectedProduct = (Product)products[productIndex];
                if (selectedProduct == null)
                {
                    MessageBox.ErrorQuery("Delete product", "No product to delete!", "OK");
                    return;
                }
                if (!productRepo.DeleteById(selectedProduct.id))
                {
                    MessageBox.ErrorQuery("Delete product", "Product cannot be deleted!", "OK");
                    return;
                }
                if (page > productRepo.GetTotalPages(pageLength) && page > 1)
                {
                    page--;
                    this.UpdCurrentPage();
                }
                allProductsListView.SetSource(productRepo.GetPage(page, pageLength));
                this.UpdCurrentPage();
            }
        }

        private void OnEditProduct()
        {
            EditProductDialog dialog = new EditProductDialog();

            int productIndex = this.allProductsListView.SelectedItem;
            if (productIndex == -1)
            {
                MessageBox.ErrorQuery("Edit product", "Select product at first!", "OK");
                return;
            }
            List<Product> products = (List<Product>)this.allProductsListView.Source.ToList();
            if (productIndex >= products.Count)
            {
                return;
            }
            Product selectedProduct = (Product)products[productIndex];
            if (selectedProduct == null)
            {
                return;
            }

            dialog.SetProduct(selectedProduct);
            Application.Run(dialog);

            if (!productRepo.Update(selectedProduct.id, dialog.GetProduct()))
            {
                MessageBox.ErrorQuery("Edit product", "Product cannot be updated!");
                return;
            }

            this.UpdCurrentPage();
        }

        private void UpdCurrentPage()
        {
            this.currentPageLbl.Text = page.ToString();
            this.totalPagesLbl.Text = productRepo.GetTotalPages(pageLength).ToString();
            this.allProductsListView.SetSource(productRepo.GetPage(page, pageLength));

            this.prevPageBtn.Visible = (page > 1);
            this.nextPageBtn.Visible = (page < productRepo.GetTotalPages(pageLength));

            if (this.allProductsListView.Source.ToList().Count == 0)
            {
                this.allProductsListView.Visible = false;
                this.noProductsLbl.Visible = true;
                this.currentPageLbl.Visible = false;
                this.totalPagesLbl.Visible = false;
                this.slash.Visible = false;
                this.deleteProductBtn.Visible = false;
                this.editProductBtn.Visible = false;
            }
            else
            {
                this.allProductsListView.Visible = true;
                this.noProductsLbl.Visible = false;
                this.currentPageLbl.Visible = true;
                this.totalPagesLbl.Visible = true;
                this.slash.Visible = true;
                this.deleteProductBtn.Visible = true;
                this.editProductBtn.Visible = true;
            }
        }

        public void OnPrevPage()
        {
            if (page == 1)
            {
                return;
            }
            page--;
            this.UpdCurrentPage();
        }

        public void OnNextPage()
        {
            if (page >= productRepo.GetTotalPages(pageLength))
            {
                return;
            }
            page++;
            this.UpdCurrentPage();
        }

        public void OnCreateButtonClicked()
        {
            CreateProductDialog dialog = new CreateProductDialog();
            Application.Run(dialog);

            if (!dialog.canceled)
            {
                Product product = dialog.GetProduct();
                if (product == null)
                {
                    MessageBox.ErrorQuery("Create product", "All text fields must be filled, and price field takes only real numbers!", "Try again");
                    return;
                }
                long productId = productRepo.Insert(product);
                product.id = productId;

                allProductsListView.SetSource(productRepo.GetPage(page, pageLength));
            }
            this.UpdCurrentPage();
        }

        public void OnOpenProduct(ListViewItemEventArgs args)
        {
            Product product = (Product)args.Value;
            OpenProductDialog dialog = new OpenProductDialog();
            dialog.SetProduct(product);

            Application.Run(dialog);

            if (dialog.deleted)
            {
                bool result = productRepo.DeleteById(product.id);
                if (result)
                {
                    if (page > productRepo.GetTotalPages(pageLength) && page > 1)
                    {
                        page--;
                    }
                    allProductsListView.SetSource(productRepo.GetPage(page, pageLength));
                    this.UpdCurrentPage();
                }
                else
                {
                    MessageBox.ErrorQuery("Delete product", "Product cannot be deleted!", "OK");
                }
            }

            if (dialog.updated)
            {
                bool result = productRepo.Update(product.id, dialog.GetProduct());
                if (result)
                {
                    allProductsListView.SetSource(productRepo.GetPage(page, pageLength));
                }
                else
                {
                    MessageBox.ErrorQuery("Update product", "Product cannot be updated!", "OK");
                }
            }
        }

        public void OnQuit()
        {
            Application.RequestStop();
        }

        public void OnNewProduct()
        {
            CreateProductDialog dialog = new CreateProductDialog();
            Application.Run(dialog);

            if (!dialog.canceled)
            {
                Product product = dialog.GetProduct();
                long productId = productRepo.Insert(product);
                product.id = productId;

                allProductsListView.SetSource(productRepo.GetPage(page, pageLength));
            }
            this.UpdCurrentPage();
        }

        public void OnAbout()
        {
            MessageBox.Query("About", "This app is for products database management!\nDeveloper: Sydorenko Oleksandr", "OK");
        }
    }
}
