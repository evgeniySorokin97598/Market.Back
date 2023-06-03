using Dapper;
using Market.Entities.Dto;
using Market.Entities.Requests;
using Market.Repositories.Interfaces;

using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Market.Repositories.Repositories.PostgresqlRepositories.ProductsRepository.Columns;
using Market.Repositories.Repositories.PostgresqlRepositories;

namespace Market.Repositories.Repositories.PostgresqlRepositories.ProductsRepository
{

    public class Repository : BaseRepository, IProductsRepository
    {

        private string TableName = TableCreater.TableName;
        private NpgsqlConnection _connection;
        private ITypeСharacteristicsRepository _typeCharacteristicsRepository;
        private IСharacteristicsRepository _сharacteristicsRepository;
        public Repository(NpgsqlConnection connection, ITypeСharacteristicsRepository typeCharacteristicsRepository, IСharacteristicsRepository сharacteristicsRepository) : base(connection)
        {
            _connection = connection;
            _typeCharacteristicsRepository = typeCharacteristicsRepository;
            _сharacteristicsRepository = сharacteristicsRepository;
        }

        public async Task<ProductDto> GetProductById(long id)
        {


            string sql = $"select * FROM {TableName} " +
    $" join {TypeСharacteristicsRepository.TableCreater.TableName} on {TableCreater.TableName}.{СharacteristicId} = {TypeСharacteristicsRepository.TableCreater.TableName}.{TypeСharacteristicsRepository.Columns.ProductId}  " +
    $" join {СharacteristicsRepository.TableCreater.TableName} on {TypeСharacteristicsRepository.TableCreater.TableName}.{TypeСharacteristicsRepository.Columns.Id}  = {СharacteristicsRepository.TableCreater.TableName}.{СharacteristicsRepository.Columns.TypeСharacteristicsId} " +
    $" left join {CommentsRepository.TableCreater.TableName} on {TableName}.{nameof(ProductDto.Id)} = {CommentsRepository.TableCreater.TableName}.{CommentsRepository.Columns.ProductId} " +
    $" left join {UsersRepository.TableCreater.Table} on {CommentsRepository.TableCreater.TableName}.{CommentsRepository.Columns.UserIdCol} = {UsersRepository.TableCreater.Table}.{UsersRepository.Columns.UserId} " + /// join пользователей которые оставляли комментарии
    $" left join {CommentsLikesRepository.TableCreater.TableName} on {CommentsRepository.TableCreater.TableName}.{CommentsRepository.Columns.Id} = {CommentsLikesRepository.TableCreater.TableName}.{CommentsLikesRepository.Columns.CommentId} " +
    $" where {TableName}.{Id} = @id ";

            var list = await _connection.QueryAsync(sql, new
            {
                id
            });
            var cooments = list.Where(p => !string.IsNullOrEmpty(p.comment) || !string.IsNullOrEmpty(p.dignity) || !string.IsNullOrEmpty(p.flaws));

            var first = list.FirstOrDefault();
            if (first == null) return null;
            
            ProductDto product = new ProductDto()
            {
                Id = first.id,
                Brend = first.brend,
                Name = first.name,
                Description = first.description,
                Image = first.image,
                Price = first.price,
                Quantity = first.quantity,
                TypesCharacteristics = list
                .GroupBy(l => l.typeСharacteristicsname)
                .Select(p => new ProductCharacteristicType()
                {
                    Name = p.Key,
                    Charastitics = list.Where(t => t.typeСharacteristicsname == p.Key)
                    .DistinctBy(p => p.Сharacteristicname)
                    .Select(k => new Charastitic()
                    {
                        CharastiticId = k.characteristicid,
                        Name = k.Сharacteristicname,
                        Text = k.Сharacteristic
                    }).ToList()
                }).ToList(),
                Comments = cooments.DistinctBy(p => p.commentid).Select(p => new CommentDto()
                {
                    CommentId = p.commentid,
                    Comment = p.comment,
                    Dignity = p.dignity,
                    Flaws = p.flaws,
                    UserName = string.IsNullOrEmpty(p.nickname) ? "Пользователь" : p.nickname,
                    Stars = p.stars,
                    CountLikes = cooments.Where(t => t.commentid == p.commentid).DistinctBy(p => p.likeid).Count()
                }).ToList()
            };

            // product.Comments = product.Comments.DistinctBy(p => p.CommentId).ToList();
            return product;
        }



        public async Task<int> AddAsync(ProductDto product)
        {
            string insert = $"INSERT INTO {TableName} (" +
                $"{nameof(product.Name)}," +
                $"{nameof(product.Description)}," +
                $"{nameof(product.Price)}," +
                $"{nameof(product.Image)}," +
                $"{nameof(product.Quantity)}," +
                $"{nameof(product.Brend)}," +
                $"{nameof(product.SubCategoryid)})" +
                $" VALUES (" +
                $" @{nameof(product.Name)}," +
                $" @{nameof(product.Description)}," +
                $" @{nameof(product.Price)}," +
                $" @{nameof(product.Image)}," +
                $" @{nameof(product.Quantity)}," +
                $" @{nameof(product.Brend)}," +
                $" @{nameof(product.SubCategoryid)}" +
                $") returning id";
            int id = (await _connection.QueryAsync<int>(insert, product)).FirstOrDefault();
            return id;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategory(string categyName)
        {
            string sql = $"SELECT  " +
                $"{TableName}.{nameof(ProductDto.Name)}," +
                $"{TableName}.{nameof(ProductDto.Description)}," +
                $"{TableName}.{nameof(ProductDto.Quantity)}," +
                $"{TableName}.{nameof(ProductDto.Brend)}," +
                $"{TableName}.{nameof(ProductDto.Price)}, " +
                $"{TableName}.{nameof(ProductDto.Id)} " +
                $" From {TableName} " +
                $" Join {SubcategoryRepository.TableCreater.TableName} ON {SubcategoryRepository.TableCreater.TableName}.{SubcategoryRepository.Columns.IdColumnName} =  {TableName}.{SubCategoryIdColumn}" +
                $" WHERE {SubcategoryRepository.TableCreater.TableName}.{SubcategoryRepository.Columns.NameColumnName} = @Category";

            var result = await _connection.QueryAsync<ProductDto>(sql, new
            {
                Category = categyName
            });
            return result;
        }



        public async Task AddCharectiristic(ProductCharacteristicType characteristic)
        {
            if (characteristic == null) {
                return;
            }
            int id = await _typeCharacteristicsRepository.Add(characteristic.Name, characteristic.ProductId);

            if (characteristic.Charastitics != null) {
                foreach (var t in characteristic.Charastitics)
                {
                    if (!string.IsNullOrEmpty(t.Name) && !string.IsNullOrEmpty(t.Text))
                    {
                        await _сharacteristicsRepository.Add(t.Name, id, t.Text, t.CharastiticId);
                    }

                }
            }
            
            if (characteristic.RemoveId != null) {
                foreach (var t in characteristic.RemoveId)
                {
                    await _сharacteristicsRepository.Remove(t);
                }
            }
            
        }

        public async Task<IEnumerable<ProductDto>> GetProducts(GetProductsRequest request)
        {
            List<ProductDto> products = new List<ProductDto>();
            foreach (var t in request.Id)
            {
                if (t != 0) products.Add(await GetProductById(t));
            }
            return products;
        }
    }
}
