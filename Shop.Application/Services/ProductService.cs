using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shop.Application.Extensions;
using Shop.Application.Interfaces;
using Shop.Application.Utils;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Product;
using Shop.Domain.ViewModels.Product;
using Shop.Domain.ViewModels.ProductCategory;
using Shop.Domain.ViewModels.ProductColor;
using Shop.Domain.ViewModels.ProductDiscount;
using Shop.Domain.ViewModels.ProductGallery;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Shop.Application.Services
{
    public class ProductService : IProductService
    {
        #region constructor

        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IProductDiscountRepository _productDiscountRepository;
        private readonly IProductGalleryRepository _productGalleryRepository;
        private readonly IProductColorRepository _productColorRepository;

        public ProductService(IProductRepository productRepository, IProductCategoryRepository productCategoryRepository, IProductDiscountRepository productDiscountRepository, IProductGalleryRepository productGalleryRepository, IProductColorRepository productColorRepository)
        {
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
            _productDiscountRepository = productDiscountRepository;
            _productGalleryRepository = productGalleryRepository;
            _productColorRepository = productColorRepository;
        }

        #endregion


        #region product

        public async Task<FilterProductViewModel> FilterProducts(FilterProductViewModel filter)
        {
            return await _productRepository.FilterProduct(filter);
        }

        public async Task<CreateProductResult> CreateProductInAdmin(CreateProductViewModel productViewModel)
        {



            Product product = new Product
            {
                Title = productViewModel.Title,
                Description = productViewModel.Description,
                Feature = productViewModel.Feature,
                Price = productViewModel.Price,
                ProductCategoryId = productViewModel.CategoryId
            };

            #region insert image

            if (productViewModel.ImageProduct != null && productViewModel.ImageProduct.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(productViewModel.ImageProduct.FileName);
                productViewModel.ImageProduct.AddImageToServer(
                    imageName,
                    PathExtension.ProductImageOriginServer,
                    100,
                    100,
                    PathExtension.ProductImageThumbServer,
                null
                    );
                product.ImageName = imageName;
            }
            else
            {
                return CreateProductResult.NotValidImage;
            }

            #endregion

            await _productRepository.AddProduct(product);

            return CreateProductResult.Success;
        }

        public async Task<UpdateProductViewModel?> GetProductByIdForEditInAdmin(int productId)
        {
            var product = await _productRepository.GetProductById(productId);
            if (product == null) return null;

            return new UpdateProductViewModel
            {
                ProductId = product.Id,
                Description = product.Description,
                Feature = product.Feature,
                ImageName = product.ImageName,
                Price = product.Price,
                Title = product.Title,
                CategoryId = product.ProductCategoryId
            };
        }

        public async Task<UpdateProductResult> EditProductinAdmin(UpdateProductViewModel productViewModel)
        {
            var product = await _productRepository.GetProductById(productViewModel.ProductId);
            if (product == null) return UpdateProductResult.NotFoundProduct;

            product.Title = productViewModel.Title;
            product.Description = productViewModel.Description;
            product.Feature = productViewModel.Feature;
            product.Price = productViewModel.Price;
            product.ProductCategoryId = productViewModel.CategoryId;

            #region insert image

            if (productViewModel.ImageProduct != null)
            {
                if (!productViewModel.ImageProduct.IsImage())
                    return UpdateProductResult.NotValidImage;

                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(productViewModel.ImageProduct.FileName);
                productViewModel.ImageProduct.AddImageToServer(
                    imageName,
                    PathExtension.ProductImageOriginServer,
                    100,
                    100,
                    PathExtension.ProductImageThumbServer,
                product.ImageName
                    );
                product.ImageName = imageName;
            }

            #endregion

            await _productRepository.UpdateProduct(product);
            return UpdateProductResult.Success;

        }

        public async Task<bool> DeleteProductByIdInAdmin(int productId)
        {
            var user = await _productRepository.GetProductById(productId);
            if (user == null || user.IsDelete) return false;
            user.IsDelete = true;
            await _productRepository.UpdateProduct(user);
            return true;
        }

        public async Task<string> GetProductTitleByProductId(int productId)
        {
            return await _productRepository.GetProductTitleByProductId(productId);
        }

        public async Task<List<Product>> FilterProducstByTitle(string title)
        {
            return await _productRepository.FilterProductByTitle(title);
        }

        public async Task<List<ProductCardViewModel>> GetLastProductForShowHome()
        {
            var products = await _productRepository.GetProducts(10);

            List<ProductCardViewModel> productCardsViewModel = new List<ProductCardViewModel>();
            foreach (var product in products)
            {
                productCardsViewModel.Add(new ProductCardViewModel
                {
                    ProductId = product.Id,
                    ProductPrice = product.Price,
                    ImageName = product.ImageName,
                    ProductTitle = product.Title,
                    ColorCodes = product.ProductColors.Where(p=> !p.IsDelete).Select(p=> p.ColorCode).ToList()
                });
            };
            return productCardsViewModel;
        }

        #endregion

        #region product category

        public async Task<FilterProductCategoryViewModel> FilterProductCategoryInAdmin(FilterProductCategoryViewModel filter)
        {
            return await _productCategoryRepository.FilterProductCategory(filter);
        }

        public async Task CreateProductCategoryInAdmin(CreateOrEditProductCategoryViewModel createProductCategory)
        {
            ProductCategory productCategory = new ProductCategory
            {
                Title = createProductCategory.Title,
                ParentId = createProductCategory.ParentId,
            };
            await _productCategoryRepository.AddProductCategory(productCategory);
        }

        public async Task<CreateOrEditProductCategoryViewModel?> GetProductCategoryByIdForEditinAdmin(int productCategoryId)
        {
            var productCategory = await _productCategoryRepository.GetProductCategoryById(productCategoryId);
            if (productCategory == null) return null;
            return new CreateOrEditProductCategoryViewModel
            {
                Title = productCategory.Title,
            };
        }

        public async Task<bool> UpdateProductCategoryInAdmin(CreateOrEditProductCategoryViewModel createProductCategory, int productCategoryId)
        {
            ProductCategory productCategory = await _productCategoryRepository.GetProductCategoryById(productCategoryId);
            if (productCategory == null) return false;
            productCategory.Title = createProductCategory.Title;
            await _productCategoryRepository.UpdateProductCategory(productCategory);
            return true;
        }

        public async Task<bool> DeleteProductCategoryInAdmin(int productCategoryId)
        {
            ProductCategory productCategory = await _productCategoryRepository.GetProductCategoryById(productCategoryId);
            if (productCategory == null || productCategory.IsDelete) return false;
            productCategory.IsDelete = true;
            await _productCategoryRepository.UpdateProductCategory(productCategory);
            return true;
        }

        public async Task<ProductCategory> GetProductCategoryById(int productCategoryId)
        {
            return await _productCategoryRepository.GetProductCategoryById(productCategoryId);
        }

        public async Task<List<SelectListItem>> GetAllProductCategories()
        {
            var categories = await _productCategoryRepository.GetAllProductCategory();

            var selectListItemCategories = new List<SelectListItem>();
            foreach (var category in categories)
            {
                selectListItemCategories.Add(new SelectListItem
                {
                    Text = category.Title,
                    Value = category.Id.ToString(),

                });
            }

            return selectListItemCategories;
        }

        public async Task<List<ProductCategory>> GetAllCategories()
        {
            return await _productCategoryRepository.GetAllProductCategory();
        }

        public async Task<ProductDetailViewModel?> GetProductForShowDetailProductById(int productId)
        {
            var product = await _productRepository.GetProductById(productId);
            if (product == null || product.IsDelete) return null;

            ProductDetailViewModel productDetail = new ProductDetailViewModel()
            {
                Description = product.Description,
                Feature = product.Feature,
                ImageName = product.ImageName,
                ProductId = productId,
                ProductPrice = product.Price,
                ProductTitle = product.Title,
                ProductColors = product.ProductColors.Where(s => !s.IsDelete).ToList(),
                Gallery = product.ProductGalleries.Where(s => !s.IsDelete).Select(s => s.ImageName).ToList()
            };

            return productDetail;
        }

        #endregion

        #region product discount

        public async Task<FilterProductDiscountViewModel> FilterProductDiscountInAdmin(FilterProductDiscountViewModel filter)
        {
            return await _productDiscountRepository.FilterProductDiscount(filter);
        }

        public async Task<CreateProductDiscountResult> CreateProductDiscountInAdmin(CreateProductDiscountViewModel createProductDiscount)
        {
            var productDiscount = await _productDiscountRepository.GetProductDiscountByDiscountCode(createProductDiscount.DiscountCode);
            if (productDiscount != null && !productDiscount.IsDelete) return CreateProductDiscountResult.ExistDiscount;

            var newProductDiscount = new ProductDiscount
            {
                DiscountCode = createProductDiscount.DiscountCode,
                Percentage = createProductDiscount.Percentage,
                StartTime = createProductDiscount.StartTime.ToMiladi(),
                EndTime = createProductDiscount.EndTime.ToMiladi(),
                ProductId = createProductDiscount.ProductId,
            };
            await _productDiscountRepository.AddProductDiscount(newProductDiscount);
            return CreateProductDiscountResult.Success;
        }

        public async Task<UpdateProductDiscountViewModel?> GetProductDiscountForEditInAdmin(int productDiscountId)
        {
            var productDiscount = await _productDiscountRepository.GetProductDiscountById(productDiscountId);
            if (productDiscount == null || productDiscount.IsDelete) return null;
            return new UpdateProductDiscountViewModel
            {
                StartTime = productDiscount.StartTime.ToShamsiDateTime(),
                EndTime = productDiscount.EndTime.ToShamsiDateTime(),
                DiscountCode = productDiscount.DiscountCode,
                Percentage = productDiscount.Percentage,
                ProductId = productDiscount.ProductId,
                ProductTitle = await GetProductTitleByProductId(productDiscount.ProductId),
            };
        }

        public async Task<UpdateProductDiscountResult> EditProductDiscountInAdmin(UpdateProductDiscountViewModel updateProductDiscount, int productDiscountId)
        {
            var productDiscount = await _productDiscountRepository.GetProductDiscountById(productDiscountId);
            if (productDiscount == null || productDiscount.IsDelete) return UpdateProductDiscountResult.NotFoundDiscount;

            var productDiscountWithDiscountCode = await _productDiscountRepository.GetProductDiscountByDiscountCode(updateProductDiscount.DiscountCode);
            if (productDiscountWithDiscountCode != null && productDiscountWithDiscountCode.Id != productDiscountId)
                return UpdateProductDiscountResult.ExistDiscount;

            productDiscount.ProductId = updateProductDiscount.ProductId;
            productDiscount.DiscountCode = updateProductDiscount.DiscountCode;
            productDiscount.Percentage = updateProductDiscount.Percentage;
            productDiscount.StartTime = updateProductDiscount.StartTime.ToMiladi();
            productDiscount.EndTime = updateProductDiscount.EndTime.ToMiladi();

            await _productDiscountRepository.UpdateProductDiscount(productDiscount);
            return UpdateProductDiscountResult.Success;
        }

        public async Task<bool> DeleteProductDiscountInAdmin(int productDiscountId)
        {
            var productDiscount = await _productDiscountRepository.GetProductDiscountById(productDiscountId);
            if (productDiscount == null || productDiscount.IsDelete) return false;
            productDiscount.IsDelete = true;
            await _productDiscountRepository.UpdateProductDiscount(productDiscount);
            return true;
        }

        public async Task<int?> GetProductDiscountAmount(string discountCode, int productId, int price)
        {
            var productDiscount = await _productDiscountRepository.GetProductDiscountByDiscountCodeAndProductId(discountCode, productId);

            if (productDiscount != null)
            {
                return (int)Math.Ceiling(price * productDiscount.Percentage / (decimal)100);
            }

            return null;
        }

        public async Task<int?> GetPercentageDiscount(string discountCode, int productId)
        {
            return await _productDiscountRepository.GetPercentageDiscountByDiscountCodeAndProductId(discountCode, productId);
        }

        #endregion

        #region product gallery

        public async Task<ProductGalleryViewModel> GetProductGalleryByProductId(int productId)
        {
            var productGallery = await _productGalleryRepository.GetProductGalleryByProductId(productId);

            ProductGalleryViewModel galleryViewModels = new ProductGalleryViewModel();
            galleryViewModels.ProductTitle = await GetProductTitleByProductId(productId);
            galleryViewModels.ProductGalleries = productGallery;

            return galleryViewModels;
        }

        public async Task<CreateProductGalleryResult> CreateProductGallery(CreateProductGalleryViewModel createProductGallery)
        {
            var isExistProduct = await _productRepository.IsExistProductById(createProductGallery.ProductId);
            if (!isExistProduct) return CreateProductGalleryResult.NotFoundProduct;

            ProductGallery productGallery = new ProductGallery
            {
                ProductId = createProductGallery.ProductId,
            };

            #region insert image

            if (createProductGallery.ProductImage != null && createProductGallery.ProductImage.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(createProductGallery.ProductImage.FileName);
                createProductGallery.ProductImage.AddImageToServer(
                    imageName,
                    PathExtension.ProductImageOriginServer,
                    100,
                    100,
                    PathExtension.ProductImageThumbServer,
                null
                    );
                productGallery.ImageName = imageName;
            }
            else
            {
                return CreateProductGalleryResult.NotValidImage;
            }

            #endregion

            await _productGalleryRepository.AddProductGallery(productGallery);
            return CreateProductGalleryResult.Success;
        }

        public async Task<UpdateProductGalleryViewModel?> GetProductGalleryByIdForEdit(int productGalleryId)
        {
            var productGallery = await _productGalleryRepository.GetProductGalleryById(productGalleryId);
            if (productGallery == null) return null;

            return new UpdateProductGalleryViewModel
            {
                ProductId = productGallery.ProductId,
                ProductTitle = await _productRepository.GetProductTitleByProductId(productGallery.Id),
                ProductImageName = productGallery.ImageName,
            };
        }

        public async Task<UpdateProductGalleryResult> EditProductGallery(int productGalleryId, UpdateProductGalleryViewModel updateProductGallery)
        {
            var productGallery = await _productGalleryRepository.GetProductGalleryById(productGalleryId);
            if (productGallery == null) return UpdateProductGalleryResult.NotFoundProductGallery;

            #region insert image

            if (updateProductGallery.ProductImage != null && updateProductGallery.ProductImage.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(updateProductGallery.ProductImage.FileName);
                updateProductGallery.ProductImage.AddImageToServer(
                    imageName,
                    PathExtension.ProductImageOriginServer,
                    100,
                    100,
                    PathExtension.ProductImageThumbServer,
                updateProductGallery.ProductImageName
                    );
                productGallery.ImageName = imageName;
            }
            else
            {
                return UpdateProductGalleryResult.NotValidImage;
            }

            #endregion

            await _productGalleryRepository.UpdateProductGallery(productGallery);
            return UpdateProductGalleryResult.Success;

        }

        public async Task<bool> DeleteProductGalleryById(int productGalleryId)
        {
            var productGallery = await _productGalleryRepository.GetProductGalleryById(productGalleryId);
            if (productGallery == null || productGallery.IsDelete) return false;
            productGallery.IsDelete = true;
            await _productGalleryRepository.UpdateProductGallery(productGallery);

            return true;
        }
        #endregion

        #region product color

        public async Task<ProductColorViewModel> GetProductColorsByProductId(int productId)
        {
            var productColor = await _productColorRepository.GetAllProductColorBtProductId(productId);
            var productTitle = await GetProductTitleByProductId(productId);

            return new ProductColorViewModel
            {
                ProductTitle = productTitle,
                ProductColors = productColor
            };
        }

        public async Task<CreateProductColorViewModel?> GetProductForAddProductColorToProduct(int productId)
        {
            var product = await _productRepository.GetProductById(productId);
            if (product == null || product.IsDelete) return null;
            return new CreateProductColorViewModel
            {
                ProductId = productId,
                ProductTitle = product.Title,
            };
        }

        public async Task<CreateProductColorResult> CreateProductColor(CreateProductColorViewModel createProductColor)
        {
            var product = await _productRepository.GetProductById(createProductColor.ProductId);
            if (product == null || product.IsDelete) return CreateProductColorResult.NotFoundProduct;
            if (await _productColorRepository.IsExistProductColorByColorNameAndProductId(createProductColor.ColorName, createProductColor.ProductId))
                return CreateProductColorResult.ExistProductColorForProduct;

            ProductColor productColor = new ProductColor
            {
                ColorName = createProductColor.ColorName,
                ColorCode = createProductColor.ColorCode,
                Count = createProductColor.Count,
                ProductId = createProductColor.ProductId,

            };
            await _productColorRepository.AddProductColor(productColor);
            return CreateProductColorResult.Success;
        }

        public async Task<UpdateProductColorViewModel?> GetProductColorForEdit(int productColorId)
        {
            var productColor = await _productColorRepository.GetProductColorById(productColorId);
            if (productColor == null || productColor.IsDelete) return null;
            var productTitle = await _productRepository.GetProductTitleByProductId(productColor.ProductId);
            return new UpdateProductColorViewModel
            {
                ProductId = productColor.ProductId,
                ColorCode = productColor.ColorCode,
                ColorName = productColor.ColorName,
                ProductTitle = productTitle,
                Count = productColor.Count
            };
        }

        public async Task<UpdateProductColorResult> UpdateProductColor(UpdateProductColorViewModel updateProductColor, int productColorId)
        {
            var productColor = await _productColorRepository.GetProductColorById(productColorId);
            if (productColor == null || productColor.IsDelete) return UpdateProductColorResult.NotFoundProductColor;

            if (await _productColorRepository.IsExistProductColorByColorNameAndProductId(updateProductColor.ColorName, updateProductColor.ProductId))
            {
                var productColorExist = await _productColorRepository.GetProductColorByColorNameAndProductId(updateProductColor.ColorName, updateProductColor.ProductId);
                if (productColorExist.Id != productColor.Id)
                    return UpdateProductColorResult.ExistProductColorForProduct;
            }


            productColor.ColorName = updateProductColor.ColorName;
            productColor.ColorCode = updateProductColor.ColorCode;
            productColor.Count = updateProductColor.Count;
            await _productColorRepository.UpdateProductColor(productColor);
            return UpdateProductColorResult.Success;
        }

        public async Task<bool> DeleteProductColor(int productColorId, int productId)
        {
            var productColor = await _productColorRepository.GetProductColorById(productColorId);
            if (productColor == null || productColor.IsDelete || productColor.ProductId != productId) return false;

            productColor.IsDelete = true;
            await _productColorRepository.UpdateProductColor(productColor);
            return true;
        }

        #endregion
    }
}
