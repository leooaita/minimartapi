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
        /// Gets the create product category.
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
        /// Gets the create product category.
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
                                    Id,[Name],[Price],[ProductCategoryId]
                                FROM
                                    Product
                            END
                            ELSE IF(@Mode = 'GETBYID')
                            BEGIN
                                SELECT
                                    Id,[Name],[Price],[ProductCategoryId]
                                FROM
                                    Product
                                WHERE
                                    Id = @Id
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
        /// Gets the constraint.
        /// </summary>
        /// <returns>String</returns>
        public static string getConstraint()
        {
            // Creating primary key on[Id] in table 'Product'
            // Creating primary key on[Id] in table 'ProductCategory'
            // Creating foreign key on[SolicitudDestinoId] in table 'Product'
            return @"-- Creating primary key on [Id] in table 'Product'
                    ALTER TABLE [dbo].[Product]
                    ADD CONSTRAINT [PK_Product]
                        PRIMARY KEY NONCLUSTERED ([Id] ASC);
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
    }

 
}
