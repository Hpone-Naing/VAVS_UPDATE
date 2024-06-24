using VAVS_Client.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using VAVS_Client.Paging;
using VAVS_Client.Classes;
using Microsoft.AspNetCore.Mvc.Rendering;
using VAVS_Client.Controllers;
using System.Runtime.CompilerServices;

namespace VAVS_Client.Services.Impl
{
    public class AbstractServiceImpl<T> : AbstractService<T> where T : class
    {
        protected readonly VAVSClientDBContext _context;
        private readonly ILogger<AbstractServiceImpl<T>> _logger;
        public AbstractServiceImpl(VAVSClientDBContext context, ILogger<AbstractServiceImpl<T>> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public PagingList<T> GetAllWithPagin(List<T> list, int? pageNo, int pageSize)
        {
            _logger.LogInformation(">>>>>>>>>> [AbstractServiceImpl][GetAllWithPagin] Retrieve Object List and make pagination <<<<<<<<<<");
            try
            {
                _logger.LogInformation($">>>>>>>>>> Retrieve Object List success. <<<<<<<<<<");
                return PagingList<T>.CreateAsync(list.AsQueryable<T>(), pageNo ?? 1, pageSize);
            }
            catch(Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur when retrieveing Object List. <<<<<<<<<<" + e);
                throw;
            }
        }

        public List<T> GetUniqueList(Func<T, object> keySelector)
        {
            _logger.LogInformation(">>>>>>>>>> [AbstractServiceImpl][GetUniqueList] Retrieve Unique List <<<<<<<<<<");
            try
            {
                _logger.LogInformation(">>>>>>>>>> Retrieve Unique List success. <<<<<<<<<<");
                return _context.Set<T>().GroupBy(keySelector).Select(group => group.First()).ToList();
            }
            catch(Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur when retrieveing Unique Object List. <<<<<<<<<<" + e);
                throw;
            }
        }

        protected bool IsSearchDataContained(object obj, string searchData, string column = null)
        {
            _logger.LogInformation(">>>>>>>>>> [AbstractServiceImpl][IsSearchDataContained] Check searchString contain in table columns value <<<<<<<<<<");
            try
            {
                if (obj != null)
                {
                    foreach (PropertyInfo prop in obj.GetType().GetProperties())
                    {
                        if (prop.PropertyType == typeof(string))
                        {
                            if (column == null || string.Equals(prop.Name, column, StringComparison.OrdinalIgnoreCase))
                            {
                                string propValue = (string)prop.GetValue(obj);

                                if (propValue != null)
                                {
                                    if (propValue.IndexOf(searchData, StringComparison.OrdinalIgnoreCase) >= 0)
                                    {
                                        _logger.LogInformation(">>>>>>>>>> Match searchString and colVal <<<<<<<<<<");
                                        return true;
                                    }
                                }
                            }

                        }
                    }
                }
                _logger.LogInformation(">>>>>>>>>> Not match searchString and colVal <<<<<<<<<<");
                return false;
            }
            catch(Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur when checking searchString contain in table columns value. <<<<<<<<<<" + e);
                throw;
            }
        }
        
        public T FindById(int id)
        {
            _logger.LogInformation(">>>>>>>>>> [AbstractServiceImpl][IsSearchDataContained] Find object by pkId <<<<<<<<<<");
            try
            {
                _logger.LogInformation(">>>>>>>>>> Found match object by pkId <<<<<<<<<<");
                return _context.Set<T>().Find(id);
            }catch(Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur when finding object by pkId. <<<<<<<<<<" + e);
                throw;
            }
        }

        public T FindByString(string columnName, string str)
        {
            _logger.LogInformation(">>>>>>>>>> [AbstractServiceImpl][FindByString] Find object's specific columnName's value that match stringValue <<<<<<<<<<");
            try
            {
                _logger.LogInformation(">>>>>>>>>> Found object's specific columnName's value match stringValue <<<<<<<<<<");
                return _context.Set<T>().FirstOrDefault(entity =>
                   entity != null && EF.Property<string>(entity, columnName)!=null && EF.Property<string>(entity, columnName) == str);
            }
            catch(Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur when finding object's specific columnName's value match stringValue. <<<<<<<<<<" + e);
                throw;
            }
        }

        public List<T> GetListByIntVal(string columnName, int intVal)
        {
            _logger.LogInformation(">>>>>>>>>> [AbstractServiceImpl][GetListByIntVal] Get object list match specific columnName's value that match intValue <<<<<<<<<<");
            try
            {
                _logger.LogInformation(">>>>>>>>>> Success. Get object list match specific columnName's value match intValue <<<<<<<<<<");
                return _context.Set<T>().Where(entity =>
                EF.Property<int>(entity, columnName) == intVal).ToList();
            }catch(Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur when getting object list match specific columnName's value match intValue. <<<<<<<<<<" + e);
                throw;
            }
        }

        public T GetObjByIntVal(string columnName, int intVal)
        {
            _logger.LogInformation(">>>>>>>>>> [AbstractServiceImpl][GetObjByIntVal] Get object match specific columnName's value that match intValue <<<<<<<<<<");
            try
            {
                _logger.LogInformation(">>>>>>>>>> Success. Get object match specific columnName's value match intValue <<<<<<<<<<");
                return _context.Set<T>().FirstOrDefault(entity =>
                EF.Property<int>(entity, columnName) == intVal);
            }catch(Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur when getting object that match specific columnName's value match intValue. <<<<<<<<<<" + e);
                throw;
            }
        }

        public List<SelectListItem> GetItemsFromList<T>(List<T> list, string valuePropertyName, string textPropertyName)
        {
            _logger.LogInformation(">>>>>>>>>> [AbstractServiceImpl][GetItemsFromList] Get SelectList for selectbox by giving object list and make selectbox's option value and name.  <<<<<<<<<<");
            try
            {
                var lstItems = new List<SelectListItem>();
                foreach (T item in list)
                {
                    var itemType = item.GetType();
                    var valueProperty = itemType.GetProperty(valuePropertyName);
                    var textProperty = itemType.GetProperty(textPropertyName);

                    if (valueProperty != null && textProperty != null)
                    {
                        var value = valueProperty.GetValue(item)?.ToString();
                        var text = textProperty.GetValue(item)?.ToString();

                        lstItems.Add(new SelectListItem
                        {
                            Value = value,
                            Text = text
                        });
                    }
                }

                var defItem = new SelectListItem()
                {
                    Value = "",
                    Text = "ရွေးချယ်ရန်"
                };

                lstItems.Insert(0, defItem);
                _logger.LogInformation(">>>>>>>>>> Success. Get SelectList for selectbox by giving object list and make selectbox's option value and name. <<<<<<<<<<");
                return lstItems;
            }catch(Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur when getting SelectList for selectbox by giving object list and make selectbox's option value and name. <<<<<<<<<<" + e);
                throw;
            }
        }

        public bool Create(T t)
        {
            _logger.LogInformation(">>>>>>>>>> [AbstractServiceImpl][Create] Save object.  <<<<<<<<<<");
            try
            {
                _context.Set<T>().Add(t);
                _context.SaveChanges();
                _logger.LogInformation(">>>>>>>>>> Create Success. <<<<<<<<<<");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(">>>>>>>>>> Create Fail. <<<<<<<<<<" + e);
                throw;
            }
        }

        public bool Update(T t)
        {
            _logger.LogInformation(">>>>>>>>>> [AbstractServiceImpl][Update] Update object.  <<<<<<<<<<");
            try
            {
                _context.Entry(t).State = EntityState.Modified;
                _context.SaveChanges();
                _logger.LogInformation(">>>>>>>>>> Update Success. <<<<<<<<<<");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(">>>>>>>>>> Update Fail. <<<<<<<<<<" + e);
                throw;
            }
        }
    }
}
