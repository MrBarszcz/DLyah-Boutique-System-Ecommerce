using DLyah_Boutique_System.Models;
using Microsoft.EntityFrameworkCore;

namespace DLyah_Boutique_System.Data;

public class BankContext : DbContext {
    public BankContext(DbContextOptions<BankContext> options) : base(options) {
    }

    public DbSet<UserModel> Users { get; set; } = null!;
    public DbSet<ClientModel> Clients { get; set; } = null!;
    public DbSet<GenderModel> Genders { get; set; } = null!;
    public DbSet<ProductModel> Products { get; set; } = null!;
    public DbSet<SizeModel> Sizes { get; set; } = null!;
    public DbSet<ColorModel> Colors { get; set; } = null!;
    public DbSet<CategoryModel> Categories { get; set; } = null!;
    public DbSet<StockProductModel> StockProducts { get; set; } = null!;
    public DbSet<OrderModel> Orders { get; set; } = null!;
    public DbSet<PaymentModel> Payments { get; set; } = null!;
    public DbSet<OrderItemModel> OrderItems { get; set; } = null!;
    public DbSet<AddressModel> Addresses { get; set; } = null!;
    public DbSet<ProductColorModel> ProductColors { get; set; } = null!;
    public DbSet<ProductSizeModel> ProductSizes { get; set; } = null!;
    public DbSet<ProductCategoryModel> ProductCategories { get; set; } = null!;
    public DbSet<UserPaymentModel> UserPayments { get; set; } = null!;
    public DbSet<UserProfileImageModel> UserProfileImages { get; set; } = null!;
    public DbSet<ProductImageModel> ProductImages { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<UserModel>().ToTable("Users");
        modelBuilder.Entity<ClientModel>().ToTable("Clients");
        modelBuilder.Entity<GenderModel>().ToTable("Gender"); // Usar o nome correto da tabela
        modelBuilder.Entity<ProductModel>().ToTable("Products");
        modelBuilder.Entity<SizeModel>().ToTable("Sizes");
        modelBuilder.Entity<ColorModel>().ToTable("Colors");
        modelBuilder.Entity<CategoryModel>().ToTable("Categories");
        modelBuilder.Entity<OrderModel>().ToTable("Orders");
        modelBuilder.Entity<PaymentModel>().ToTable("Payments");
        modelBuilder.Entity<OrderItemModel>().ToTable("OrderItems");
        modelBuilder.Entity<AddressModel>().ToTable("Address"); // Usar o nome correto da tabela
        modelBuilder.Entity<ProductColorModel>().ToTable("ProductColors");
        modelBuilder.Entity<ProductSizeModel>().ToTable("ProductSizes");
        modelBuilder.Entity<ProductCategoryModel>().ToTable("ProductCategories");
        modelBuilder.Entity<UserPaymentModel>().ToTable("UserPayments");
        modelBuilder.Entity<UserProfileImageModel>().ToTable("UserProfileImages");
        modelBuilder.Entity<ProductImageModel>().ToTable("ProductImages");
        modelBuilder.Entity<StockProductModel>().ToTable("StockProducts");

        // Configurações de chave primária e nomes de coluna (se necessário para corresponder ao SQL)
        modelBuilder.Entity<StockProductModel>()
            .HasKey(si => si.StockId);
            
        modelBuilder.Entity<StockProductModel>().Property(sp => sp.StockId).HasColumnName("stock_prduct_id"); // Especifica o nome da coluna da chave primária

        modelBuilder.Entity<StockProductModel>()
            .Property(si => si.ProductId)
            .HasColumnName("product_id"); // Especifica o nome da coluna de ProductId

        modelBuilder.Entity<StockProductModel>()
            .HasOne(si => si.Product)
            .WithMany(p => p.StockProducts)
            .HasForeignKey(si => si.ProductId)
            .HasConstraintName("FK_StockItem_Product") // Mantenha o nome da constraint consistente
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<StockProductModel>()
            .Property(si => si.ColorId)
            .HasColumnName("color_id"); // Especifica o nome da coluna de ColorId

        modelBuilder.Entity<StockProductModel>()
            .HasOne(si => si.Color)
            .WithMany(c => c.StockProducts)
            .HasForeignKey(si => si.ColorId)
            .HasConstraintName("FK_StockItem_Color") // Mantenha o nome da constraint consistente
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<StockProductModel>()
            .Property(si => si.SizeId)
            .HasColumnName("size_id"); // Especifica o nome da coluna de SizeId

        modelBuilder.Entity<StockProductModel>()
            .HasOne(si => si.Size)
            .WithMany(s => s.StockProducts)
            .HasForeignKey(si => si.SizeId)
            .HasConstraintName("FK_StockItem_Size") // Mantenha o nome da constraint consistente
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<StockProductModel>()
            .Property(si => si.QuantityStock)
            .HasColumnName("quantity_stock"); // Especifica o nome da coluna de QuantityStock

        modelBuilder.Entity<StockProductModel>()
            .HasIndex(si => new { si.ProductId, si.ColorId, si.SizeId })
            .HasDatabaseName("UQ_StockItem_ProductColorSize") // Especifica o nome do índice único
            .IsUnique();
        
        modelBuilder.Entity<CategoryModel>().HasKey(c => c.CategoryId);
        modelBuilder.Entity<CategoryModel>().Property(c => c.CategoryId).HasColumnName("category_id")
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<CategoryModel>().Property(c => c.Category).HasColumnName("category").IsRequired()
            .HasMaxLength(100);
        modelBuilder.Entity<CategoryModel>().HasMany(c => c.ProductCategories).WithOne(pc => pc.Category)
            .HasForeignKey(pc => pc.CategoryId).HasConstraintName("FK_ProductCategory_Category");

        modelBuilder.Entity<UserProfileImageModel>().HasKey(upi => upi.UserImageId);
        modelBuilder.Entity<UserProfileImageModel>().Property(upi => upi.UserImageId).HasColumnName("user_image_id");
        modelBuilder.Entity<UserProfileImageModel>().Property(upi => upi.UserId).HasColumnName("user_id").IsRequired();
        modelBuilder.Entity<UserProfileImageModel>().Property(upi => upi.UserImagePath).HasColumnName("user_image_path")
            .HasMaxLength(255);
        modelBuilder.Entity<UserProfileImageModel>().HasOne(upi => upi.User).WithOne(u => u.UserProfileImage)
            .HasForeignKey<UserProfileImageModel>(upi => upi.UserId).HasConstraintName("FK_UserProfileImage_User");

        modelBuilder.Entity<UserPaymentModel>().HasKey(up => new { up.UserId, up.PaymentId });
        modelBuilder.Entity<UserPaymentModel>().Property(up => up.UserId).HasColumnName("user_id");
        modelBuilder.Entity<UserPaymentModel>().Property(up => up.PaymentId).HasColumnName("payment_id");
        modelBuilder.Entity<UserPaymentModel>().HasOne(up => up.User).WithMany(u => u.UserPayments)
            .HasForeignKey(up => up.UserId).HasConstraintName("FK_UserPayment_User").OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<UserPaymentModel>().HasOne(up => up.Payment).WithMany(p => p.UserPayments)
            .HasForeignKey(up => up.PaymentId).HasConstraintName("FK_UserPayment_Payment")
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<ProductImageModel>().HasKey(pi => pi.ProductImageId);
        modelBuilder.Entity<ProductImageModel>().Property(pi => pi.ProductImageId).HasColumnName("prduct_image_id");
        modelBuilder.Entity<ProductImageModel>().Property(pi => pi.ProductId).HasColumnName("product_id");
        modelBuilder.Entity<ProductImageModel>().Property(pi => pi.ProductImagePath).HasColumnName("product_image_path")
            .IsRequired().HasMaxLength(255);
        modelBuilder.Entity<ProductImageModel>().Property(pi => pi.ImageOrder).HasColumnName("ImageOrder");
        modelBuilder.Entity<ProductImageModel>().HasOne(pi => pi.Product).WithMany(p => p.ProductImages)
            .HasForeignKey(pi => pi.ProductId).HasConstraintName("FK_ProductImage_Product");

        modelBuilder.Entity<ProductSizeModel>().HasKey(ps => new { ps.ProductId, ps.SizeId });
        modelBuilder.Entity<ProductSizeModel>().Property(ps => ps.ProductId).HasColumnName("product_id");
        modelBuilder.Entity<ProductSizeModel>().Property(ps => ps.SizeId).HasColumnName("size_id");
        modelBuilder.Entity<ProductSizeModel>().HasOne(ps => ps.Product).WithMany(p => p.ProductSizes)
            .HasForeignKey(ps => ps.ProductId).HasConstraintName("FK_ProductSize_Product");
        modelBuilder.Entity<ProductSizeModel>().HasOne(ps => ps.Size).WithMany(s => s.ProductSizes)
            .HasForeignKey(ps => ps.SizeId).HasConstraintName("FK_ProductSize_Size");

        modelBuilder.Entity<ProductCategoryModel>().HasKey(pc => new { pc.ProductId, pc.CategoryId });
        modelBuilder.Entity<ProductCategoryModel>().Property(pc => pc.ProductId).HasColumnName("product_id");
        modelBuilder.Entity<ProductCategoryModel>().Property(pc => pc.CategoryId).HasColumnName("category_id");
        modelBuilder.Entity<ProductCategoryModel>().HasOne(pc => pc.Product).WithMany(p => p.ProductCategories)
            .HasForeignKey(pc => pc.ProductId).HasConstraintName("FK_ProductCategory_Product");
        modelBuilder.Entity<ProductCategoryModel>().HasOne(pc => pc.Category).WithMany(c => c.ProductCategories)
            .HasForeignKey(pc => pc.CategoryId).HasConstraintName("FK_ProductCategory_Category");

        modelBuilder.Entity<ProductColorModel>().HasKey(pc => new { pc.ProductId, pc.ColorId });
        modelBuilder.Entity<ProductColorModel>().Property(pc => pc.ProductId).HasColumnName("product_id");
        modelBuilder.Entity<ProductColorModel>().Property(pc => pc.ColorId).HasColumnName("color_id");
        modelBuilder.Entity<ProductColorModel>().HasOne(pc => pc.Product).WithMany(p => p.ProductColors)
            .HasForeignKey(pc => pc.ProductId).HasConstraintName("FK_ProductColor_Product");
        modelBuilder.Entity<ProductColorModel>().HasOne(pc => pc.Color).WithMany(c => c.ProductColors)
            .HasForeignKey(pc => pc.ColorId).HasConstraintName("FK_ProductColor_Color");

        modelBuilder.Entity<UserModel>().HasKey(u => u.UserId);
        modelBuilder.Entity<UserModel>().Property(u => u.UserId ).HasColumnName("user_id");
        modelBuilder.Entity<UserModel>().Property(u => u.UserNameComplete).HasColumnName("user_name_complete")
            .IsRequired().HasMaxLength(100);
        modelBuilder.Entity<UserModel>().Property(u => u.Username).HasColumnName("username").IsRequired()
            .HasMaxLength(100);
        modelBuilder.Entity<UserModel>().Property(u => u.UserEmail).HasColumnName("user_email").IsRequired()
            .HasMaxLength(100);
        modelBuilder.Entity<UserModel>().HasIndex(u => u.UserEmail).HasDatabaseName("IX_Users_user_email")
            .IsUnique(); // Explicitly name the index
        modelBuilder.Entity<UserModel>().Property(u => u.UserPassword).HasColumnName("user_password").IsRequired()
            .HasMaxLength(50);
        modelBuilder.Entity<UserModel>().Property(u => u.UserType).HasColumnName("user_type").IsRequired()
            .HasMaxLength(20);
        modelBuilder.Entity<UserModel>().Property(u => u.UserDateRegister).HasColumnName("user_date_register")
            .HasDefaultValueSql("GETDATE()");
        modelBuilder.Entity<UserModel>().HasOne(u => u.Client).WithOne(c => c.User)
            .HasForeignKey<ClientModel>(c => c.UserId).HasConstraintName("FK_UserID");
        modelBuilder.Entity<UserModel>().HasMany(u => u.Addresses).WithOne(a => a.User).HasForeignKey(a => a.UserId)
            .HasConstraintName("FK_Address_User");
        modelBuilder.Entity<UserModel>().HasMany(u => u.UserPayments).WithOne(up => up.User)
            .HasForeignKey(up => up.UserId).HasConstraintName("FK_UserPayment_User");
        modelBuilder.Entity<UserModel>().HasOne<UserProfileImageModel>(u => u.UserProfileImage).WithOne(upi => upi.User)
            .HasForeignKey<UserProfileImageModel>(upi => upi.UserId).HasConstraintName("FK_UserProfileImage_User");

        modelBuilder.Entity<ClientModel>().HasKey(c => c.ClientId);
        modelBuilder.Entity<ClientModel>().Property(c => c.ClientId).HasColumnName("client_id");
        modelBuilder.Entity<ClientModel>().Property(c => c.ClientCpf).HasColumnName("client_cpf").IsRequired()
            .HasMaxLength(20);
        modelBuilder.Entity<ClientModel>().HasIndex(c => c.ClientCpf).HasDatabaseName("IX_Clients_client_cpf")
            .IsUnique(); // Explicitly name the index
        modelBuilder.Entity<ClientModel>().Property(c => c.ClientPhonenumber).HasColumnName("client_phonenumber")
            .IsRequired().HasMaxLength(15);
        modelBuilder.Entity<ClientModel>().Property(c => c.ClientDateBirth).HasColumnName("client_date_birth");
        modelBuilder.Entity<ClientModel>().Property(c => c.ClientAddress).HasColumnName("client_address").IsRequired()
            .HasMaxLength(255);
        modelBuilder.Entity<ClientModel>().Property(c => c.ClientCity).HasColumnName("client_city").IsRequired()
            .HasMaxLength(100);
        modelBuilder.Entity<ClientModel>().Property(c => c.ClientState).HasColumnName("client_state").IsRequired()
            .HasMaxLength(50);
        modelBuilder.Entity<ClientModel>().Property(c => c.ClientCep).HasColumnName("client_cep").IsRequired()
            .HasMaxLength(10);
        modelBuilder.Entity<ClientModel>().Property(c => c.ClientDateRegister).HasColumnName("client_date_register")
            .HasDefaultValueSql("GETDATE()");
        modelBuilder.Entity<ClientModel>().Property(c => c.ClientStatus).HasColumnName("client_status").IsRequired()
            .HasMaxLength(20).HasDefaultValue("ativo").HasAnnotation("Relational:CheckConstraint",
                "[client_status] IN ('suspenso', 'inativo', 'ativo')");
        modelBuilder.Entity<ClientModel>().Property(c => c.UserId).HasColumnName("user_id").IsRequired();
        modelBuilder.Entity<ClientModel>().HasMany(c => c.Orders).WithOne(o => o.Client).HasForeignKey(o => o.ClientId)
            .HasConstraintName("FK_Order_Client").OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<GenderModel>().HasKey(g => g.GenderId);
            modelBuilder.Entity<GenderModel>().Property(g => g.GenderId).HasColumnName("gender_id");
        modelBuilder.Entity<GenderModel>().Property(g => g.Gender).HasColumnName("gender").IsRequired()
            .HasMaxLength(50);
        modelBuilder.Entity<GenderModel>().HasIndex(g => g.Gender).HasDatabaseName("IX_Gender_gender")
            .IsUnique(); // Explicitly name the index
        modelBuilder.Entity<GenderModel>().HasMany(g => g.Products).WithOne(p => p.Gender)
            .HasForeignKey(p => p.GenderId).HasConstraintName("FK_Product_Gender");

        modelBuilder.Entity<ProductModel>().HasKey(p => p.ProductId);
            modelBuilder.Entity<ProductModel>().Property(p => p.ProductId).HasColumnName("product_id");
        modelBuilder.Entity<ProductModel>().Property(p => p.ProductName).HasColumnName("product_name").IsRequired()
            .HasMaxLength(100);
        modelBuilder.Entity<ProductModel>().Property(p => p.ProductDescription).HasColumnName("product_description")
            .IsRequired().HasMaxLength(255);
        modelBuilder.Entity<ProductModel>().Property(p => p.ProductPrice).HasColumnName("product_price").IsRequired()
            .HasColumnType("DECIMAL(10, 2)");
        modelBuilder.Entity<ProductModel>().Property(p => p.ProductQuantity).HasColumnName("product_quantity");
        modelBuilder.Entity<ProductModel>().Property(p => p.GenderId).HasColumnName("gender_id").IsRequired();
        modelBuilder.Entity<ProductModel>().HasOne(p => p.Gender).WithMany(g => g.Products)
            .HasForeignKey(p => p.GenderId).HasConstraintName("FK_Product_Gender");
        modelBuilder.Entity<ProductModel>().HasMany(p => p.OrderItems).WithOne(oi => oi.Product)
            .HasForeignKey(oi => oi.ProductId).HasConstraintName("FK_OrderItem_Product")
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<ProductModel>().HasMany(p => p.ProductColors).WithOne(pc => pc.Product)
            .HasForeignKey(pc => pc.ProductId).HasConstraintName("FK_ProductColor_Product");
        modelBuilder.Entity<ProductModel>().HasMany(p => p.ProductSizes).WithOne(ps => ps.Product)
            .HasForeignKey(ps => ps.ProductId).HasConstraintName("FK_ProductSize_Product");
        modelBuilder.Entity<ProductModel>().HasMany(p => p.ProductCategories).WithOne(pc => pc.Product)
            .HasForeignKey(pc => pc.ProductId).HasConstraintName("FK_ProductCategory_Product");
        modelBuilder.Entity<ProductModel>().HasMany(p => p.ProductImages).WithOne(pi => pi.Product)
            .HasForeignKey(pi => pi.ProductId).HasConstraintName("FK_ProductImage_Product");

        modelBuilder.Entity<SizeModel>().HasKey(s => s.SizeId);
        modelBuilder.Entity<SizeModel>().Property(s => s.SizeId).HasColumnName("size_id");
        modelBuilder.Entity<SizeModel>().Property(s => s.Size).HasColumnName("size").IsRequired().HasMaxLength(10);
        modelBuilder.Entity<SizeModel>().HasIndex(s => s.Size).HasDatabaseName("IX_Sizes_size")
            .IsUnique(); // Explicitly name the index
        modelBuilder.Entity<SizeModel>().HasMany(s => s.ProductSizes).WithOne(ps => ps.Size)
            .HasForeignKey(ps => ps.SizeId).HasConstraintName("FK_ProductSize_Size");

        modelBuilder.Entity<ColorModel>().HasKey(c => c.ColorId);
        modelBuilder.Entity<ColorModel>().Property(c => c.ColorId).HasColumnName("color_id");
        modelBuilder.Entity<ColorModel>().Property(c => c.Color).HasColumnName("color").IsRequired().HasMaxLength(50);
        modelBuilder.Entity<ColorModel>().HasIndex(c => c.Color).HasDatabaseName("IX_Colors_color")
            .IsUnique(); // Explicitly name the index
        modelBuilder.Entity<ColorModel>().Property(c => c.HexColor).HasColumnName("hex_color").IsRequired()
            .HasMaxLength(7);
        modelBuilder.Entity<ColorModel>().HasIndex(c => c.HexColor).HasDatabaseName("IX_Colors_hex_color")
            .IsUnique(); // Explicitly name the index
        modelBuilder.Entity<ColorModel>().HasMany(c => c.ProductColors).WithOne(pc => pc.Color)
            .HasForeignKey(pc => pc.ColorId).HasConstraintName("FK_ProductColor_Color");

        modelBuilder.Entity<OrderModel>().HasKey(o => o.OrderId);
        modelBuilder.Entity<OrderModel>().Property(o => o.ClientId).HasColumnName("order_id");
        
        modelBuilder.Entity<OrderModel>().ToTable("Orders");
        modelBuilder.Entity<OrderModel>().HasKey(o => o.OrderId);
        modelBuilder.Entity<OrderModel>().Property(o => o.OrderId).HasColumnName("order_id").ValueGeneratedOnAdd();
        modelBuilder.Entity<OrderModel>().Property(o => o.ClientId).HasColumnName("client_id").IsRequired();
        modelBuilder.Entity<OrderModel>().Property(o => o.DateOrder).HasColumnName("date_order")
            .HasDefaultValueSql("GETDATE()");
        modelBuilder.Entity<OrderModel>().Property(o => o.OrderStatus).HasColumnName("order_status").IsRequired()
            .HasMaxLength(20);
        modelBuilder.Entity<OrderModel>().HasCheckConstraint("CK_OrderStatus",
            "[order_status] IN ('cancelado', 'enviado', 'pago', 'pendente')");
        modelBuilder.Entity<OrderModel>().Property(o => o.OrderValueTotal).HasColumnName("order_value_total")
            .IsRequired().HasColumnType("DECIMAL(10, 2)");
        modelBuilder.Entity<OrderModel>().Property(o => o.PaymentId).HasColumnName("payment_id");
        modelBuilder.Entity<OrderModel>().HasOne(o => o.Client).WithMany(c => c.Orders).HasForeignKey(o => o.ClientId)
            .HasConstraintName("FK_Order_Client").OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<OrderModel>().HasMany(o => o.OrderItems).WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId).HasConstraintName("FK_OrderItem_Order").OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<OrderModel>()
            .HasOne(o => o.Payment)
            .WithOne(p => p.Order)
            .HasForeignKey<OrderModel>(o => o.PaymentId) // Order tem a FK para Payment
            .HasConstraintName("FK_Order_Payment");

