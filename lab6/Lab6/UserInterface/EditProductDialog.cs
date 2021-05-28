namespace lab6
{
    public class EditProductDialog : CreateProductDialog
    {
        public EditProductDialog()
        {
            this.Title = "Edit product";
        }

        public void SetProduct(Product product)
        {
            this.productNameInput.Text = product.name;
            this.productDescriptionInput.Text = product.description;
            this.productPriceInput.Text = product.price.ToString();
            this.isExistCheckbox.Checked = product.isExist;
            // this.createdAtDatefield.Text = product.createdAt.ToShortDateString();
        }
    }
}
