# MiniMartApi

> To get started...

### Step 1
  Get MiniMartApi sources

- **Option 1**
    - 🍴 Fork this repo!

- **Option 2**
    - 👯 Clone this repo to your local machine using `https://github.com/leooaita/minimartapi.git`

### Step 2
  Create on your local SQL Express a Database named MiniMartDB and configure the MiniMartApi database connection in DefaultConnection key from appsettings.json file on root folder solution. Maybe you should not modify the file if your local instance of SQLExpress is located on your machine and its address and name is ". \\ SQLEXPRESS; initial catalog = MinimartDB" 

### Step 3

- Open visual studio solution file (MiniMartApi.sln)

### Step 4

- Compile and run

### Step 5

- Call Api rest method /api/Setup to instantiate MiniMartDb
- Example:
- curl -X GET "https://{URL}/api/Setup" -H "accept: text/plain"

---
> DataBase Diagram:

https://github.com/leooaita/minimartapi/blob/master/MiniMart%20ER%20Diagram.pdf
--- 
> We own a chain of mini marts spread around the city, called COCO.
The system should:

- Be able to setup all data from a simple GET: 
    - **curl -X GET "https://{url}:{port}/api/Setup" -H "accept: text/plain"**
- Be able to query available stores at a certain time in the day and return only those that
apply: 
    - **curl -X GET "https://{url}:{port}/api/MiniMart/GetStoresByTime?isAvailableHour=8" -H "accept: text/plain"**
- Be able to query all available products, across stores, with their total stock.
    - **curl -X GET "https://{url}:{port}/api/MiniMart/GetAvailableProductsAcrossStores" -H "accept: text/plain"**
- Be able to query if a product is available, at a certain store, and return that product's
info
    - Example: Get Sprute,  available almost one unit: **curl -X GET "https://{url}:{port}/api/MiniMart/GetProductsBy?productName=Sprute&cant=1" -H "accept: text/plain"**
- Be able to query available products for a particular store
    - Option 1: Get available products in store id 1: curl -X GET "https://{uri}:{port}:44305/api/Store/1/Products" -H "accept: text/plain"
    - Option 2: Example: Get Sprute in COCO Downtown, available almost one unit: **curl -X GET "https://{url}:{port}/api/MiniMart/GetProductsBy?productName=Sprute&storeName=COCO%20Downtown&cant=1" -H "accept: text/plain"**
- Be able to manage a simple virtual cart (add/remove from it). It cannot allow to add a
product that has NO stock
    - Create Cart with unique defined User Tag (Owner/Person) 
        - Example: Raul creates a Cart on Store with id =1; **curl -X POST "https://localhost:44305/api/MiniMart/CartFor/Raul/Store/1" -H "accept: text/plain" -d ""**
    - Add Product with unique defined User Tag (Owner/Person)
        - Example: Raul adds a one unit of product with id=1 (Cold Ice Tea) to cart,  **curl -X POST "https://localhost:44305/api/MiniMart/CartFor/Raul/AddProduct/1/Cant/1" -H "accept: text/plain" -d ""**
        - Example: Raul change 1 unit to 5 units of product with id=1 on cart, **curl -X POST "https://localhost:44305/api/MiniMart/CartFor/Raul/AddProduct/5/Cant/1" -H "accept: text/plain" -d ""**
- Be able to check the validity of a Voucher code on said virtual cart. Calculate discounts
and return both original and discounted prices
    - Example: Applying voucher COCO0FLEQ287CC05 on Raul Cart for date 1-2-2020 : **https://localhost:44305/api/MiniMart/CartFor/Raul/ApplyVoucher/COCO0FLEQ287CC05/Date/1-2-2020**

- All stores share the same products/categories with their pricing and descriptions but keep
their own stock. Every product belongs to at least one category.
We offer voucher discounts that apply to certain seasons and select products, these only
apply to a particular store and under certain conditions:
- Product is included in voucher promo (can be more than one)
- Today's date is inside the voucher's date range
- Only valid for a specific store
- Limit on number of products it applies to (for example, 20% off on GrandCookies on up
to 6 units; 10% off on Cleaning products except SuperMuscle 1l, 5% off on second unit)
Every store can manage their own profile for "personal information" (workdays and hours,
address, logo)

> SYSTEM REQUIREMENTS
From the tech side, you will need to use:
- ASP.NET Web API written in C#
- .NET (Preferably .NET Core 3.x or higher, .NET Framework 4.7.2 or higher is also
acceptable)
- Dapper (preferred) or Entity Framework as ORM
- SQL Express
- IIS Express
- Json.NET (Newtonsoft.Json)
- Use of interfaces is encouraged wherever suitable
- Unit tests using any of the options natively supported by the framework. Look for at
least 60% to 70% of coverage

---
> INITIAL LOAD
There are 3 COCO stores in the city:
- COCO Downtown
- COCO Bay
- COCO Mall
We are currently managing 22 different products, from cleaning items to beverages and
food.

## Sodas
- Cold Ice Tea
- Coffee flavoured milk
- Nuke-Cola
- Sprute
- Slurm
- Diet Slurm
Cleaning
- Atlantis detergent
- Virulanita
- Sponge, Bob
- Generic mop

## Food
- Salsa Cookies
- Windmill Cookies
- Garlic-o-bread 2000
- LACTEL bread
- Ravioloches x12
- Ravioloches x48
- Milanga ganga
- Milanga ganga napo
## Bathroom
- Pure steel toilet paper
- Generic soap
- PANTONE shampoo
- Cabbagegate toothpaste

### COCO Bay doesn't have stock of:
## Sodas
- Diet Slurm
## Bathroom
- PANTONE shampoo
- Pure steel toilet paper
- Generic soap
- Cabbagegate toothpaste

### COCO Mall doesn't have stock of:
## Food
- Ravioloches x12
- Ravioloches x48
- Milanga ganga
- Milanga ganga napo
## Cleaning
- Atlantis detergent
- Virulanita

- Sponge, Bob
- Generic mop
### COCO Downtown doesn't have stock of:
## Sodas
- Sprute
- Slurm
## Cleaning
- Atlantis detergent
- Virulanita
- Sponge, Bob
- Generic mop
Bathroom
- Pure steel toilet paper
For the rest of products and their stock, you are free to load as many as you like for them.
--- 
### REGARDING VOUCHERS
## COCO Bay has:
- COCO1V1F8XOG1MZZ: 20% off on Wednesdays and Thursdays, on Cleaning products,
from Jan 27th to Feb 13th
- COCOKCUD0Z9LUKBN: Pay 2 take 3 on "Windmill Cookies" on up to 6 units, from Jan 24th
to Feb 6th

## COCO Mall has:
- COCOG730CNSG8ZVX: 10% off on Bathroom and Sodas, from Jan 31th to Feb 9th

## COCO Downtown has:
- COCO2O1USLC6QR22: 30% off on the second unit (of the same product), on "Nuka-Cola",
"Slurm" and "Diet Slurm", for all February

- COCO0FLEQ287CC05: 50% off on the second unit (of the same product), on "Hang-
yourself toothpaste", only on Mondays, first half of February.
