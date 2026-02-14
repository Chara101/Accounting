using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountAPI.EnumList;
using AccountAPI.Models;

namespace TestAcounting.DataStorage
{
    internal interface IDataStorage
    {
        void Add(RecordForm r);
        void AddCategory(string name);
        void AddSubCategory(int category_id, string name);
        void Remove(RecordForm r);
        void RmCategory(int id);
        void RmSubCategory(int id);
        List<RecordForm> GetAllRecords();
        List<RecordForm> GetRecordsBy(Records_search_form r);
        List<RecordForm> GetRecordsBy(Records_search_form r1, Records_search_form r2);
        List<RecordForm> GetAllTotals();
        List<RecordForm> GetTotals(Total_search_form r);
        List<RecordForm> GetTotals(Total_search_form r1, Total_search_form r2);
        List<RecordForm> GetAllCategories();
        List<RecordForm> GetAllSubCategories();
        List<RecordForm> GetAllCategoriesAndSub();
        void Update(RecordForm tartget, RecordForm content);
    }
}
