using Dapper;
using DataBaseLib.Interfaces;
using Market.Entities.Dto;
using Market.Repositories.Interfaces;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Market.Repositories.Repositories.PostgresqlRepositories.CategoriesRepository.Columns;
namespace Market.Repositories.Repositories.PostgresqlRepositories.CategoriesRepository
{
    public class Repository : BaseRepository, ICategoriesRepository
    {
        private string _tableName = "Categories";
 
         
        public Repository(IDbProvider connection):base(connection) 
        {
            
        }


        public async Task<int> GetCount()
        {
            string sql = $"SELECT COUNT(*) from {_tableName} ";
            return (await _connection.QueryAsync<int>(sql)).First();
        }

        public async Task<long> AddCategoryAsync(CategoryDto category)
        {
            string checkSql = $"SELECT {IdColumn} FROM {_tableName} where {ColumnName} = @name ";

            var checkId = await _connection.QueryAsync<int>(checkSql, new { name = category.CategoryName });
            if (!checkId.Any())
            {
                /// если ранее небыло такой категории
                string sql = $"INSERT INTO {_tableName} ({ColumnName},{UrlIconColumnName}) VALUES (@Name,@UrlIcon) returning {IdColumn}";
                var result = (await _connection.QueryAsync<long>(sql, new
                {
                    Name = category.CategoryName,
                    UrlIcon = category.CategoryIconUrl,
                })).FirstOrDefault();
                return result;
            }
            else return checkId.FirstOrDefault();
        }

        public async Task<IEnumerable<SubCategory>> GetSubCategories(string category)
        {
            var result = (await GetCategoriesAsync(category))?.FirstOrDefault()?.SubCategories;
            if (result == null)
            {
                return new List<SubCategory>();
            }
            return result;

        }


        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync(string category = "")
        {
            string sql = $"select  " +
                $" {SubcategoryRepository.TableCreater.TableName}.{SubcategoryRepository.Columns.NameColumnName} as {nameof(SubCategory.SubCategoryName)}, " +
                $" {SubcategoryRepository.TableCreater.TableName}.{SubcategoryRepository.Columns.UrlIconcColumnName} as {nameof(SubCategory.SubCategoryUrlIcon)}, " +
                $" {CategoriesRepository.TableCreater.TableName}.{CategoriesRepository.Columns.UrlIconColumnName} as {nameof(CategoryDto.CategoryIconUrl)}, " +
                $" {CategoriesRepository.TableCreater.TableName}.{CategoriesRepository.Columns.ColumnName} as {nameof(CategoryDto.CategoryName)} " +
                $"from {SubcategoryRepository.TableCreater.TableName}  " +
                $"join {_tableName} on {SubcategoryRepository.TableCreater.TableName}.{SubcategoryRepository.Columns.CategoryIdColumnName} = {_tableName}.{IdColumn} ";
            if (!string.IsNullOrEmpty(category))
            {
                sql += $" WHERE {_tableName}.name = @category ";
            }
            IEnumerable<dynamic> responce = null;
            if (string.IsNullOrEmpty(category))
            {
                responce = await _connection.QueryAsync(sql);
            }
            else
            {
                responce = await _connection.QueryAsync(sql, new { category });
            }
            var result = new List<CategoryDto>();
            foreach (var t in responce)
            {
                var find = result.FirstOrDefault(p => p.CategoryName == t.categoryname);
                if (find == null)
                {
                    result.Add(new CategoryDto()
                    {
                        CategoryName = t.categoryname,
                        CategoryIconUrl = t.subCategoryurlicon,
                        SubCategories = new List<SubCategory>() {
                             new SubCategory(){
                                SubCategoryName = t.subcategoryname,
                                 SubCategoryUrlIcon= t.subcategoryurlicon,
                             }
                         }
                    });
                }
                else
                {
                    find.SubCategories.Add(new SubCategory()
                    {
                        SubCategoryName = t.subcategoryname,
                        SubCategoryUrlIcon = t.subcategoryurlicon,
                    });
                }
            }


            return result;
        }
    }
}
