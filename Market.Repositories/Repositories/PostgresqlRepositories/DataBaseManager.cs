using DataBaseLib.Interfaces;
using Market.Entities.Dto;
using Market.Repositories.Interfaces;

namespace Market.Repositories.Repositories.PostgresqlRepositories
{
    public class DataBaseManager : IDataBaseManager
    {
        public ICategoriesRepository CategoriesRepository { get; private set; }

        public ISubcategoryRepository SubcategoryRepository { get; private set; }

        public IProductsRepository ProductsRepository { get; private set; }

        public ICommentsRepository CommentsRepository { get; private set; }
        public IUsersRepository UsersRepository { get; private set; }

        public ICommentsLikesRepository CommentsLikesRepository { get; private set; }

        public ITypeСharacteristicsRepository TypeСharacteristicsRepository { get; private set; }

        public IСharacteristicsRepository СharacteristicsRepository { get; private set; }

        private LoggerLib.Interfaces.ILogger _logger;
        public DataBaseManager(IDbProvider connection, LoggerLib.Interfaces.ILogger logger)
        {
            _logger = logger;
            CategoriesRepository = new CategoriesRepository.Repository(connection);
            SubcategoryRepository = new SubcategoryRepository.Repository(connection);
            СharacteristicsRepository = new СharacteristicsRepository.Repository(connection);
            TypeСharacteristicsRepository = new TypeСharacteristicsRepository.Repository(connection);
            ProductsRepository = new ProductsRepository.Repository(connection, TypeСharacteristicsRepository, СharacteristicsRepository);
            UsersRepository = new UsersRepository.Repository(connection);
            CommentsRepository = new CommentsRepository.Repository(connection, UsersRepository);
            CommentsLikesRepository = new CommentsLikesRepository.Repository(connection, UsersRepository);
        }

        public async Task AddTestData()
        {
            if ((await CategoriesRepository.GetCount()) != 0) return;


            List<string> sucategirs = new List<string>();
            sucategirs.Add("Недорогие");
            sucategirs.Add("Дорогие");
            sucategirs.Add("Микрокомпьютеры");
            sucategirs.Add("Игровые");


            Dictionary<string, List<string>> categories = new Dictionary<string, List<string>>();
            categories.Add("компьютеры", sucategirs);

            await CreateCategories(categories);

        }
         

        private async Task CreateCategories(Dictionary<string, List<string>> categories)
        {
            foreach (var t in categories)
            {
                List<string> urls = new List<string>
                {
                    "https://cdn.citilink.ru/lmOlbDE8Y765hxGtJl23QbDhVuuiGBJBrT81ib8D45k/resizing_type:fit/gravity:sm/width:1200/height:1200/plain/items/1859665_v01_b.jpg",
                    "https://cdn.citilink.ru/o61XYzzwAA5xnk75ip6NaUOetsdbJ_bHI7LAbV7wiwQ/resizing_type:fit/gravity:sm/width:1200/height:1200/plain/items/1856653_v01_b.jpg",
                    "https://cdn.citilink.ru/YFnwtlCFnjO6jp8M572GgZNTQ2H6CfLVagWXPLmGD3s/resizing_type:fit/gravity:sm/width:1200/height:1200/plain/items/1863687_v01_b.jpg",
                    "https://cdn.citilink.ru/y3kQwyUmX1pIZoASUgt_rKhJxrcUxUCz-vfXWYTknaY/resizing_type:fit/gravity:sm/width:1200/height:1200/plain/items/1549847_v01_b.jpg"
                };
                var CategoryId = await CategoriesRepository.AddCategoryAsync(new Market.Entities.Dto.CategoryDto()
                {
                    CategoryName = t.Key,
                    Description = "Description",
                    CategoryIconUrl = "https://cdn.citilink.ru/magjqha5wz4kARnf2OGpTvOoT6StVnkREEVlbQOxEHM/resizing_type:fit/gravity:sm/width:1200/height:1200/plain/items/1478082_v01_b.jpg"
                });
                foreach (var subcategory in t.Value)
                {
                    var SubcategoryId = await SubcategoryRepository.AddAsync(new Market.Entities.Dto.SubCategory()
                    {
                        CategoryId = CategoryId,
                        SubCategoryName = subcategory,
                        SubCategoryUrlIcon = "https://imdiz.ru/files/store/img/icons_catalog/desktops.png"
                    });
                    Random random = new Random();
                    for (int i = 0; i < 40; i++)
                    {

                        var id = ProductsRepository.AddAsync(new Market.Entities.Dto.ProductDto()
                        {
                            Brend = $"Brend{i}",
                            SubCategoryid = SubcategoryId,
                            Description = $"Description{i}",
                            Image = urls[random.Next(0, urls.Count() - 1)],
                            Name = $"дорогой комп{i}",
                            Price = new Random().Next(100000, 200000),
                            Quantity = new Random().Next(1, 100),
                        }).GetAwaiter().GetResult(); ;
                        await AddCharectiristic(id);

                    }

                }

            }
        }
        private async Task AddCharectiristic(int productId)
        {
            await ProductsRepository.AddCharectiristic(new ProductCharacteristicType()
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
            });
        }

    }
}
