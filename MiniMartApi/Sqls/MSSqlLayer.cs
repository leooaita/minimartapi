using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniMartApi.Sqls
{
    /// <summary>
    /// 
    /// </summary>
    public static class MSSqlLayer
    {
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

    }
}
