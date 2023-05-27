using Market.Entities.Dto;
using Market.Repositories.Interfaces;
using Market.Repositories.Repositories.PostgresqlRepositories;
using static System.Net.WebRequestMethods;

namespace TestApp
{
    internal class Program
    {
        private static IDataBaseManager manager;
        static void Main(string[] args)
        {
            //DataBaseCreater creater = new DataBaseCreater();
            //creater.Create();
             
            ////CreateCategories();

            //try
            //{
            //    CreateCategories().GetAwaiter().GetResult();
            //}
            //catch (Exception ex) {
            //    Console.WriteLine("error " + ex.Message);

            //}

            
        }


        private static void AddCharectiristic(int productId) {
            manager.ProductsRepository.AddCharectiristic(new ProductCharacteristicType()
            {
                ProductId = productId,
                Name = "Экран ноутбука",
                Charastitics = new List<Charastitic> {
                    new Charastitic(){
                      Name = "Бренд",
                      Text = "DIGMA"
                    },
                    new Charastitic(){
                       Name = "Модель",
                      Text = "15 P417"
                    },
                    new Charastitic(){
                       Name = "Диагональ экрана в дюймах",
                      Text = "15.6"
                    },
               }
            }).GetAwaiter().GetResult();
        }

        private static void AddProduct()
        {
            for (int i = 0; i < 40; i++)
            {

                manager.ProductsRepository.AddAsync(new Market.Entities.Dto.ProductDto()
                {
                    Brend = $"Brend{i}",
                    SubCategoryid = 3,
                    Description = $"Description{i}",
                    Image = "https://www.citilink.ru/product/kabel-buro-usb-tc-1-2b2a-usb-a-m-usb-type-c-m-1-2m-chernyi-1478082/",
                    Name = $"дорогой комп{i}",
                    Price = new Random().Next(100000, 200000),
                    Quantity = new Random().Next(1, 100),
                }).GetAwaiter().GetResult(); ;
            }

        }
        private static async Task CreateCategories()
        {
            List<string> sucategirs = new List<string>();
            sucategirs.Add("Недорогие");
            sucategirs.Add("Дорогие");
            sucategirs.Add("Микрокомпьютеры");
            sucategirs.Add("Игровые");
            sucategirs.Add("так себе компы");
            sucategirs.Add("Полный шлак");

            Dictionary<string, List<string>> categories = new Dictionary<string, List<string>>();
            categories.Add("Компы", sucategirs);

           await CreateCategories(categories);

        }
        private static async Task CreateCategories(Dictionary<string, List<string>> categories)
        {
            foreach (var t in categories)
            {
                var CategoryId = await manager.CategoriesRepository.AddCategoryAsync(new Market.Entities.Dto.CategoryDto()
                {
                    CategoryName = t.Key,
                    Description = "Description",
                    CategoryIconUrl = "https://cdn.citilink.ru/magjqha5wz4kARnf2OGpTvOoT6StVnkREEVlbQOxEHM/resizing_type:fit/gravity:sm/width:1200/height:1200/plain/items/1478082_v01_b.jpg"
                });
                foreach (var subcategory in t.Value)
                {
                    var SubcategoryId = await manager.SubcategoryRepository.AddAsync(new Market.Entities.Dto.SubCategory()
                    {
                        CategoryId = CategoryId,
                        SubCategoryName = subcategory,
                        SubCategoryUrlIcon = "https://imdiz.ru/files/store/img/icons_catalog/desktops.png"
                    });


                    for (int i = 0; i < 40; i++)
                    {

                       var id =  manager.ProductsRepository.AddAsync(new Market.Entities.Dto.ProductDto()
                        {
                            Brend = $"Brend{i}",
                            SubCategoryid = SubcategoryId,
                            Description = $"Description{i}",
                            Image = "https://cdn.citilink.ru/magjqha5wz4kARnf2OGpTvOoT6StVnkREEVlbQOxEHM/resizing_type:fit/gravity:sm/width:1200/height:1200/plain/items/1478082_v01_b.jpg",
                            Name = $"дорогой комп{i}",
                            Price = new Random().Next(100000, 200000),
                            Quantity = new Random().Next(1, 100),
                        }).GetAwaiter().GetResult(); ;
                        AddCharectiristic(id);
                    }
                   
                }

            }

        }
    }
}