        modelBuilder.Entity<PaymentModel>().ToTable("Payments");
        modelBuilder.Entity<PaymentModel>().HasKey(p => p.PaymentId);
        modelBuilder.Entity<PaymentModel>().Property(p => p.PaymentId).HasColumnName("payment_id").ValueGeneratedOnAdd();
        modelBuilder.Entity<PaymentModel>().Property(p => p.OrderId).HasColumnName("order_id").IsRequired();
        modelBuilder.Entity<PaymentModel>().Property(p => p.PaymentMethod).HasColumnName("payment_method").IsRequired()
            .HasMaxLength(50);
        modelBuilder.Entity<PaymentModel>().HasCheckConstraint("CK_PaymentMethod",
            "[payment_method] IN ('Dinheiro', 'PIX', 'Boleto', 'Cartão de Crédito')");
        modelBuilder.Entity<PaymentModel>().Property(p => p.PaymentStatus).HasColumnName("payment_status").IsRequired()
            .HasMaxLength(20);
        modelBuilder.Entity<PaymentModel>().HasCheckConstraint("CK_PaymentStatus",
            "[payment_status] IN ('Estornado', 'Recusado', 'Aprovado', 'Aguardando')");
        modelBuilder.Entity<PaymentModel>().Property(p => p.PaymentValuePaid).HasColumnName("payment_value_paid")
            .IsRequired().HasColumnType("DECIMAL(10, 2)");
        modelBuilder.Entity<PaymentModel>().Property(p => p.PaymentDate).HasColumnName("payment_date")
            .HasDefaultValueSql("GETDATE()");
        modelBuilder.Entity<PaymentModel>()
            .HasOne(p => p.Order)
            .WithOne(o => o.Payment)
            .HasConstraintName("FK_Payment_Order")
            .OnDelete(DeleteBehavior.NoAction);
        
        
        modelBuilder.Entity<AddressModel>().ToTable("Address");
        modelBuilder.Entity<AddressModel>().HasKey(a => a.AddressId); // ESTA LINHA É O QUE ESTÁ FALTANDO
        modelBuilder.Entity<AddressModel>().Property(a => a.AddressId).HasColumnName("address_id")
            .ValueGeneratedOnAdd(); // Se AddressId é auto-incremento no banco
        modelBuilder.Entity<AddressModel>().Property(a => a.UserId).HasColumnName("user_id").IsRequired();
        modelBuilder.Entity<AddressModel>().Property(a => a.AddressNumber).HasColumnName("address_number").IsRequired()
            .HasMaxLength(10);
        modelBuilder.Entity<AddressModel>().Property(a => a.AddressComplement).HasColumnName("address_complement")
            .HasMaxLength(50);
        modelBuilder.Entity<AddressModel>().Property(a => a.AddressNeighborhood).HasColumnName("address_neighborhood");
        modelBuilder.Entity<AddressModel>().Property(a => a.AddressCity).HasColumnName("address_city").IsRequired()
            .HasMaxLength(100);
        modelBuilder.Entity<AddressModel>().Property(a => a.AddressState).HasColumnName("address_state").IsRequired()
            .HasMaxLength(50);
        modelBuilder.Entity<AddressModel>().Property(a => a.AddressCep).HasColumnName("address_cep").IsRequired()
            .HasMaxLength(20);
        modelBuilder.Entity<AddressModel>().Property(a => a.AddressType).HasColumnName("address_type").IsRequired()
            .HasMaxLength(10).HasDefaultValue("ENTREGA").HasAnnotation("Relational:CheckConstraint",
                "[address_type] IN ('RETIRADA', 'ENTREGA')");
        modelBuilder.Entity<AddressModel>().HasOne(a => a.User).WithMany(u => u.Addresses).HasForeignKey(a => a.UserId)
            .HasConstraintName("FK_Address_User");
        
