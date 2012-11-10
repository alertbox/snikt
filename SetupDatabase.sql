-- Copyright (c) Alertbox.  All rights reserved.

use master
go
drop database MiniNW
go
create database MiniNW
go
use MiniNW
go

-- create tables
create table dbo.Categories(Id int primary key identity(1,1), [Name] nvarchar(max));
create table dbo.Products(Id int primary key identity(1,1), [Name] nvarchar(max), DateDiscontinued datetime null, CategoryId int not null references Categories(Id));
go

-- create stored procedures
create proc dbo.CreateCategory
    @Id int output,
    @Name varchar(max)
as
begin
	insert dbo.Categories(Name)
	values(@Name);
	--
	set @Id = @@IDENTITY;
end
go

create proc dbo.EditCategory
    @Id int,
    @Name varchar(max)
as
begin
	update dbo.Categories set 
		Name = @Name 
	where Id = @Id
end
go

create proc dbo.GetCategory
    @Id int
as
begin
    select Id, Name
    from dbo.Categories
    where @Id = Id
end
go

create proc dbo.GetProductsByCategory
    @CategoryId int
as
begin
    select Id, Name, DateDiscontinued, CategoryId
    from dbo.Products
    where @CategoryId = CategoryId
end
go

create proc dbo.GetCategoryAndProducts
    @CategoryId int
as
begin
    exec dbo.GetCategory @CategoryId;
    exec dbo.GetProductsByCategory @CategoryId;
end
go

create proc dbo.GetProductAndCategory
    @ProductId int
as
begin
    select p.Id, p.[Name] as ProductName, p.DateDiscontinued, c.Id, c.[name] as CategoryName
    from dbo.Products as p join dbo.Categories as c on p.CategoryId = c.Id
    where p.Id = @ProductId
end
go

create proc dbo.GetProduct
    @Id int
as
begin
    select Id, Name, DateDiscontinued, CategoryId
    from dbo.Products
    where Id = @Id
end
go

create proc dbo.GetAllCategoriesAndAllProducts
as
begin
	select Id, Name
    from dbo.Categories;
    select Id, Name, DateDiscontinued, CategoryId
    from dbo.Products;
end
go

create proc dbo.GetAllCategories
as
begin
	select Id, Name
    from dbo.Categories;
end
go

create proc dbo.GetAllProducts
as
begin
    select Id, Name, DateDiscontinued, CategoryId
    from dbo.Products;
end
go

-- insert data
set identity_insert dbo.Categories on;
insert into dbo.Categories(Id, [Name])
select 1, 'Beverage'
union all
select 2, 'Food';
set identity_insert dbo.Categories off;

set identity_insert dbo.Products on;
insert into dbo.Products(Id, [Name], DateDiscontinued, CategoryId)
select 1, 'Beer', null, 1
union all
select 2, 'Jam', null, 2
union all
select 3, 'Water', null, 1
union all
select 4, 'Toast', '2007-12-15', 2;
set identity_insert dbo.Products off;

select Id, Name from dbo.Categories
select Id, Name, DateDiscontinued, CategoryId from dbo.Products
exec dbo.GetProductAndCategory 1