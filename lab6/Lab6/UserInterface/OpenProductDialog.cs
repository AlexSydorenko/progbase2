using System;
using Terminal.Gui;

namespace lab6
{
    public class OpenProductDialog : Dialog
    {
        public bool deleted;
        public bool updated;
        private TextField productNameInput;
        private TextField productDescriptionInput;
        private TextField productPriceInput;
        private Label isExistValueLbl;
        private DateField createdAtDatefield;
        protected Product product;

        public OpenProductDialog()
        {
            this.Title = "Open product";
            
            Button backBtn = new Button("Back");
            backBtn.Clicked += OnOpenProductSubmit;

            this.AddButton(backBtn);

            int inputsColumnX = 20;

            // Name
            Label productNameLbl = new Label(2, 2, "Name:");
            productNameInput = new TextField("")
            {
                X = inputsColumnX, Y = Pos.Top(productNameLbl), Width = 40,
                ReadOnly = true
            };
            this.Add(productNameLbl, productNameInput);

            // Description
            Label productDescriptionLbl = new Label(2, 4, "Description:");
            productDescriptionInput = new TextField("")
            {
                X = inputsColumnX, Y = Pos.Top(productDescriptionLbl), Width = 40,
                ReadOnly = true
            };
            this.Add(productDescriptionLbl, productDescriptionInput);

            // Price
            Label productPriceLbl = new Label(2, 6, "Price:");
            productPriceInput = new TextField("")
            {
                X = inputsColumnX, Y = Pos.Top(productPriceLbl), Width = 40,
                ReadOnly = true
            };
            this.Add(productPriceLbl, productPriceInput);

            // IsExist
            Label isExistLbl = new Label(2, 8, "In stock:");
            isExistValueLbl = new Label(".")
            {
                X = inputsColumnX, Y = Pos.Top(isExistLbl), Width = 100,
            };
            this.Add(isExistLbl, isExistValueLbl);

            // Creation date
            Label createdAtLbl = new Label(2, 10, "Creation date:");
            createdAtDatefield = new DateField()
            {
                X = inputsColumnX, Y = Pos.Top(createdAtLbl), Width = 40,
                IsShortFormat = false,
                ReadOnly = true,
            };
            this.Add(createdAtLbl, createdAtDatefield);

            // Delete button
            Button deleteProductBtn = new Button("Delete")
            {
                X = inputsColumnX + 30,
                Y = 12
            };
            deleteProductBtn.Clicked += OnDeleteProduct;
            this.Add(deleteProductBtn);

            // Edit button
            Button editProductBtn = new Button("Edit")
            {
                X = Pos.Left(deleteProductBtn) - 10,
                Y = 12
            };
            editProductBtn.Clicked += OnEditProduct;
            this.Add(editProductBtn);
        }

        public Product GetProduct()
        {
            return this.product;
        }

        public void OnEditProduct()
        {
            EditProductDialog dialog = new EditProductDialog();
            dialog.SetProduct(this.product);

            Application.Run(dialog);

            if (!dialog.canceled)
            {
                Product updatedProduct = dialog.GetProduct();
                if (updatedProduct == null)
                {
                    MessageBox.ErrorQuery("Create product", "All text fields must be filled, and price field takes only real numbers!", "Try again");
                    return;
                }
                this.updated = true;
                this.SetProduct(updatedProduct);
            }
        }

        public void OnDeleteProduct()
        {
            int index = MessageBox.Query("Delete product", "Are you sure?", "No", "Yes");
            if (index == 1)
            {
                this.deleted = true;
                Application.RequestStop();
            }
            
        }

        public void SetProduct(Product product)
        {
            this.product = product;
            this.productNameInput.Text = product.name;
            this.productDescriptionInput.Text = product.description;
            this.productPriceInput.Text = product.price.ToString();
            this.isExistValueLbl.Text = product.isExist.ToString();
            this.createdAtDatefield.Date = product.createdAt;
        }

        public void OnOpenProductSubmit()
        {
            Application.RequestStop();
        }
    }
}
