﻿using System;
using System.Collections.Generic;

namespace DLyah_Boutique_System.Models;

public class ProductModel {
    public int ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public string ProductDescription { get; set; } = null!;
    public decimal ProductPrice { get; set; }
    public int GenderId { get; set; }
    public int ProductQuantity { get; set; }
    
    public virtual ICollection<OrderItemModel> OrderItems { get; set; } = new List<OrderItemModel>();
    public virtual GenderModel Gender { get; set; } = null!;
    public virtual ICollection<ProductImageModel> ProductImages { get; set; } = new List<ProductImageModel>();
    public virtual ICollection<StockProductModel> StockProducts { get; set; } = new List<StockProductModel>();

    public virtual ICollection<ProductCategoryModel> ProductCategories { get; set; } = new List<ProductCategoryModel>();
    public virtual ICollection<ProductColorModel> ProductColors { get; set; } = new List<ProductColorModel>();
    public virtual ICollection<ProductSizeModel> ProductSizes { get; set; } = new List<ProductSizeModel>();
}