using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniMartApi.Sqls
{
    /// <summary>
    /// Sqls statements to create and manage entities in database
    /// </summary>
    public static class MSSqlFunctions
    {
        /// <summary>
        /// Gets SQL Store Create table Sentence.
        /// </summary>
        /// <returns>String</returns>
        public static string getCreateStore()
        {
            return  @"
                DECLARE @loglines TABLE (logline VARCHAR(300));
                insert into @loglines (logline) values ('Setup Log:')
                if not exists 
                    (Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Store')
                    Begin
                        CREATE TABLE [dbo].[Store] (
                            [Id] int IDENTITY(1,1) NOT NULL,
                            [OpenedFrom] int  NOT NULL,
                            [OpenedTo] int  NOT NULL,
                            [Name] [varchar](250) NULL,
                            [Address] [varchar](250) NULL
                        );
                        insert into @loglines (logline) values ('Store table created successfully')                    
                        SET IDENTITY_INSERT [dbo].[Store] ON
				            insert into [dbo].[Store] (Id,OpenedFrom,OpenedTo,[Name],[Address]) values (1,7,24,'COCO Downtown','Silly Lane 789')
				            insert into [dbo].[Store] (Id,OpenedFrom,OpenedTo,[Name],[Address]) values (2,7,24,'COCO Bay','Bell End 0303456')
				            insert into [dbo].[Store] (Id,OpenedFrom,OpenedTo,[Name],[Address]) values (3,7,24,'COCO Mall','Eeeapp Lane 789')
				        SET IDENTITY_INSERT [dbo].[Store] OFF
                        insert into @loglines (logline) values ('Information of stores initialized correctly')                    
                    End
                    Else
                    Begin
                        insert into @loglines (logline) values ('Store table Already exists in the database')
                    End;
                    select logline from @loglines
                ";
        }
        /// <summary>
        /// Gets the create product category table sentence.
        /// </summary>
        /// <returns>String</returns>
        public static string getCreateProductCategory()
        {
            return @"
                DECLARE @loglines TABLE (logline VARCHAR(300));
                insert into @loglines (logline) values ('Setup Log:')
                if not exists 
                    (Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'ProductCategory')
                    Begin
                        CREATE TABLE [dbo].[ProductCategory] (
                            [Id] int IDENTITY(1,1) NOT NULL,
                            [Name] [varchar](max) NULL
				        );
                        insert into @loglines (logline) values ('ProductCategory table created successfully')                    
                        SET IDENTITY_INSERT [dbo].[ProductCategory] ON
				            insert into [dbo].[ProductCategory] (Id,[Name]) values (1,'Sodas')
							insert into [dbo].[ProductCategory] (Id,[Name]) values (2,'Food')
							insert into [dbo].[ProductCategory] (Id,[Name]) values (3,'Cleaning')
							insert into [dbo].[ProductCategory] (Id,[Name]) values (4,'Bathroom')
				        SET IDENTITY_INSERT [dbo].[ProductCategory] OFF
                        insert into @loglines (logline) values ('Information of product category initialized correctly')                    
                    End
                    Else
                    Begin
                        insert into @loglines (logline) values ('ProductCategory table Already exists in the database')
                    End;
                    select logline from @loglines
                ";
        }
        /// <summary>
        /// Gets the create product category table sentence
        /// </summary>
        /// <returns>String</returns>
        public static string getCreateProduct()
        {
            return @"
                DECLARE @loglines TABLE (logline VARCHAR(300));
                insert into @loglines (logline) values ('Setup Log:')
                if not exists 
                    (Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Product')
                    Begin
                        CREATE TABLE [dbo].[Product] (
                                    [Id] int IDENTITY(1,1) NOT NULL,
							        [ProductCategoryId] int NOT NULL,
                                    [Name] [varchar](Max) NULL,
							        [Price][decimal](10,2)
				        );
				        insert into @loglines (logline) values ('Product table created successfully')
                        SET IDENTITY_INSERT [dbo].[Product] ON
					        --
				            insert into [dbo].[Product] (Id,[ProductCategoryId],[Name],[Price]) values (1,1,'Cold Ice Tea',3.15)
				            insert into [dbo].[Product] (Id,[ProductCategoryId],[Name],[Price]) values (2,1,'Coffee flavoured milk',4.14)
				            insert into [dbo].[Product] (Id,[ProductCategoryId],[Name],[Price]) values (3,1,'Nuke-Cola',5.15)
					        insert into [dbo].[Product] (Id,[ProductCategoryId],[Name],[Price]) values (4,1,'Sprute',6.16)
				            insert into [dbo].[Product] (Id,[ProductCategoryId],[Name],[Price]) values (5,1,'Slurm',7.17)
				            insert into [dbo].[Product] (Id,[ProductCategoryId],[Name],[Price]) values (6,1,'Diet Slurm',8.18)
					        -- 
					        insert into [dbo].[Product] (Id,[ProductCategoryId],[Name],[Price]) values (7,2,'Salsa Cookies',3.15)
				            insert into [dbo].[Product] (Id,[ProductCategoryId],[Name],[Price]) values (8,2,'Windmill Cookies',4.14)
				            insert into [dbo].[Product] (Id,[ProductCategoryId],[Name],[Price]) values (9,2,'Garlic-o-bread 2000',5.15)
					        insert into [dbo].[Product] (Id,[ProductCategoryId],[Name],[Price]) values (10,2,'LACTEL bread',6.16)
				            insert into [dbo].[Product] (Id,[ProductCategoryId],[Name],[Price]) values (11,2,'Ravioloches x12',7.17)
				            insert into [dbo].[Product] (Id,[ProductCategoryId],[Name],[Price]) values (12,2,'Ravioloches x48',56.18)
					        insert into [dbo].[Product] (Id,[ProductCategoryId],[Name],[Price]) values (13,2,'Milanga ganga',18.18)
					        insert into [dbo].[Product] (Id,[ProductCategoryId],[Name],[Price]) values (14,2,'Milanga ganga napo',28.18)

					        insert into [dbo].[Product] (Id,[ProductCategoryId],[Name],[Price]) values (15,3,'Atlantis detergent',7.17)
				            insert into [dbo].[Product] (Id,[ProductCategoryId],[Name],[Price]) values (16,3,'Virulanita',56.18)
					        insert into [dbo].[Product] (Id,[ProductCategoryId],[Name],[Price]) values (17,3,'Sponge, Bob',18.18)
					        insert into [dbo].[Product] (Id,[ProductCategoryId],[Name],[Price]) values (18,3,'Generic mop',28.18)

					        insert into [dbo].[Product] (Id,[ProductCategoryId],[Name],[Price]) values (19,4,'Pure steel toilet paper',7.17)
				            insert into [dbo].[Product] (Id,[ProductCategoryId],[Name],[Price]) values (20,4,'Generic soap',56.18)
					        insert into [dbo].[Product] (Id,[ProductCategoryId],[Name],[Price]) values (21,4,'PANTONE shampoo',18.18)
					        insert into [dbo].[Product] (Id,[ProductCategoryId],[Name],[Price]) values (22,4,'Cabbagegate toothpaste',28.18)
                            insert into [dbo].[Product] (Id,[ProductCategoryId],[Name],[Price]) values (23,4,'Hang -yourself toothpaste',28.18)

				        SET IDENTITY_INSERT [dbo].[Product]  OFF
                        insert into @loglines (logline) values ('Information of product initialized correctly')                    
                    End
                    Else
                    Begin
                        insert into @loglines (logline) values ('Product table Already exists in the database')
                    End;
                    select logline from @loglines
                ";
        }
        /// <summary>
        /// Gets the create stock table sentence.
        /// </summary>
        /// <returns></returns>
        public static string getCreateStock()
        {
            return @"
                DECLARE @loglines TABLE (logline VARCHAR(300));
                insert into @loglines (logline) values ('Setup Log:')
                if not exists 
                    (Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Stock')
                    Begin
                        CREATE TABLE[dbo].[Stock]
                        (
                            [Id] int IDENTITY(1,1) NOT NULL,
                            [ProductId] int NOT NULL,
                            [Cant] int NOT NULL,
                            [StoreId] int not null
                        );
				        insert into @loglines (logline) values ('Stock table created successfully')
                        insert into stock select Product.Id as ProductId,10,1 as StoreId from Product
                        insert into stock select Product.Id as ProductId,10,2 as StoreId from Product
                        insert into stock select Product.Id as ProductId,10,3 as StoreId from Product
                        -- COCO Bay
                        --select stock.id,cant,p.Name from stock left join product p on stock.ProductId=p.Id where storeid = 2 and name in ('Diet Slurm','PANTONE shampoo','Pure steel toilet paper','Generic soap','Cabbagegate toothpaste')
                        update stock set cant = 0 where id in (select stock.id from stock left join product p on stock.ProductId=p.Id where storeid = 2 and name in ('Diet Slurm','PANTONE shampoo','Pure steel toilet paper','Generic soap','Cabbagegate toothpaste'))
                        -- COCO Mall
                        --select stock.id,cant,p.Name from stock left join product p on stock.ProductId=p.Id where storeid = 3 and name in ('Ravioloches x12','Ravioloches x48','Milanga ganga','Milanga ganga napo','Atlantis detergent','Virulanita','Sponge, Bob','Generic mop')
                        update stock set cant = 0 where id in (select stock.id from stock left join product p on stock.ProductId=p.Id where storeid = 3 and name in ('Ravioloches x12','Ravioloches x48','Milanga ganga','Milanga ganga napo','Atlantis detergent','Virulanita','Sponge, Bob','Generic mop'))
                        -- COCO Downtown
                        --select stock.id,cant,p.Name from stock left join product p on stock.ProductId=p.Id where storeid = 1 and name in ('Sprute','Slurm','Atlantis detergent','Virulanita','Sponge, Bob','Generic mop','Pure steel toilet paper')
                        update stock set cant = 0 where id in (select stock.id from stock left join product p on stock.ProductId=p.Id where storeid = 1 and name in ('Sprute','Slurm','Atlantis detergent','Virulanita','Sponge, Bob','Generic mop','Pure steel toilet paper'))
                        insert into @loglines (logline) values ('Information of stock initialized correctly')                    
                    End
                    Else
                    Begin
                        insert into @loglines (logline) values ('Stock table Already exists in the database')
                    End;
                    select logline from @loglines
                ";
        }
        /// <summary>
        /// Gets the create voucher table sentence.
        /// </summary>
        /// <returns></returns>
        public static string getCreateVoucher()
        {
            return @"
                DECLARE @loglines TABLE (logline VARCHAR(300));
                insert into @loglines (logline) values ('Setup Log:')
                if not exists
                    (Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Voucher_Type')
                    Begin
                        CREATE TABLE [dbo].[Voucher_Type] (
                                    [Id] int IDENTITY(1,1) NOT NULL,
                                    [Name] [varchar](250) NULL
                        );
                        SET IDENTITY_INSERT [dbo].[Voucher_Type] ON
                        insert into [dbo].[Voucher_Type](Id,[Name]) values (1,'VoucherDiscount');
                        insert into [dbo].[Voucher_Type](Id,[Name]) values (2,'VoucherDiscountPercentPerUnit');
                        insert into [dbo].[Voucher_Type](Id,[Name]) values (3,'VoucherDiscountPayTwoTakeThree');
                        SET IDENTITY_INSERT [dbo].[Voucher_Type] OFF
                        insert into @loglines (logline) values ('Voucher Type table created successfully')
                    End
                if not exists 
                    (Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'voucher_product')
                    Begin
                        create table [dbo].[voucher_product]
                        (
	                        voucherId varchar(30) not null,
	                        productId int not null
                        );
                        insert into @loglines (logline) values ('Voucher Product table created successfully')
                    end
                if not exists 
                    (Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'voucher_week_day')
                    Begin
                        create table [dbo].[voucher_week_day]
                        (
	                        voucherId varchar(30) not null,
	                        week_day int not null
                        );
                        insert into @loglines (logline) values ('Voucher Week Day table created successfully')
                    end

                if not exists 
                    (Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Voucher_Store')
                    Begin
						create table [dbo].[Voucher_Store]
						(
							VoucherId varchar(30) not null,
							StoreId int not null
						)
						-- Coco Bay
						insert into [dbo].[Voucher_Store] (VoucherId,StoreId) values ('COCO1V1F8XOG1MZZ',2);
						insert into [dbo].[Voucher_Store] (VoucherId,StoreId) values ('COCOKCUD0Z9LUKBN',2);
						-- COCO Mall
						insert into [dbo].[Voucher_Store] (VoucherId,StoreId) values ('COCOG730CNSG8ZVX',3);
						-- COCO Downtown
						insert into [dbo].[Voucher_Store] (VoucherId,StoreId) values ('COCO2O1USLC6QR22',1);
						insert into [dbo].[Voucher_Store] (VoucherId,StoreId) values ('COCO0FLEQ287CC05',1);
                        insert into @loglines (logline) values ('Voucher_Store table created successfully')
                    end
                if not exists 
                    (Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'voucher_productcategory')
                    Begin
                        create table [dbo].[voucher_productcategory]
                        (
	                        voucherId varchar(30) not null,
	                        productcategoryId int not null
                        );
                        insert into @loglines (logline) values ('Voucher Product Category table created successfully')
                end
                if not exists 
                    (Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Voucher')
                    Begin
                        create table [dbo].[voucher]  (
	                        [Id] varchar(30) NOT NULL,
	                        onUpTo int,
                            [Percent] int,
	                        valid_from_day int,
	                        valid_from_month  int,
	                        valid_from_year  int,
	                        valid_to_day  int,
	                        valid_to_month  int,
	                        valid_to_year  int,
                            valid_day_week  varchar(14),
	                        voucherType  int,
                            applyPerCantUnit int
                        );
				        insert into @loglines (logline) values ('Voucher table created successfully')
                        -- Voucher COCO1V1F8XOG1MZZ
                        INSERT INTO [dbo].[voucher]
                                   ([Id]
                                   ,[onUpTo]
		                           ,[Percent]
                                   ,[valid_from_day]
                                   ,[valid_from_month]
                                   ,[valid_from_year]
                                   ,[valid_to_day]
                                   ,[valid_to_month]
                                   ,[valid_to_year]
                                   ,[valid_day_week]
                                   ,[voucherType])
                             VALUES
                                   ('COCO1V1F8XOG1MZZ' -- Id
			                           ,0 -- upTo
			                           ,20 -- percent
			                           ,27 -- valid_from_day
			                           ,1 -- valid_from_month
			                           ,null -- valid_from_year
			                           ,13 -- valid_to_day
			                           ,2 -- valid_to_month
			                           ,null -- valid_to_year
                                       ,'4'
			                           ,1 --voucherType Discount
		                           )
                        insert into voucher_productcategory(voucherId,productcategoryId) values ('COCO1V1F8XOG1MZZ',3)
                        insert into voucher_week_day(voucherId,week_day) values ('COCO1V1F8XOG1MZZ',4)

                        -- Voucher COCOKCUD0Z9LUKBN
                        INSERT INTO [dbo].[voucher]
                                   ([Id]
                                   ,[onUpTo]
		                           ,[Percent]
                                   ,[valid_from_day]
                                   ,[valid_from_month]
                                   ,[valid_from_year]
                                   ,[valid_to_day]
                                   ,[valid_to_month]
                                   ,[valid_to_year]
                                   ,[valid_day_week]
                                   ,[voucherType])
                             VALUES
                                   ('COCOKCUD0Z9LUKBN' -- Id
			                           ,6 -- upTo
			                           ,0 -- percent
			                           ,24 -- valid_from_day
			                           ,1 -- valid_from_month
			                           ,null -- valid_from_year
			                           ,6 -- valid_to_day
			                           ,2 -- valid_to_month
			                           ,null -- valid_to_year
                                       ,''
			                           ,3 --voucherType Discount
		                           )
                        insert into [dbo].[Voucher_product] (voucherId,productId) values ('COCOKCUD0Z9LUKBN',8) -- Windmill Cookies
                        -- Voucher COCOG730CNSG8ZVX
                        INSERT INTO [dbo].[voucher]
                                   ([Id]
                                   ,[onUpTo]
		                           ,[Percent]
                                   ,[valid_from_day]
                                   ,[valid_from_month]
                                   ,[valid_from_year]
                                   ,[valid_to_day]
                                   ,[valid_to_month]
                                   ,[valid_to_year]
                                   ,[valid_day_week]
                                   ,[voucherType])
                             VALUES
                                   ('COCOG730CNSG8ZVX' -- Id
			                           ,0 -- upTo
			                           ,10 -- percent
			                           ,31 -- valid_from_day
			                           ,1 -- valid_from_month
			                           ,null -- valid_from_year
			                           ,9 -- valid_to_day
			                           ,2 -- valid_to_month
			                           ,null -- valid_to_year
                                       ,''
			                           ,1 --voucherType Discount
		                           )

                            insert into voucher_productcategory(voucherId,productcategoryId) values ('COCOG730CNSG8ZVX',4) --Bathroom
                            insert into voucher_productcategory(voucherId,productcategoryId) values ('COCOG730CNSG8ZVX',1) --Sodas
                            -- COCO2O1USLC6QR22
                            INSERT INTO [dbo].[voucher]
                                   ([Id]
                                   ,[onUpTo]
		                           ,[Percent]
                                   ,[valid_from_day]
                                   ,[valid_from_month]
                                   ,[valid_from_year]
                                   ,[valid_to_day]
                                   ,[valid_to_month]
                                   ,[valid_to_year]
                                   ,[valid_day_week]
                                   ,[voucherType]
								   ,[applyPerCantUnit]
								   )
                             VALUES
                                   ('COCO2O1USLC6QR22' -- Id
			                           ,0 -- upTo
			                           ,30 -- percent
			                           ,null -- valid_from_day
			                           ,2 -- valid_from_month
			                           ,null -- valid_from_year
			                           ,null -- valid_to_day
			                           ,2 -- valid_to_month
			                           ,null -- valid_to_year
                                       ,''
			                           ,2 --voucherType Discount
									   ,2
		                           )
                        insert into [dbo].[Voucher_product] (voucherId,productId) values ('COCO2O1USLC6QR22',5)  -- Slurm
					    insert into [dbo].[Voucher_product] (voucherId,productId) values ('COCO2O1USLC6QR22',3)  -- Nuke-Cola
					    insert into [dbo].[Voucher_product] (voucherId,productId) values ('COCO2O1USLC6QR22',6)  -- Diet Slurm

                        -- COCO0FLEQ287CC05
                        INSERT INTO [dbo].[voucher]
                                   ([Id]
                                   ,[onUpTo]
		                           ,[Percent]
                                   ,[valid_from_day]
                                   ,[valid_from_month]
                                   ,[valid_from_year]
                                   ,[valid_to_day]
                                   ,[valid_to_month]
                                   ,[valid_to_year]
                                   ,[valid_day_week]
                                   ,[voucherType]
								   ,[applyPerCantUnit]
								   )
                             VALUES
                                   ('COCO0FLEQ287CC05' -- Id
			                           ,0 -- upTo
			                           ,50 -- percent
			                           ,1 -- valid_from_day
			                           ,2 -- valid_from_month
			                           ,null -- valid_from_year
			                           ,15 -- valid_to_day
			                           ,2 -- valid_to_month
			                           ,null -- valid_to_year
                                       ,'1'
			                           ,2 --voucherType Discount
									   ,2
		                           )
                        insert into [dbo].[Voucher_product] (voucherId,productId) values ('COCO0FLEQ287CC05',23)  -- Slurm
                        insert into @loglines (logline) values ('Information of voucher initialized correctly')                    
                    End
                    Else
                    Begin
                        insert into @loglines (logline) values ('Voucher table Already exists in the database')
                    End;
                    select logline from @loglines
                ";
        }
        /// <summary>
        /// Gets the create cart table sentence.
        /// </summary>
        /// <returns></returns>
        public static string getCreateCart()
        {
            return @"
                DECLARE @loglines TABLE (logline VARCHAR(300));
                if not exists 
                    (Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Cart')
                    Begin
                        	CREATE TABLE[dbo].[Cart]
                            (
                                [Id] int IDENTITY(1,1) NOT NULL,
                                [StoreId] int NOT NULL,
                                [Created] date not null,
		                        [Owner] varchar(100),
		                        [Total] decimal(10,2),
		                        [Total_discount] decimal(10,2)
                            );
                        insert into @loglines (logline) values ('Cart table created successfully')
                    end
                if not exists 
                    (Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'CartItem')
                    Begin
                            CREATE TABLE[dbo].[CartItem]
                            (
                                [Id] int IDENTITY(1,1) NOT NULL,
                                [ProductId] int NOT NULL, 
                                [CartId] int not null,
		                        [Cant] int NOT NULL,
		                        [Total] decimal(10,2),
		                        [Total_discount] decimal(10,2),
                            );
                        insert into @loglines (logline) values ('CartItem table created successfully')
                    end
                if not exists 
                    (Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'CartVoucher')
                    Begin
                        	CREATE TABLE[dbo].[CartVoucher]
                            (
                                [Id] int IDENTITY(1,1) NOT NULL,
                                [VoucherId] varchar(30) NOT NULL, 
                                [CartId] int not null
                            );
                        insert into @loglines (logline) values ('CartVoucher table created successfully')
                    end
                select logline from @loglines
                ";
        }
        /// <summary>
        /// Gets SQL Store Create Function Sentence.
        /// </summary>
        /// <returns>String</returns>
        public static string getFunctionStore()
        {
            return @"
                    CREATE PROCEDURE[dbo].[StoreFunc](
                            @Mode           VARCHAR(10),  
		                    @Id INT = NULL,
                            @OpenedFrom     int= NULL,
                            @OpenedTo       int= NULL,
                            @Name           VARCHAR(350) = NULL,  
		                    @Address VARCHAR(150)= NULL
                    )     
                    AS
                    BEGIN
                        SET NOCOUNT ON;
                                IF(@Mode = 'GETALL')
                            BEGIN
                            SELECT
                                    Id,OpenedFrom,OpenedTo,[Name],[Address]
                                FROM
                                    Store
                            END
                            ELSE IF(@Mode = 'GETBYID')
                            BEGIN
                                SELECT
                                    Id,OpenedFrom,OpenedTo,[Name],[Address]
                                FROM
                                    Store
                                WHERE
                                    Id = @Id
                            END
                            ELSE IF(@Mode = 'EDIT')
                            BEGIN
                                IF NOT EXISTS(SELECT 1 FROM Store WHERE Id = @Id)  
                                BEGIN
                                    INSERT INTO Store(
                                            OpenedFrom, OpenedTo, [Name], [Address]
                                        )
                                        VALUES(
                                            @OpenedFrom, @OpenedTo, @Name, @Address
                                        )
                                END
                                ELSE
                                BEGIN
                                    UPDATE
                                        Store
                                    SET
                                        OpenedFrom = @OpenedFrom,OpenedTo = @OpenedTo,[Name]= @Name,[Address]= @Address
                                    WHERE
                                        Id = @Id
                                END
                            END
                            ELSE IF(@Mode= 'DELETE')
                            BEGIN
                                DELETE FROM Store WHERE Id = @Id
                            END
                    END
            ";

        }
        /// <summary>
        /// Gets SQL Product Category Create Function Sentence.
        /// </summary>
        /// <returns></returns>
        public static string getFunctionProductCategory()
        {
            return @"
                    CREATE PROCEDURE[dbo].[ProductCategoryFunc](
                            @Mode           VARCHAR(10),  
		                    @Id             INT = NULL,
                            @Name           VARCHAR(max) = NULL
                    )     
                    AS
                    BEGIN
                        SET NOCOUNT ON;
                                IF(@Mode = 'GETALL')
                            BEGIN
                            SELECT
                                    Id,[Name]
                                FROM
                                    ProductCategory
                            END
                            ELSE IF(@Mode = 'GETBYID')
                            BEGIN
                                SELECT
                                    Id,[Name]
                                FROM
                                    ProductCategory
                                WHERE
                                    Id = @Id
                            END
                            ELSE IF(@Mode = 'EDIT')
                            BEGIN
                                IF NOT EXISTS(SELECT 1 FROM ProductCategory WHERE Id = @Id)  
                                BEGIN
                                    INSERT INTO ProductCategory(
                                           [Name]
                                        )
                                        VALUES(
                                            @Name
                                        )
                                END
                                ELSE
                                BEGIN
                                    UPDATE
                                        ProductCategory
                                    SET
                                        [Name]= @Name
                                    WHERE
                                        Id = @Id
                                END
                            END
                            ELSE IF(@Mode= 'DELETE')
                            BEGIN
                                DELETE FROM ProductCategory WHERE Id = @Id
                            END
                    END
            ";
        }
        /// <summary>
        /// Gets SQL Product Category Create Function Sentence.
        /// </summary>
        /// <returns></returns>
        public static string getFunctionProduct()
        {
            return @"
                    CREATE PROCEDURE[dbo].[ProductFunc](
                            @Mode           VARCHAR(10),  
		                    @Id             INT = NULL,
                            @Name           VARCHAR(max) = NULL,
                            @ProductCategoryId INT = NULL,
							@Price          decimal(10,2)=NULL
                    )     
                    AS
                    BEGIN
                        SET NOCOUNT ON;
                                IF(@Mode = 'GETALL')
                            BEGIN
                            SELECT
                                    Product.*, ProductCategory.*
                                FROM
                                    Product join ProductCategory on Product.ProductCategoryId = ProductCategory.Id
                            END
                            ELSE IF(@Mode = 'GETBYID')
                            BEGIN
                                SELECT
                                    Product.*, ProductCategory.*
                                FROM
                                    Product join ProductCategory on Product.ProductCategoryId = ProductCategory.Id
                                WHERE
                                    Product.Id = @Id
                            END
                            ELSE IF(@Mode = 'EDIT')
                            BEGIN
                                IF NOT EXISTS(SELECT 1 FROM Product WHERE Id = @Id)  
                                BEGIN
                                    INSERT INTO Product(
                                           [Name],[Price],[ProductCategoryId]
                                        )
                                        VALUES(
                                            @Name,@Price,@ProductCategoryId
                                        )
                                END
                                ELSE
                                BEGIN
                                    UPDATE
                                        Product
                                    SET
                                        [Name]= @Name,
                                        [Price]= @Price,
                                        [ProductCategoryId]= @ProductCategoryId
                                    WHERE
                                        Id = @Id
                                END
                            END
                            ELSE IF(@Mode= 'DELETE')
                            BEGIN
                                DELETE FROM Product WHERE Id = @Id
                            END
                    END
            ";
        }
        /// <summary>
        /// Gets the function voucher.
        /// </summary>
        /// <returns></returns>
        public static string getFunctionVoucher()
        {
            return @"
                  CREATE PROCEDURE[dbo].[VoucherFunc](
                            @Mode           VARCHAR(10),  
		                    @Id             VARCHAR(30) = NULL,
                            @onUpTo INT = NULL,
							@valid_from_day INT = NULL,
							@valid_from_month  INT = NULL,
							@valid_from_year  INT = NULL,
							@valid_to_day  INT = NULL,
							@valid_to_month  INT = NULL,
							@valid_to_year  INT = NULL,
							@voucherType  INT = NULL
                    )     
                    AS
                    BEGIN
                        SET NOCOUNT ON;
                                IF(@Mode = 'GETALL')
                            BEGIN
                            SELECT
                                    Id,[onUpTo],[valid_from_day],[valid_from_month],[valid_from_year],[valid_to_day],[valid_to_month],[valid_to_year],[voucherType]
                                FROM
                                    Voucher
                            END
                            ELSE IF(@Mode = 'GETBYID')
                            BEGIN
                                SELECT
                                    Id,[onUpTo],[valid_from_day],[valid_from_month],[valid_from_year],[valid_to_day],[valid_to_month],[valid_to_year],[voucherType]
                                FROM
                                    Voucher
                                WHERE
                                    Id = @Id
                            END
                            ELSE IF(@Mode = 'EDIT')
                            BEGIN
                                IF NOT EXISTS(SELECT 1 FROM Product WHERE Id = @Id)  
                                BEGIN
                                    INSERT INTO Voucher(
                                           [onUpTo],[valid_from_day],[valid_from_month],[valid_from_year],[valid_to_day],[valid_to_month],[valid_to_year],[voucherType]
                                        )
                                        VALUES(
                                            @onUpTo,@valid_from_day,@valid_from_month,@valid_from_year,@valid_to_day,@valid_to_month,@valid_to_year,@voucherType
                                        )
                                END
                                ELSE
                                BEGIN
                                    UPDATE
                                        Voucher
                                    SET
									[onUpTo]=@onUpTo,
									[valid_from_day]=@valid_from_day,
									[valid_from_month]=@valid_from_month,
									[valid_from_year]=@valid_from_year,
									[valid_to_day]=@valid_to_day,
									[valid_to_month]=@valid_to_month,
									[valid_to_year]=@valid_to_year,
									[voucherType]=@voucherType
                                    WHERE
                                        Id = @Id
                                END
                            END
                            ELSE IF(@Mode= 'DELETE')
                            BEGIN
                                DELETE FROM Voucher WHERE Id = @Id
                            END
                    END
            ";
        }
        /// <summary>
        /// Gets the function cart.
        /// </summary>
        /// <returns></returns>
        public static string getFunctionCart()
        {
            return @"
                  CREATE PROCEDURE[dbo].[CartFunc](
                            @Mode           VARCHAR(10),  
		                    @Id             VARCHAR(30) = NULL,
                            @StoreId        int =NULL,
                            @Created        date =null,
		                    @Owner          VARCHAR(100)=NULL,
		                    @Total          decimal(10,2)=NULL,
		                    @Total_discount decimal(10,2) =NULL
                    )     
                    AS
                    BEGIN
                        SET NOCOUNT ON;
                                IF(@Mode = 'GETALL')
                            BEGIN
                            SELECT
                                    Id, [StoreId] ,[Created] ,[Owner] ,[Total] ,[Total_discount] 
                                FROM
                                    Cart
                            END
                            ELSE IF(@Mode = 'GETBYID')
                            BEGIN
                                SELECT
                                    Id, [StoreId] ,[Created] ,[Owner] ,[Total] ,[Total_discount] 
                                FROM
                                    Cart
                                WHERE
                                    Id = @Id
                            END
                            ELSE IF(@Mode = 'EDIT')
                            BEGIN
                                IF NOT EXISTS(SELECT 1 FROM Product WHERE Id = @Id)  
                                BEGIN
                                    INSERT INTO Cart(
                                           [StoreId] ,[Created] ,[Owner] ,[Total] ,[Total_discount] 
                                        )
                                        VALUES(
                                            @StoreId ,@Created ,@Owner ,@Total ,@Total_discount
                                        )
                                END
                                ELSE
                                BEGIN
                                    UPDATE
                                        Cart
                                    SET
									    [StoreId]=@StoreId,
									    [Created]=@Created,
									    [Owner]=@Owner,
									    [Total]=@Total,
									    [Total_discount]=@Total_discount
                                    WHERE
                                        Id = @Id
                                END
                            END
                            ELSE IF(@Mode= 'DELETE')
                            BEGIN
                                DELETE FROM Cart WHERE Id = @Id
                            END
                    END
            ";
        }
        /// <summary>
        /// Gets the function cart item.
        /// Manage items on product list and Update Stock.
        /// </summary>
        /// <returns></returns>
        public static string getFunctionCartItem()
        {
            return @"
                    CREATE PROCEDURE[dbo].[CartItemFunc](
                            @Mode           VARCHAR(10),  
		                    @Id             int = NULL,
                            @ProductId      int =NULL,
                            @CartId         int =null,
		                    @Cant           int =NULL,
							@result         int OUTPUT
                    )     
                    AS
					DECLARE @CantStock INT
					DECLARE @CantBefore INT
					DECLARE @CantPrev INT
                    BEGIN
						SET @result = 1
                        SET NOCOUNT ON;
                            IF(@Mode = 'EDIT')
                            BEGIN
								SET @CantStock = (select cant  from Stock where (StoreId = (Select StoreId from Cart where  Id =@CartId)) AND ProductId = @ProductId)
					            IF NOT EXISTS(SELECT 1 FROM CartItem WHERE [ProductId] = @ProductId and [CartId] =@CartId  )  
									BEGIN
										if ((NOT @CantStock IS NULL) AND (@CantStock>=@Cant)) 
											BEGIN
												INSERT INTO CartItem(
													   [ProductId] ,[CartId] ,[Cant] ,[Total]
													)
													VALUES(
														@ProductId ,@CartId ,@Cant ,(Select price* @Cant from Product where Id = @ProductId)
													)
												UPDATE STOCK SET CANT =@CantStock-@Cant WHERE (StoreId = (Select StoreId from Cart where  Id =@CartId)) AND ProductId = @ProductId
											END
											ELSE
											BEGIN
												SET @result = 5 -- Not enough
											END
									END
								 ELSE
									BEGIN
											SET @CantStock = (select cant  from Stock where (StoreId = (Select StoreId from Cart where  Id =@CartId)) AND ProductId = @ProductId) 
											SET @CantPrev = (select cant  from CartItem where CartId =@CartId AND ProductId = @ProductId) 
											if (NOT ((@CantPrev + @CantStock) < @Cant))
											BEGIN
												UPDATE
													CartItem
												SET
													[ProductId]=@ProductId ,
													[CartId]=@CartId,
													[Cant] =@Cant,
													[Total] = (Select price* @Cant from Product where Id = @ProductId)
												WHERE
													CartId = @CartId AND ProductId = @ProductId
												UPDATE 
													Stock
												SET
													[Cant] =(@CantPrev + @CantStock) - @Cant
												where 
													StoreId = (select StoreId from Cart where Id =@CartId) and
													ProductId =@ProductId
											END
											ELSE 
											BEGIN
												SET @result = 5 -- Not enough
											END
									END
							END
                            ELSE IF(@Mode= 'DELETE')
                            BEGIN
								SET @CantStock = (select cant  from Stock where (StoreId = (Select StoreId from Cart where  Id =@CartId)) AND ProductId = @ProductId) 
								SET @CantPrev = (select cant  from CartItem where CartId =@CartId AND ProductId = @ProductId) 			
                                DELETE FROM CartItem WHERE CartId = @CartId and ProductId = @ProductId
								UPDATE 
									Stock
								SET
									[Cant] =(@CantPrev + @CantStock)
								where 
									StoreId = (select StoreId from Cart where Id =@CartId) and
									ProductId =@ProductId
                            END
                        SELECT @result
						END
            ";
        }

        /// <summary>
        /// Gets the constraint.
        /// </summary>
        /// <returns>String</returns>
        public static string getConstraint()
        {
            // Creating primary key on[Id] in table 'Product'
            // Creating primary key on[Id] in table 'ProductCategory'
            // Creating foreign key on[SolicitudDestinoId] in table 'Product'
            return @"	ALTER TABLE [dbo].[Product] ADD CONSTRAINT [PK_Product] pRIMARY KEY NONCLUSTERED ([Id] ASC);
                -- Creating primary key on [Id] in table 'ProductCategory'
                ALTER TABLE [dbo].[ProductCategory]
                ADD CONSTRAINT [PK_ProductCategory]
                    PRIMARY KEY NONCLUSTERED ([Id] ASC);
                ALTER TABLE [dbo].[Product]
                ADD CONSTRAINT [FK_ProductCategoryProduct]
	                FOREIGN KEY ([ProductCategoryId])
	                REFERENCES [dbo].[ProductCategory]
		                ([Id])
	                ON DELETE NO ACTION ON UPDATE NO ACTION;
                ALTER TABLE [dbo].[Cart]
	                ADD CONSTRAINT [PK_Cart]
		                PRIMARY KEY NONCLUSTERED ([Id] ASC);
                ALTER TABLE [dbo].[CartItem]
	                ADD CONSTRAINT [PK_CartItem]
		                PRIMARY KEY NONCLUSTERED ([Id] ASC);
                ALTER TABLE [dbo].[CartVoucher]
	                ADD CONSTRAINT [PK_CartVoucher]
		                PRIMARY KEY NONCLUSTERED ([Id] ASC);
                ALTER TABLE [dbo].[Stock]
	                ADD CONSTRAINT [PK_Stock]
		                PRIMARY KEY NONCLUSTERED ([Id] ASC);
                ALTER TABLE [dbo].[Store]
	                ADD CONSTRAINT [PK_Store]
		                PRIMARY KEY NONCLUSTERED ([Id] ASC);
                ALTER TABLE [dbo].[Voucher]
	                ADD CONSTRAINT [PK_Voucher]
		                PRIMARY KEY NONCLUSTERED ([Id] ASC);
                ALTER TABLE [dbo].[Voucher_Product]
	                ADD CONSTRAINT [PK_Voucher_Product]
		                PRIMARY KEY NONCLUSTERED (voucherId,productId);
                ALTER TABLE [dbo].[Voucher_ProductCategory]
	                ADD CONSTRAINT [PK_Voucher_ProductCategory]
		                PRIMARY KEY NONCLUSTERED (voucherId,productCategoryId);
                ALTER TABLE [dbo].[Voucher_Store]
	                ADD CONSTRAINT [PK_Voucher_Store]
		                PRIMARY KEY NONCLUSTERED (VoucherId,StoreId);
                ALTER TABLE [dbo].[Voucher_Type]
	                ADD CONSTRAINT [PK_Voucher_Type]
		                PRIMARY KEY NONCLUSTERED ([Id] ASC);
                alter table [dbo].[Cart]
                ADD CONSTRAINT [FK_CartStore]
	                FOREIGN KEY ([StoreId])
	                REFERENCES [dbo].[Store]
		                ([Id])
	                ON DELETE NO ACTION ON UPDATE NO ACTION;
                alter table [dbo].[CartItem]
                ADD CONSTRAINT [FK_CartItemProduct]
	                FOREIGN KEY ([ProductId])
	                REFERENCES [dbo].[Product]
		                ([Id])
	                ON DELETE NO ACTION ON UPDATE NO ACTION;
                alter table [dbo].[CartItem]
                ADD CONSTRAINT [FK_CartItemCart]
	                FOREIGN KEY ([CartId])
	                REFERENCES [dbo].[Cart]
		                ([Id])
	                ON DELETE NO ACTION ON UPDATE NO ACTION;
                alter table [dbo].[CartVoucher]
                ADD CONSTRAINT [FK_CartVoucherVoucher]
	                FOREIGN KEY ([VoucherId])
	                REFERENCES [dbo].[Voucher]
		                ([Id])
	                ON DELETE NO ACTION ON UPDATE NO ACTION;
                alter table [dbo].[CartVoucher]
                ADD CONSTRAINT [FK_CartVoucherCart]
	                FOREIGN KEY ([CartId])
	                REFERENCES [dbo].[Cart]
		                ([Id])
	                ON DELETE NO ACTION ON UPDATE NO ACTION;
                alter table [dbo].[Stock]
                ADD CONSTRAINT [FK_StockProduct]
	                FOREIGN KEY ([ProductId])
	                REFERENCES [dbo].[Product]
		                ([Id])
	                ON DELETE NO ACTION ON UPDATE NO ACTION;
                alter table [dbo].[Stock]
                ADD CONSTRAINT [FK_StockStore]
	                FOREIGN KEY ([StoreId])
	                REFERENCES [dbo].[Store]
		                ([Id])
	                ON DELETE NO ACTION ON UPDATE NO ACTION;
                alter table [dbo].[Voucher_ProductCategory]
                ADD CONSTRAINT [FK_VoucherProductCategoryVoucher]
	                FOREIGN KEY ([VoucherId])
	                REFERENCES [dbo].[Voucher]
		                ([Id])
	                ON DELETE NO ACTION ON UPDATE NO ACTION;
                alter table [dbo].[Voucher_ProductCategory]
                ADD CONSTRAINT [FK_VoucherProductCategoryProductCategory]
	                FOREIGN KEY ([ProductCategoryId])
	                REFERENCES [dbo].[ProductCategory]
		                ([Id])
	                ON DELETE NO ACTION ON UPDATE NO ACTION;
                alter table [dbo].[Voucher_Store]
                ADD CONSTRAINT [FK_VoucherStoreStore]
	                FOREIGN KEY ([StoreId])
	                REFERENCES [dbo].[Store]
		                ([Id])
	                ON DELETE NO ACTION ON UPDATE NO ACTION;
                alter table [dbo].[Voucher_Store]
                ADD CONSTRAINT [FK_VoucherStoreVoucher]
	                FOREIGN KEY ([VoucherId])
	                REFERENCES [dbo].[Voucher]
		                ([Id])
	                ON DELETE NO ACTION ON UPDATE NO ACTION;
	
                
                alter table [dbo].[Voucher_Product]
                ADD CONSTRAINT [FK_VoucherProductProduct]
	                FOREIGN KEY ([ProductId])
	                REFERENCES [dbo].[Product]
		                ([Id])
	                ON DELETE NO ACTION ON UPDATE NO ACTION;

                alter table [dbo].[Voucher_Product]
                ADD CONSTRAINT [FK_VoucherProductVoucher]
	                FOREIGN KEY ([VoucherId])
	                REFERENCES [dbo].[Voucher]
		                ([Id])
	                ON DELETE NO ACTION ON UPDATE NO ACTION;
                
          
                alter table [dbo].[Voucher]
                ADD CONSTRAINT [FK_VoucherType]
					FOREIGN KEY ([voucherType])
					REFERENCES [dbo].[Voucher_type]
						([Id])
				ON DELETE NO ACTION ON UPDATE NO ACTION;
                    ";
        }
        public static string getQueryStore(int? Id)
        {
            string sql = "select Store.*, Stock.*, Product.*, ProductCategory.* from Store left join Stock on Store.Id = Stock.StoreId left join Product on Product.Id = Stock.ProductId left join ProductCategory on Product.ProductCategoryId = ProductCategory.Id";
            if (Id.HasValue)
            {
                return String.Format("{0} Where Store.Id={1}", sql, Id);
            }
            return sql;
        }
        public static string getQueryVoucher(string Id)
        {
            string sql = @"select Voucher.*, Product.*, ProductCategory.* from Voucher
                    left join voucher_product on voucher_product.voucherId = Voucher.Id
                    left
                        join voucher_productcategory on voucher_productcategory.voucherId = Voucher.Id
                    left
                        join Product on Product.Id = voucher_product.ProductId
                    left
                        join ProductCategory on ProductCategory.Id = voucher_productcategory.productcategoryId
            ";
            if (!String.IsNullOrEmpty(Id))
            {
                return String.Format("{0} Where Voucher.Id='{1}'", sql, Id);
            }
            return sql;
        }
        public static string getQueryCart(int? Id)
        {
            string sql = @"
                select Cart.*, CartItem.*,Product.*,ProductCategory.*, Voucher.* from Cart 
                left join CartItem on Cart.Id = CartItem.CartId
                left join Product on Product.Id = CartItem.ProductId
                left join ProductCategory on ProductCategory.Id = Product.ProductCategoryId
                left join CartVoucher on CartVoucher.CartId = Cart.Id
                left join Voucher on Voucher.Id = CartVoucher.VoucherId 
            ";
            if (Id.HasValue)
            {
                return String.Format("{0} Where Cart.Id={1}", sql, Id.Value);
            }
            return sql;
        }
    }

 
}
