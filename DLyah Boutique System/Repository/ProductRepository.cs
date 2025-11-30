using DLyah_Boutique_System.Data;
using DLyah_Boutique_System.Models;
using DLyah_Boutique_System.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DLyah_Boutique_System.Repository;

public class ProductRepository : IProductRepository
{
    private readonly BankContext _context;

    public ProductRepository(BankContext context) {
        _context = context;
    }

    public List<ProductModel> FindAll() {
        return _context.Products
            .Include(pg => pg.Gender)
            .Include(p => p.ProductCategories)
            .ThenInclude(pc => pc.Category)
            .Include(p => p.ProductColors)
            .ThenInclude(pc => pc.Color)
            .Include(p => p.ProductSizes)
            .ThenInclude(ps => ps.Size)
            .Include(p => p.ProductImages.OrderBy(pi => pi.ImageOrder))
            .Include(pstck => pstck.StockProducts)
            .ToList();
    }

    public ProductModel? FindById(int id) {
        return _context.Products
            .Include(pg => pg.Gender)
            .Include(p => p.ProductCategories)
            .ThenInclude(pc => pc.Category)
            .Include(p => p.ProductColors)
            .ThenInclude(pc => pc.Color)
            .Include(p => p.ProductSizes)
            .ThenInclude(ps => ps.Size)
            .Include(p => p.ProductImages.OrderBy(pi => pi.ImageOrder))
            .Include(p => p.StockProducts) // Inclui os itens de estoque
            .FirstOrDefault(p => p.ProductId == id);
    }

    public ProductModel Update(ProductModel product) {
        ProductModel productDb = FindById(product.ProductId);

        if (productDb == null) throw new Exception("Houve um erro ao atualizar o produto");

        productDb.ProductName = product.ProductName;
        productDb.ProductDescription = product.ProductDescription;
        productDb.ProductPrice = product.ProductPrice;
        productDb.GenderId = product.GenderId;

        productDb.ProductColors.Clear();
        if (product.ProductColors != null) {
            foreach (var pc in product.ProductColors) {
                productDb.ProductColors.Add(
                    new ProductColorModel {
                        ProductId = product.ProductId,
                        ColorId = pc.ColorId
                    }
                );
            }
        }

        // Atualizar ProductSizes
        productDb.ProductSizes.Clear();
        if (product.ProductSizes != null) {
            foreach (var ps in product.ProductSizes) {
                productDb.ProductSizes.Add(
                    new ProductSizeModel {
                        ProductId = product.ProductId,
                        SizeId = ps.SizeId
                    }
                );
            }
        }

        // Atualizar ProductCategories
        productDb.ProductCategories.Clear();
        if (product.ProductCategories != null) {
            foreach (var pc in product.ProductCategories) {
                productDb.ProductCategories.Add(
                    new ProductCategoryModel {
                        ProductId = product.ProductId,
                        CategoryId = pc.CategoryId
                    }
                );
            }
        }


        // Atualizar ProductImages (requer mais lógica dependendo de como você gerencia as imagens - adicionar, remover, etc.)
        // Este é um exemplo básico que remove todas as existentes e adiciona as novas
        productDb.ProductImages.Clear();
        if (product.ProductImages != null) {
            foreach (var pi in product.ProductImages) {
                productDb.ProductImages.Add(
                    new ProductImageModel {
                        ProductId = product.ProductId,
                        ProductImagePath = pi.ProductImagePath,
                        ImageOrder = pi.ImageOrder
                    }
                );
            }
        }

        _context.Products.Update(productDb);
        _context.SaveChanges();

        return productDb;
    }

    public ProductModel Create(ProductModel product) {
        _context.Products.Add(product);
        _context.SaveChanges();

        return product;
    }

    public ProductCategoryModel CreateProductCategory(int productId, int categoryId) {
        ProductCategoryModel productCategory = new ProductCategoryModel {
            ProductId = productId,
            CategoryId = categoryId
        };

        _context.ProductCategories.Add(productCategory);

        return productCategory;
    }

