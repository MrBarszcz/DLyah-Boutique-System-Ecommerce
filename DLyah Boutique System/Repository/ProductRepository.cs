using DLyah_Boutique_System.Data;
using DLyah_Boutique_System.Models;
using Microsoft.EntityFrameworkCore;

namespace DLyah_Boutique_System.Repository;

public class ProductRepository : IProductRepository {
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
            .Include(p => p.ProductImages)
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
            .Include(p => p.ProductImages)
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
                productDb.ProductColors.Add(new ProductColorModel
                    { ProductId = product.ProductId, ColorId = pc.ColorId });
            }
        }

        // Atualizar ProductSizes
        productDb.ProductSizes.Clear();
        if (product.ProductSizes != null) {
            foreach (var ps in product.ProductSizes) {
                productDb.ProductSizes.Add(new ProductSizeModel { ProductId = product.ProductId, SizeId = ps.SizeId });
            }
        }

        // Atualizar ProductCategories
        productDb.ProductCategories.Clear();
        if (product.ProductCategories != null) {
            foreach (var pc in product.ProductCategories) {
                productDb.ProductCategories.Add(new ProductCategoryModel
                    { ProductId = product.ProductId, CategoryId = pc.CategoryId });
            }
        }

        // Atualizar ProductImages (requer mais lógica dependendo de como você gerencia as imagens - adicionar, remover, etc.)
        // Este é um exemplo básico que remove todas as existentes e adiciona as novas
        productDb.ProductImages.Clear();
        if (product.ProductImages != null) {
            foreach (var pi in product.ProductImages) {
                productDb.ProductImages.Add(new ProductImageModel {
                    ProductId = product.ProductId, ProductImagePath = pi.ProductImagePath, ImageOrder = pi.ImageOrder
                });
            }
        }

        _context.Products.Update(productDb);
        _context.SaveChanges();

        return productDb;
    }

    public ProductModel Create(ProductModel product) {
        _context.Products.Add(product);
        _context.SaveChanges();

        if (product.ProductColors != null && product.ProductColors.Any()) {
            foreach (var colorId in product.ProductColors) {
                _context.ProductColors.Add(new ProductColorModel {
                    ProductId = product.ProductId,
                    ColorId = colorId.ColorId
                });
            }
        }

        if (product.ProductSizes != null && product.ProductSizes.Any()) {
            foreach (var sizeId in product.ProductSizes) {
                _context.ProductSizes.Add(new ProductSizeModel {
                    ProductId = product.ProductId,
                    SizeId = sizeId.SizeId
                });
            }
        }
        
        if (product.ProductCategories != null) {
            foreach (var categoryId in product.ProductCategories) {
                _context.ProductCategories.Add(new ProductCategoryModel {
                    ProductId = product.ProductId,
                    CategoryId = categoryId.CategoryId
                });
            }
        }
        
        if (product.ProductImages != null) {
            foreach (var pi in product.ProductImages) {
               
            }
        }

        return product;
    }

    public ProductModel Kill(ProductModel product) {
        throw new NotImplementedException();
    }
}