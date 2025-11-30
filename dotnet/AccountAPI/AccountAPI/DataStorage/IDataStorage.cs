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
        List<RecordForm> GetRecordsBy(RecordForm r);
        List<RecordForm> GetRecordsBy(RecordForm r1, RecordForm r2);
        List<RecordForm> GetAllTotals();
        List<RecordForm> GetTotals(RecordForm r);
        List<RecordForm> GetTotals(RecordForm r1, RecordForm r2);
        List<RecordForm> GetAllCategories();
        List<RecordForm> GetAllSubCategories();
        List<RecordForm> GetAllCategoriesAndSub();
        void Update(RecordForm tartget, RecordForm content);
    }
}