        modelBuilder.Entity<OrderItemModel>().ToTable("OrderItems");
        modelBuilder.Entity<OrderItemModel>().HasKey(oi => new { oi.OrderId, oi.ProductId }); // Define chave primária composta
        modelBuilder.Entity<OrderItemModel>().Property(oi => oi.OrderId).HasColumnName("order_id").IsRequired();
        modelBuilder.Entity<OrderItemModel>().Property(oi => oi.ProductId).HasColumnName("product_id").IsRequired();
        modelBuilder.Entity<OrderItemModel>().Property(oi => oi.OrderQuantity).HasColumnName("quantity").IsRequired();
        modelBuilder.Entity<OrderItemModel>().Property(oi => oi.OrderPriceUnitary).HasColumnName("price_per_unit")
            .IsRequired().HasColumnType("DECIMAL(10, 2)");
        modelBuilder.Entity<OrderItemModel>().Property(oi => oi.Subtotal).HasColumnName("total_value")
            .IsRequired().HasColumnType("DECIMAL(10, 2)");
        modelBuilder.Entity<OrderItemModel>().HasOne(oi => oi.Order).WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId).HasConstraintName("FK_OrderItem_Order").OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<OrderItemModel>().HasOne(oi => oi.Product).WithMany(p => p.OrderItems)
            .HasForeignKey(oi => oi.ProductId).HasConstraintName("FK_OrderItem_Product")
            .OnDelete(DeleteBehavior.NoAction);
    }
}