    public ProductColorModel CreateProductColor(int productId, int colorId) {
        ProductColorModel productColor = new ProductColorModel {
            ProductId = productId,
            ColorId = colorId
        };

        _context.ProductColors.Add(productColor);

        return productColor;
    }

    public ProductSizeModel CreateProductSize(int productId, int sizeId) {
        ProductSizeModel productSize = new ProductSizeModel {
            ProductId = productId,
            SizeId = sizeId
        };

        _context.ProductSizes.Add(productSize);

        return productSize;
    }

    public ProductImageModel CreateProductImage(ProductImageModel productImage) {
        _context.ProductImages.Add(productImage);

        return productImage;
    }

    public StockProductModel CreateStockProduct(StockProductModel stock) {
        _context.StockProducts.Add(stock);

        return stock;
    }

    public void Kill(int id) {
        var product = FindById(id);

        if (product != null) {
            _context.Products.Remove(product);
        }
    }

    public void UpdateProductCategories(int productId, List<int> categoryIds) {
        // Abordagem "Limpar e Recriar"
        var existingCategories = _context.ProductCategories.Where(pc => pc.ProductId == productId);
        _context.ProductCategories.RemoveRange(existingCategories);

        if (categoryIds != null && categoryIds.Any()) {
            var newCategories = categoryIds.Select(
                catId => new ProductCategoryModel {
                    ProductId = productId,
                    CategoryId = catId
                }
            );
            _context.ProductCategories.AddRange(newCategories);
        }
    }

    public void UpdateProductColors(int productId, List<int> colorIds) {
        var existingColors = _context.ProductColors.Where(pc => pc.ProductId == productId);
        _context.ProductColors.RemoveRange(existingColors);

        if (colorIds != null && colorIds.Any()) {
            var newColors = colorIds.Select(
                colorId => new ProductColorModel {
                    ProductId = productId,
                    ColorId = colorId
                }
            );
            _context.ProductColors.AddRange(newColors);
        }
    }

    public void UpdateProductSizes(int productId, List<int> sizeIds) {
        var existingSizes = _context.ProductSizes.Where(ps => ps.ProductId == productId);
        _context.ProductSizes.RemoveRange(existingSizes);

        if (sizeIds != null && sizeIds.Any()) {
            var newSizes = sizeIds.Select(
                sizeId => new ProductSizeModel {
                    ProductId = productId,
                    SizeId = sizeId
                }
            );
            _context.ProductSizes.AddRange(newSizes);
        }
    }

    public void DeleteImages(List<int> imageIds, string webRootPath) {
        if (imageIds == null || !imageIds.Any()) return;

        foreach (var imageId in imageIds) {
            var imageToDelete = _context.ProductImages.Find(imageId);
            if (imageToDelete != null) {
                // Deleta o arquivo físico do servidor
                var fullPath = Path.Combine(webRootPath, imageToDelete.ProductImagePath.TrimStart('/'));
                if (File.Exists(fullPath)) {
                    File.Delete(fullPath);
                }

                // Remove a entidade do banco de dados
                _context.ProductImages.Remove(imageToDelete);
            }
        }
    }


    public void UpdateStock(int productId, List<StockEditViewModel> stockEntries) {
        var existingStock = _context.StockProducts.Where(s => s.ProductId == productId);
        _context.StockProducts.RemoveRange(existingStock);

        if (stockEntries != null && stockEntries.Any()) {
            var newStock = stockEntries.Select(
                se => new StockProductModel {
                    ProductId = productId,
                    ColorId = se.ColorId,
                    SizeId = se.SizeId,
                    StockQuantity = se.StockQuantity
                }
            );
            _context.StockProducts.AddRange(newStock);
        }
    }

    public Task<int> SaveChanges() {
        return _context.SaveChangesAsync();
    }
}