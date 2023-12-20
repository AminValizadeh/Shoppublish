using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shop.Domain.Models.Product;
using Shop.Domain.ViewModels.Product;
using Shop.Domain.ViewModels.ProductCategory;
using Shop.Domain.ViewModels.ProductColor;
using Shop.Domain.ViewModels.ProductDiscount;
using Shop.Domain.ViewModels.ProductGallery;

namespace Shop.Application.Interfaces
{
    public interface IProductService
    {
        #region product

        Task<FilterProductViewModel> FilterProducts(FilterProductViewModel filter);
        Task<CreateProductResult> CreateProductInAdmin(CreateProductViewModel productViewModel);
        Task<UpdateProductViewModel?> GetProductByIdForEditInAdmin(int productId);
        Task<UpdateProductResult> EditProductinAdmin(UpdateProductViewModel productViewModel);
        Task<bool> DeleteProductByIdInAdmin(int productId);
        Task<string> GetProductTitleByProductId(int productId);
        Task<List<Product>> FilterProducstByTitle(string title);

        Task<List<ProductCardViewModel>> GetLastProductForShowHome();
        Task<ProductDetailViewModel?> GetProductForShowDetailProductById(int productId);

        #endregion

        #region product category

        Task<FilterProductCategoryViewModel> FilterProductCategoryInAdmin(FilterProductCategoryViewModel filter);
        Task CreateProductCategoryInAdmin(CreateOrEditProductCategoryViewModel createProductCategory);
        Task<CreateOrEditProductCategoryViewModel?> GetProductCategoryByIdForEditinAdmin(int productCategoryId);
        Task<bool> UpdateProductCategoryInAdmin(CreateOrEditProductCategoryViewModel createProductCategory, int productCategoryId);
        Task<bool> DeleteProductCategoryInAdmin(int productCategoryId);
        Task<ProductCategory> GetProductCategoryById(int productCategoryId);
        Task<List<SelectListItem>> GetAllProductCategories();

        Task<List<ProductCategory>> GetAllCategories();

        #endregion

        #region product discount

        Task<FilterProductDiscountViewModel> FilterProductDiscountInAdmin(FilterProductDiscountViewModel filter);
        Task<CreateProductDiscountResult> CreateProductDiscountInAdmin(CreateProductDiscountViewModel createProductDiscount);
        Task<UpdateProductDiscountViewModel?> GetProductDiscountForEditInAdmin(int productDiscountId);
        Task<UpdateProductDiscountResult> EditProductDiscountInAdmin(UpdateProductDiscountViewModel updateProductDiscount, int productDiscountId);
        Task<bool> DeleteProductDiscountInAdmin(int productDiscountId);
        Task<int?> GetProductDiscountAmount(string discountCode, int productId, int price);
        Task<int?> GetPercentageDiscount(string discountCode, int productId);

        #endregion

        #region product gallery

        Task<ProductGalleryViewModel> GetProductGalleryByProductId(int productId);
        Task<CreateProductGalleryResult> CreateProductGallery(CreateProductGalleryViewModel createProductGallery);
        Task<UpdateProductGalleryViewModel?> GetProductGalleryByIdForEdit(int productGalleryId);
        Task<UpdateProductGalleryResult> EditProductGallery(int productGalleryId, UpdateProductGalleryViewModel updateProductGallery);
        Task<bool> DeleteProductGalleryById(int productGalleryId);

        #endregion

        #region product color

        Task<ProductColorViewModel> GetProductColorsByProductId(int productId);
        Task<CreateProductColorViewModel?> GetProductForAddProductColorToProduct(int productId);
        Task<CreateProductColorResult> CreateProductColor(CreateProductColorViewModel createProductColor);
        Task<UpdateProductColorViewModel?> GetProductColorForEdit(int productColorId);
        Task<UpdateProductColorResult> UpdateProductColor(UpdateProductColorViewModel updateProductColor, int productColorId);
        Task<bool> DeleteProductColor(int productColorId, int productId);

        #endregion
    }
}
