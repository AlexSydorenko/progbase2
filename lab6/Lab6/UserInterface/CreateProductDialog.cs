using System;
using Terminal.Gui;

namespace lab6
{
    public class CreateProductDialog : Dialog
    {
        protected TextField productNameInput;
        protected TextField productDescriptionInput;
        protected TextField productPriceInput;
        protected CheckBox isExistCheckbox;
        protected DateField createdAtDatefield;
        public bool canceled;

        public CreateProductDialog()
        {
            this.Title = "Create new product";
            
            Button okBtn = new Button("OK");
            okBtn.Clicked += OnCreateDialogSubmit;

            Button cancelBtn = new Button("Cancel");
            cancelBtn.Clicked += OnCreatedDialogCanceled;

            this.AddButton(okBtn);
            this.AddButton(cancelBtn);

            int inputsColumnX = 20;

            // Name
            Label productNameLbl = new Label(2, 2, "Name:");
            productNameInput = new TextField("")
            {
                X = inputsColumnX, Y = Pos.Top(productNameLbl), Width = 40
            };
            this.Add(productNameLbl, productNameInput);

            // Description
            Label productDescriptionLbl = new Label(2, 4, "Description:");
            productDescriptionInput = new TextField("")
            {
                X = inputsColumnX, Y = Pos.Top(productDescriptionLbl), Width = 40
            };
            this.Add(productDescriptionLbl, productDescriptionInput);

            // Price
            Label productPriceLbl = new Label(2, 6, "Price:");
            productPriceInput = new TextField("")
            {
                X = inputsColumnX, Y = Pos.Top(productPriceLbl), Width = 40
            };
            this.Add(productPriceLbl, productPriceInput);

            // IsExist
            Label isExistLbl = new Label(2, 8, "In stock:");
            isExistCheckbox = new CheckBox()
            {
                X = inputsColumnX, Y = Pos.Top(isExistLbl), Width = 2
            };
            this.Add(isExistLbl, isExistCheckbox);

            // Creation date
            // Label createdAtLbl = new Label(2, 10, "Creation date:");
            // createdAtDatefield = new DateField()
            // {
            //     X = inputsColumnX, Y = Pos.Top(createdAtLbl), Width = 40,
            // };
            // this.Add(createdAtLbl, createdAtDatefield);
        }

        public Product GetProduct()
        {
            Product product = new Product();
            if (productNameInput.Text == "" || productDescriptionInput.Text == "" ||
                !double.TryParse(productPriceInput.Text.ToString(), out product.price))
            {
                return null;
            }
            return new Product()
            {
                name = productNameInput.Text.ToString(),
                description = productDescriptionInput.Text.ToString(),
                price = double.Parse(productPriceInput.Text.ToString()),
                isExist = isExistCheckbox.Checked,
                createdAt = DateTime.Now,
            };
        }

        public void OnCreatedDialogCanceled()
        {
            this.canceled = true;
            Application.RequestStop();
        }

        public void OnCreateDialogSubmit()
        {
            this.canceled = false;
            Application.RequestStop();
        }
    }
}
